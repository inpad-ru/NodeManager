using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NodeManager.Domain
{
    public class FSTags
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int FSId { get; set; }

        [ForeignKey("TagId")]
        public virtual Tags Tag { get; set; }

        [ForeignKey("FSId")]
        public virtual FamilySymbol Symbol { get; set; }
    }
}
