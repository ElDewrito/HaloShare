using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.BBCode.Codes
{
    [Export(typeof(IBBCode))]
    [ExportMetadata("Tag", "sup")]
    public class Sup : IBBCode
    {
        private static string format = "<sup>{0}</sup>";

        public string Parse(string token, string parameter, string value)
        {
            return string.Format(format, value);
        }
    }
}
