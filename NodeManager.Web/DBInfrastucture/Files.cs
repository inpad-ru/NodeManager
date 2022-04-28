using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeManager.Web.DBInfrastucture
{
    public partial class Files
    {
        public Files()
        {
            this.FamilySymbols = new HashSet<FamilySymbol>();
        }
        public int Id { get; set; }
        public string FilePath { get; set; }
        public virtual ICollection<FamilySymbol> FamilySymbols { get; set; }
    }
}
