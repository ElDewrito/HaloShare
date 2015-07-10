using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HaloShare.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }
        public DateTime UploadedOn { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public long FileSize { get; set; }

        public int DownloadCount { get; set; }

        public string MD5 { get; set; }

        public virtual ICollection<FileDownload> Downloads { get; set; }
    }
}