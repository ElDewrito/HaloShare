using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaloShare.ViewModels
{
    public class UploadViewModel
    {
        public HttpPostedFileWrapper File { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        [StringLength(128)]
        public string Description { get; set; }

        [AllowHtml]
        public string Content { get; set; }
    }
}