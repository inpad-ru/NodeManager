using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeManager.Domain
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
		public RevitProject(string name, string filePath, int numberOfSaves, string tags)
		{
			//ID = id;
			Name = name;
			FilePath = filePath;
			NumberOfSaves = numberOfSaves;
			Tags = tags;
		}
	}

	public class RevitView
	{
		public int ID;
		public int? ProjectID;
		public string Name;
		public string ImagePath;
		public int Scale;
		public string Category;
		public string Section;
		public RevitProject RevProj;
		public string Tags { get; set; }


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
		public int? ViewID;
		public string Name;
		public string Value;
		public int StorageType;
		public RevitView RevView;

		public RevitParameterOld() { }


		public RevitParameterOld(int viewID, string name, string value, int storageType)
		{
			//ID = id;
			ViewID = viewID;
			Name = name;
			Value = value;
			StorageType = storageType;
		}
	}
}
