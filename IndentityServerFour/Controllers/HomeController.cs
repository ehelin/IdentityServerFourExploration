using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServerFour.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndentityServerFour.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        public IActionResult Index()
        {
            var result = GetToken().Result;

            var items = result?.Ticket?.Properties?.Items;
            if(items.ContainsKey(".Token.id_token"))
            {
                var idToken = items[".Token.id_token"];
                var accessToken = items[".Token.access_token"];
            }

            return View();
        }
        protected async Task<AuthenticateResult> GetToken()
        {
            var claims = (User as System.Security.Claims.ClaimsPrincipal).Claims;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var result = await HttpContext.AuthenticateAsync();

            return result;
        }

        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if(message != null)
            {
                vm.Error = message;
                message.ErrorDescription = null;
            }

            return View("Error", vm);
        }
    }
}