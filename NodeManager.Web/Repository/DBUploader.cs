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
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;

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
        public async void UploadToDB(string root, string path)
        {
            try
            {
                root = root.Replace("\\", "/");
                XDocument xmlDoc = null;
                Stream stream = null;
                Dictionary<int, string> streamDic = new Dictionary<int, string>();

                //var path = root + "/Files/" + link.Split('/').Last().Split('.').First();
                
                var temp = ZipFile.OpenRead(root + path).Entries;
                foreach (var entry in temp)
                {
                    if (entry.FullName.EndsWith(".xml"))
                    {
                        xmlDoc = XDocument.Load(entry.Open());
                        stream = entry.Open();
                    }
                    if (entry.FullName.EndsWith(".jpg"))
                    {
                        var splitedName = entry.FullName.Split('-', '.');
                        if (!Directory.Exists(root + "/Files/Images/")) Directory.CreateDirectory(root + "/Files/Images/");
                        string pathToImg = ("/Files/Images/" +  Guid.NewGuid().ToString() + splitedName[1] + ".jpg");
                        UploadImage(root + pathToImg, entry.Open());
                        streamDic.Add(Convert.ToInt32(splitedName[1]), pathToImg);
                    }
                }
                
                int counter = 0;

                var xmlParser = new XMLParser();
                var oldDb = new OldDB(xmlParser.XMLToObjects3(stream));
                //var oldDb2 = xmlParser.XMLToObjects2(xmlDoc);

                IEnumerable<Categories> cats = oldDb.RevViews
                    .Select(x => new Categories() { Name = x.Category })
                    .GroupBy(x => x.Name)
                    .Select(x => x.First());

                IEnumerable<Sections> sects = oldDb.RevViews
                    .Select(x => new Sections() { Name = x.Section })
                    .GroupBy(x => x.Name)
                    .Select(x => x.First());

                foreach (var i in cats)
                {
                    if (!context.Categories.Where(x => x.Name.Equals(i.Name)).Any())
                    {
                        context.dbContext.Categories.Add(
                        new Categories { Name = i.Name });
                    }
                }
                foreach (var i in sects)
                {
                    if (!context.Sections.Where(x => x.Name.Equals(i.Name)).Any())
                    {
                        context.dbContext.Sections.Add(
                        new Sections { Name = i.Name });
                    }
                }

                //if (!context.Files.Where(x => x.FilePath.Equals(oldDb.FileName)).Any())
                //{
                //    context.dbContext.Files.Add(new Files { FilePath = oldDb.FileName });
                //}
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
                            Image = streamDic.ContainsKey(j.ID) ? streamDic[j.ID] : null,
                            FileId = context.Files.FirstOrDefault(x => x.FilePath.Equals(path)).Id,
                            CategoryId = context.Categories.FirstOrDefault(x => x.Name.Equals(j.Category)).Id,
                            SectionId = context.Sections.FirstOrDefault(x => x.Name.Equals(j.Section)).Id
                        });
                    context.dbContext.SaveChanges();
                    counter++;
                    if (j.Parameters != null && j.Parameters.Any())
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
                    if (j.Tags != null && j.Tags.Any())
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

                if (oldDb.RevParameters.Any())
                {
                    foreach (var z in oldDb.RevParameters)
                    {
                        context.dbContext.RevitParameters.AddAsync(
                            new RevitParameter
                            {
                                SymbolId = context.FamilySymbols.FirstOrDefault(r => r.Name == z.RevView.Name).Id,
                                Name = z.Name,
                                Value = z.Value,
                                StorageType = (int)z.StorageType
                            });
                    }
                    context.dbContext.SaveChanges();
                }

                //File.Delete(link);
            }
            catch (Exception ex) 
            { 
                ex.ToString();
            }
        }

        private void UploadImage(string path, Stream stream)
        {
            try
            {
                using (var streamReader = new FileStream(path, FileMode.Create))
                {
                    stream.CopyTo(streamReader);
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
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
