using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HaloShare.Core
{
    public static class Helpers
    {


        public static string ToBase64(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static MarkdownSharp.Markdown _markdown;
        public static string TransformMarkdown(this string markdown)
        {
            if (_markdown == null)
            {
                _markdown = new MarkdownSharp.Markdown(new MarkdownSharp.MarkdownOptions
                {
                    AutoHyperlink = true
                });
            }

            return _markdown.Transform(markdown);
        }

        public static string FromBase64(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string BytesToString(this long byteCount)
        {
            string[] suf = { "B", "Kb", "Mb", "Gb", "Tb", "Pb", "Eb" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }


        public static string RemoveIllegal(this string text)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(text, "");
        }

        public static string ToMD5(this Stream stream)
        {
            HashAlgorithm hasher = new MD5CryptoServiceProvider();
            hasher.Initialize();

            stream.Seek(0, System.IO.SeekOrigin.Begin);
            return BitConverter.ToString(hasher.ComputeHash(stream)).Replace("-", "");
        }
    }
}