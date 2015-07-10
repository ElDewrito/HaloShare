using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HaloShare.Core;

namespace HaloShare.Services
{
    public class TokenService
    {
        public const int DateTimeValid = 1;

        private SimpleAES simpleAes;

        public TokenService()
        {
            simpleAes = new SimpleAES(Configuration.Settings.TokenKey, Configuration.Settings.TokenVector);
        }

        public string CreateDateTimeToken()
        {
            DateTime time = DateTime.UtcNow;

            string token = time.ToString("s");
            token = simpleAes.EncryptToString(token);
            return token;
        }

        public bool ValidateDateTimeToken(string token)
        {
            string aes = simpleAes.DecryptString(token);

            DateTime time;
            if (DateTime.TryParse(aes, out time))
            {
                if (time > DateTime.UtcNow.AddHours(-DateTimeValid))
                    return true;
            }
            return false;
        }

    }
}