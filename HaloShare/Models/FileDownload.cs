using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HaloShare.Models
{
    public class FileDownload
    {
        [Key]
        public long Id { get;set;}
        public int FileId { get; set; }
        public int? UserId { get; set; }


        public string UserIP { get; set; }
        public DateTime DownloadedOn { get; set; }


        [ForeignKey("FileId")]
        public virtual File File { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}