using System.Collections.Generic;
using NodeManager.Domain;

namespace NodeManager.Web.Models
{
    public class NodesViewModel
    {
        public IEnumerable<FamilySymbol> Symbols { get; set; }
        public PagingInfo PagingInfo { get; set; }
        
        public Node CurrentNode { get; set; }
    }
}