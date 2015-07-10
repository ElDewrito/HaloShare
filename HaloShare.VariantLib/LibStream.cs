using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.VariantLib
{
    public class LibStream
    {
        private Stream _stream;
        public Stream InternalStream
        {
            get { return _stream; }
        }

        public LibStream(Stream stream)
        {
            _stream = stream;
            stream.Seek(0, SeekOrigin.Begin);
        }

        public void Seek(long offset)
        {
            _stream.Seek(offset, SeekOrigin.Begin);
        }

        #region Reading

        public byte[] ReadBytes(long offset, int count)
        {
            Seek(offset);
            byte[] buffer = new byte[count];
            _stream.Read(buffer, 0, count);
            return buffer;
        }

        public Int16 ReadInt16(long offset)
        {
            return BitConverter.ToInt16(ReadBytes(offset, 2), 0);
        }

        public Int32 ReadInt32(long offset)
        {
            return BitConverter.ToInt32(ReadBytes(offset, 4), 0);
        }

        public Int64 ReadInt64(long offset)
        {
            return BitConverter.ToInt64(ReadBytes(offset, 8), 0);
        }

        public string ReadString(long offset, int length)
        {
            return Encoding.ASCII.GetString(ReadBytes(offset, length)).Replace("\0", "");
        }

        public string ReadUnicodeString(long offset, int length)
        {
            return Encoding.Unicode.GetString(ReadBytes(offset, length)).Replace("\0", "");
        }

        #endregion

        #region Writing

        public void Write(long offset, byte[] buffer)
        {
            Seek(offset);
            _stream.Write(buffer, 0, buffer.Length);
        }

        public void Write(long offset, Int16 value)
        {
            Seek(offset);
            _stream.Write(BitConverter.GetBytes(value), 0, 2);
        }

        public void Write(long offset, Int32 value)
        {
            Seek(offset);
            _stream.Write(BitConverter.GetBytes(value), 0, 4);
        }

        public void Write(long offset, Int64 value)
        {
            Seek(offset);
            _stream.Write(BitConverter.GetBytes(value), 0, 8);
        }

        public void Write(long offset, string value, int length, Encoding encoding)
        {
            byte[] buffer = new byte[length];
            byte[] chars = encoding.GetBytes(value);
            chars.CopyTo(buffer, 0);

            Write(offset, buffer);
        }

        #endregion
    }
}
