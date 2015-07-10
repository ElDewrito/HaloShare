using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.BBCode.Codes
{
    [Export(typeof(IBBCode))]
    [ExportMetadata("Tag", "b")]
    public class Bold : IBBCode
    {
        private static string format = "<b>{0}</b>";

        public string Parse(string token, string parameter, string value)
        {
            return string.Format(format, value);
        }
    }
}
