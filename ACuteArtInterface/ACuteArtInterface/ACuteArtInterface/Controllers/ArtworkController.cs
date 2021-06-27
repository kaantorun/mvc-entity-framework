using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ACuteArtInterface.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using ACuteArtInterface.Services;
using ACuteArtInterface.Helper;
using ACuteArtInterface.Enums;
using Microsoft.Extensions.Logging;

namespace ACuteArtInterface.Controllers
{
    public class ArtworkController : Controller
    {
        private readonly ApplicationContext _context;

        public IConfiguration Configuration { get; }
        private readonly ILogger<ArtworkController> _logger;

        /// <summary>
        /// Artwork controller constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        public ArtworkController(ApplicationContext context, IConfiguration configuration, ILogger<ArtworkController> logger)
        {
            _context = context;
            _logger = logger;
            Configuration = configuration;
        }

        #region Index and Details

        /// <summary>
        /// Returns all of the Artworks with pagination
        /// </summary>
        /// <param name="sortOrder">sort order by name, artist name or year</param>
        /// <param name="currentFilter">current sort filter</param>
        /// <param name="searchString">search keyword</param>
        /// <param name="pageNumber">page number for navigation</param>
        /// <returns></returns>
        [Authorize]
        [RequestSizeLimit(737280000)]
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ArtistSortParm"] = sortOrder == "Artist" ? "artist_desc" : "Artist";
            ViewData["YearSortParm"] = sortOrder == "Year" ? "year_desc" : "Year";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            //item types combo
            List<TypeItem> typeItems = Helper.Helper.GetTypeItems();

            //get the artwork and fixing the english tag clear
            var artworks = _context.Artworks.Include(d => d.Artist).Select(item => new ArtworkModel
            {
                ApSize = item.ApSize,
                ArtistId = item.ArtistId,
                ArtworkId = item.ArtworkId,
                Color = (item.Color != null && item.Color.Length > 10) ? item.Color.Substring(0, 10) : item.Color,
                Creation = item.Creation,
                EditionSize = item.EditionSize,
                Active = item.Active,
                Guid = item.Guid,
                IconUrl = item.IconUrl,
                ImageUrl = item.ImageUrl,
                Name = item.Name,
                ObjectUrl = item.ObjectUrl,
                PrefabName = item.PrefabName,
                Preload = item.Preload,
                SignUrl = item.SignUrl,
                Size = item.Size,
                SizeDescription = item.SizeDescription,
                SourceMeta = (item.SourceMeta != null && item.SourceMeta.Length > 30) ? item.SourceMeta.Substring(0, 30) : item.SourceMeta,
                MarshalCode = (item.MarshalCode != null && item.MarshalCode.Length > 10) ? item.MarshalCode.Substring(0, 10) : item.MarshalCode,
                ThumbUrl = item.ThumbUrl,
                TradeUrl = item.TradeUrl,
                Type = item.Type,
                WpId = item.WpId,
                Year = item.Year,
                Description = item.Description,
                DescriptionEn = Helper.Helper.GetStringByLanguage(item.Description, 30),
                NameEn = Helper.Helper.GetStringByLanguage(item.Name, 0),
                TypeItem = typeItems[item.Type],
                ArtistNameEn = Helper.Helper.GetStringByLanguage(item.Artist.Name, 0),
                ColorEn = Helper.Helper.GetStringByLanguage(item.Color, 0),
            }).ToList();

            //show the last text after the forward slash to fit the page
            foreach (ArtworkModel artwork in artworks)
            {
                if (!string.IsNullOrEmpty(artwork.ObjectUrl) && artwork.ObjectUrl.Contains('/'))
                {
                    artwork.ObjectUrl = artwork.ObjectUrl.Split('/').ToList().Last();
                }
            }

            //check the search string and compare the database columns if it is empty
            if (!String.IsNullOrEmpty(searchString))
            {
                artworks = artworks.Where(s =>
                (!string.IsNullOrEmpty(s.Name) && s.Name.ToUpper().Contains(searchString.ToUpper())) ||
                (!string.IsNullOrEmpty(s.Description) && s.Description.ToUpper().Contains(searchString.ToUpper())) ||
                (!string.IsNullOrEmpty(s.ArtistNameEn) && s.ArtistNameEn.ToUpper().Contains(searchString.ToUpper()))).ToList();
            }

            //sort by specific orders
            switch (sortOrder)
            {
                case "name_desc":
                    artworks = artworks.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Artist":
                    artworks = artworks.OrderBy(s => s.ArtistNameEn).ToList();
                    break;
                case "artist_desc":
                    artworks = artworks.OrderByDescending(s => s.ArtistNameEn).ToList();
                    break;
                case "Year":
                    artworks = artworks.OrderBy(s => s.Year).ToList();
                    break;
                case "year_desc":
                    artworks = artworks.OrderByDescending(s => s.Year).ToList();
                    break;
                default:
                    artworks = artworks.OrderBy(s => s.Name).ToList();
                    break;
            }

