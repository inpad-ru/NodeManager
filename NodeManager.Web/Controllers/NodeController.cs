using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Session;

//using NodeManager.Domain;
using NodeManager.Web.Abstract;
using NodeManager.Web.Models;
using NodeManager.Web.Repository;
using NodeManager.Web.DBInfrastucture;

using System.Linq;
using System.Drawing;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using System.Data.Entity;
using System.Web;


namespace NodeManager.Web.Controllers
{
    [Route("")]
    [Route("Node")]
    public class NodeController : Controller
    {
        private INodes repos;
        private readonly IWebHostEnvironment _appEnvironment;
        private IHostingEnvironment Environment;
        private string logPath;
        public static List<string> logs;

        static NodeController()
        {
            logs = new List<string>();
        }

        public NodeController(INodes repo, IWebHostEnvironment appEnvironment)
        {
            repos = repo;
            _appEnvironment = appEnvironment;
            logPath = _appEnvironment.WebRootPath + "/Files/log.txt";
        }

        //~NodeController()
        //{
        //    Console.WriteLine("I'm here");
        //}

        [Route("")]
        [Route("List/{page:int}/{section?}/{category?}")]
        public async Task<ViewResult> List(string section, string category, int page = 1)
        {
            var pagInfo = new PagingInfo();
            pagInfo.ItemsPerPage = 12;
            pagInfo.CurrentPage = page;
            //var tagList = repos.Tags.Select(x => x.Value).ToListAsync();

            if (!repos.Categories.Any(x => x.Name == category))
                category = (string)null;

            if (!repos.Sections.Any(x => x.Name == section))
                section = (string)null;

            Users user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
                user = repos.Users.FirstOrDefault(x => x.Name.ToLower() == HttpContext.User.Identity.Name);
            logs.Add(String.Format(System.DateTime.Now.ToString() + " CurrentUser: {1}, Action: {0}", nameof(List), user == null ? "null" : user.Name));
            logs.Add(String.Format("Parameters - Section: {0}, Category: {1}, Page: {2}", section, category, page.ToString()));

            //Task<Categories> cat = repos.Categories.FirstOrDefaultAsync(x => x.Name.Equals(category));
            //Task<Sections> sec = repos.Sections.FirstOrDefaultAsync(x => x.Name.Equals(section));
            Categories cat = repos.Categories.FirstOrDefault(x => x.Name.Equals(category));
            Sections sec = repos.Sections.FirstOrDefault(x => x.Name.Equals(section));

            pagInfo.TotalItems = repos.FamilySymbols
                    .Where(x => (category == null || x.CategoryId == cat.Id) && (section == null || x.SectionId == sec.Id))
                    .Count();
            var nodes = repos.FamilySymbols
                    .Where(x => (category == null || x.CategoryId == cat.Id) && (section == null || x.SectionId == sec.Id))
                    .Skip(pagInfo.ItemsPerPage * (pagInfo.CurrentPage - 1))
                    .Take(pagInfo.ItemsPerPage)
                    .OrderBy(x => x.Id)
                    .ToListAsync();

            NodesViewModel model = new NodesViewModel() { CurrentSec = sec };
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.UserName = HttpContext.User.Identity.Name;
            //if(model.IsLogin)
            model.Role = user != null ? user.Role : -1;
            Dictionary<int, string> data = new Dictionary<int, string>();
            model.Symbols = await nodes;
            foreach (var file in repos.Files) data.Add(file.Id, file.FilePath);
            model.PrjList = data;

            model.tagList = await repos.Tags.Select(x => x.Value).ToListAsync();
            model.PagingInfo = pagInfo;
            model.categorySection = GetCategorySection();
            if (sec == null)
            {
                model.categorySection.SelectedSection = null;
            }
            else
            {
                model.categorySection.SelectedSection = sec.Id;
            }
            //PutLogsIntoFile();
            return View(model);
            //return View("AddFile");
        }

        [HttpGet]
        [Route("List")]
        public IActionResult List1()
        {
            return View();
        }

