using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.Core
{
    public static class Configuration
    {
        public static Models.Config Settings { get; set; }

        public static void Initialize(string fileName)
        {
            string content = File.ReadAllText(fileName);
            Settings = JsonConvert.DeserializeObject<Models.Config>(content);
        }
    }
}
