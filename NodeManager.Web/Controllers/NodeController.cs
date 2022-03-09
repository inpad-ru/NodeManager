using NodeManager.Domain;
using Microsoft.AspNetCore.Mvc;
using NodeManager.Web.Abstract;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using NodeManager.Web.Models;
using System.Drawing;
using System.IO;
using NodeManager.Web.Repository;
using System;
using System.Collections.Generic;

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

        [Route("Search/{tags}")]
        public ViewResult Search(string tags)
        {
            NodesViewModel model = new NodesViewModel();
            var splittedTags = tags.Split(';');
            HashSet<int> tagsId = new HashSet<int>();
            IEnumerable<int> connections;
            try
            {
                foreach (var tag in splittedTags)
                {
                    tagsId.Add(repos.Tags.FirstOrDefault(x => x.Value.Equals(tag)).Id);
                }
                connections = repos.FSTags.Where(x => x.TagId == tagsId.FirstOrDefault()).Select(x => x.FSId);

                foreach (var tag in tagsId.Skip(1))
                {
                    connections = connections.Intersect(repos.FSTags.Where(x => x.TagId == tag).Select(x => x.FSId));
                }

                var symbols = repos.FamilySymbols.ToList();
                foreach (var fs in connections)
                {
                    model.Symbols.Add(symbols.FirstOrDefault(x => x.Id == fs));
                }
            }
            catch (Exception ex)
            {
                model.Symbols = repos.FamilySymbols.ToList();
            }
            
            model.categorySection = GetCategorySection();
            model.categorySection.SelectedSection = null;

            return View("List", model);
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