using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServerFour.Misc;
using IdentityServerFour.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndentityServerFour.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly TestUserStore _users;
        private readonly IIdentityServerInteractionService _interaction;

        public UserController(IIdentityServerInteractionService interaction, TestUserStore users)
        {
            _users = users;
            _interaction = interaction;
        }

        public IActionResult Index()
        {
            var model = new UserListModel();

            model.Users = TestUsers.Users;  // TODO - add wrapper for this collection...

            return View(model);
        }

        public IActionResult Edit(string id)
        {
            var user = _users.FindBySubjectId(id);
            var model = new UserModel(user);
            model.CreateUser = false;
            
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new UserModel();
            model.CreateUser = true;

            return View("Edit", model);
        }

        [HttpPost]
        public IActionResult SaveUser(UserModel model)
        {
            if(model.CreateUser)
            {
                TestUsers.AddUser(model);
            }
            else
            {
                TestUsers.EditUser(model);
            }

            return RedirectToAction("Index");
        }
    }
}