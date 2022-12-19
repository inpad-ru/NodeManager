using System.Collections.Generic;

namespace NodeManager.Web.Models.Entities
{
    public class NodeManagerDB
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public List<RevitProject> Projects { get; set; }

        public NodeManagerDB(string name, string password = null)
        {
            Name = name;
            Password = password;
            Projects = new List<RevitProject>();
        }
        public NodeManagerDB()
        {
            Projects = new List<RevitProject>();
        }

        public List<RevitView> GetViews()
        {
            List<RevitView> views = new List<RevitView>();
            foreach (var proj in Projects)
                views.AddRange(proj.Views);
            return views;
        }
        public void SetParents()
        {
            foreach (var proj in Projects)
            {
                proj.Parent = this;
                foreach (var view in proj.Views)
                {
                    view.Parent = proj;
                    foreach (var param in view.Parameters)
                        param.Parent = view;
                }
            }
        }

        public void Dispose()
        {
            foreach (RevitProject proj in Projects)
                proj.Dispose();
        }
    }
}
