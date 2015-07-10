using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloShare.Areas.Admin.Models
{
    public class HomeIndexViewModel
    {
        public int UploadCount { get; set; }
        public int DownloadCount { get; set; }
        public int ReactionCount { get; set; }
        public int AccountCount { get; set; }

        public IEnumerable<HaloShare.Models.GameMapVariant> ForgeVariants { get; set; }
        public IEnumerable<HaloShare.Models.GameTypeVariant> GameVariants { get; set; }
    }
}