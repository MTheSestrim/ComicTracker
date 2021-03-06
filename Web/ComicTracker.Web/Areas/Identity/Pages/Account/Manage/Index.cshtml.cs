namespace ComicTracker.Web.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using ComicTracker.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
#pragma warning disable SA1309 // Field names should not begin with underscore
        private readonly UserManager<ApplicationUser> _userManager;
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning disable SA1309 // Field names should not begin with underscore
        private readonly SignInManager<ApplicationUser> _signInManager;
#pragma warning restore SA1309 // Field names should not begin with underscore

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public string UserName { get; set; }
        }

#pragma warning disable SA1201 // Elements should appear in the correct order
        private async Task LoadAsync(ApplicationUser user)
#pragma warning restore SA1201 // Elements should appear in the correct order
        {
            var userName = await this._userManager.GetUserNameAsync(user);

            this.Input = new InputModel
            {
                UserName = userName,
            };
        }

#pragma warning disable SA1202 // Elements should be ordered by access
        public async Task<IActionResult> OnGetAsync()
#pragma warning restore SA1202 // Elements should be ordered by access
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var username = await this._userManager.GetUserNameAsync(user);

            if (this.Input.UserName != username)
            {
                user.UserName = this.Input.UserName;

                var setUserNameResult = await this._userManager.UpdateAsync(user);

                if (!setUserNameResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set username.";
                    return this.RedirectToPage();
                }
            }

            await this._signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }
    }
}
