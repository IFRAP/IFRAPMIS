//using IFRAPMIS.Data;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IFRAPMIS.Services.Profile
{
    public class ProfileManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IHttpContextAccessor _httpContextAccessor;

        private ApplicationUser _currentUser;

        public ProfileManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;

                return _currentUser;
            }
        }

        public bool IsHasPassword(ApplicationUser user)
        {
            return _userManager.HasPasswordAsync(user).Result;
        }

        public bool IsEmailConfirmed(ApplicationUser user)
        {
            return _userManager.IsEmailConfirmedAsync(user).Result;
        }

        public async Task<bool> CurrentRole(string role)
        {
            _currentUser = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
            return await _userManager.IsInRoleAsync(_currentUser, role);
        }

       
    }
}