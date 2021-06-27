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
using Microsoft.AspNetCore.Http;
using ACuteArtInterface.Helper;
using ACuteArtInterface.Services;
using Microsoft.Extensions.Configuration;
using ACuteArtInterface.Enums;
using X.PagedList;

namespace ACuteArtInterface.Controllers
{
    public class ExhibitionController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ExhibitionController> _logger;

        public IConfiguration Configuration { get; }

        public ExhibitionController(ApplicationContext context, IConfiguration configuration, ILogger<ExhibitionController> logger)
        {
            _context = context;
            _logger = logger;
            Configuration = configuration;
        }

        #region Search

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="pageNumber"></param>
        /// <param name="exhibitionId"></param>
        /// <returns></returns>
        public ActionResult SearchUser(string sortOrder, string currentFilter, string searchString, int? pageNumber, long? exhibitionId)
        {
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = string.Empty;
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ExhibitionId"] = exhibitionId;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var users = _context.AccUsers.Select(item => new UserModel
            {
                UserId = item.UserId,
                Name = item.Name,
                LastName = item.LastName,
                Email = item.Email
            }).ToList();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(p => (p.Name != null && p.Name.ToLower().Contains(searchString.ToLower())) || (p.Email != null && p.Email.ToLower().Contains(searchString.ToLower()))).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    users = users.OrderBy(p => p.Name).ToList();
                    break;
            }

            int pageSize = 10;
            return PartialView("_SearchUser", users.ToPagedList(pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="pageNumber"></param>
        /// <param name="exhibitionId"></param>
        /// <returns></returns>
        public ActionResult AddArtwork(string sortOrder, string currentFilter, string searchString, int? pageNumber, long? exhibitionId)
        {
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = string.Empty;
            }

            ViewData["ExhibitionId"] = exhibitionId;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ArtworkExhibitionCheckModel model = new ArtworkExhibitionCheckModel();

            var artworks = _context.Artworks.Include(d => d.Artist).Select(item => new ArtworkModel
            {
                ArtistId = item.ArtistId,
                ArtworkId = item.ArtworkId,
                Active = item.Active,
                Name = item.Name,
                PrefabName = item.PrefabName,
                DescriptionEn = Helper.Helper.GetStringByLanguage(item.Description, 30),
                NameEn = Helper.Helper.GetStringByLanguage(item.Name, 0),
                ArtistNameEn = Helper.Helper.GetStringByLanguage(item.Artist.Name, 0),
            }).ToList();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                artworks = artworks.Where(p => (p.Name != null && p.Name.ToLower().Contains(searchString.ToLower())) || (p.Description != null && p.Description.ToLower().Contains(searchString.ToLower()))).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    artworks = artworks.OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    artworks = artworks.OrderBy(p => p.Name).ToList();
                    break;
            }

            int pageSize = 10;
            return PartialView("_AddArtwork", artworks.ToPagedList(pageNumber ?? 1, pageSize));
        }

        #endregion Search

        #region Index and Details

        /// <summary>
        /// Returns all of the Exhibitions with pagination
        /// </summary>
        /// <param name="sortOrder">sort order by name, order, start date or end date</param>
        /// <param name="currentFilter">current sort filter</param>
        /// <param name="searchString">search keyword</param>
        /// <param name="pageNumber">page number for navigation</param>
        /// <returns></returns>
        [Authorize]
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["OrderSortParm"] = sortOrder == "Order" ? "order_desc" : "Order";
            ViewData["StartDateSortParm"] = sortOrder == "StartDate" ? "start_date_desc" : "StartDate";
            ViewData["EndDateSortParm"] = sortOrder == "EndDate" ? "end_date_desc" : "EndDate";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var exhibitions = _context.Exhibitions.Include(d => d.Owner).Select(item => new ExhibitionModel
            {
                Active = item.Active,
                Created = item.Created,
                Description = item.Description,
                EndDate = item.EndDate,
                ExhibitionId = item.ExhibitionId,
                GeoFenced = item.GeoFenced,
                Howto = item.Howto,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                MetaData = item.MetaData,
                OwnerId = item.OwnerId,
                Radius = item.Radius,
                ScanRadius = item.ScanRadius,
                ShowRadius = item.ShowRadius,
                SponsorImages = item.SponsorImages,
                SponsorName = item.SponsorName,
                StartDate = item.StartDate,
                Title = item.Title,
                UseGps = item.UseGps,
                ViewRadius = item.ViewRadius,
                IntroUrl = item.IntroUrl,
                MainMapUrl = item.MainMapUrl,
                MapUrl = item.MapUrl,
                IconUrl = item.IconUrl,
                ThumbUrl = item.ThumbUrl,
                Owner = _context.AccUsers.FirstOrDefault(user => user.UserId == item.OwnerId),
                DescriptionEn = Helper.Helper.GetStringByLanguage(item.Description, 30),
            }).ToList();

            //TODO: null check control

            if (!String.IsNullOrEmpty(searchString))
            {
                exhibitions = exhibitions.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()) ||
                                                    (s.Description != null && s.Description.ToUpper().Contains(searchString.ToUpper())) ||
                                                    (s.Owner != null && s.Owner.Name != null && s.Owner.Name.ToUpper().Contains(searchString.ToUpper()))).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    exhibitions = exhibitions.OrderByDescending(s => s.Title).ToList();
                    break;
                case "Order":
                    exhibitions = exhibitions.OrderBy(s => s.Order).ToList();
                    break;
                case "order_desc":
                    exhibitions = exhibitions.OrderByDescending(s => s.Order).ToList();
                    break;
                case "StartDate":
                    exhibitions = exhibitions.OrderBy(s => s.StartDate).ToList();
                    break;
                case "start_date_desc":
                    exhibitions = exhibitions.OrderByDescending(s => s.StartDate).ToList();
                    break;
                case "EndDate":
                    exhibitions = exhibitions.OrderBy(s => s.EndDate).ToList();
                    break;
                case "end_date_desc":
                    exhibitions = exhibitions.OrderByDescending(s => s.EndDate).ToList();
                    break;
                default:
                    exhibitions = exhibitions.OrderBy(s => s.Title).ToList();
                    break;
            }

            int pageSize = 10;
            return View(PaginatedList<ExhibitionModel>.Create(exhibitions, pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// GET: Exhibition/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Commenting out original code to show how to use a raw SQL query.
            ExhibitionModel exhibition = await _context.Exhibitions.FindAsync(id);
            if (exhibition == null)
            {
                return NotFound();
            }

            if (id != null)
            {
                var exhibitionRooms = from d in _context.ExhibitionRooms
                                      orderby d.Title
                                      where d.ExhibitionId == exhibition.ExhibitionId
                                      select d;

                ViewData["ExhibitionRooms"] = exhibitionRooms.ToList();
            }

            exhibition.Owner = _context.AccUsers.FirstOrDefault(user => user.UserId == exhibition.OwnerId);

            return View(exhibition);
        }

        #endregion Index and Details

        #region Create

        /// <summary>
        /// GET: Exhibition/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ExhibitionModel exModel = new ExhibitionModel { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            exModel.Owner = new UserModel();

            PopulateExhibitionRoomsDropDownList(0);

            ViewBag.UserList = GetUserList(0);
            ViewBag.ArtworkList = GetArtworkNewList(0);

            exModel.ExhibitionArtworkIds = new List<long>().ToArray();

            return View(exModel);
        }

        /// <summary>
        /// POST: Exhibition/Create
        /// </summary>
        /// <param name="exhibitionModel"></param>
        /// <param name="MainMapUrlFile"></param>
        /// <param name="MapUrlFile"></param>
        /// <param name="IconUrlFile"></param>
        /// <param name="ThumbUrlFile"></param>
        /// <param name="IntroUrlFile"></param>
        /// <param name="HowtoFile"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(737280000)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OwnerId,Title,Description,GeoFenced,Latitude,Longitude,Radius,MetaData,StartDate,EndDate,Active,Order,MapUrlFile,IconUrlFile,ThumbUrlFile,MainMapUrlFile,IntroUrlFile,HowtoFile,UseGps,ScanRadius,ShowRadius,ViewRadius")] ExhibitionModel exhibitionModel)
        {
            if (ModelState.IsValid)
            {
                exhibitionModel.Active = true;
                exhibitionModel.Guid = Guid.NewGuid().ToString();
                exhibitionModel.Created = DateTime.UtcNow;
                //exhibitionModel.WpId = Guid.NewGuid().ToString();

                #region Upload File to azure storage

                List<IFormFile> allFiles = new List<IFormFile>
                {
                    exhibitionModel.IconUrlFile,
                    exhibitionModel.ThumbUrlFile,
                    exhibitionModel.MainMapUrlFile,
                    exhibitionModel.MapUrlFile,
                    exhibitionModel.IntroUrlFile,
                    exhibitionModel.HowtoFile
                };

                foreach (IFormFile file in allFiles)
                {
                    if (file == null) { continue; }

                    //create file class to use uploading files
                    AzureBlobFile azureBlobFile = new AzureBlobFile(Enums.InterfaceFileTypes.Exhibition,
                                                                    file,
                                                                    await file.GetBytes(),
                                                                    exhibitionModel.Title,
                                                                    string.Empty,
                                                                    Configuration.GetConnectionString("AccessKey"),
                                                                    Configuration.GetConnectionString("ContainerName"));

                    //create azure storace service
                    BlobStorageService objBlobService = new BlobStorageService(azureBlobFile.AccessKey, azureBlobFile.ContainerName);

                    //upload file to azure storage
                    string uploadedFileName = objBlobService.UploadFileToBlob(azureBlobFile);

                    //Set azure storage file nime into the model property
                    Helper.Helper.UpdateProperty(exhibitionModel, azureBlobFile.PropertyName, uploadedFileName);
                }

                #endregion

                _context.Add(exhibitionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateExhibitionRoomsDropDownList(exhibitionModel.ExhibitionId);

            ViewBag.UserList = GetUserList(exhibitionModel.OwnerId);
            ViewBag.ArtworkList = GetArtworkNewList(exhibitionModel.ExhibitionId);

            exhibitionModel.ExhibitionArtworkIds = new List<long>().ToArray();

            return View(exhibitionModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exhibitionId"></param>
        private void PopulateExhibitionRoomsDropDownList(long exhibitionId)
        {
            var exhibitionRooms = new List<ExhibitionRoomModel>();

            if (exhibitionId > 0)
            {
                exhibitionRooms = (from d in _context.ExhibitionRooms
                                   orderby d.Title
                                   where d.ExhibitionId == exhibitionId
                                   select d).ToList();
            }
            else
            {
                exhibitionRooms = (from d in _context.ExhibitionRooms
                                   orderby d.Title
                                   select d).ToList();
            }

            ViewData["ExhibitionRooms"] = exhibitionRooms.ToList();
        }

        #endregion Create

        #region Edit

        /// <summary>
        /// GET: Exhibition/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exhibition = await _context.Exhibitions
                .FirstOrDefaultAsync(m => m.ExhibitionId == id);
            if (exhibition == null)
            {
                return NotFound();
            }

            PopulateExhibitionRoomsDropDownList(exhibition.ExhibitionId);

            ViewBag.UserList = GetUserList(exhibition.OwnerId);
            ViewBag.ArtworkList = GetArtworkNewList(exhibition.ExhibitionId);

            exhibition.ExhibitionArtworkIds = GetExhibitionArtworks(exhibition.ExhibitionId).Select(select => select.ArtworkId).ToArray();

            return View(exhibition);
        }

        /// <summary>
        /// POST: Exhibitions/Edit/5
        /// </summary>
        /// <param name="exhibition"></param>
        /// <param name="id"></param>
        /// <param name="MainMapUrlFile"></param>
        /// <param name="MapUrlFile"></param>
        /// <param name="IconUrlFile"></param>
        /// <param name="ThumbUrlFile"></param>
        /// <param name="IntroUrlFile"></param>
        /// <param name="HowtoFile"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long? id, [Bind("ExhibitionId,OwnerId,Title,Description,GeoFenced,Latitude,Longitude,Radius,MetaData,Created,StartDate,EndDate,Active,Guid,Order,MapUrl,IconUrl,ThumbUrl,MainMapUrl,IntroUrl,Howto,MapUrlFile,IconUrlFile,ThumbUrlFile,MainMapUrlFile,IntroUrlFile,HowtoFile,UseGps,ScanRadius,ShowRadius,ViewRadius,ExhibitionArtworkIds")] ExhibitionModel exhibitionModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                List<IFormFile> allFiles = new List<IFormFile> { exhibitionModel.IconUrlFile, exhibitionModel.ThumbUrlFile, exhibitionModel.MainMapUrlFile, exhibitionModel.MapUrlFile, exhibitionModel.IntroUrlFile, exhibitionModel.HowtoFile };

                var exhibitionToUpdate = await _context.Exhibitions
                    .FirstOrDefaultAsync(m => m.ExhibitionId == id);

                if (await TryUpdateModelAsync<ExhibitionModel>(
                    exhibitionToUpdate,
                    "",
                    i => i.Title,
                    i => i.Description,
                    i => i.OwnerId,
                    i => i.GeoFenced,
                    i => i.Latitude,
                    i => i.Longitude,
                    i => i.Radius,
                    i => i.StartDate,
                    i => i.EndDate,
                    i => i.Active,
                    i => i.Order,
                    i => i.UseGps,
                    i => i.ScanRadius,
                    i => i.ShowRadius,
                    i => i.ViewRadius,
                    i => i.MainMapUrl,
                    i => i.MapUrl,
                    i => i.IconUrl,
                    i => i.ThumbUrl,
                    i => i.IntroUrl,
                    i => i.Howto))
                {
                    try
                    {
                        #region Upload File to azure storage

                        foreach (IFormFile file in allFiles)
                        {
                            if (file == null) { continue; }

                            //create file class to use uploading files
                            AzureBlobFile azureBlobFile = new AzureBlobFile(InterfaceFileTypes.Exhibition,
                                                                            file,
                                                                            await file.GetBytes(),
                                                                            exhibitionToUpdate.Title,
                                                                            string.Empty,
                                                                            Configuration.GetConnectionString("AccessKey"),
                                                                            Configuration.GetConnectionString("ContainerName"));

                            //create azure storace service
                            BlobStorageService objBlobService = new BlobStorageService(azureBlobFile.AccessKey, azureBlobFile.ContainerName);

                            //upload file to azure storage
                            string uploadedFileName = objBlobService.UploadFileToBlob(azureBlobFile);

                            //if the previous file exists delete
                            //string previousPropertyBlobUrlToUpdate = Helper.Helper.GetPropertyValue(exhibitionToUpdate, azureBlobFile.PropertyName);
                            //If necessary, open the remove option
                            //if (!string.IsNullOrEmpty(previousPropertyBlobUrlToUpdate))
                            //{
                            //    objBlobService.DeleteBlobData(previousPropertyBlobUrlToUpdate, azureBlobFile.FilePath);
                            //}

                            //Set azure storage file nime into the model property
                            Helper.Helper.UpdateProperty(exhibitionToUpdate, azureBlobFile.PropertyName, uploadedFileName);
                        }

                        #endregion

                        #region Exhibition Artworks

                        IQueryable<ExhibitionArtworkModel> exhibitionArtworkModels = _context.ExhibitionArtworks.Where(where => where.ExhibitionId == id);

                        //there ara no selected artworks so that if any relation exists on the db must be deleted
                        if (exhibitionModel.ExhibitionArtworkIds != null && exhibitionModel.ExhibitionArtworkIds.Length == 0 && exhibitionArtworkModels.Count() > 0)
                        {
                            _context.ExhibitionArtworks.RemoveRange(exhibitionArtworkModels);
                        }

                        //there are selected artworks on UI
                        if (exhibitionModel.ExhibitionArtworkIds != null && exhibitionModel.ExhibitionArtworkIds.Length > 0)
                        {
                            //there is no relation record for the exhibiton
                            //add items to db
                            if (exhibitionArtworkModels.Count() == 0)
                            {
                                addExhibitionArtworkModel((Int64)id, exhibitionModel.ExhibitionArtworkIds.ToList());
                            }
                            else//there are items on the db and selected items too
                            {
                                var newListArtworkList = exhibitionModel.ExhibitionArtworkIds.Except(exhibitionArtworkModels.Select(select => select.ArtworkId).ToList());
                                IQueryable<ExhibitionArtworkModel> itemsToBeRemoved = exhibitionArtworkModels.Where(where => !exhibitionModel.ExhibitionArtworkIds.Any(any => any == where.ArtworkId));

                                //some items removed from the list
                                if (itemsToBeRemoved.Count() > 0)
                                {
                                    _context.ExhibitionArtworks.RemoveRange(itemsToBeRemoved);
                                }

                                //there are new items added
                                if (newListArtworkList.Count() > 0)
                                {
                                    //add the new added items
                                    addExhibitionArtworkModel((Int64)id, newListArtworkList.ToList());
                                }
                            }
                        }

                        #endregion Exhibition Artworks

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        _logger.LogError($"Exhibition -> Edit -> exception: {ex.ToString()}");

                        //Log the error (uncomment ex variable name and write a log.)
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateExhibitionRoomsDropDownList(exhibitionModel.ExhibitionId);

            ViewBag.UserList = GetUserList(exhibitionModel.OwnerId);
            ViewBag.ArtworkList = GetArtworkNewList(exhibitionModel.ExhibitionId);

            exhibitionModel.ExhibitionArtworkIds = GetExhibitionArtworks(exhibitionModel.ExhibitionId).Select(select => select.ArtworkId).ToArray();

            return View(exhibitionModel);
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// GET: Exhibitions/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="concurrencyError"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(long? id, bool? concurrencyError)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return NotFound();
            }
            ExhibitionModel exhibition = await _context.Exhibitions.FindAsync(id);
            if (exhibition == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                //return HttpNotFound();
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

            return View(exhibition);
        }

        /// <summary>
        /// POST: Exhibitions/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            ExhibitionModel exhibition = await _context.Exhibitions
                .SingleAsync(i => i.ExhibitionId == id);

            _context.Exhibitions.Remove(exhibition);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// POST: Exhibitions/Delete/5
        /// </summary>
        /// <param name="exhibition"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(ExhibitionModel exhibition)
        {
            try
            {
                _context.Entry(exhibition).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = exhibition.ExhibitionId });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(exhibition);
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

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="pageNumber"></param>
        /// <param name="exhibitionId"></param>
        /// <returns></returns>
        public ActionResult SaveArtworks(ArtworkExhibitionCheckModel artworkModels)
        {
            return RedirectToAction("Edit", "ExhibitionController", new { id = 12 });
        }

        /// <summary>
        /// Get country method.
        /// </summary>
        /// <returns>Return country for drop down list.</returns>
        private IEnumerable<SelectListItem> GetUserList(long? userId)
        {
            // Initialization.
            SelectList lstobj = null;

            try
            {

                // Loading.
                var list = _context.AccUsers.OrderBy(item => item.Name).ToList()
                                  .Select(p =>
                                            new SelectListItem
                                            {
                                                Value = p.UserId.ToString(),
                                                Text = p.Name + " " + p.LastName + " Email: " + p.Email,
                                                Selected = (userId != null && userId > 0 && p.UserId == userId) ? true : false
                                            });

                if (userId != null && userId > 0)
                {
                    lstobj = new SelectList(list, "Value", "Text", userId);
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

            // info.
            return lstobj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exhibitionId"></param>
        /// <returns></returns>
        private IQueryable<ArtworkModel> GetExhibitionArtworks(long exhibitionId)
        {
            var exhibitionArtworks = from d in _context.ExhibitionArtworks
                                     orderby d.Order
                                     where d.ExhibitionId == exhibitionId
                                     select d;

            return _context.Artworks.Where(item => exhibitionArtworks.Any(eA => item.ArtworkId == eA.ArtworkId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exhibitionId"></param>
        /// <returns></returns>
        private IEnumerable<SelectListItem> GetArtworkNewList(long exhibitionId)
        {
            // Initialization.
            SelectList lstobj = null;

            try
            {
                var artworks = GetExhibitionArtworks(exhibitionId);

                // Loading.
                var list = _context.Artworks.ToList()
                                  .Select(p =>
                                            new SelectListItem
                                            {
                                                Value = p.ArtworkId.ToString(),
                                                Text = p.Name
                                            });

                if (artworks.ToList().Count > 0)
                {
                    lstobj = new SelectList(list, "Value", "Text", artworks.Select(item => item.ArtworkId).ToArray());
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

            // info.
            return lstobj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exhibitionId"></param>
        /// <param name="artworks"></param>
        private void addExhibitionArtworkModel(long exhibitionId, List<long> artworks)
        {
            List<ExhibitionArtworkModel> newRelationTable = new List<ExhibitionArtworkModel>();

            foreach (long artworkId in artworks)
            {
                newRelationTable.Add(new ExhibitionArtworkModel
                {
                    Active = true,
                    ArtworkId = artworkId,
                    ExhibitionId = exhibitionId,
                    Order = 0
                });
            }
            _context.ExhibitionArtworks.AddRange(newRelationTable);
        }

        #endregion Private Methods
    }
}
