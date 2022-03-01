using System.Collections.Generic;
using NodeManager.Domain;
using System.Linq;
using NodeManager.Web.Abstract;
using Microsoft.EntityFrameworkCore;

namespace NodeManager.Web.Repository
{
    public class NodeRep : INodes
    {
        private readonly NodeManagerDBEntities dbContext;

        public NodeRep(NodeManagerDBEntities dbContext)
        {
            this.dbContext = dbContext;
        }

        //public IEnumerable<Node> Nodes => dbContext.Nodes;

        public IEnumerable<FamilySymbol> FamilySymbols => dbContext.FamilySymbols;

        public IEnumerable<RevitParameter> RevParameters => dbContext.RevitParameters;

        public IEnumerable<Categories> Categories => dbContext.Categories;

        public IEnumerable<Sections> Sections => dbContext.Sections;
    }
}