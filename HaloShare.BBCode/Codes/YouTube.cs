using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HaloShare.BBCode.Codes
{
    [Export(typeof(IBBCode))]
    [ExportMetadata("Tag", "youtube")]
    public class YouTube : IBBCode
    {
        private static string format = "<div class=\"embed-responsive embed-responsive-16by9\"><iframe class=\"embed-responsive-item\" src=\"https://www.youtube.com/embed/{0}?wmode=opaque\" data-youtube-id=\"{0}\" allowfullscreen=\"\"></iframe></div>";

        public string Parse(string token, string parameter, string value)
        {
            value = value != "" ? value : parameter;

            string id = null;
            if(value.Length == 11)
            {
                id = value;
            } 
            else
            {
                var match = Regex.Match(value, @"^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*");
                if (match != null && match.Groups[2].Length == 11)
                {
                    id = match.Groups[2].Value;
                }
            }

            if(id == null)
                return token;

            return string.Format(format, id);
        }
    }
}
