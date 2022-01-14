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
        // GET : Nav
        public PartialViewResult Menu(string category)
        {
            //ViewBag.SelectedCategory = category;
            //Node cat = repository.Nodes.FirstOrDefault(x => x.Name == category);
            IEnumerable<NodeInfo> categories = repository.Nodes
                .Select(symb => new NodeInfo()
                {
                    Node = symb,
                    IsSelected = category != null ? symb.Name == category : false
                });
                //.GroupBy(p => p.Node.Id)
                //.Select(g => g.First())
                //.OrderBy(x => x.Node.Id);
            return PartialView(categories);
        }
    }
}