        [HttpPost]
        [Route("List1")]
        public ViewResult List1(NodeSearchModel inputData)
        {

            var pagInfo = new PagingInfo();
            pagInfo.ItemsPerPage = 12;
            pagInfo.CurrentPage = inputData.Page;
            if (!repos.Categories.Any(x => x.Name == inputData.Category))
            {
                inputData.Category = (string)null;
            }
            if (!repos.Sections.Any(x => x.Name == inputData.Section))
            {
                inputData.Section = (string)null;
            }

            Users user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
                user = repos.Users.FirstOrDefault(x => x.Name.ToLower() == HttpContext.User.Identity.Name);

            Categories cat = repos.Categories.FirstOrDefault(x => x.Name.Equals(inputData.Category));
            Sections sec = repos.Sections.FirstOrDefault(x => x.Name.Equals(inputData.Section));
            pagInfo.TotalItems = repos.FamilySymbols
                    .Where(x => (inputData.Category == null || x.CategoryId == cat.Id) && (inputData.Section == null || x.SectionId == sec.Id))
                    .Count();
            NodesViewModel model = new NodesViewModel()
            {
                Symbols = repos.FamilySymbols
                    .Where(x => (inputData.Category == null || x.CategoryId == cat.Id) && (inputData.Section == null || x.SectionId == sec.Id))
                    .Skip(pagInfo.ItemsPerPage * (pagInfo.CurrentPage - 1))
                    .Take(pagInfo.ItemsPerPage)
                    .OrderBy(x => x.Id)
                    .ToList(),
                CurrentSec = sec
            };
            model.categorySection = GetCategorySection();

            if (sec == null)
            {
                model.categorySection.SelectedSection = null;
            }
            else
            {
                model.categorySection.SelectedSection = sec.Id;
            }
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.UserName = HttpContext.User.Identity.Name;
            //if (model.IsLogin)
            model.Role = user != null ? user.Role : -1;
            Dictionary<int, string> data = new Dictionary<int, string>();
            foreach (var file in repos.Files) data.Add(file.Id, file.FilePath);
            model.PrjList = data;

            model.tagList = repos.Tags.Select(x => x.Value).ToList();
            model.PagingInfo = pagInfo;
            return View(model);
            //return View("AddFile");
        }

        [Route("Symbol/{id:int}")]
        public async Task<ViewResult> FamSymbol(int id)
        {
            Users user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
                user = repos.Users.FirstOrDefault(x => x.Name.ToLower() == HttpContext.User.Identity.Name);
            logs.Add(String.Format(System.DateTime.Now.ToString() + " CurrentUser: {1}, Action: {0}", nameof(FamSymbol), user == null ? "null" : user.Name));
            logs.Add(String.Format("Parameters - Id: {0}", id.ToString()));

            FamSymbolViewModel model = new FamSymbolViewModel()
            {
                _familySymbol = await repos.FamilySymbols.FirstOrDefaultAsync(x => x.Id == id),
                _revitParameters = repos.RevParameters
                    .Where(c => c.SymbolId == id)
                    .OrderBy(c => c.Id)
            };
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.UserName = HttpContext.User.Identity.Name;
            //if (model.IsLogin)
            model.Role = user != null ? user.Role : -1;
            //PutLogsIntoFile();
            return View(model);
        }

        [HttpPost]
        [Route("{page:int}/Search")]
        public async Task<IActionResult> Search(int page, string[] tags)
        {
            var pagInfo = new PagingInfo();
            pagInfo.ItemsPerPage = 12;
            pagInfo.CurrentPage = page;
            NodesViewModel model = new NodesViewModel();
            HashSet<int> tagsId = new HashSet<int>();
            List<FamilySymbol> resList = new List<FamilySymbol>();
            IEnumerable<int> connections;
            Users user = null;

            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                    user = repos.Users.FirstOrDefault(x => x.Name.ToLower() == HttpContext.User.Identity.Name);
                logs.Add(String.Format(System.DateTime.Now.ToString() + " CurrentUser: {1}, Action: {0}", nameof(Search), user == null ? "null" : user.Name));
                logs.Add(String.Format("Parameters - Page: {0}, Tags: {1}", page.ToString(), tags));

                foreach (var tag in tags)
                {
                    tagsId.Add((await repos.Tags.FirstOrDefaultAsync(x => x.Value.Equals(tag.ToLower()))).Id);
                }
                connections = repos.FSTags.Where(x => x.TagId == tagsId.First()).Select(x => x.FSId);

                foreach (var tag in tagsId.Skip(1))
                {
                    connections = connections.Intersect(repos.FSTags.Where(x => x.TagId == tag).Select(x => x.FSId));
                }

                var symbols = repos.FamilySymbols.ToList();
                foreach (var fs in connections)
                {
                    resList.Add(symbols.FirstOrDefault(x => x.Id == fs));
                }
            }
            catch (Exception ex)
            {
                resList = new List<FamilySymbol>();
                model.IsTagSearchEmpty = true;
            }
            pagInfo.TotalItems = resList.Count();


