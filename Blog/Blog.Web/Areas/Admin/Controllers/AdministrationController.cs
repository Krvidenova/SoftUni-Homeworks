namespace Blog.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class AdministrationController : AuthorController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}