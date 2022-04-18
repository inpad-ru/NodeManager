using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeManager.Domain
{
    public class OldDB
    {
        public string FileName { get; set; }
        public HashSet<RevitProject> RevProjects { get; set; }
        public List<RevitView> RevViews { get; set; }
        public List<RevitParameterOld> RevParameters { get; set; }

        public OldDB()
        {
            this.RevProjects = new HashSet<RevitProject>();
            this.RevViews = new List<RevitView>();
            this.RevParameters = new List<RevitParameterOld>();
        }
    }
}
