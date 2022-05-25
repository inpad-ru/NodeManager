using System.Collections.Generic;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;
using NodeManager.Web.Abstract;

namespace NodeManager.Web.Models
{
    public class NodesViewModel : IUser
    {
        public NodesViewModel()
        {
            Symbols = new List<FamilySymbol>();
        }

        public List<FamilySymbol> Symbols { get; set; }
        // public PagingInfo PagingInfo { get; set; }
        public Sections CurrentSec { get; set; }
        public CategorySection categorySection { get; set; }
        public string UserName { get; set; }
        public bool IsLogin { get; set; }
        public List<string> tagList { get; set; }

        public bool IsTagSearchEmpty  = false;
        public List<string> PrjList { get; set; }

        public bool IsProjectSection = false;
        public PagingInfo PagingInfo { get; set; }

    }
}