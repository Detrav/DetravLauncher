using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace Detrav.Launcher.Server.Services
{
    public interface IFileService
    {
        Task<FileModel> StoreAsync(string collection, string path, byte[] bytes);
        Task<byte[]?> GetOrDefaultAsync(int fileId);
        Task<byte[]?> GetOrDefaultAsync(string collection, string filePath);
        Task RemoveAsync(FileModel? file);
        Task<FileModel> StoreAsync(string collection, string filePath, Stream body, long fileSize);
        Task RemoveAsync(string? collection, string? filePath);
    }
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext contextCommon;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private const int BlobSize = 256 * 1024;

        public FileService(ApplicationDbContext context, IServiceScopeFactory serviceScopeFactory)
        {
            this.contextCommon = context;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<byte[]?> GetOrDefaultAsync(int fileId)
        {
            var file = await contextCommon.Files.FirstOrDefaultAsync(m => m.Id == fileId);
            if (file == null)
                return null;
            var result = new byte[file.Size];

            await foreach (var blob in contextCommon.FileBlobs.Include(m => m.Blob).Where(m => m.FileId == fileId).AsAsyncEnumerable())
            {
                if (blob.Blob?.Data != null && blob.Blob.Size > 0)
                {
                    Array.Copy(blob.Blob.Data, 0, result, blob.Seek, blob.Blob.Size);
                }
            }
            return result;
        }

        public async Task<FileModel> StoreAsync(string collection, string path, byte[] bytes)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentNullException(nameof(bytes));

            using var scope = serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            FileModel model = new FileModel();

            model.Path = path;
            model.Size = bytes.LongLength;
            model.Collection = collection;
            context.Files.Add(model);
            await context.SaveChangesAsync();

            for (long i = 0; i < bytes.LongLength; i += BlobSize)
            {
                var block = GetBlock(bytes, i);
                await StoreBlobAsync(context, block, i, model);
            }


            await context.SaveChangesAsync();

            return model;
        }

        private async Task<FileBlobModel> StoreBlobAsync(ApplicationDbContext context, byte[] block, long seek, FileModel file)
        {
            var md5 = CreateMD5(block);
            var size = block.Length;
            await foreach (var blob in context.Blobs.Where(m => m.Hash == md5 && m.Size == size).AsAsyncEnumerable())
            {
                if (blob.Data != null)
                {
                    bool equals = true;
                    for (int i = 0; i < blob.Size; i++)
                    {
                        if (block[i] != blob.Data[i])
                        {
                            equals = false;
                            break;
                        }
                    }
                    if (equals)
                    {
                        var result = new FileBlobModel()
                        {
                            Blob = blob,
                            Seek = seek,
                            FileId = file.Id
                        };
                        context.FileBlobs.Add(result);
                        return result;
                    }
                }
            }
            {
                BlobModel blob = new BlobModel();
                blob.Hash = md5;
                blob.Size = block.Length;
                blob.Data = block.ToArray();

                var result = new FileBlobModel()
                {
                    Blob = blob,
                    Seek = seek,
                    FileId = file.Id
                };
                context.FileBlobs.Add(result);
                return result;
            }
        }



        private byte[] GetBlock(byte[] bytes, long seek)
        {
            long size = bytes.Length - seek;
            if (size > BlobSize)
                size = BlobSize;

            byte[] subBytes = new byte[size];
            Array.Copy(bytes, seek, subBytes, 0, size);
            return subBytes;
        }

        public static string CreateMD5(byte[] bytes)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(bytes);
                return Convert.ToHexString(hashBytes);
            }
        }

        public async Task RemoveAsync(FileModel? file)
        {
            if (file != null)
            {
                contextCommon.Files.Remove(file);
                await contextCommon.SaveChangesAsync();
            }
        }

        public async Task<FileModel> StoreAsync(string collection, string filePath, Stream body, long fileSize)
        {
            IServiceScope? scope = null;
            try
            {
                scope = serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                int counter = 0;

                var file = new FileModel()
                {
                    Path = filePath,
                    Collection = collection,
                    Size = fileSize
                };

                context.Files.Add(file);

                await context.SaveChangesAsync();

                byte[] buffer = new byte[BlobSize];
                int offset = 0;
                long globalOffset = 0;
                while (true)
                {
                    var readBytes = await body.ReadAsync(buffer, offset, BlobSize - offset);
                    offset += readBytes;
                    if (readBytes == 0)
                    {
                        if (offset > 0)
                        {
                            await StoreBlobAsync(context, buffer.Take(offset).ToArray(), globalOffset, file);
                            globalOffset += offset;
                        }
                        break;
                    }
                    else if (offset == BlobSize)
                    {
                        await StoreBlobAsync(context, buffer, globalOffset, file);
                        globalOffset += offset;
                        offset = 0;

                        counter++;
                        if (counter % 13 == 0)
                        {
                            // free mamory on 3.3mb
                            await context.SaveChangesAsync();
                            scope.Dispose();
                            scope = serviceScopeFactory.CreateScope();
                            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        }
                    }
                }

                file.Size = globalOffset;

                await context.SaveChangesAsync();

                return file;
            }
            finally
            {
                if (scope != null)
                    scope.Dispose();
            }

        }

        public async Task<byte[]?> GetOrDefaultAsync(string collection, string filePath)
        {
            var file = await contextCommon.Files.FirstOrDefaultAsync(m => m.Collection == collection && m.Path == filePath);
            if (file == null)
                return null;
            var result = new byte[file.Size];

            await foreach (var blob in contextCommon.FileBlobs.Include(m => m.Blob).Where(m => m.FileId == file.Id).AsAsyncEnumerable())
            {
                if (blob.Blob?.Data != null && blob.Blob.Size > 0)
                {
                    Array.Copy(blob.Blob.Data, 0, result, blob.Seek, blob.Blob.Size);
                }
            }
            return result;
        }

        public async Task RemoveAsync(string? collection, string? filePath)
        {
            var file = await contextCommon.Files.FirstOrDefaultAsync(m => m.Collection == collection && m.Path == filePath);
            if (file != null)
            {
                contextCommon.Files.Remove(file);
            }
        }
    }
}
