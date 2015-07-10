using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HaloShare.BBCode.Codes
{
    [Export(typeof(IBBCode))]
    [ExportMetadata("Tag", "url")]
    public class Anchor : IBBCode
    {
        private static string format = "<a href=\"{0}\" target=\"_blank\" rel=\"nofollow\">{1}</a>";

        public string Parse(string token, string parameter, string value)
        {
            return string.Format(format,
                HttpUtility.UrlEncode(parameter != "" ? parameter : value), 
                value != "" ? value : parameter);
        }
    }
}
