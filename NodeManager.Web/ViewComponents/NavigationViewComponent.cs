using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using NodeManager.Web.Models;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;

namespace NodeManager.Web.ViewComponents
{
    [ViewComponent(Name = "Menu")]
    public class NavigationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CategorySection cat)
        {
            return View("NavBar", cat);
        }
    }
}
