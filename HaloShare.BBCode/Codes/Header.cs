using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.BBCode.Codes
{
    [Export(typeof(IBBCode))]
    [ExportMetadata("Tag", "header")]
    public class Header : IBBCode
    {
        private static string format = "<h2>{0}</h2>";

        public string Parse(string token, string parameter, string value)
        {
            return string.Format(format, value);
        }
    }
}
