using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloShare.Services
{
    public class UserService : BaseService
    {
        public int Count()
        {
            return db.Users.Count();
        }

        public Models.User Get(string userId)
        {
            return db.Users.Find(userId);
        }

        public ViewModels.Profile GetProfile(string userId)
        {
            return GetProfile(this.Get(userId));
        }

        public ViewModels.Profile GetProfileByUserName(string username)
        {
            return GetProfile(db.Users.First(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)));
        }

        private ViewModels.Profile GetProfile(Models.User user)
        {
            return new ViewModels.Profile
            {
                Id = user.Id,
                UserName = user.UserName,
                //LastActiveOn = user.LastActiveOn,
                //JoinedOn = user.JoinedOn,
                Maps = user.GameMapVariants.Where(n => n.IsDeleted == false).OrderBy(n => n.Rating).ThenBy(n => n.Title).Take(6),
                Types = user.GameTypeVariants.Where(n => n.IsDeleted == false).OrderBy(n => n.Rating).ThenBy(n => n.Title).Take(6)
            };
        }
    }
}