using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NodeManager.Web.Models;
using System.Linq;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;
using NodeManager.Web.Abstract;

namespace NodeManager.Web.ViewComponents
{
    [ViewComponent(Name = "Tags")]
    public class TagsViewComponent : ViewComponent
    {
        INodes repos;

        public TagsViewComponent(INodes r)
        {
            repos = r;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new TagsModel();
            model.Tags = repos.Tags.Select(x => x.Value).ToList();
            return View("Index", model);
        }
    }
}
