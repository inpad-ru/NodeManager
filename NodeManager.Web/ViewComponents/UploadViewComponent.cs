using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NodeManager.Web.Models;
using System.Linq;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;
using NodeManager.Web.Abstract;

namespace NodeManager.Web.ViewComponents
{
    [ViewComponent(Name = "Upload")]
    public class UploadViewComponent : ViewComponent
    {
        INodes repos;

        public UploadViewComponent(INodes r)
        {
            repos = r;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new NodesViewModel();
            return View("Upload", model);
        }
    }
}
