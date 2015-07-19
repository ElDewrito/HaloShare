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
        private static string format = "<a href=\"{0}\" target=\"{2}\" rel=\"nofollow\">{1}</a>";

        public string Parse(string token, string parameter, string value)
        {
            string url = parameter != "" ? parameter : value;
            string name = value != "" ? value : parameter;

            Uri uri;
            if(Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                if (!uri.IsAbsoluteUri || uri.Scheme.StartsWith("http"))
                {
                    string target = uri.IsAbsoluteUri ? "_blank" : "";

                    return string.Format(format, uri.ToString(), name, target);
                }
            } 
            return name;
        }
    }
}
