using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeManager.Web.DBInfrastucture
{
    public class Tags
    {
        public Tags()
        {
            Symbols = new List<FSTags>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public virtual ICollection<FSTags> Symbols { get; set; }
    }
}
