using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloShare.Models
{
    public class GameMapVariantImage
    {
        public int Id { get; set; }
        public int GameMapVariantId { get; set; }

        public GameMapVariantImageType Type { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedOn { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum GameMapVariantImageType 
    {
        Gallery,
        Main
    }
}