using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using NodeManager.Domain;

namespace NodeManager.Web
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

            //db.RevProjects = doc.Element("NodeManagerDB")
            //                    .Elements("Projects")
            //                    .Elements("RevitProject")
            //                    //.Elements("Views")
            //                    //.Nodes()
            //                    //.Select(x => x as XElement)
            //                    .Select(x => new RevitProject { Name = x.Element("Name").Value.Split('_').First() })
            //                    .ToHashSet();

            db.RevViews = doc.Element("NodeManagerDB")
                             .Elements("Projects")
                             .Elements("RevitProject")
                             .Elements("Views")
                             .Elements("RevitView")
                             .Where(x => x.Element("ViewName").Value.Split('_').Length > 2)
                             .Select(x => new RevitView
                             {
                                 Name = x.Element("ViewName").Value.Split('_').Last(),
                                 ID = Convert.ToInt32(x.Element("ViewId").Value),
                                 ImagePath = db.FileName,
                                 Scale = Convert.ToInt32(x.Element("Scale").Value),
                                 Category = x.Element("ViewName").Value.Split('_')[1],
                                 Section = x.Element("ViewName").Value.Split('_')[0],
                                 Tags = GetTags(x),
                                 Parameters = GetRevitParamsList(x.Elements("Parameters").Elements("Parameter"))
                             })
                             .ToList();



            //foreach (XElement el in doc.Element("NodeManagerDB").Elements("Projects").Elements("RevitProject").Elements("Views").Elements("RevitView"))
            //{
            //    //rpj = new RevitProject(el.Element("ViewName").Value.Split('_').First());
            //    //db.RevProjects.Add(rpj);
            //    var splitedName = el.Element("ViewName").Value.Split('_');

            //    if (splitedName.Length <= 2) continue;

            //    rv = new RevitView()
            //    {
            //        Name = splitedName.Last(),
            //        ID = Convert.ToInt32(el.Element("ViewId").Value),
            //        ImagePath = db.FileName,
            //        Scale = Convert.ToInt32(el.Element("Scale").Value),
            //        Category = splitedName[0],
            //        Section = splitedName[1],
            //        Tags = GetTags(el),
            //        Parameters = GetRevitParamsList(el.Elements("Parameters"))
            //    };
            //    db.RevViews.Add(rv);
            //}
            return db;
        }

        private List<string> GetTags(XElement view)
        {
            //List<string> result = null;
            try
            {
                if (view.Element("Tags").Value.Equals("")) return null;
                return view.Element("Tags").Value.Split(';').ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        private List<RevitParameterOld> GetRevitParamsList(IEnumerable<XElement> innerElements)
        {
            if (!innerElements.Descendants().Any()) return null;

            return innerElements.Select(x => new RevitParameterOld
            {
                Name = x.Element("Name").Value,
                Value = x.Element("Value").Value
            })
            .ToList();
        }

    }
}
