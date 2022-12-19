using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;

namespace NodeManager.Web.Abstract
{
    public interface INodes
    {
        NodeManagerDBEntities dbContext { get; }
        //IEnumerable<Node> Nodes { get; }
        IQueryable<FamilySymbol> FamilySymbols { get; }
        IQueryable<RevitParameter> RevParameters { get; }
        IQueryable<Categories> Categories { get; }
        IQueryable<Sections> Sections { get; }
        IQueryable<Tags> Tags { get; }
        IQueryable<FSTags> FSTags { get; }
        IQueryable<Users> Users { get; }
        IQueryable<Files> Files { get; }
    }
}