using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using NodeManager.Domain;
using NodeManager.Web.Abstract;

namespace NodeManager.Web.Repository
{
    //public class RevParamRep : IRevParam
    //{
    //    private readonly NodeManagerDBEntities dbContext;

    //    public RevParamRep(NodeManagerDBEntities dbContext)
    //    {
    //        this.dbContext = dbContext;
    //    }

    //    public IEnumerable<RevitParameter> AllRevParam => dbContext.RevitParameters.Include(c => c.Name);

    //    public IEnumerable<FamilySymbol> getFamSymbolWithRevParam(RevitParameter revId) => 
    //        dbContext.FamilySymbols.Where(c => c.Id == dbContext.RevitParameters.FirstOrDefault(p => p.Id == revId.Id).SymbolId).Include(r => r.Name);
        
    //}
}