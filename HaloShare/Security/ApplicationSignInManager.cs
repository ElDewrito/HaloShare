using HaloShare.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.Security
{
    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public async Task<SignInStatus> CookieSignInAsync(string passHash, string sessionId, bool isPersistent, bool rememberBrowser)
        {
            try
            {
                User authUser = await ForumApi.AuthenticateSession(passHash, sessionId);

                User user = await UserManager.FindByIdAsync(authUser.Id);
                if (user == null)
                {
                    var result = await UserManager.CreateAsync(authUser);
                    if (!result.Succeeded)
                        return SignInStatus.RequiresVerification;
                    user = authUser;
                }

                if (authUser.IsBanned)
                    return SignInStatus.LockedOut;

                await UserManager.UpdateAsync(user);
                await SignInAsync(user, isPersistent, rememberBrowser);
                return SignInStatus.Success;

            }
            catch
            {
                return SignInStatus.Failure;
            }
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            try
            {
                User authUser = await ForumApi.AuthenticateUser(userName, password);

                User user = await UserManager.FindByIdAsync(authUser.Id);
                if (user == null)
                {
                    var result = await UserManager.CreateAsync(authUser);
                    if (!result.Succeeded)
                        return SignInStatus.RequiresVerification;
                    user = authUser;
                }

                if (authUser.IsBanned)
                    return SignInStatus.LockedOut;

                await UserManager.UpdateAsync(user);
                await SignInAsync(user, isPersistent, true);

                return SignInStatus.Success;
            }
            catch
            {
                return SignInStatus.Failure;
            }
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
