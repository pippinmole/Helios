using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace Helios.Data.Users;

public class AppUserManager : IAppUserManager {
    private readonly IConfiguration _configuration;
    private readonly ILogger<AppUserManager> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;

    public AppUserManager(IConfiguration configuration, ILogger<AppUserManager> logger,
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager, IMapper mapper) {
        _configuration = configuration;
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string role) {
        return _userManager.GetUsersInRoleAsync(role);
    }

    public async Task<SafeApplicationUser> GetSafeUserAsync(ClaimsPrincipal principal) {
        var user = await GetUserAsync(principal);
        return _mapper.Map<SafeApplicationUser>(user);
    }

    public async Task<SafeApplicationUser> GetSafeUserByIdAsync(string id) {
        var user = await GetUserByIdAsync(id);
        return _mapper.Map<SafeApplicationUser>(user);
    }

    public async Task<SafeApplicationUser> GetSafeUserByIdAsync(ObjectId id) {
        var user = await GetUserByIdAsync(id);
        return _mapper.Map<SafeApplicationUser>(user);
    }

    public Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal) {
        return _userManager.GetUserAsync(principal);
    }

    public IEnumerable<ApplicationUser> GetUsersWhere(Expression<Func<ApplicationUser, bool>> predicate) {
        return _userManager.Users.Where(predicate);
    }

    public Task<ApplicationUser> GetUserByIdAsync(string id) {
        if ( Guid.TryParse(id, out var guid) )
            return _userManager.FindByIdAsync(guid.ToString());

        _logger.LogWarning("User Guid given is not in the correct format!");
        return null;
    }

    public Task<ApplicationUser> GetUserByIdAsync(ObjectId id) {
        return _userManager.FindByIdAsync(id.ToString());
    }

    public Task<IdentityResult> UpdateUserAsync(ApplicationUser user) {
        return _userManager.UpdateAsync(user);
    }

    public Task<IdentityResult> RemoveUserAsync(ApplicationUser user) {
        return _userManager.DeleteAsync(user);
    }

    public Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user) {
        return _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public Task<string> GenerateEmailConfirmTokenAsync(ApplicationUser user) {
        return _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public Task<ApplicationUser> GetUserByEmailAsync(string? email) {
        return _userManager.FindByEmailAsync(email);
    }

    public Task SignOutAsync() {
        return _signInManager.SignOutAsync();
    }

    public Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string password) {
        return _userManager.ResetPasswordAsync(user, token, password);
    }

    public Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword) {
        return _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role) {
        return _userManager.AddToRoleAsync(user, role);
    }

    public Task<bool> IsUserInRole(ApplicationUser user, string role) {
        return _userManager.IsInRoleAsync(user, role);
    }

    public Task<IdentityResult> CreateAsync(ApplicationUser newAccount, string? password) {
        return _userManager.CreateAsync(newAccount, password);
    }

    public Task SignInAsync(ApplicationUser newAccount, bool isPersistent) {
        return _signInManager.SignInAsync(newAccount, isPersistent);
    }

    public Task<bool> VerifyUserTokenForLoginAsync(ApplicationUser user, string tokenProvider, string token) {
        return _userManager.VerifyUserTokenAsync(user, tokenProvider, "Login", token);
    }

    public Task<IdentityResult> ResetAccessFailedCountAsync(ApplicationUser identityUser) {
        return _userManager.ResetAccessFailedCountAsync(identityUser);
    }

    public Task<string> GenerateUserTokenAsync(ApplicationUser user, string defaultProvider, string login) {
        return _userManager.GenerateUserTokenAsync(user, defaultProvider, login);
    }

    public Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user) {
        return _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token) {
        return _userManager.ConfirmEmailAsync(user, token);
    }
}