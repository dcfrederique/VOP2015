using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Carcassonne_Desktop.Models;
using Carcassonne_Web.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System.Web.Security;
using Carcassonne_Web.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using Carcassonne_Webcarc;

namespace Carcassonne_Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {

        //private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ILogRepository logRepo;
        

        public ProfileController()
        {
            logRepo = new LogRepository(new CarcassonneContext());
        }

        public ProfileController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Profile
        public ActionResult Index(string sortOrder, string currentFilter, int? page, string searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.RankSortParm = sortOrder == "Rank" ? "rank_desc" : "Rank";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var users = from u in UserManager.Users
                           select u;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.UserName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                //case "Rank":
                //    users = users.OrderBy(s => s.Id);
                //    break;
                //case "rank_desc":
                //    users = users.OrderByDescending(s => s.Id);
                //    break;
                default:
                    users = users.OrderBy(s => s.UserName);
                    break;
            }
            
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Profile/Details/5
        public async Task<ActionResult> Details(string id)
        {
            ViewBag.UserID = User.Identity.GetUserId();
            return View("Profile",await UserManager.FindByIdAsync(id));
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Change(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            ViewBag.IsAdmin = await UserManager.IsInRoleAsync(user.Id, "Admin");
            ViewBag.IsBlocked = user.Blocked;
            return View("ChangeProfile",user);
        }

        [Authorize (Roles="Admin")]
        public async Task<ActionResult> SetAdminRights(string id, string role, string act)
        {
            if (role != null)
            {
                var rol = RoleManager.FindByName(role);
                if (rol == null)
                {
                    RoleManager.Create(new ApplicationRole(role));
                }
                IdentityResult result = null;
                if (act == "give")
                {
                    result = await UserManager.AddToRolesAsync(id, role); 
                }
                else
                {
                    result = await UserManager.RemoveFromRoleAsync(id, role); 
                }
                //if (!result.Succeeded)
                //{
                //    ModelState.AddModelError("", result.Errors.ToString());
                //}
            }
            return RedirectToAction("Change", new { id = id });
        }

        public ActionResult GetAbuseModal(string id)
        {
            return View("Abuse", new Abuse() { User = new ApplicationUser() { Id = id } });
        }

        public async Task<ActionResult> DidAbuse(Abuse abuse)
        {
            string id = abuse.User.Id;

            var user = await UserManager.FindByIdAsync(id);
            abuse.User = user;
            if (user.Abuse == null)
            {
                user.Abuse = new HashSet<Abuse>();
            }
            user.Abuse.Add(abuse);

            if (user.Abuse.Count == 3)
            {
                user.Blocked = true;
            }


            await UserManager.UpdateAsync(user);

            logRepo.InsertLog(new Log()
            {
                Category = LogType.Security,
                CategoryAttribute = "",
                Date = DateTime.Now,
                Message = String.Format("Player {0} has done something bad: {1}", user.UserName, abuse.Reason)
            });
            logRepo.Save();

            return RedirectToAction("Details",new{ id = id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Unblock(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user.Blocked)
            {
                user.Blocked = false;
                logRepo.InsertLog(new Log()
                {
                    Category = LogType.Security,
                    CategoryAttribute = "",
                    Date = DateTime.Now,
                    Message = String.Format("Player {0} was unblocked", user.UserName)
                });
                logRepo.Save();
            }
            await UserManager.UpdateAsync(user);
            return RedirectToAction("Change", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Block(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (!user.Blocked )
            {
                user.Blocked = true;
                logRepo.InsertLog(new Log()
                {
                    Category = LogType.Security,
                    CategoryAttribute = "",
                    Date = DateTime.Now,
                    Message = String.Format("Player {0} was blocked", user.UserName)
                });
                logRepo.Save();
            }
            await UserManager.UpdateAsync(user);
            return RedirectToAction("Change", new { id = id });
        }
    }
}
