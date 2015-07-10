using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HaloShare.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int? GameMapVariantId { get; set; }
        public int? GameTypeVariantId { get; set; }


        public int Rate { get; set; }
        public DateTime RatedOn { get; set; }


        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
    }
}