using NodeManager.Domain;
using Microsoft.AspNetCore.Mvc;
using NodeManager.Web.Abstract;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using NodeManager.Web.Models;
using System.Drawing;
using System.IO;

namespace NodeManager.Web.Controllers
{
    [Route("")]
    [Route("Node")]

    public class NodeController : Controller
    {
        private INodes repos;
        private readonly NodeManagerDBEntities dbContext;
        //public int pageSize = 4;
        
        public NodeController(INodes repo)
        {
            repos = repo;
        }

        //[Route("")]

        //public ViewResult Index()
        //{ 
        //    return View();
        //}

        [Route("")]
        //[Route("List")]
        //[Route("List/{section:string}")]
        [Route("List/{section?}/{category?}")]


        public ViewResult List(string section, string category)
        {
            if (!repos.Categories.Any(x => x.Name == category))
            {
                category = (string) null;
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
            model.categorySection = GetCategorySection();

            if (sec == null)
            {
                model.categorySection.SelectedSection = null;
            }
            else
            {
                model.categorySection.SelectedSection = sec.Id;
            }
            
            return View(model);
        }

        [Route("Symbol/{id:int}")]
        public ViewResult FamSymbol(int id)
        {
            FamSymbolViewModel model = new FamSymbolViewModel()
            {
                _familySymbol = repos.FamilySymbols.FirstOrDefault(x => x.Id == id),
                _revitParameters = repos.RevParameters
                    .Where(c => c.FamilySymbol.Id == id)
                    .OrderBy(c=>c.Id)
            };
            return View(model);
        }

        [Route("Search/{tag}")]
        public ViewResult Search(string tag)
        {
            NodesViewModel model = new NodesViewModel();
            //FamilySymbol sym = repos.FamilySymbols.FirstOrDefault();
            foreach(var f in repos.FamilySymbols)
            {
                if (f.Tags == null) continue;
                var splitedTags = f.Tags.Split(";");
                foreach(var t in splitedTags)
                {
                    if (t.Equals(tag))
                    {
                        model.Symbols.Add(f);
                    }
                }
            }
            model.categorySection = GetCategorySection();
            model.categorySection.SelectedSection = null;


            //NodesViewModel model = new NodesViewModel();

            return View("List",model);
        }

        private CategorySection GetCategorySection()
        {
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
            return categorySection;
        }

        //[Authorize]
        ////[AcceptVerbs( HttpVerbs.Get )]
        //public ActionResult DisplayImage( int id )
        //{
        //    var symb = dbContext.FamilySymbols.FirstOrDefault(r => r.Id == id);
            
        //    return File( symb.Image, "image/jpg" );
        //}
        
        //public Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image returnImage = Image.FromStream(ms);
        //    return returnImage;
        //}
    }
}