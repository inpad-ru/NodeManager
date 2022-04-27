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
using NodeManager.Web.Abstract;
using Microsoft.AspNetCore.Hosting;
using NodeManager.Domain;

namespace NodeManager.Web
{
    public class DBUploader
    {
        private INodes context;
        private readonly IWebHostEnvironment _appEnvironment;

        public DBUploader(INodes repo, IWebHostEnvironment appEnvironment)
        {
            context = repo;
            _appEnvironment = appEnvironment;
        }
        //public void UploadToDB(XDocument xmlDoc, string[] allFiles)
        //X:\Projects\NodeMan\NodeManager.Web\wwwroot\Files\База данных узлов_2019.zip
        public void UploadToDB(string link = "X:/Projects/NodeMan/NodeManager.Web/wwwroot/Files/База данных узлов_2019.zip")
        {
            try
            {
                var path = "wwwroot/Files/" + link.Split('/').Last().Split('.').First();
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
                Dictionary<int, string> images = new Dictionary<int, string>();
                foreach (string file in allImages)
                {
                    var pathOfImg = file.Split('-', '.');
                    images.Add(Convert.ToInt32(pathOfImg[1]), file);
                }

                //var xmlParser = new XMLParser();
                //var oldDb = xmlParser.XMLToObjects2(xmlDoc);

                int counter = 0;


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

                IEnumerable<Categories> cats = oldDb.RevViews
                    .Select(x => new Categories() { Name = x.Category })
                    .GroupBy(x => x.Name)
                    .Select(x => x.First());

                IEnumerable<Sections> sects = oldDb.RevViews
                    .Select(x => new Sections() { Name = x.Section })
                    .GroupBy(x => x.Name)
                    .Select(x => x.First());

                //context.dbContext.Categories.Add(new Categories { Name = "TestCat1" });
                //context.dbContext.SaveChanges();

                //foreach (var i in cats)
                //{
                //    if (context.Categories.Where(x => x.Name.Equals(i.Name)).Any())
                //    {
                //        context.dbContext.Categories.Add(
                //        new Categories { Name = i.Name });
                //    }
                //}
                //foreach (var i in sects)
                //{
                //    if (context.Sections.Where(x => x.Name.Equals(i.Name)).Any())
                //    {
                //        context.dbContext.Sections.Add(
                //        new Sections { Name = i.Name });
                //    }
                //}
                //context.SaveChanges();

                context.dbContext.Files.Add(new Files { FilePath = oldDb.FileName });
                context.dbContext.SaveChanges();

                foreach (var j in oldDb.RevViews)
                {
                    context.dbContext.FamilySymbols.Add(
                        new FamilySymbol
                        {
                                //FamilyId = context.Nodes0.FirstOrDefault(r => r.Name == j.RevProj.Name).Id,
                                Name = j.Name,
                                //ImagePath = j.ImagePath,
                                Scale = j.Scale,
                                //Image = ImgToBytes(allfiles[counter]),
                                Image = ImgToBytes(images[j.ID]),
                            FileId = context.Files.FirstOrDefault(x => x.FilePath.Equals(j.ImagePath)).Id,
                            CategoryId = context.Categories.FirstOrDefault(x => x.Name.Equals(j.Category)).Id,
                            SectionId = context.Sections.FirstOrDefault(x => x.Name.Equals(j.Section)).Id
                        });
                    context.dbContext.SaveChanges();
                    counter++;
                    if (j.Parameters.Any())
                    {
                        foreach (var c in j.Parameters)
                        {
                            context.dbContext.RevitParameters.Add(
                                new RevitParameter
                                {
                                    Name = c.Name,
                                    Value = c.Value,
                                    SymbolId = context.FamilySymbols.FirstOrDefault(x => x.Name.Equals(j.Name)).Id,
                                });
                        }
                    }
                    if (j.Tags.Any())
                    {
                        foreach (var p in j.Tags)
                        {
                            if (context.Tags.Where(x => x.Value.Equals(p)).Any())
                            {
                                context.dbContext.Tags.Add(new Tags
                                {
                                    Value = p
                                });
                                context.dbContext.SaveChanges();
                            }
                            context.dbContext.FSTags.Add(new FSTags
                            {
                                FSId = context.FamilySymbols.FirstOrDefault(x => x.Name.Equals(j.Name)).Id,
                                TagId = context.Tags.FirstOrDefault(x => x.Value.Equals(p)).Id
                            });
                        }
                    }
                }
                context.dbContext.SaveChanges();
                //foreach (var z in oldDb.RevParameters)
                //{
                //    context.dbContext.RevitParameters.AddAsync(
                //        new RevitParameter
                //        {
                //            SymbolId = context.FamilySymbols.FirstOrDefault(r => r.Name == z.RevView.Name).Id,
                //            Name = z.Name,
                //            Value = z.Value,
                //            StorageType = z.StorageType
                //        });
                //}
                //context.dbContext.SaveChanges();

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
