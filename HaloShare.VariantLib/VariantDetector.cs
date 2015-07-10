using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.VariantLib
{
    public class VariantDetector
    {

        public static VariantType Detect(Stream stream)
        {
            LibStream _stream = new LibStream(stream);

            // Validate the Magic
            if (_stream.ReadString(0, 4) != "_blf") 
                return VariantType.Invalid;

            // Check what the file variant is.
            switch (_stream.ReadString(0x138, 4))
            {
                case "mapv":
                    return VariantType.ForgeVariant;
                case "mpvr":
                    return VariantType.GameVariant;
                default:
                    return VariantType.Invalid;
            }
        }

        
    }
}
