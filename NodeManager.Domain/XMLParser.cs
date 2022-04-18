using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeManager.Domain
{
    public class XMLParser
    {
        public OldDB XMLToObjects2(XDocument doc)
        {
            RevitProject rpj = new RevitProject();
            RevitView rv = new RevitView();
            RevitParameterOld rpo = new RevitParameterOld();

            var db = new OldDB();
            db.FileName = doc.Element("NodeManagerDB")
                        .Elements("Projects")
                        .Elements("RevitProject")
                        .Nodes()
                        .Select(x => x as XElement)
                        .Where(x => x.Name.ToString().Equals("Filepath"))
                        .Select(x => x.Value)
                        .FirstOrDefault();

            //var x2 = x1.Where(x => x.Name.ToString().Equals("Filepath"))
            //           .Select(x => x.Value)
            //           .FirstOrDefault();

            //var x1 = doc.Element("NodeManagerDB")
            //            .Elements("Projects")
            //            .Elements("RevitProject")
            //            .Elements("Views")
            //            .Elements("RevitView")
            //            .Nodes()
            //            .Select(x => x as XElement)
            //            .Select(x => new RevitProject { FilePath =  x.})
                        



            foreach (XElement el in doc.Element("NodeManagerDB").Elements("Projects").Elements("RevitProject").Elements("Views").Elements("RevitView").Nodes().Select(x => x as XElement))
            {
                rpj = new RevitProject(el.Element("ViewName").Value.Split('_').First());
                db.RevProjects.Add(rpj);

                //var temp = el.

                if (el.Element("ViewName").Value.Split('_').Length <= 2) continue;
                rv = new RevitView(Convert.ToInt32(el.Element("ViewId").Value),
                    el.Element("ViewName").Value,
                    Convert.ToInt32(el.Element("Scale").Value),
                    rpj);
                //if(el.H)
                db.RevViews.Add(rv);
            }
            return db;
        }
    }
}
