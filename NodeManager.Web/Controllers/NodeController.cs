using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

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
using System.Data.Entity;
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

        public NodeController(INodes repo, IWebHostEnvironment appEnvironment)
        {
            repos = repo;
            _appEnvironment = appEnvironment;
        }

        [Route("")]
        [Route("List/{page:int}/{section?}/{category?}")]
        public ViewResult List(string section, string category, int page = 1)
        {
            var pagInfo = new PagingInfo();
            pagInfo.ItemsPerPage = 12;
            pagInfo.CurrentPage = page;
            if (!repos.Categories.Any(x => x.Name == category))
            {
                category = (string)null;
            }
            if (!repos.Sections.Any(x => x.Name == section))
            {
                section = (string)null;
            }
            Categories cat = repos.Categories.FirstOrDefault(x => x.Name.Equals(category));
            Sections sec = repos.Sections.FirstOrDefault(x => x.Name.Equals(section));
            pagInfo.TotalItems = repos.FamilySymbols
                    .Where(x => (category == null || x.CategoryId == cat.Id) && (section == null || x.SectionId == sec.Id))
                    .Count();
            NodesViewModel model = new NodesViewModel()
            {
                Symbols = repos.FamilySymbols
                    .Where(x => (category == null || x.CategoryId == cat.Id) && (section == null || x.SectionId == sec.Id))
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
            model.UserName = HttpContext.User.Identity.Name;
            Dictionary<int, string> data = new Dictionary<int, string>();
            foreach (var file in repos.Files) data.Add(file.Id, file.FilePath);
            model.PrjList = data;
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.tagList = repos.Tags.Select(x => x.Value).ToList();
            model.PagingInfo = pagInfo;
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
        [Route("List")]
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
            model.UserName = HttpContext.User.Identity.Name;
            Dictionary<int, string> data = new Dictionary<int, string>();
            foreach (var file in repos.Files) data.Add(file.Id, file.FilePath);
            model.PrjList = data;
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.tagList = repos.Tags.Select(x => x.Value).ToList();
            model.PagingInfo = pagInfo;
            return View(model);
            //return View("AddFile");
        }

        [Route("Symbol/{id:int}")]
        public ViewResult FamSymbol(int id)
        {
            FamSymbolViewModel model = new FamSymbolViewModel()
            {
                _familySymbol = repos.FamilySymbols.FirstOrDefault(x => x.Id == id),
                _revitParameters = repos.RevParameters
                    .Where(c => c.SymbolId == id)
                    .OrderBy(c => c.Id)
            };
            model.UserName = HttpContext.User.Identity.Name;
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            return View(model);
        }

        [HttpPost]
        [Route("{id:int}/Search")]
        public IActionResult Search(int page, string[] tags)
        {
            var pagInfo = new PagingInfo();
            pagInfo.ItemsPerPage = 12;
            pagInfo.CurrentPage = page;
            NodesViewModel model = new NodesViewModel();
            HashSet<int> tagsId = new HashSet<int>();
            List<FamilySymbol> resList = new List<FamilySymbol>();
            IEnumerable<int> connections;
            try
            {
                foreach (var tag in tags)
                {
                    tagsId.Add(repos.Tags.FirstOrDefault(x => x.Value.Equals(tag.ToLower())).Id);
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

            model.Symbols = resList.Skip(pagInfo.ItemsPerPage * (pagInfo.CurrentPage - 1))
                                   .Take(pagInfo.ItemsPerPage)
                                   .ToList();
            Dictionary<int, string> data = new Dictionary<int, string>();
            foreach (var file in repos.Files) data.Add(file.Id, file.FilePath);
            model.PrjList = data;
            model.PagingInfo = pagInfo;
            model.categorySection = GetCategorySection();
            model.categorySection.SelectedSection = null;
            model.tagList = repos.Tags.Select(x => x.Value).ToList();
            model.UserName = HttpContext.User.Identity.Name;
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;

            return View("List", model);
        }

        [HttpPost]
        [Route("{id:int}/SearchName")]
        public IActionResult SearchName(int page, string name)
        {
            var pagInfo = new PagingInfo();
            pagInfo.ItemsPerPage = 12;
            pagInfo.CurrentPage = page;

            name = name.ToLower();
            NodesViewModel model = new NodesViewModel();
            //HashSet<int> tagsId = new HashSet<int>();
            IEnumerable<int> connections;
            try
            {
                model.Symbols = repos.FamilySymbols.Where(x => x.Name.ToLower().Contains(name))
                                                   .Skip(pagInfo.ItemsPerPage * (pagInfo.CurrentPage - 1))
                                                   .Take(pagInfo.ItemsPerPage)
                                                   .ToList();
            }
            catch (Exception ex)
            {
                model.Symbols = new List<FamilySymbol>();
                model.IsTagSearchEmpty = true;
            }
            pagInfo.TotalItems = repos.FamilySymbols.Where(x => x.Name.ToLower().Contains(name)).Count();
            model.PagingInfo = pagInfo;
            Dictionary<int, string> data = new Dictionary<int, string>();
            foreach (var file in repos.Files) data.Add(file.Id, file.FilePath);
            model.PrjList = data;
            model.categorySection = GetCategorySection();
            model.categorySection.SelectedSection = null;
            model.tagList = repos.Tags.Select(x => x.Value).ToList();
            model.UserName = HttpContext.User.Identity.Name;
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;

            return View("List", model);
        }



        [Authorize]
        [Route("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var fs = repos.FamilySymbols.FirstOrDefault(x => x.Id == id);
            var fsTagIds = repos.FSTags.Where(x => x.FSId == id);
            repos.dbContext.Remove(fs);
            repos.dbContext.RemoveRange(fsTagIds);
            repos.dbContext.SaveChanges();
            return RedirectToAction("List", "Node");
        }

        [Route("GetFile/{id:int}")]
        public IActionResult GetFile(int id)
        {
            string file_path = Path.Combine(_appEnvironment.ContentRootPath, repos.Files.FirstOrDefault(x => x.Id == id).FilePath);
            string file_type = "archive/rvt";
            return PhysicalFile(file_path, file_type);
        }

        [Route("{id:int}/ProjectSection/{fileId:int}")]
        public IActionResult ProjectSection(int page, int fileId)
        {
            var pagInfo = new PagingInfo();
            pagInfo.ItemsPerPage = 12;
            pagInfo.CurrentPage = page;
            
            var model = new NodesViewModel();

            pagInfo.TotalItems = repos.FamilySymbols.Where(x => x.FileId == fileId).Count();
            model.PagingInfo = pagInfo;
            model.Symbols = repos.FamilySymbols.Where(x => x.FileId == fileId)
                                               .Skip(pagInfo.ItemsPerPage * (pagInfo.CurrentPage - 1))
                                               .Take(pagInfo.ItemsPerPage)
                                               .ToList();
            model.categorySection = GetCategorySection(fileId);
            model.CurrentSec = null;
            model.UserName = HttpContext.User.Identity.Name;
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.tagList = repos.Tags.Select(x => x.Value).ToList();
            model.IsProjectSection = true;

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
        [RequestFormLimits(MultipartBodyLengthLimit = Int64.MaxValue)]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = _appEnvironment.WebRootPath + "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                Files file = new Files { FilePath = path };
                repos.dbContext.Add(file);
                repos.dbContext.SaveChanges();
                var db = new DBUploader(repos, _appEnvironment);
                db.UploadToDB(_appEnvironment.WebRootPath, path);
            }

            return RedirectToAction("List", "Node");
        }


        [Route("dbClean")]
        public IActionResult DBClean()
        {
            var fs = repos.FamilySymbols.Where(x => x.Id != null);
            var fsTagIds = repos.FSTags.Where(x => x.Id != null);
            var cat = repos.Categories.Where(x => x.Id != null);
            var sec = repos.Sections.Where(x => x.Id != null);
            var fi = repos.Files.Where(x => x.Id != null);
            var tags = repos.Tags.Where(x => x.Id != null);
            var revP = repos.RevParameters.Where(x => x.Id != null);

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
    }
}