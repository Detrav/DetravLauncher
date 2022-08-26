using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Detrav.Launcher.Server.Services
{
    public interface IFileService
    {
        Task<FileModel> StoreAsync(string path, byte[] bytes, bool autoSave = false);
        Task<byte[]> GetAsync(int fileId);
        Task<bool> RemoveAsync(FileModel? poster, bool autoSave = false);
    }
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext context;
        private const int BlobSize = 256 * 1024;

        public FileService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<byte[]> GetAsync(int fileId)
        {
            var file = await context.Files.FirstOrDefaultAsync(m => m.Id == fileId);
            if (file == null)
                throw new KeyNotFoundException(nameof(fileId));
            var result = new byte[file.Size];

            await foreach (var blob in context.FileBlobs.Include(m => m.Blob).Where(m => m.FileId == fileId).AsAsyncEnumerable())
            {
                if (blob.Blob?.Data != null && blob.Blob.Size > 0)
                {
                    Array.Copy(blob.Blob.Data, 0, result, blob.Seek, blob.Blob.Size);
                }
            }
            return result;
        }

        public async Task<FileModel> StoreAsync(string path, byte[] bytes, bool autoSave = false)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentNullException(nameof(bytes));

            FileModel model = new FileModel();

            model.Path = path;
            model.Hash = CreateMD5(bytes);
            model.Size = bytes.LongLength;

            for (long i = 0; i < bytes.LongLength; i += BlobSize)
            {
                var block = GetBlock(bytes, i);
                await StoreBlobAsync(block, i, model, autoSave);
            }

            context.Add(model);

            if (autoSave)
                await context.SaveChangesAsync();

            return model;
        }

        private async Task<FileBlobModel> StoreBlobAsync(byte[] block, long seek, FileModel file, bool autoSave)
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
                            File = file
                        };
                        context.FileBlobs.Add(result);
                        if (autoSave)
                            await context.SaveChangesAsync();
                        return result;
                    }
                }
            }
            {
                BlobModel blob = new BlobModel();
                blob.Hash = md5;
                blob.Size = block.Length;
                blob.Data = block;

                var result = new FileBlobModel()
                {
                    Blob = blob,
                    Seek = seek,
                    File = file
                };
                context.FileBlobs.Add(result);
                if (autoSave)
                    await context.SaveChangesAsync();
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

        public async Task<bool> RemoveAsync(FileModel? file, bool autoSave = false)
        {
            if (file != null)
            {
                context.Files.Remove(file);
                if (autoSave)
                    await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
