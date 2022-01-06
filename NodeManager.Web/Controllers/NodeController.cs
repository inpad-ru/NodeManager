using NodeManager.Domain;
using Microsoft.AspNetCore.Mvc;
using NodeManager.Web.Abstract;
using System.Linq;
using NodeManager.Web.Models;

namespace NodeManager.Web.Controllers
{
    public class NodeController : Controller
    {
        private INodes repos;
        private readonly NodeManagerDBEntities dbContext;
        public int pageSize = 4;
        
        public NodeController(INodes repo)
        {
            repos = repo;
        }
        
        public ViewResult List(string category, int page)
        {
            Node cat = repos.Nodes.FirstOrDefault(x => x.Name == category);
            //var nodeNAdo = repos.Nodes;
            var nado = repos.FamilySymbols.Where(x=>x.Scale==10);
            //var nodoRevParam = repos.RevParameters;
            NodesViewModel model = new NodesViewModel()
            {
                Symbols = repos.FamilySymbols
                    .Where(x => category == null || x.Node.Id == cat.Id)
                    .OrderBy(x => x.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    //TotalItems = 4
                    TotalItems = category == null ?
                        repos.FamilySymbols.Count() :
                        repos.FamilySymbols.Where(game => game.Node.Id == cat.Id).Count()
                },
                CurrentNode = cat
            };
            return View(model);
        }

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
    }
}