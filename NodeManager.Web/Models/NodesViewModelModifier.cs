using NodeManager.Domain;
using System.Collections.Generic;

namespace NodeManager.Web.Models
{
    public static class NodesViewModelModifier
    {
        public static IEnumerable<FamilySymbol> Add(this IEnumerable<FamilySymbol> Symbols, FamilySymbol symbol)
        {
            Symbols.Add(symbol);
            return Symbols;
        }

        public static NodesViewModel AddSymbol(this NodesViewModel model, FamilySymbol symbol)
        {
            model.Symbols.Add(symbol);
            return model;
        }
    }
}
