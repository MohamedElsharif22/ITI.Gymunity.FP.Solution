using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.Admin.MVC.Controllers
{
    public class UsersController() : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
