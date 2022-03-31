using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NodeManager.Web.Models;
using System.Linq;
using NodeManager.Domain;
using NodeManager.Web.Abstract;

namespace NodeManager.Web.ViewComponents
{
    [ViewComponent(Name = "Parameters")]
    public class ParametersViewComponent : ViewComponent
    {
        INodes repos;

        public ParametersViewComponent(INodes r)
        {
                repos= r;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = new FamSymbolViewModel();
            model._familySymbol = repos.FamilySymbols.FirstOrDefault(x=>x.Id==id);
            model._revitParameters = repos.RevParameters.Where(x=>x.SymbolId==id).ToList();

            return View("Index", model);
        }
    }
}
