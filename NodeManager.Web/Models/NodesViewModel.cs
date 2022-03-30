using System.Collections.Generic;
using NodeManager.Domain;

namespace NodeManager.Web.Models
{
    public class NodesViewModel
    {
        public NodesViewModel()
        {
            Symbols = new List<FamilySymbol>();
        }

        public List<FamilySymbol> Symbols { get; set; }
        // public PagingInfo PagingInfo { get; set; }
        public Sections CurrentSec { get; set; }

        public CategorySection categorySection { get; set; }

        public string userName = null;

        public List<string> tagList = new List<string> { "tag1", "tag2", "tag3", "tag4", "tag5" };

    }
}