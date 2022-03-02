using System.Collections.Generic;
using NodeManager.Domain;
namespace NodeManager.Web.Models
{
    public class CategorySection
    {
        public Dictionary<Categories, IEnumerable<Sections>> Menu { get; set; }
    }
}
