using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Text.RegularExpressions;
using System.Web;

namespace HaloShare.BBCode
{
    public class Parser
    {
        private static CompositionContainer _container;

        private static IEnumerable<Lazy<IBBCode, IBBCodeData>> tags;

        public Parser()
        {
            if (_container == null)
            {
                var catalog = new AggregateCatalog();

                catalog.Catalogs.Add(new AssemblyCatalog(typeof(Parser).Assembly));

                _container = new CompositionContainer(catalog);

                _container.ComposeParts(this);

                tags = _container.GetExports<IBBCode, IBBCodeData>();
            }
        }

        public string Parse(string text)
        {
            text = HttpUtility.HtmlEncode(text).Replace("\n\r", "<br>");

            foreach (Lazy<IBBCode, IBBCodeData> code in tags)
            {
                var regexString = string.Format(@"\[{0}(?:=(.*?))?\](.*?)\[\/{0}\]", code.Metadata.Tag);
                Regex regex = new Regex(regexString, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

                text = regex.Replace(text, delegate(Match match)
                {
                    return code.Value.Parse(
                        match.Groups[0].Value,
                        match.Groups[1].Value.Trim(),
                        match.Groups[2].Value.Trim());
                });
            }

            return Regex.Replace(text, @"(\s)\s+", "$1");
        }
    }
}
