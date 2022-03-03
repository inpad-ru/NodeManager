using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using NodeManager.Web.Models;
using NodeManager.Domain;

namespace NodeManager.Web.ViewComponents
{
    [ViewComponent(Name = "Menu")]
    public class NavigationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CategorySection cat)
        {
            return View("Index", cat);
        }
    }
}
