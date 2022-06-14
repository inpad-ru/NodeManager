using System.Collections.Generic;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;
using System.Linq;
using NodeManager.Web.Abstract;
using Microsoft.EntityFrameworkCore;

namespace NodeManager.Web.Repository
{
    public class NodeRep : INodes
    {
        private NodeManagerDBEntities dbContext;

        public NodeRep(NodeManagerDBEntities dbContext)
        {
            this.dbContext = dbContext;
        }

        //public IEnumerable<Node> Nodes => dbContext.Nodes;

        public IQueryable<FamilySymbol> FamilySymbols => dbContext.FamilySymbols;

        public IQueryable<RevitParameter> RevParameters => dbContext.RevitParameters;

        public IQueryable<Categories> Categories => dbContext.Categories;

        public IQueryable<Sections> Sections => dbContext.Sections;

        public IQueryable<Tags> Tags => dbContext.Tags;

        public IQueryable<FSTags> FSTags => dbContext.FSTags;

        public IQueryable<Users> Users => dbContext.Users;

        public IQueryable<Files> Files => dbContext.Files;

        NodeManagerDBEntities INodes.dbContext => dbContext;
    }
}