using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HaloShare.Models
{
    public class Reaction
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int? GameMapVariantId { get; set; }
        public int? GameTypeVariantId { get; set; }
        public int? ParentReactionId { get; set; }


        public string Comment { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime PostedOn { get; set; }


        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
        [ForeignKey("ParentReactionId")]
        public virtual Reaction ParentReaction { get; set; }
    }
}