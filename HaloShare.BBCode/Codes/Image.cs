using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.BBCode.Codes
{
    [Export(typeof(IBBCode))]
    [ExportMetadata("Tag", "img")]
    public class Image : IBBCode
    {
        private static string format = "<img src=\"{0}\" title=\"{1}\" style=\"max-width: 100%;\" />";

        public string Parse(string token, string parameter, string value)
        {
            return string.Format(format, parameter != "" ? parameter : value, parameter != "" ? value : "");
        }
    }
}
