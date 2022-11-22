using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

namespace NodeManager.Web.Models.Entities
{
    public class RevitProject
    {
        [XmlIgnoreAttribute]
        public NodeManagerDB Parent { get; set; }
        public string Name { get; set; }
        public string Filepath { get; set; }
        public int NumberOfSaves { get; set; }
        public List<RevitView> Views { get; set; }
        public List<string> Tags { get; set; }


        public RevitProject(string path, List<RevitView> views, List<string> tags = null)
        {
            Filepath = path;
            Name = Path.GetFileNameWithoutExtension(path);
            Views = views;
            Tags = tags != null ? tags : new List<string>();
        }
        public RevitProject()
        {
            Views = new List<RevitView>();
            Tags = new List<string>();
        }

        public void Dispose()
        {
            Parent = null;
            Tags.Clear();
            foreach (RevitView view in Views)
                view.Dispose();
        }
    }
}
