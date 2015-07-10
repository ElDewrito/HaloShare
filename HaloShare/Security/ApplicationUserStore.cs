using HaloShare.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Disable complaints about async methods not using await.
#pragma warning disable 1998

namespace HaloShare.Security
{

    public class ApplicationUserStore : IUserStore<Models.User, int>, IUserPasswordStore<Models.User, int>, IUserRoleStore<Models.User, int>
    {
        private static Dictionary<int, string[]> roles = new Dictionary<int, string[]>()
        {
            { 1 /* Validating */,       new [] { "Guest" } },
            { 3 /* Members */,          new [] { "User" } },
            { 4 /* Administrators */,   new [] { "User", "Mod", "Admin" } },
            { 6 /* Moderators */,       new [] { "User", "Mod" } },
            { 7 /* Developers */,       new [] { "User", "Mod" } }
        };

        ApplicationDbContext _context;

        public ApplicationUserStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(User user)
        {
            user.UserName = user.ForumDisplayName.Length > 15 ? user.ForumDisplayName.Substring(0, 15) : user.ForumDisplayName;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(int userId)
        {
            return _context.Users.Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return _context.Users.Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            throw new NotImplementedException();
        }

       
        public async Task<IList<string>> GetRolesAsync(User user)
        {
            if (user.Id == 50) // User: Wombarly
                return roles[4]; // Administrators

            if (roles.ContainsKey(user.ForumGroupId))
                return roles[user.ForumGroupId];
            else return roles[1]; // Guest
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            string[] userRoles;
            if (roles.ContainsKey(user.ForumGroupId))
                userRoles = roles[user.ForumGroupId];
            else
                userRoles = roles[1]; // Guest

            if (user.Id == 50) // User: Wombarly
                userRoles = roles[4]; // Administrators
 
            return userRoles.Any(userRole => string.Compare(userRole, roleName, true) != -1);
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(User user)
        {
            var dbUser = _context.Users.Find(user.Id);

            dbUser.ForumDisplayName = user.ForumDisplayName;
            dbUser.ForumGroupId = user.ForumGroupId;
            dbUser.ForumJoined = user.ForumJoined;
            dbUser.ForumName = user.ForumName;
            dbUser.ForumTitle = user.ForumTitle;
            dbUser.IsBanned = user.IsBanned;
            await _context.SaveChangesAsync();
        }
    }
}

#pragma warning restore 1998