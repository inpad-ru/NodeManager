using Microsoft.AspNetCore.Mvc;
using NodeManager.Web.Abstract;
using NodeManager.Domain;
using System.Linq;
using NodeManager.Web.Models;
using System.Collections.Generic;
//using System.Web.Mvc;

namespace NodeManager.Web.Controllers
{
    public class NavController : Controller
    {
        private INodes repository;

        public NavController(INodes repo)
        {
            repository = repo;
        }
        // GET
        public PartialViewResult Menu(string category)
        {
            CategorySection categorySection = new CategorySection();
            foreach(var item in repository.Sections)
            {
                categorySection.Menu.Add(item, repository.FamilySymbols
                    .Where(x => x.Section == item)
                    .Select(symb => new Categories() { Id = symb.Category.Id, Name = symb.Section.Name })
                    .GroupBy(p => p.Id)
                    .Select(g => g.First())
                    .OrderBy(x => x.Id));
            }
            //Categories cat = repository.Categories.FirstOrDefault(x => x.Name == category);





            //ViewBag.SelectedCategory = category;
            //Categories cat = repository.Categories.FirstOrDefault(x => x.Name == category);
            //IEnumerable<NodeInfo> categories = repository.FamilySymbols
            //    .Select(symb => new NodeInfo()
            //    {
            //        Category = symb.Category,
            //        IsSelected = category != null ? symb.Category.Id == cat.Id : false
            //    })
            //    .GroupBy(p => p.Category.Id)
            //    .Select(g => g.First())
            //    .OrderBy(x => x.Category.Id);
            return PartialView(categorySection);
        }
    }
}