            int pageSize = 10;
            return View(PaginatedList<ArtworkModel>.Create(artworks, pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// GET: Artwork/Details/5
        /// returns the details of the selected artwork
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArtworkModel artwork = await _context.Artworks.FindAsync(id);

            if (artwork == null)
            {
                return NotFound();
            }

            artwork.Artist = _context.Artists.FirstOrDefault(user => user.ArtistId == artwork.ArtistId);


            return View(artwork);
        }

        #endregion Index and Details

        #region Create

        /// <summary>
        /// GET: Artwork/Create 
        /// </summary>
        /// <returns></returns>
        // 
        public ActionResult Create()
        {
            PopulateTypesDropDownList(0);
            PopulateArtistsDropDownList(0);

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exhibitionId"></param>
        private void PopulateTypesDropDownList(long typeId)
        {
            if (typeId == 0)
            {
                ViewData["TypeID"] = new SelectList(Helper.Helper.GetTypeItems(), "TypeId", "Description");
            }
            else
            {
                ViewData["TypeID"] = new SelectList(Helper.Helper.GetTypeItems(), "TypeId", "Description", typeId);
            }
        }

        private IEnumerable<SelectListItem> PopulateArtistsDropDownList(long? artistId)
        {
            // Initialization.
            SelectList lstobj = null;

            try
            {

                // Loading.
                var list = _context.Artists.OrderBy(item => item.Name).ToList()
                                  .Select(p =>
                                            new SelectListItem
                                            {
                                                Value = p.ArtistId.ToString(),
                                                Text = Helper.Helper.GetStringByLanguage(p.Name, 0),
                                                Selected = (artistId != null && artistId > 0 && p.ArtistId == artistId) ? true : false
                                            });

                if (artistId != null && artistId > 0)
                {
                    lstobj = new SelectList(list, "Value", "Text", artistId);
                }
                else
                {
                    lstobj = new SelectList(list, "Value", "Text");
                }

            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }

            ViewBag.ArtistList = lstobj;

            // info.
            return lstobj;
        }

        /// <summary>
        /// POST: Artwork/Create
        /// </summary>
        /// <param name="artworkModel"></param>
        /// <param name="IconUrlFile"></param>
        /// <param name="ImageUrlFile"></param>
        /// <param name="ThumbUrlFile"></param>
        /// <param name="TradeUrlFile"></param>
        /// <param name="SignUrlFile"></param>
        /// <param name="iosFile"></param>
        /// <param name="androidFile"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(737280000)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ArtistId,Type,SourceMeta,WpId,PrefabName,MarshalCode,SizeDescription,Year,EditionSize,Color,Size,Active,IosFile,AndroidFile,ImageUrlFile,IconUrlFile,ThumbUrlFile,SignUrlFile,TradeUrlFile")] ArtworkModel artworkModel)
        {
            if (ModelState.IsValid)
            {
                artworkModel.Active = true;
                artworkModel.Guid = Guid.NewGuid().ToString();
                artworkModel.Creation = DateTime.UtcNow;
                artworkModel.WpId = Guid.NewGuid().ToString();
                artworkModel.ApSize = 1;
                artworkModel.Preload = false;
                artworkModel.Artist = await _context.Artists.FirstOrDefaultAsync(artist => artist.ArtistId == artworkModel.ArtistId);

                #region Upload File to azure storage

                List<IFormFile> allFiles = new List<IFormFile>
                {
                    artworkModel.IconUrlFile,
                    artworkModel.ImageUrlFile,
                    artworkModel.ThumbUrlFile,
                    artworkModel.TradeUrlFile,
                    artworkModel.SignUrlFile,
                    artworkModel.IosFile,
                    artworkModel.AndroidFile
                };

                foreach (IFormFile file in allFiles)
                {
                    if (file == null) { continue; }

                    //create file class to use uploading files
                    AzureBlobFile azureBlobFile = new AzureBlobFile(InterfaceFileTypes.Artwork,
                                                                    file,
                                                                    await file.GetBytes(),
                                                                    artworkModel.Artist.Name,
                                                                    artworkModel.Name,
                                                                    Configuration.GetConnectionString("AccessKey"),
                                                                    Configuration.GetConnectionString("ContainerName"));

                    //create azure storace service
                    BlobStorageService objBlobService = new BlobStorageService(azureBlobFile.AccessKey, azureBlobFile.ContainerName);

                    //upload file to azure storage
                    string uploadedFileName = objBlobService.UploadFileToBlob(azureBlobFile);

                    //Set azure storage file nime into the model property
                    Helper.Helper.UpdateProperty(artworkModel, azureBlobFile.PropertyName, uploadedFileName);
                }

                #endregion

                _context.Add(artworkModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateTypesDropDownList(0);
            PopulateArtistsDropDownList(0);

            return View(artworkModel);
        }

        #endregion Create

        #region Edit

        /// <summary>
        /// GET: Artwork/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks.Include(d => d.Artist)
                .FirstOrDefaultAsync(m => m.ArtworkId == id);
            if (artwork == null)
            {
                return NotFound();
            }

            PopulateTypesDropDownList(artwork.Type);
            PopulateArtistsDropDownList(artwork.ArtistId);

            return View(artwork);
        }

        /// <summary>
        /// POST: Artist/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="artworkModel"></param>
        /// <param name="IconUrlFile"></param>
        /// <param name="ImageUrlFile"></param>
        /// <param name="ThumbUrlFile"></param>
        /// <param name="TradeUrlFile"></param>
        /// <param name="SignUrlFile"></param>
        /// <param name="iosFile"></param>
        /// <param name="androidFile"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(737280000)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long? id, [Bind("ArtworkId,Name,Description,Creation,ArtistId,Type,SourceMeta,Guid,ObjectUrl,ImageUrl,IconUrl,ThumbUrl,SignUrl,TradeUrl,WpId,PrefabName,MarshalCode,SizeDescription,Year,EditionSize,Color,Size,ApSize,Preload,Active,IosFile,AndroidFile,ImageUrlFile,IconUrlFile,ThumbUrlFile,SignUrlFile,TradeUrlFile")] ArtworkModel artworkModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var artworkToUpdate = await _context.Artworks.Include(x => x.Artist).FirstOrDefaultAsync(m => m.ArtworkId == id);

                List<IFormFile> allFiles = new List<IFormFile>
                {
                    artworkModel.IconUrlFile,
                    artworkModel.ImageUrlFile,
                    artworkModel.ThumbUrlFile,
                    artworkModel.TradeUrlFile,
                    artworkModel.SignUrlFile,
                    artworkModel.IosFile,
                    artworkModel.AndroidFile
                };

                if (await TryUpdateModelAsync<ArtworkModel>(
                    artworkToUpdate,
                    "",
                    i => i.Name,
                    i => i.Description,
                    i => i.ArtistId,
                    i => i.Type,
                    i => i.SourceMeta,
                    i => i.ObjectUrl,
                    i => i.PrefabName,
                    i => i.MarshalCode,
                    i => i.SizeDescription,
                    i => i.Size,
                    i => i.Active,
                    i => i.Year,
                    i => i.EditionSize,
                    i => i.Color,
                    i => i.ImageUrl,
                    i => i.ThumbUrl,
                    i => i.IconUrl,
                    i => i.TradeUrl,
                    i => i.SignUrl))
                {
                    try
                    {
                        #region Upload File to azure storage

                        foreach (IFormFile file in allFiles)
                        {
                            if (file == null) { continue; }

                            //create file class to use uploading files
                            AzureBlobFile azureBlobFile = new AzureBlobFile(InterfaceFileTypes.Artwork,
                                                                            file,
                                                                            await file.GetBytes(),
                                                                            artworkToUpdate.Artist.Name,
                                                                            artworkToUpdate.Name,
                                                                            Configuration.GetConnectionString("AccessKey"),
                                                                            Configuration.GetConnectionString("ContainerName"));

                            //create azure storace service
                            BlobStorageService objBlobService = new BlobStorageService(azureBlobFile.AccessKey, azureBlobFile.ContainerName);

                            //upload file to azure storage
                            string uploadedFileName = objBlobService.UploadFileToBlob(azureBlobFile);

                            //if the previous file exists delete
                            //string previousPropertyBlobUrlToUpdate = Helper.Helper.GetPropertyValue(artworkToUpdate, azureBlobFile.PropertyName);
                            //If necessary, open the remove option
                            //if (!string.IsNullOrEmpty(previousPropertyBlobUrlToUpdate))
                            //{
                            //    objBlobService.DeleteBlobData(previousPropertyBlobUrlToUpdate, azureBlobFile.FilePath);
                            //}

                            //Set azure storage file nime into the model property
                            Helper.Helper.UpdateProperty(artworkToUpdate, azureBlobFile.PropertyName, uploadedFileName);
                        }

                        #endregion

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        _logger.LogError($"Artwork -> Edit -> exception: {ex.ToString()}");

                        //Log the error (uncomment ex variable name and write a log.)
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateTypesDropDownList(artworkModel.Type);
            PopulateArtistsDropDownList(artworkModel.ArtistId);

            return View(artworkModel);
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// GET: Artist/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="concurrencyError"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(long? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }
            ArtworkModel artwork = await _context.Artworks.FindAsync(id);
            if (artwork == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(artwork);
        }

        // POST: Artwork/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            ArtworkModel artwork = await _context.Artworks
                .SingleAsync(i => i.ArtistId == id);

            _context.Artworks.Remove(artwork);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Artwork/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(ArtworkModel artwork)
        {
            try
            {
                _context.Entry(artwork).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = artwork.ArtworkId });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(artwork);
            }
        }

        #endregion Delete

        #region Dispose and Error

        /// <summary>
        /// 
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
        /// 
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
