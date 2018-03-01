using Microsoft.AspNetCore.Mvc;

namespace JgMaschineAspWeb.Controllers
{
    public class RoleController : Controller
    {
        //private ApplicationRoleManager _roleManager;

        //public RoleController()
        //{
        //}

        //public RoleController(ApplicationRoleManager roleManager)
        //{
        //    RoleManager = roleManager;
        //}

        //public ApplicationRoleManager RoleManager
        //{
        //    get
        //    {
        //        return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
        //    }
        //    private set
        //    {
        //        _roleManager = value;
        //    }
        //}

        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
    }
}