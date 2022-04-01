using System.Collections.Generic;
using NodeManager.Domain;
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

        public List<string> tagList = new List<string> { ".tag1", ".tag2", ".tag3", ".tag4", ".tag5" };

        public bool IsTagSearchEmpty  = false;

    }
}