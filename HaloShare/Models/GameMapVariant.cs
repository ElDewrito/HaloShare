using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaloShare.Core;

//http://www.se7ensins.com/forums/threads/usermap-structure-thanks-shade45.45119/

namespace HaloShare.Models
{
    public class GameMapVariant
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public int GameMapId { get; set; }
        public int AuthorId { get; set; }


        [Required]
        [StringLength(16, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(128)]
        [Display(Name = "Description")]
        public string ShortDescription { get; set; }
        [Display(Name = "Content")]
        [AllowHtml]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Min Players")]
        public int? MinPlayers { get; set; }
        [Display(Name = "Max Players")]
        public int? MaxPlayers { get; set; }

        public double Rating { get; set; }
        public int RatingCount { get; set; }

        [Display(Name = "Staff Pick")]
        public bool IsStaffPick { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }
        [ForeignKey("GameMapId")]
        public virtual GameMap GameMap { get; set; }
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Reaction> Reactions { get; set; }

        public virtual ICollection<GameType> SupportedGameTypes { get; set; }

        public virtual string DescriptionHTML
        {
            get
            {
                return new BBCode.Parser().Parse(this.Description);
            }
        }

        public virtual string DownloadName
        {
            get
            {
                return string.Format("{0} ({1}).bin", this.Title.RemoveIllegal(), this.Author.UserName);
            }
        }

        public virtual string Slug
        {
            get
            {
                return string.Format("{0}-{1}", this.Id, this.Title.GenerateSlug());
            }
        }
    }
}