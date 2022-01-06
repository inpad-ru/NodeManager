using System.Collections.Generic;
using NodeManager.Domain;

namespace NodeManager.Web.Models
{
    public class FamSymbolViewModel
    {
        public FamilySymbol _familySymbol { get; set; }
        public IEnumerable<RevitParameter> _revitParameters { get; set; }
    }
}