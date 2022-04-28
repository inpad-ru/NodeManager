using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;

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
		public string Tags { get; set; }

		public RevitProject()
		{ }
		public RevitProject(string name)
		{
			Name = name;
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
	}

	public class RevitParameterOld
	{
		//public int ID;
		public int? ViewID { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public int StorageType { get; set; }
		public RevitView RevView { get; set; }

		public RevitParameterOld() { }

	}
}
