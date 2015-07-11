using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HaloShare.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public int? AuthorId {get;set;}

        public string Description { get; set; }
        [Required]
        public string Url { get; set; }
        [Display(Name = "Handled")]
        public bool IsHandled { get; set; }
        [Display(Name = "Reported On")]
        public DateTime ReportedOn { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
    }
}