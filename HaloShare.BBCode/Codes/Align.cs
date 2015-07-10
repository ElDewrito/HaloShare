using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.BBCode.Codes
{
    [Export(typeof(IBBCode))]
    [ExportMetadata("Tag", "align")]
    public class Align : IBBCode
    {
        private static string format = "<div style=\"text-align: {0}\">{1}</div>";
        private static string[] validAlignments = new string[] { "left", "right", "center", "justify" };

        public string Parse(string token, string parameter, string value)
        {
            parameter = parameter.ToLower();
            // Check if the parameter is valid to block CSS injection.
            if (!validAlignments.Contains(parameter))
                parameter = "left";

            return string.Format(format, parameter, value);
        }
    }
}
