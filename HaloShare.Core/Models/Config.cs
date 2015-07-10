using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.Core.Models
{
    public class Config
    {
        /// <summary>
        /// http://forum.halo.click API key for authentication. Production only.
        /// </summary>
        public string ForumApiKey { get; set; }

        /// <summary>
        /// A 32 character string for AES Encryption
        /// </summary>
        public string TokenKey { get; set; }
        /// <summary>
        /// A 16 character string for AES Encryption
        /// </summary>
        public string TokenVector { get; set; }
    }
}
