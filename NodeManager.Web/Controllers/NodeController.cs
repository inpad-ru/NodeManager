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


        public ViewResult List(string category, string section)
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
                    .OrderBy(x => x.Id),
                CurrentSec = sec
            };

            CategorySection categorySection = new CategorySection();
            if (repos.FamilySymbols.Count() != 0)
            {
                foreach (var item in repos.Sections)
                {
                    categorySection.Menu.Add(item, repos.FamilySymbols
                        .Where(x => x.Section == item)
                        .Select(symb => new Categories() { Id = symb.Category.Id, Name = symb.Category.Name })
                        .GroupBy(p => p.Id)
                        .Select(g => g.First())
                        .OrderBy(x => x.Id));
                }
            }
            if (sec == null)
            {
                categorySection.SelectedSection = null;
            }
            else
            {
                categorySection.SelectedSection = sec.Id;
            }
            model.categorySection=categorySection;
            return View(model);
        }

        [Route("Symbol")]
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

        [Route("Search")]
        public ViewResult Search(string tag)
        {
            NodesViewModel model = new NodesViewModel();
            FamilySymbol sym = repos.FamilySymbols.FirstOrDefault();
            foreach(var f in repos.FamilySymbols)
            {
                var splitedTags = f.Tags.Split(";");
                foreach(var t in splitedTags)
                {
                    if (t.Equals(tag))
                    {
                        model.AddSymbol(f);
                    }
                }
            }

            //NodesViewModel model = new NodesViewModel();
            
            return View(model);
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