using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.Models
{
    public class User : IUser<int>
    { 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("member_id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string UserName { get; set; }

        [JsonProperty("members_display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("member_group_id")]
        public int GroupId { get; set; }

        [JsonProperty("member_banned")]
        [JsonConverter(typeof(Core.BoolJsonConverter))]
        public bool IsBanned { get; set; }

        [JsonProperty("title")]
        public string ForumTitle { get; set; }

        [JsonProperty("joined")]
        public string ForumJoined { get; set; }

        public DateTime Joined { get; set; }

        //[Display(Name = "Uploader Name")]
        //[StringLength(15, MinimumLength = 3)]
        //[RegularExpression("[a-zA-Z0-9 ]")]
        //public string UploaderName { get; set; }


        public virtual ICollection<GameMapVariant> GameMapVariants { get; set; }
        public virtual ICollection<GameTypeVariant> GameTypeVariants { get; set; }
    }
}
