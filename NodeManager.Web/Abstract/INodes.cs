using System.Collections;
using System.Collections.Generic;
using NodeManager.Domain;
namespace NodeManager.Web.Abstract
{
    public interface INodes
    {
        //IEnumerable<Node> Nodes { get; }
        IEnumerable<FamilySymbol> FamilySymbols { get; }
        IEnumerable<RevitParameter> RevParameters { get; }
        IEnumerable<Categories> Categories { get; }
        IEnumerable<Sections> Sections { get; }

    }
}