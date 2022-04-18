using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NodeManager.Domain
{
    public class DBUploader
    {
        //public void UploadToDB(XDocument xmlDoc, string[] allFiles)
        public void UploadToDB(string link = "C:/Users/user43/source/repos/DimaSaGit/NodeManager/NodeManager.Web/wwwroot/Files/База данных узлов_2019.zip")
        {
            try
            {
                var path = "wwwroot/Files/"+link.Split('/').Last().Split('.').First();
                //DirectoryInfo di = Directory.CreateDirectory(path);
                
                //ZipFile.ExtractToDirectory(link, path);

                var filesNames = Directory.GetFiles(path, "*.xml");
                var allImages = Directory.GetFiles(path + "/Images");

                //var i = 0;
                XDocument xmlDoc = XDocument.Load(filesNames.First());

                //string[] allFiles = Directory.GetFiles("//Mac/Home/Desktop/База данных узлов/Images");
                //foreach (string filename in allfiles)
                //{
                //    Console.WriteLine(filename);
                //}
                //Dictionary<int, string> images = new Dictionary<int, string>();
                //foreach (string file in allImages)
                //{
                //    var pathOfImg = file.Split('-', '.');
                //    images.Add(Convert.ToInt32(pathOfImg[1]), file);
                //}

                int counter = 0;

                using (NodeManagerDBEntities context = new NodeManagerDBEntities(new DbContextOptions<NodeManagerDBEntities>()))
                {
                    var xmlParser = new XMLParser();
                    var oldDb = xmlParser.XMLToObjects2(xmlDoc);

                    //foreach (var i in oldDb.RevProjects)
                    //{
                    //    if (!context.Nodes.Any(r => r.Name == i.Name))
                    //    {
                    //        context.Nodes0.Add(
                    //            new Node0
                    //            {
                    //                Name = i.Name,
                    //                //FilePath = i.FilePath
                    //            });
                    //    }
                    //    context.SaveChanges();
                    //}

                    //IEnumerable<Categories> cats = oldDb.RevViews
                    //    .Select(x => new Categories() { Name = x.Category })
                    //    .GroupBy(x => x.Name)
                    //    .Select(x => x.First());

                    //IEnumerable<Sections> sects = oldDb.RevViews
                    //    .Select(x => new Sections() { Name = x.Section })
                    //    .GroupBy(x => x.Name)
                    //    .Select(x => x.First());

                    //foreach (var i in cats)
                    //{
                    //    context.Categories.Add(
                    //        new Categories { Name = i.Name });
                    //}
                    //foreach (var i in sects)
                    //{
                    //    context.Sections.Add(
                    //        new Sections { Name = i.Name });
                    //}
                    //context.SaveChanges();


                    //foreach (var j in oldDb.RevViews)
                    //{
                    //    context.FamilySymbols.Add(
                    //        new FamilySymbol
                    //        {
                    //            //FamilyId = context.Nodes0.FirstOrDefault(r => r.Name == j.RevProj.Name).Id,
                    //            Name = j.Name,
                    //            //ImagePath = j.ImagePath,
                    //            Scale = j.Scale,
                    //            //Image = ImgToBytes(allfiles[counter]),
                    //            Image = ImgToBytes(images[j.ID]),
                    //            CategoryId = context.Categories.FirstOrDefault(x => x.Name == j.Category).Id,
                    //            SectionId = context.Sections.FirstOrDefault(x => x.Name == j.Section).Id
                    //        });
                    //    counter++;
                    //}
                    //context.SaveChanges();
                    //foreach (var z in oldDb.RevParameters)
                    //{
                    //    context.RevitParameters.Add(
                    //        new RevitParameter
                    //        {
                    //            SymbolId = context.FamilySymbols.FirstOrDefault(r => r.Name == z.RevView.Name).Id,
                    //            Name = z.Name,
                    //            Value = z.Value,
                    //            StorageType = z.StorageType
                    //        });
                    //}
                    //context.SaveChanges();
                }
            }
            catch { }
        }

        private byte[] ImgToBytes(string filename)
        {
            Image img = Image.FromFile(filename);
            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                var r = ms.ToArray();
                return ms.ToArray();
            }
        }
    }
}
