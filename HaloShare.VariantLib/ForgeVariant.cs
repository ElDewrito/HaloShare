using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.VariantLib
{
    public class ForgeVariant : Variant
    {

        public int MapId { get; set; }

        public ForgeVariant(Stream stream)
            : base(stream)
        {
            
            MapId = _stream.ReadInt32(0x120);
        }

        public override void Save()
        {
            base.Save();
            _stream.Write(0x150, VariantName, 32, Encoding.Unicode);
            _stream.Write(0x170, VariantDescription, 128, Encoding.ASCII);
            _stream.Write(0x1F0, VariantAuthor, 16, Encoding.ASCII);
        }
    }
       
}
