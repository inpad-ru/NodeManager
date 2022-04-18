using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

using NodeManager.Domain;
using NodeManager.Web.Abstract;
using NodeManager.Web.Models;
using NodeManager.Web.Repository;

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

        public NodeController(INodes repo, IWebHostEnvironment appEnvironment)
        {
            repos = repo;
            _appEnvironment = appEnvironment;
        }

        [Route("")]
        [Route("List/{section?}/{category?}")]
        public ViewResult List(string section, string category)
        {
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
            NodesViewModel model = new NodesViewModel()
            {
                Symbols = repos.FamilySymbols
                    .Where(x => (category == null || x.CategoryId == cat.Id) && (section == null || x.SectionId == sec.Id))
                    .OrderBy(x => x.Id).ToList(),
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
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.tagList = repos.Tags.Select(x => x.Value).ToList();
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
        [Route("Search")]
        public IActionResult Search(string[] tags)
        {
            NodesViewModel model = new NodesViewModel();
            HashSet<int> tagsId = new HashSet<int>();
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
                    model.Symbols.Add(symbols.FirstOrDefault(x => x.Id == fs));
                }
            }
            catch (Exception ex)
            {
                model.Symbols = repos.FamilySymbols.ToList();
                model.IsTagSearchEmpty = true;
            }

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

        [Route("ProjectSection/{fileId:int}")]
        public IActionResult ProjectSection(int fileId)
        {
            var model = new NodesViewModel();
            model.Symbols = repos.FamilySymbols.Where(x => x.FileId == fileId).ToList();
            model.categorySection = GetCategorySection(fileId);
            model.CurrentSec = null;
            model.UserName = HttpContext.User.Identity.Name;
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.tagList = repos.Tags.Select(x => x.Value).ToList();
            model.IsProjectSection = true;
        
            return View("List", model);
        }

        [Route("db")]
        public IActionResult DBUploader()
        {
            var db = new NodeManager.Domain.DBUploader();
            db.UploadToDB();
            var model = new NodesViewModel();
            model.Symbols = repos.FamilySymbols.ToList();
            model.categorySection = GetCategorySection();
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
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                Files file = new Files { FilePath = path };
                repos.dbContext.Add(file);
                repos.dbContext.SaveChanges();
                var db = new DBUploader();
                db.UploadToDB(path);
            }
            
            return RedirectToAction("List", "Node");
        }

        //[HttpPost]
        //[Route(nameof(UploadLargeFile))]
        //public async Task<IActionResult> UploadLargeFile()
        //{
        //    var request = HttpContext.Request;

        //    // validation of Content-Type
        //    // 1. first, it must be a form-data request
        //    // 2. a boundary should be found in the Content-Type
        //    if (!request.HasFormContentType ||
        //        !MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader) ||
        //        string.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))
        //    {
        //        return new UnsupportedMediaTypeResult();
        //    }

        //    var reader = new MultipartReader(mediaTypeHeader.Boundary.Value, request.Body);
        //    var section = await reader.ReadNextSectionAsync();

        //    // This sample try to get the first file from request and save it
        //    // Make changes according to your needs in actual use
        //    while (section != null)
        //    {
        //        var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
        //            out var contentDisposition);

        //        if (hasContentDispositionHeader && contentDisposition.DispositionType.Equals("form-data") &&
        //            !string.IsNullOrEmpty(contentDisposition.FileName.Value))
        //        {
        //            // Don't trust any file name, file extension, and file data from the request unless you trust them completely
        //            // Otherwise, it is very likely to cause problems such as virus uploading, disk filling, etc
        //            // In short, it is necessary to restrict and verify the upload
        //            // Here, we just use the temporary folder and a random file name

        //            // Get the temporary folder, and combine a random file name with it
        //            var fileName = Path.GetRandomFileName();
        //            var saveToPath = Path.Combine(Path.GetTempPath(), fileName);

        //            using (var targetStream = System.IO.File.Create(saveToPath))
        //            {
        //                await section.Body.CopyToAsync(targetStream);
        //            }

        //            return Ok();
        //        }

        //        section = await reader.ReadNextSectionAsync();
        //    }

        //    // If the code runs to this location, it means that no files have been saved
        //    return BadRequest("No files data in the request.");
        //}

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
                }
            }
            return categorySection;
        }
    }
}