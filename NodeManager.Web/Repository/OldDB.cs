using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;
using NodeManager.Web.Models.Entities;

namespace NodeManager.Web
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

        public OldDB(NodeManagerDB db)
        {
            RevProjects = new HashSet<RevitProject>();
            RevViews = new List<RevitView>();
            RevParameters = new List<RevitParameterOld>();
            //var res = new OldDB();
            FileName = db.Projects.First().Filepath;
            foreach(var project in db.Projects)
            {
                var p = new RevitProject(project);
                foreach(var view in project.Views)
                {
                    var v = new RevitView(view, p);
                    foreach(var parameter in view.Parameters)
                    {
                        RevParameters.Add(new RevitParameterOld(parameter, v));
                    }
                    RevViews.Add(v);
                }
                RevProjects.Add(p);
            }
        
        }
    }
}
