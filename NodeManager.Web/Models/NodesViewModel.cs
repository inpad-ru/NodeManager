using System.Collections.Generic;
using NodeManager.Domain;

namespace NodeManager.Web.Models
{
    public class NodesViewModel
    {
        public IEnumerable<FamilySymbol> Symbols { get; set; }
       // public PagingInfo PagingInfo { get; set; }
        public Sections CurrentSec { get; set; }

        public CategorySection categorySection { get; set; }

    }
}