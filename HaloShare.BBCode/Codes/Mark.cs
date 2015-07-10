using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.BBCode.Codes
{
    [Export(typeof(IBBCode))]
    [ExportMetadata("Tag", "m")]
    public class Mark : IBBCode
    {
        private static string format = "<mark>{0}</mark>";

        public string Parse(string token, string parameter, string value)
        {
            return string.Format(format, value);
        }
    }
}
