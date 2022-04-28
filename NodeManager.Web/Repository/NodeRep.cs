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

        public IEnumerable<FamilySymbol> FamilySymbols => dbContext.FamilySymbols;

        public IEnumerable<RevitParameter> RevParameters => dbContext.RevitParameters;

        public IEnumerable<Categories> Categories => dbContext.Categories;

        public IEnumerable<Sections> Sections => dbContext.Sections;

        public IEnumerable<Tags> Tags => dbContext.Tags;

        public IEnumerable<FSTags> FSTags => dbContext.FSTags;

        public IEnumerable<Users> Users => dbContext.Users;

        public IEnumerable<Files> Files => dbContext.Files;

        NodeManagerDBEntities INodes.dbContext => dbContext;
    }
}