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
    [ExportMetadata("Tag", "gallery")]
    public class Gallery : IBBCode
    {
        private static string formatStart = "<div class=\"gallery\">";
        private static string formatEnd = "</div>";
        private static string formatImage = "<a data-lightbox=\"gallery\" href=\"{1}\"><img  class=\"thumbnail\" src=\"{1}\"/></a>";

        public string Parse(string token, string parameter, string value)
        {
            string[] urlsString = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            List<Uri> urls = new List<Uri>();

            foreach (string str in urlsString)
            {
                Uri uri;
                if (Uri.TryCreate(str, UriKind.Absolute, out uri))
                {
                    urls.Add(uri);
                }
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(formatStart);
            for (var i = 0; i < urls.Count; i++)
            {
                Uri uri = urls[i];

                builder.AppendFormat(formatImage, i, uri.ToString());
            }
            builder.Append(formatEnd);

            return builder.ToString();
        }
    }
}
