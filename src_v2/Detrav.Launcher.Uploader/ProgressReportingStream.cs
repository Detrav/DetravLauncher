using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.Launcher.Uploader
{
    public class ProgressReportingStream : Stream
    {
        private readonly Stream innerStream;

        public event EventHandler<int>? OnRead;
        public event EventHandler<int>? OnWrite;

        public ProgressReportingStream(Stream stream)
        {
            this.innerStream = stream;
        }
        public override bool CanRead => innerStream.CanRead;

        public override bool CanSeek => innerStream.CanSeek;

        public override bool CanWrite => innerStream.CanWrite;

        public override long Length => innerStream.Length;

        public override long Position { get => innerStream.Position; set => innerStream.Position = value; }

        public override void Flush()
        {
            innerStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var size = innerStream.Read(buffer, offset, count);
            OnRead?.Invoke(this, size);
            return size;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            innerStream.Write(buffer, offset, count);
            OnWrite?.Invoke(this, count);
        }
    }
}
