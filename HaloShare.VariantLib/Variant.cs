using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.VariantLib
{
    public class Variant
    {
        protected LibStream _stream;

        public VariantType Type { get; set; }

        public string VariantName { get; set; }
        public string VariantDescription { get; set; }
        public string VariantAuthor { get; set; }

        public Variant(Stream stream)
        {
            _stream = new LibStream(stream);

            VariantName = _stream.ReadUnicodeString(0x48, 32);
            VariantDescription = _stream.ReadString(0x68, 128);
            VariantAuthor = _stream.ReadString(0xE8, 16);
        }

        public virtual void Save()
        {
            _stream.Write(0x48, VariantName, 32, Encoding.Unicode);
            _stream.Write(0x68, VariantDescription, 128, Encoding.ASCII);
            _stream.Write(0xE8, VariantAuthor, 16, Encoding.ASCII);
        }
    }
}