            model.Symbols = resList.GroupBy(x => x.Id)
                                   .Select(x => x.First())
                                   .Skip(pagInfo.ItemsPerPage * (pagInfo.CurrentPage - 1))
                                   .Take(pagInfo.ItemsPerPage)
                                   .ToList();

            Dictionary<int, string> data = new Dictionary<int, string>();
            foreach (var file in repos.Files) data.Add(file.Id, file.FilePath);
            model.PrjList = data;
            model.PagingInfo = pagInfo;
            model.categorySection = GetCategorySection();
            model.categorySection.SelectedSection = null;
            model.tagList = await repos.Tags.Select(x => x.Value).ToListAsync();
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.UserName = HttpContext.User.Identity.Name;
            //if (model.IsLogin)
            model.Role = user != null ? user.Role : -1;
            //PutLogsIntoFile();
            return View("List", model);
        }

        //[HttpPost]
        [Route("{page:int}/SearchName/{name}")]
        public async Task<IActionResult> SearchName(int page, string name)
        {
            var pagInfo = new PagingInfo();
            pagInfo.ItemsPerPage = 12;
            pagInfo.CurrentPage = page;

            name = name.ToLower();
            NodesViewModel model = new NodesViewModel();
            //HashSet<int> tagsId = new HashSet<int>();
            IEnumerable<int> connections;
            Task<List<FamilySymbol>> nodes = null;
            Users user = null;
            try
            {

                if (HttpContext.User.Identity.IsAuthenticated)
                    user = repos.Users.FirstOrDefault(x => x.Name.ToLower() == HttpContext.User.Identity.Name);
                logs.Add(String.Format(System.DateTime.Now.ToString() + " CurrentUser: {1}, Action: {0}", nameof(SearchName), user == null ? "null" : user.Name));
                logs.Add(String.Format("Parameters - Page: {0}, Name: {1}", page.ToString(), name));

                model.Symbols = repos.FamilySymbols.Where(x => x.Name.ToLower().Contains(name.ToLower()))
                                                   .Skip(pagInfo.ItemsPerPage * (pagInfo.CurrentPage - 1))
                                                   .Take(pagInfo.ItemsPerPage)
                                                   .ToList();
                //await nodes;
                //model.Symbols = nodes.Result;
            }
            catch (Exception ex)
            {
                model.Symbols = new List<FamilySymbol>();
                model.IsTagSearchEmpty = true;
            }


            model.PagingInfo = pagInfo;
            Dictionary<int, string> data = new Dictionary<int, string>();
            //model.Symbols = await nodes;
            foreach (var file in repos.Files) data.Add(file.Id, file.FilePath);
            model.PrjList = data;
            model.categorySection = GetCategorySection();
            model.categorySection.SelectedSection = null;
            model.tagList = await repos.Tags.Select(x => x.Value).ToListAsync();
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.UserName = HttpContext.User.Identity.Name;
            //if (model.IsLogin)
            model.Role = user != null ? user.Role : -1;

            pagInfo.TotalItems = await repos.FamilySymbols.Where(x => x.Name.ToLower().Contains(name)).CountAsync();
            //PutLogsIntoFile();
            return View("List", model);
        }



        [Authorize]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Users user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
                user = repos.Users.FirstOrDefault(x => x.Name.ToLower() == HttpContext.User.Identity.Name);
            logs.Add(String.Format(System.DateTime.Now.ToString() + " CurrentUser: {1}, Action: {0}", nameof(Delete), user == null ? "null" : user.Name));
            logs.Add(String.Format("Parameters - Id: {0}", id.ToString()));

            var fs = await repos.FamilySymbols.FirstOrDefaultAsync(x => x.Id == id);
            if (fs == null) return RedirectToAction("List", "Node");

