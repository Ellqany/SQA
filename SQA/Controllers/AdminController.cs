using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQA.Models;
using SQA.Models.FacultyContext;
using SQA.Models.Identity;
using SQA.Services.Abstraction;
using System.Linq;
using System.Threading.Tasks;

namespace SQA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : FacultyController
    {
        #region Constractor
        public AdminController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserServices userServices,
            FacultyDBContext _facultyDBContext) :
            base(userManager, signInManager, roleManager,
                userServices, _facultyDBContext)
        {
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Index(string Search, int Page = 1, int PageSize = 10)
        {
            return View(await GetUserwithRoles(Search, Page));
        }

        [HttpGet]
        public async Task<IActionResult> List
            (string Search, int Page = 1, int PageSize = 10)
        {
            FacultyController.PageSize = PageSize;
            return View("ListUsers", await GetUserwithRoles(Search, Page));
        }

        [HttpGet]
        public IActionResult Create() =>
            View(new CreateUser()
            {
                Faculties = facultyDBContext.Faculties.ToList()
            });
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUser CreateUser)
        {
            if (ModelState.IsValid)
            {
                var result = await UserServices.Create(CreateUser);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Errors = AddErrorsFromResult(result);
                }
            }
            return View(CreateUser);
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await CurrentUser;
            return View(new ChangePassword() { UserName = user.UserName });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePassword ChangePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await UserServices.ChangePassword(ChangePassword);
                if (result.Succeeded)
                {
                    string Search = ChangePassword.Search;
                    TempData["Sucssess"] = "The password has been successfully changed";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Errors"] = AddErrorsFromResult(result);
                }
            }
            return View(ChangePassword);
        }

        public async Task<ActionResult> Delete(string uname, string Search)
        {
            User User = await _userManager.FindByNameAsync(uname);
            var Result = await _userManager.DeleteAsync(User);
            if (Result.Succeeded)
            {
                TempData["Sucssess"] = "The Student has been Deleted Successfully";
                return View("ListUsers", await GetUserwithRoles(Search, 1));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string uname, string Search) =>
            View(await EditUser(uname, Search));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUser EditUser)
        {
            if (ModelState.IsValid)
            {
                var result = await UserServices.Edit(EditUser);
                if (result.Succeeded)
                {
                    TempData["Sucssess"] = "Student Data has been successfully modified.";
                    int Page = 1;
                    string Search = EditUser.Search;
                    return RedirectToAction("Index", routeValues: new { Search, Page });
                }
                else
                {
                    TempData["error"] = AddErrorsFromResult(result);
                }
            }
            return View(EditUser);
        }
        public async Task<JsonResult> SearchResult(string search) =>
            await SearchJson(search);
    }
}