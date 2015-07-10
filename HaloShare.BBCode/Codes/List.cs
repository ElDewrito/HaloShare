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
    [ExportMetadata("Tag", "list")]
    public class List : IBBCode
    {
        private static string formatUnordered = "<ul>{0}</ul>";
        private static string formatOrdered = "<ol>{0}</ol>";

        public string Parse(string token, string parameter, string value)
        {
            if (parameter == "1")
                return string.Format(formatOrdered, value);
            else
                return string.Format(formatUnordered, value);
        }
    }
}
