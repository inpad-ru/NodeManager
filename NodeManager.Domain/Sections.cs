namespace NodeManager.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Sections
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public Sections()
        {
            this.FamilySymbols = new HashSet<FamilySymbol>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<FamilySymbol> FamilySymbols { get; set; }
    }
}
