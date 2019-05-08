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
    public class StudentController : FacultyController
    {
        #region Constractor
        public StudentController(UserManager<User> userManager,
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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await CurrentUser;
            return View(await EditUser(user.UserName));
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Registration() =>
            View(new CreateUser()
            {
                Faculties = facultyDBContext.Faculties.ToList()
            });
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(CreateUser CreateUser)
        {
            if (ModelState.IsValid)
            {
                var result = await UserServices.Create(CreateUser);
                if (result.Succeeded)
                {
                    var Result = await _signInManager.PasswordSignInAsync(CreateUser.UserName, CreateUser.Password,
                        true, lockoutOnFailure: true);
                    if (Result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.Errors = AddErrorsFromResult(result);
                }
            }
            return View(CreateUser);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var Result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,
                    true, lockoutOnFailure: true);
                if (Result.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid User name or Passowrd");
                }
            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(StudentController.Registration));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await CurrentUser;
            return View(new ChangePassword() { UserName = user.UserName });
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePassword ChangePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await UserServices.ChangePassword(ChangePassword);
                if (result.Succeeded)
                {
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string uname) =>
            View(await EditUser(uname, null));
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditUser EditUser)
        {
            if (ModelState.IsValid)
            {
                var result = await UserServices.Edit(EditUser);
                if (result.Succeeded)
                {
                    TempData["Sucssess"] = "Student Data has been successfully modified.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = AddErrorsFromResult(result);
                }
            }
            return View(EditUser);
        }

        #region Service
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerifyUserName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                return Json($"The user name {username} is already in use.");
            }
            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerifyEmail(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                return Json($"The user name {Email} is already in use.");
            }
            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckPassword(string Password)
        {
            var user = await CurrentUser;
            var result = await _userManager.CheckPasswordAsync(user, Password);
            if (result)
            {
                return Json($"You can not add the same password.");
            }
            return Json(true);
        }

        public IActionResult GetDepartment(string Faculty)
            => Json(facultyDBContext.Departments.
                Where(x => x.FacultyID == Faculty).ToList());

        #endregion
    }
}