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
	internal class OldEntities
	{
	}

	public class RevitProject
	{
		//public int ID;
		public string Name { get; set; }
		public string FilePath { get; set; }
		public int NumberOfSaves { get; set; }
		public List<string> Tags { get; set; }

		public RevitProject()
		{ }
		public RevitProject(string name)
		{
			Name = name;
		}

		public RevitProject(Models.Entities.RevitProject proj)
        {
			Name = proj.Name;
			FilePath = proj.Filepath;
			NumberOfSaves = proj.NumberOfSaves;
			Tags = proj.Tags;
		}
	}

	public class RevitView
	{
		public int ID { get; set; }
		public int? ProjectID { get; set; }
		public string Name { get; set; }
		public string ImagePath { get; set; }
		public int Scale { get; set; }
		public string Category { get; set; }
		public string Section { get; set; }
		public List<string> Tags { get; set; }
		public RevitProject RevProj { get; set; }
		public List<RevitParameterOld> Parameters { get; set; }

		//public string Tags { get; set; }


		public RevitView()
		{ }
		public RevitView(int id, string name, int scale, RevitProject rp)
		{
			var n = name.Split('_');
			ID = id;
			//ProjectID = projectID;
			Name = String.Join("_", n.Skip(2));
			//ImagePath = imagePath;
			Scale = scale;
			RevProj = rp;
			Section = n.First();
			Category = n[1];
		}

		public RevitView(Models.Entities.RevitView view, RevitProject proj)
        {
			ID = view.ViewId;
			var splitedName = view.ViewName.Split('_');
			switch(splitedName.Count())
            {
				case 0:
				case 1:
					Section = "undefined";
					Category = "undefined";
					Name = view.ViewName;
					break;
				case 2:
					Section = splitedName[0];
					Category = "undefined";
					Name = String.Concat(splitedName.Skip(1));
					break;
				default:
					Section = splitedName[0];
					Category = splitedName[1];
					Name = String.Concat(splitedName.Skip(2));
					break;

			}

			ImagePath = view.ImagePath;
			Scale = view.Scale;
			Tags = view.Tags;
			Parameters = view.Parameters.Select(p => new RevitParameterOld(p, this)).ToList();
			RevProj = proj;
        }
	}

	public class RevitParameterOld
	{
		//public int ID;
		public int? ViewID { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public StorageType StorageType { get; set; }
		public RevitView RevView { get; set; }

		public RevitParameterOld() { }

		public RevitParameterOld(Models.Entities.RevitParameter param, RevitView view)
        {
			this.Name = param.Name;
			this.Value = param.Value.ToString();
			this.StorageType = param.StorageType;
			this.RevView = view;
			this.ViewID = view.ID;
        }

	}
}