            List<FSTags> fsTagsIds = new List<FSTags>();
            List<FamilySymbol> famSymbols = new List<FamilySymbol>();
            List<RevitParameter> revitParameters = new List<RevitParameter>();

            famSymbols.AddRange(repos.FamilySymbols.Where(x => x.FileId == fs.FileId));
            foreach (var i in famSymbols)
            {
                fsTagsIds.AddRange(repos.FSTags.Where(x => x.FSId == i.Id));
                revitParameters.AddRange(repos.RevParameters.Where(x => x.SymbolId == i.Id));
            }
            var file = repos.Files.FirstOrDefault(x => x.Id == fs.FileId);

            repos.dbContext.RemoveRange(famSymbols);
            repos.dbContext.RemoveRange(fsTagsIds);
            repos.dbContext.RemoveRange(revitParameters);
            repos.dbContext.Remove(file);
            await repos.dbContext.SaveChangesAsync();
            if (System.IO.File.Exists(file.FilePath)) System.IO.File.Delete(file.FilePath);
            //PutLogsIntoFile();
            return RedirectToAction("List", "Node");
        }

        [Route("GetFile/{id:int}")]
        public async Task<IActionResult> GetFile(int id)
        {
            Users user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
                user = repos.Users.FirstOrDefault(x => x.Name.ToLower() == HttpContext.User.Identity.Name);
            logs.Add(String.Format(System.DateTime.Now.ToString() + " CurrentUser: {1}, Action: {0}", nameof(GetFile), user == null ? "null" : user.Name));
            logs.Add(String.Format("Parameters - Id: {0}", id.ToString()));

            //List<string> logs = new List<string>();
            //logs.Add(System.DateTime.Now.ToString() + " GetFile - {");
            //logs.AddRange(Directory.GetFiles(_appEnvironment.WebRootPath + "/Files/").ToList());

            string file_path = _appEnvironment.WebRootPath + (await repos.Files.FirstOrDefaultAsync(x => x.Id == id)).FilePath;
            string file_type = "archive/.nmdb";
            string file_name = file_path.Split('/').Last();

            //logs.Add("After work");
            //logs.AddRange(Directory.GetFiles(_appEnvironment.WebRootPath + "/Files/").ToList());
            //logs.Add("} - GetFile");
            //logs.Add("");
            //System.IO.File.AppendAllLines(logPath, logs);
            //PutLogsIntoFile();
            return PhysicalFile(file_path, file_type, file_name);
        }

        [Route("{page:int}/ProjectSection/{fileId:int}")]
        public IActionResult ProjectSection(int page, int fileId)
        {

            var pagInfo = new PagingInfo();
            pagInfo.ItemsPerPage = 12;
            pagInfo.CurrentPage = page;

            var model = new NodesViewModel();
            Users user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
                user = repos.Users.FirstOrDefault(x => x.Name.ToLower() == HttpContext.User.Identity.Name);
            logs.Add(String.Format(System.DateTime.Now.ToString() + " CurrentUser: {1}, Action: {0}", nameof(GetFile), user == null ? "null" : user.Name));
            logs.Add(String.Format("Parameters - Page: {0}, FileId: {1}", page.ToString(), fileId.ToString()));

            pagInfo.TotalItems = repos.FamilySymbols.Where(x => x.FileId == fileId).Count();
            model.PagingInfo = pagInfo;
            model.Symbols = repos.FamilySymbols.Where(x => x.FileId == fileId)
                                               .Skip(pagInfo.ItemsPerPage * (pagInfo.CurrentPage - 1))
                                               .Take(pagInfo.ItemsPerPage)
                                               .ToList();
            model.categorySection = GetCategorySection(fileId);
            model.CurrentSec = null;
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.UserName = HttpContext.User.Identity.Name;
            //if (model.IsLogin)
            model.Role = user != null ? user.Role : -1;
            model.tagList = repos.Tags.Select(x => x.Value).ToList();
            model.IsProjectSection = true;
            //PutLogsIntoFile();
            return View("List", model);
        }


        [HttpGet]
        [Route("AddFile")]
        public IActionResult AddFile()
        {
            return View("AddFile");
        }

        [HttpPost]
        [Route("AddFile")]
        //[RequestFormLimits(MultipartBodyLengthLimit = 262144000)]
        //[RequestFormLimits(MultipartBodyLengthLimit = Int64.MaxValue)]
        [RequestSizeLimit(268435456)]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            List<string> logs = new List<string>();
            //System.IO.File.AppendAllText(logPath, "i'm here");
            //throw new NotImplementedException(uploadedFile.Length.ToString());
            try
            {
                Users user = null;
                if (HttpContext.User.Identity.IsAuthenticated)
                    user = repos.Users.FirstOrDefault(x => x.Name.ToLower() == HttpContext.User.Identity.Name);
                logs.Add(String.Format(System.DateTime.Now.ToString() + " CurrentUser: {1}, Action: {0}", nameof(AddFile), user == null ? "null" : user.Name));
                logs.Add(String.Format("Parameters - File Size: {0}", uploadedFile == null ? "null" : uploadedFile.Length.ToString()));

                if (uploadedFile == null) logs.Add("uploadedFile = null");
                else
                {
                    // путь к папке Files
                    string guid = Guid.NewGuid().ToString();
                    string root = _appEnvironment.WebRootPath;
                    string path = "/Files/" + guid + uploadedFile.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(root + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    if (!repos.Files.Where(x => x.FilePath == path).Any())
                    {
                        Files file = new Files { FilePath = path };
                        repos.dbContext.Add(file);
                        await repos.dbContext.SaveChangesAsync();
                    }

                    var db = new DBUploader(repos, _appEnvironment);

                    db.UploadToDB(root, path);
                }
                //System.IO.File.AppendAllLines(logPath, logs);
                //PutLogsIntoFile();
                return RedirectToAction("List", "Node");
            }
            catch (Exception ex)
            {
                //System.IO.File.AppendAllText(logPath, uploadedFile.ToString());
                logs.Add(ex.Message);
                //PutLogsIntoFile();
                return RedirectToAction("List", "Node");
            }
        }


        [Route("dbClean")]
        public IActionResult DBClean()
        {
            var fs = repos.FamilySymbols.Where(x => true);
            var fsTagIds = repos.FSTags.Where(x => true);
            var cat = repos.Categories.Where(x => true);
            var sec = repos.Sections.Where(x => true);
            var fi = repos.Files.Where(x => true);
            var tags = repos.Tags.Where(x => true);
            var revP = repos.RevParameters.Where(x => true);

            repos.dbContext.RemoveRange(revP);
            repos.dbContext.RemoveRange(fs);
            repos.dbContext.RemoveRange(fsTagIds);
            repos.dbContext.RemoveRange(tags);
            repos.dbContext.RemoveRange(cat);
            repos.dbContext.RemoveRange(sec);
            repos.dbContext.RemoveRange(fi);
            repos.dbContext.SaveChanges();
            return RedirectToAction("List", "Node");
        }

        [HttpGet]
        [Route("db")]
        public IActionResult DBLargeFile()
        {
            return View("LargeFile");
        }

        [HttpPost]
        [Route("db")]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public IActionResult Upload(IFormFile file, [FromServices] IHostingEnvironment env)
        {

            string fileName = $"{env.WebRootPath}\\{file.FileName}";

            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            ViewData["message"] = $"{file.Length} bytes uploaded successfully!";

            return View("List");
        }

        private CategorySection GetCategorySection(Nullable<int> fileId = null)
        {
            CategorySection categorySection = new CategorySection();
            if (repos.FamilySymbols.Any())
            {
                var list = repos.FamilySymbols.ToList();
                foreach (var item in repos.Sections)
                {
                    categorySection.Menu.Add(item, list
                        .Where(x => (fileId == null) || (x.FileId == fileId))
                        .Where(x => x.Section == item)
                        .Select(symb => new Categories() { Id = symb.CategoryId.Value, Name = repos.Categories.FirstOrDefault(x => x.Id == symb.CategoryId).Name })
                        .GroupBy(p => p.Id)
                        .Select(g => g.First())
                        .OrderBy(x => x.Id));
                    categorySection.SelectedNodeSearch = new NodeSearchModel();
                }
            }
            return categorySection;
        }
        [HttpPost]
        [Route("Logs")]
        public void LogsIn()
        {
            PutLogsIntoFile();
        }

        private void PutLogsIntoFile()
        {
            if (logs.Count() != 0)
            {
                System.IO.File.AppendAllLines(logPath, logs);
                logs.Clear();
            }
        }
    }
}