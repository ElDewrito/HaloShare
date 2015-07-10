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
        /// <summary>
        /// Display Name used for HaloShare and uploaded files.
        /// </summary>
        [StringLength(15, MinimumLength = 3)]
        public string UserName { get; set; }

 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("member_id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string ForumName { get; set; }

        [JsonProperty("title")]
        public string ForumTitle { get; set; }

        [JsonProperty("joined")]
        public string ForumJoined { get; set; }

        [JsonProperty("member_group_id")]
        public int ForumGroupId { get; set; }

        [JsonProperty("members_display_name")]
        public string ForumDisplayName { get; set; }

        [JsonProperty("member_banned")]
        [JsonConverter(typeof(Core.BoolJsonConverter))]
        public bool IsBanned { get; set; }



        public virtual ICollection<GameMapVariant> GameMapVariants { get; set; }
        public virtual ICollection<GameTypeVariant> GameTypeVariants { get; set; }
    }
}
