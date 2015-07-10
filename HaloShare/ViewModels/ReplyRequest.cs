using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HaloShare.ViewModels
{
    public class ReplyRequest
    {
        [Required]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}