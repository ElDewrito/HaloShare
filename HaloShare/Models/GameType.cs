using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace HaloShare.Models
{
    public class GameType
    {
        [Key]
        public int Id { get; set; }


        public string Name { get; set; }
        public string InternalName { get; set; }
        public int InternalId { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<GameTypeVariant> Variants { get; set; }
    }
}