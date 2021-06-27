using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ACuteArtInterface.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Threading;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using ACuteArtInterface.Helper;
using Microsoft.Extensions.Configuration;
using ACuteArtInterface.Services;
using ACuteArtInterface.ViewModels;

namespace ACuteArtInterface.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ApplicationContext _context;
        public IConfiguration Configuration { get; }
        private readonly ILogger<ArtistController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        public ArtistController(ApplicationContext context, IConfiguration configuration, ILogger<ArtistController> logger)
        {
            _context = context;
            _logger = logger;
            Configuration = configuration;
        }

        #region Index and Details

        /// <summary>
        /// Returns all of the Artists with pagination
        /// </summary>
        /// <param name="sortOrder">sort order by name, order or artist name</param>
        /// <param name="currentFilter">current sort filter</param>
        /// <param name="searchString">search keyword</param>
        /// <param name="pageNumber">page number for navigation</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, List<ArtistModel> artistModel)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["OrderSortParm"] = sortOrder == "Order" ? "order_desc" : "Order";
            ViewData["ArtistSortParm"] = sortOrder == "Artist" ? "order_desc" : "Artist";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var artists = _context.Artists.Select(item => new ArtistModel
            {
                ArtistId = item.ArtistId,
                CreationTime = item.CreationTime,
                FullName = item.FullName,
                Guid = item.Guid,
                IconUrl = item.IconUrl,
                ImageUrl = item.ImageUrl,
                ThumbUrl = item.ThumbUrl,
                Active = item.Active,
                Name = item.Name,
                Order = item.Order,
                NameEn = Helper.Helper.GetStringByLanguage(item.Name, 0),
                FullNameEn = Helper.Helper.GetStringByLanguage(item.FullName, 0),
                Description = item.Description,
                DescriptionEn = Helper.Helper.GetStringByLanguage(item.Description, 30)
            });

            if (!String.IsNullOrEmpty(searchString))
            {
                artists = artists.Where(s => s.Name.Contains(searchString) || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    artists = artists.OrderByDescending(s => s.Name);
                    break;
                case "Order":
                    artists = artists.OrderBy(s => s.Order);
                    break;
                case "order_desc":
                    artists = artists.OrderByDescending(s => s.Order);
                    break;
                default:
                    artists = artists.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<ArtistModel>.CreateAsync(artists.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// GET: ArtistModel/Details/5
        /// </summary>
        /// <param name="id">artistId</param>
        /// <returns></returns>
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //finds the artist
            ArtistModel artist = await _context.Artists.FindAsync(id);

            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        #endregion Index and Details

        #region Create

        /// <summary>
        /// GET: ArtistModel/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: ArtistModel/Create
        /// </summary>
        /// <param name="artistModel">artist model to be created</param>
        /// <param name="IconUrlFile">icon url file</param>
        /// <param name="ImageUrlFile">image url file</param>
        /// <param name="ThumbUrlFile">thumb url file</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,FullName,Order,Active,IconUrl,ImageUrl,ThumbUrl")] ArtistModel artistModel, IFormFile IconUrlFile, IFormFile ImageUrlFile, IFormFile ThumbUrlFile)
        {
            //check the model is valid
            if (ModelState.IsValid)
            {
                artistModel.Active = true;
                artistModel.CreationTime = DateTime.UtcNow;
                artistModel.Guid = Guid.NewGuid().ToString();

                #region Upload File to azure storage

                List<IFormFile> allFiles = new List<IFormFile> { IconUrlFile, ImageUrlFile, ThumbUrlFile };

                foreach (IFormFile file in allFiles)
                {
                    if (file == null) { continue; }

                    //create file class to use uploading files
                    AzureBlobFile azureBlobFile = new AzureBlobFile(Enums.InterfaceFileTypes.Artist,
                                                                    file,
                                                                    await file.GetBytes(),
                                                                    artistModel.Name,
                                                                    string.Empty,
                                                                    Configuration.GetConnectionString("AccessKey"),
                                                                    Configuration.GetConnectionString("ContainerName"));

                    //create azure storace service
                    BlobStorageService objBlobService = new BlobStorageService(azureBlobFile.AccessKey, azureBlobFile.ContainerName);

                    //upload file to azure storage
                    string uploadedFileName = objBlobService.UploadFileToBlob(azureBlobFile);

                    //Set azure storage file nime into the model property
                    Helper.Helper.UpdateProperty(artistModel, azureBlobFile.PropertyName, uploadedFileName);
                }

                #endregion

                _context.Add(artistModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(artistModel);
        }

        #endregion Create

        #region Edit

        /// <summary>
        /// Artist/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        /// <summary>
        /// POST: Artist/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="artistModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long? id, ArtistModel artistModel)
        {

            if (id == null)
            {
                return NotFound();
            }

            //getting the artist
            var artistToUpdate = await _context.Artists.FirstOrDefaultAsync(m => m.ArtistId == id);
            if (ModelState.IsValid)
            {
                List<IFormFile> allFiles = new List<IFormFile> { artistModel.IconUrlFile, artistModel.ImageUrlFile, artistModel.ThumbUrlFile };

                //preparing the properties to update
                if (await TryUpdateModelAsync<ArtistModel>(
                    artistToUpdate,
                    "",
                    i => i.Name,
                    i => i.Description,
                    i => i.FullName,
                    i => i.Order,
                    i => i.Active,
                    i => i.IconUrl,
                    i => i.ImageUrl,
                    i => i.ThumbUrl))
                {
                    try
                    {
                        #region Upload File to azure storage

                        foreach (IFormFile file in allFiles)
                        {
                            if (file == null) { continue; }

                            //create file class to use uploading files
                            AzureBlobFile azureBlobFile = new AzureBlobFile(Enums.InterfaceFileTypes.Artist,
                                                                            file,
                                                                            await file.GetBytes(),
                                                                            artistToUpdate.Name,
                                                                            string.Empty,
                                                                            Configuration.GetConnectionString("AccessKey"),
                                                                            Configuration.GetConnectionString("ContainerName"));

                            //create azure storace service
                            BlobStorageService objBlobService = new BlobStorageService(azureBlobFile.AccessKey, azureBlobFile.ContainerName);

                            //upload file to azure storage
                            string uploadedFileName = objBlobService.UploadFileToBlob(azureBlobFile);

                            //if the previous file exists delete
                            //If necessary, open the remove option
                            //string previousPropertyBlobUrlToUpdate = Helper.Helper.GetPropertyValue(artistToUpdate, azureBlobFile.PropertyName);

                            //if (!string.IsNullOrEmpty(previousPropertyBlobUrlToUpdate))
                            //{
                            //    objBlobService.DeleteBlobData(previousPropertyBlobUrlToUpdate, azureBlobFile.FilePath);
                            //}

                            //Set azure storage file nime into the model property
                            Helper.Helper.UpdateProperty(artistToUpdate, azureBlobFile.PropertyName, uploadedFileName);
                        }

                        #endregion

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        _logger.LogError($"Artist -> Edit -> exception: {ex.ToString()}");

                        //Log the error (uncomment ex variable name and write a log.)
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(artistToUpdate);
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// GET: Artist/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        /// <summary>
        /// POST: Artist/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            ArtistModel artistToDelete = await _context.Artists
                .SingleAsync(i => i.ArtistId == id);

            artistToDelete.Active = true;
            _context.Artists.Remove(artistToDelete);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion Delete

        #region Dispose and Error

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion Dispose and Error
    }
}
