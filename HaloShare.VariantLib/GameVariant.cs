using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.VariantLib
{
    public class GameVariant : Variant
    {
        public int TypeId { get; set; }

        public GameVariant(Stream stream)
            : base(stream)
        {
            Type = VariantType.GameVariant;
            TypeId = _stream.ReadInt32(0xF8);
        }

        public override void Save()
        {
            base.Save();
            _stream.Write(0x178, VariantName, 32, Encoding.Unicode);
            _stream.Write(0x198, VariantDescription, 128, Encoding.ASCII);
            _stream.Write(0x218, VariantAuthor, 16, Encoding.ASCII);
        }
    }
}
