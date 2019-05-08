using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQA.Models;
using SQA.Models.FacultyContext;
using SQA.Models.Identity;
using SQA.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQA.Controllers
{
    public class FacultyController : Controller
    {
        #region Private Variables
        readonly protected UserManager<User> _userManager;
        readonly protected RoleManager<IdentityRole> RoleManager;
        readonly protected SignInManager<User> _signInManager;
        readonly protected FacultyDBContext facultyDBContext;
        readonly protected IUserServices UserServices;
        protected static int PageSize = 10;

        protected Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        #endregion

        #region Constractor
        public FacultyController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserServices userServices,
            FacultyDBContext _facultyDBContext)
        {
            _userManager = userManager;
            RoleManager = roleManager;
            _signInManager = signInManager;
            facultyDBContext = _facultyDBContext;
            UserServices = userServices;
        }
        #endregion

        #region Protected Methods
        protected async Task<JsonResult> SearchJson(string Search)
        {
            var task = Task.Factory.StartNew<JsonResult>(() =>
            {
                var SearchResult = _userManager.Users.Select(x => x.Name).Where(x => x.ToLower()
                     .Contains(Search.ToLower())).ToList();
                SearchResult.Sort();
                JsonResult result = new JsonResult(SearchResult, new Newtonsoft.Json.JsonSerializerSettings { MaxDepth = 6 });
                return result;
            });
            await task;
            return task.Result;
        }

        protected List<IdentityError> AddErrorsFromResult(IdentityResult result)
        {
            return result.Errors.ToList();
        }

        protected async Task<UsersPaging> GetUserwithRoles(string Search, int Page)
        {
            List<UserandRole> Result = await GetUserwithRoles(Search);
            return new UsersPaging
            {
                UserandRoles = Result
                            .OrderBy(x => x.User.Name)
                            .Skip((Page - 1) * PageSize)
                            .Take(PageSize).ToList(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = Result.Count
                },
                Search = Search
            };
        }

        protected async Task<List<UserandRole>> GetUserwithRoles(string Search)
        {
            List<UserandRole> Result = new List<UserandRole>();
            List<User> users = new List<User>();
            if (string.IsNullOrEmpty(Search))
            {
                users = _userManager.Users.ToList();
            }
            else
            {
                users = _userManager.Users.Where(x => x.Name.ToLower()
               .Contains(Search.ToLower())).ToList();
            }
            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                try
                {
                    var Department = facultyDBContext.Departments.
                        SingleOrDefault(x => x.ID == user.Department);
                    var Faculty = facultyDBContext.
                        Faculties.SingleOrDefault(x => x.ID == Department.FacultyID).Name;
                    Result.Add(new UserandRole
                    {
                        User = user,
                        Role = role,
                        Faculty = Faculty,
                        Department = Department.Name
                    });
                }
                catch (Exception)
                {
                    Result.Add(new UserandRole
                    {
                        User = user,
                        Role = role
                    });
                }
            }
            return Result;
        }

        protected async Task<EditUser> EditUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            string Faculty = "";
            string dep = "";
            try
            {
                var Department = facultyDBContext.Departments.
                     SingleOrDefault(x => x.ID == user.Department);
                dep = Department.Name;
                Faculty = facultyDBContext.
                   Faculties.SingleOrDefault(x => x.ID == Department.FacultyID).Name;
            }
            catch { Faculty = ""; }
            return new EditUser
            {
                Id = user.Id,
                Department = dep,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Gender = user.Gender,
                Phone = user.PhoneNumber,
                Faculty = Faculty
            };
        }

        protected async Task<EditUser> EditUser(string username, string Search)
        {
            var user = await _userManager.FindByNameAsync(username);
            string Faculty = "";
            List<Department> myDepartment = null;
            try
            {
                var Department = facultyDBContext.Departments.
                     SingleOrDefault(x => x.ID == user.Department);
                Faculty = facultyDBContext.
                   Faculties.SingleOrDefault(x => x.ID == Department.FacultyID).ID;
                myDepartment = facultyDBContext.Departments.
                    Where(x => x.FacultyID == Faculty).ToList();
            }
            catch { Faculty = ""; }
            return new EditUser
            {
                Id = user.Id,
                Department = user.Department,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Gender = user.Gender,
                Phone = user.PhoneNumber,
                Faculty = Faculty,
                Search = Search,
                MyDepartment = myDepartment,
                Faculties = facultyDBContext.Faculties.ToList()
            };
        }
        #endregion
    }
}