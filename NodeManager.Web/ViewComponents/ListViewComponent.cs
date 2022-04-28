 using Microsoft.AspNetCore.Http;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;
using Microsoft.AspNetCore.Mvc;
using NodeManager.Web.Abstract;
using System.Linq;
using NodeManager.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeManager.Web.ViewComponents
{
    [ViewComponent(Name = "List")]
    public class ListViewComponent : ViewComponent
    {
        private INodes repos;
        private readonly NodeManagerDBEntities dbContext;
        //public int pageSize = 4;

        public ListViewComponent(INodes repo)
        {
            repos = repo;
        }
        // GET
        public async Task<IViewComponentResult> InvokeAsync(string section, string category)
        {
            if (!repos.Categories.Any(x => x.Name == category))
            {
                category = (string)null;
            }
            if (!repos.Sections.Any(x => x.Name == section))
            {
                section = (string)null;
            }
            Categories cat = repos.Categories.FirstOrDefault(x => x.Name == category);
            Sections sec = repos.Sections.FirstOrDefault(x => x.Name == section);
            NodesViewModel model = new NodesViewModel()
            {
                Symbols = repos.FamilySymbols
                    .Where(x => (category == null || x.CategoryId == cat.Id) && (section == null || x.SectionId == sec.Id))
                    .OrderBy(x => x.Id).ToList(),
                CurrentSec = sec
            };
            CategorySection categorySection = new CategorySection();
            if (repos.FamilySymbols.Count() != 0)
            {
                var list = repos.FamilySymbols.ToList();
                foreach (var item in repos.Sections)
                {
                    categorySection.Menu.Add(item, list
                        .Where(x => x.Section == item)
                        .Select(symb => new Categories() { Id = symb.CategoryId.Value, Name = repos.Categories.FirstOrDefault(x => x.Id == symb.CategoryId).Name })
                        .GroupBy(p => p.Id)
                        .Select(g => g.First())
                        .OrderBy(x => x.Id));
                }
            }
            model.categorySection = categorySection;

            if (sec == null)
            {
                model.categorySection.SelectedSection = null;
            }
            else
            {
                model.categorySection.SelectedSection = sec.Id;
            }
            return View("List", model);
        }
    }
}
