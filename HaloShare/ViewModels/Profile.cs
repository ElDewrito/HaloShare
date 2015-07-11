using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloShare.ViewModels
{
    public class Profile
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public DateTime LastActiveOn { get; set; }
        public DateTime JoinedOn { get; set; }
        public IEnumerable<Models.GameMapVariant> Maps { get; set; }
        public IEnumerable<Models.GameTypeVariant> Types { get; set; }
    }
}