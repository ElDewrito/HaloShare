using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HaloShare.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{
    //    [StringLength(15, MinimumLength = 3)]
    //    public override string UserName { get; set; }

    //    public DateTime JoinedOn { get; set; }
    //    public DateTime LastActiveOn { get; set; }

    //    // Got to get that Xbox 360 feeling, yo.
    //    public string Motto { get; set; }
    //    public string Bio { get; set; }
    //    public string Avatar { get; set; }

    //    // Social Gatherings.
    //    public string Website { get; set; }
    //    public string SocialPSN { get; set; }
    //    public string SocialXBL { get; set; }
    //    public string SocialSteam { get; set; }
    //    public string SocialEvolve { get; set; }
    //    public string SocialTwitter { get; set; }


    //    public virtual ICollection<GameMapVariant> GameMapVariants { get; set; }
    //    public virtual ICollection<GameTypeVariant> GameTypeVariants { get; set; }

    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}
}