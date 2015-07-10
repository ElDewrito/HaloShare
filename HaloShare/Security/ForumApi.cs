using HaloShare.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.Security
{
    public static class ForumApi
    {
        private static string SessionEndpoint = "https://forum.halo.click/api/authenticate_session?session_password={0}&session_id={1}";
        private static string UserEndpoint = "https://forum.halo.click/api/authenticate_user?name={0}&password={1}";
        private static string EmailEndpoint = "https://forum.halo.click/api/authenticate_user?email={0}&password={1}";

        private static string apiKey
        {
            get
            {
                return Core.Configuration.Settings != null ? Core.Configuration.Settings.ForumApiKey ?? "" : "";
            }
        }

        public static async Task<User> AuthenticateSession(string sessionPassword, string sessionId)
        {
            HttpClient client = new HttpClient();

            string uri = string.Format(SessionEndpoint, sessionPassword, sessionId);

            if (!string.IsNullOrWhiteSpace(apiKey))
                uri += "&api_key=" + apiKey;

            return JsonConvert.DeserializeObject<User>(await client.GetStringAsync(uri));
        }

        public static async Task<User> AuthenticateUser(string name, string password)
        {
            HttpClient client = new HttpClient();

            string uri;
            if (name.Contains("@"))
                uri = String.Format(EmailEndpoint, name, password);
            else
                uri = String.Format(UserEndpoint, name, password);

            if (!string.IsNullOrWhiteSpace(apiKey))
                uri += "&api_key=" + apiKey;

            return JsonConvert.DeserializeObject<User>(await client.GetStringAsync(uri));
        }
    }
}
