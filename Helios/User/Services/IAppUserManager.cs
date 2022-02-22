using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace Helios.Data.Users;

public interface IAppUserManager {

    Task<IList<ApplicationUser>> GetUsersInRoleAsync(string role);
    
    Task<SafeApplicationUser> GetSafeUserAsync(ClaimsPrincipal principal);
    Task<SafeApplicationUser> GetSafeUserByIdAsync(string id);
    Task<SafeApplicationUser> GetSafeUserByIdAsync(ObjectId id);

    Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal);
    Task<ApplicationUser> GetUserByIdAsync(string id);
    Task<ApplicationUser> GetUserByIdAsync(ObjectId id);
    Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
    Task<ApplicationUser> GetUserByEmailAsync(string? email);
    Task<IdentityResult> RemoveUserAsync(ApplicationUser user);
    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
    Task<string> GenerateEmailConfirmTokenAsync(ApplicationUser user);
    Task SignOutAsync();
    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string password);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    Task<bool> IsUserInRole(ApplicationUser user, string role);
    Task<IdentityResult> CreateAsync(ApplicationUser newAccount, string? password);
    Task SignInAsync(ApplicationUser newAccount, bool isPersistent);
    Task<bool> VerifyUserTokenForLoginAsync(ApplicationUser user, string tokenProvider, string token);
    Task<IdentityResult> ResetAccessFailedCountAsync(ApplicationUser identityUser);
    Task<string> GenerateUserTokenAsync(ApplicationUser user, string defaultProvider, string login);
    Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);

    IEnumerable<ApplicationUser> GetUsersWhere(Expression<Func<ApplicationUser, bool>> predicate);
}