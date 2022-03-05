using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NodeManager.Web.Abstract;
using NodeManager.Domain;
using System.Linq;
using NodeManager.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeManager.Web.ViewComponents
{
    [ViewComponent(Name = "NavigationMenu")]
    public class NavigationMenuViewComponent : ViewComponent
    {
        private INodes repository;

        public NavigationMenuViewComponent(INodes repo)
        {
            repository = repo;
        }
        // GET
        public async Task<IViewComponentResult> InvokeAsync(string category = null)
        {
            CategorySection categorySection = new CategorySection();
            //foreach (var item in repository.Categories)
            //{
            //    categorySection.Menu.Add(item, repository.FamilySymbols
            //        .Where(x => x.Category == item)
            //        .Select(symb => new Sections() { Id = symb.Section.Id, Name = symb.Section.Name })
            //        .GroupBy(p => p.Id)
            //        .Select(g => g.First())
            //        .OrderBy(x => x.Id));
            //}
            return View("NavigationMenu", categorySection);
        }
    }
}
