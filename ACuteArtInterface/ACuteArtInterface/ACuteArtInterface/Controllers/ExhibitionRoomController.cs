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
using Microsoft.Extensions.Configuration;
using ACuteArtInterface.Helper;
using ACuteArtInterface.Services;
using ACuteArtInterface.Enums;

namespace ACuteArtInterface.Controllers
{
    public class ExhibitionRoomController : Controller
    {
        private readonly ApplicationContext _context;

        private readonly ILogger<ExhibitionRoomController> _logger;

        public IConfiguration Configuration { get; }

        public ExhibitionRoomController(ApplicationContext context, IConfiguration configuration, ILogger<ExhibitionRoomController> logger)
        {
            _context = context;
            _logger = logger;
            Configuration = configuration;
        }

        #region Index and Details

        /// <summary>
        /// Returns all of the ExhibitionRooms with pagination
        /// </summary>
        /// <param name="sortOrder">sort order by title, exhibition title or order</param>
        /// <param name="currentFilter">current sort filter</param>
        /// <param name="searchString">search keyword</param>
        /// <param name="pageNumber">page number for navigation</param>
        /// <returns></returns>
        [Authorize]
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["ExhibitionTitleSortParm"] = sortOrder == "ExhibitionTitle" ? "exhibition_title_desc" : "ExhibitionTitle";
            ViewData["OrderSortParm"] = sortOrder == "Order" ? "order_desc" : "Order";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var exhibitionRooms = _context.ExhibitionRooms.Include(i => i.Exhibition).Select(item => new ExhibitionRoomModel
            {
                ExhibitionId = item.ExhibitionId,
                IconUrl = string.IsNullOrEmpty(item.IconUrl) ? "" : item.IconUrl,
                MapIconUrl = string.IsNullOrEmpty(item.MapIconUrl) ? "" : item.MapIconUrl,
                MapUrl = string.IsNullOrEmpty(item.MapUrl) ? "" : item.MapUrl,
                Order = item.Order,
                Active = item.Active,
                RoomId = item.RoomId,
                ScanRadius = item.ScanRadius,
                ShowOnMap = item.ShowOnMap,
                ThumbUrl = string.IsNullOrEmpty(item.ThumbUrl) ? "" : item.ThumbUrl,
                Title = string.IsNullOrEmpty(item.Title) ? "" : item.Title,
                Va = item.Va,
                VaGuid = string.IsNullOrEmpty(item.VaGuid) ? "" : item.VaGuid,
                Description = string.IsNullOrEmpty(item.Description) ? "" : item.Description,
                TitleEn = Helper.Helper.GetStringByLanguage(item.Title, 0),
                DescriptionEn = Helper.Helper.GetStringByLanguage(item.Description, 30),
                Exhibition = _context.Exhibitions.FirstOrDefault(user => user.ExhibitionId == item.ExhibitionId)
            }).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                exhibitionRooms = exhibitionRooms.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper())
                || (s.Description != null && s.Description.ToUpper().Contains(searchString.ToUpper())) ||
                   (s.Exhibition != null && s.Exhibition.Title != null && s.Exhibition.Title.ToUpper().Contains(searchString.ToUpper()))).ToList();
            }

            switch (sortOrder)
            {
                case "title_desc":
                    exhibitionRooms = exhibitionRooms.OrderByDescending(s => s.Title).ToList();
                    break;
                case "ExhibitionTitleSortParm":
                    exhibitionRooms = exhibitionRooms.OrderBy(s => s.Exhibition.Title).ToList();
                    break;
                case "exhibition_title_desc":
                    exhibitionRooms = exhibitionRooms.OrderByDescending(s => s.Exhibition.Title).ToList();
                    break;
                case "Order":
                    exhibitionRooms = exhibitionRooms.OrderBy(s => s.Order).ToList();
                    break;
                case "order_desc":
                    exhibitionRooms = exhibitionRooms.OrderByDescending(s => s.Order).ToList();
                    break;
                default:
                    exhibitionRooms = exhibitionRooms.OrderBy(s => s.Title).ToList();
                    break;
            }

            int pageSize = 10;
            return View(PaginatedList<ExhibitionRoomModel>.Create(exhibitionRooms, pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// GET: ExhibitionRoom/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Commenting out original code to show how to use a raw SQL query.
            ExhibitionRoomModel exhibitionRooms = await _context.ExhibitionRooms.FindAsync(id);
            if (exhibitionRooms == null)
            {
                return NotFound();
            }

            //if (id != null)
            //{
            //    var exhibitionRooms = from d in _context.ExhibitionRooms
            //                          orderby d.Title
            //                          where d.ExhibitionId == exhibitionRooms.ExhibitionId
            //                          select d;

            //    ViewData["ExhibitionRooms"] = exhibitionRooms.ToList();
            //}

            //exhibitionRooms.Owner = _context.AccUsers.FirstOrDefault(user => user.UserId == exhibitionRooms.OwnerId);

            return View(exhibitionRooms);
        }

        #endregion Index and Details

        #region Create

        // GET: Exhibition/Create
        [RequestSizeLimit(737280000)]
        public ActionResult Create()
        {
            PopulateExhibitionsDropDownList(0);

            return View();
        }

        /// <summary>
        /// POST: ExhibitionRoom/Create
        /// </summary>
        /// <param name="exhibitionRoomModel"></param>
        /// <param name="MapIconUrlFile"></param>
        /// <param name="MapUrlFile"></param>
        /// <param name="ThumbUrlFile"></param>
        /// <param name="IconUrlFile"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(737280000)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ExhibitionId,Order,Active,ScanRadius,ShowOnMap,VaGuid,Va,IconUrl,MapIconUrl,MapUrl,ThumbUrl,IconUrlFile,MapIconUrlFile,MapUrlFile,ThumbUrlFile")] ExhibitionRoomModel exhibitionRoomModel)
        {
            if (ModelState.IsValid)
            {
                exhibitionRoomModel.Active = true;

                #region Upload File to azure storage

                List<IFormFile> allFiles = new List<IFormFile> { exhibitionRoomModel.MapIconUrlFile, exhibitionRoomModel.MapUrlFile, exhibitionRoomModel.ThumbUrlFile, exhibitionRoomModel.IconUrlFile };

                foreach (IFormFile file in allFiles)
                {
                    if (file == null) { continue; }

                    //create file class to use uploading files
                    AzureBlobFile azureBlobFile = new AzureBlobFile(Enums.InterfaceFileTypes.Exhibition,
                                                                    file,
                                                                    await file.GetBytes(),
                                                                    exhibitionRoomModel.Title,
                                                                    string.Empty,
                                                                    Configuration.GetConnectionString("AccessKey"),
                                                                    Configuration.GetConnectionString("ContainerName"));

                    //create azure storace service
                    BlobStorageService objBlobService = new BlobStorageService(azureBlobFile.AccessKey, azureBlobFile.ContainerName);

                    //upload file to azure storage
                    string uploadedFileName = objBlobService.UploadFileToBlob(azureBlobFile);

                    //Set azure storage file nime into the model property
                    Helper.Helper.UpdateProperty(exhibitionRoomModel, azureBlobFile.PropertyName, uploadedFileName);
                }

                #endregion

                _context.Add(exhibitionRoomModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateExhibitionsDropDownList(0);

            return View(exhibitionRoomModel);
        }

        #endregion Create

        #region Edit

        /// <summary>
        /// GET: ExhibitionRoom/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exhibitionRoom = await _context.ExhibitionRooms
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (exhibitionRoom == null)
            {
                return NotFound();
            }

            var exhibitionRoomItems = from d in _context.ExhibitionRoomItems.Include(i=> i.Artwork)
                                      orderby d.Order
                                      where d.RoomId == exhibitionRoom.RoomId
                                      select d;

            ViewData["ExhibitionRoomItems"] = exhibitionRoomItems.ToList();

            PopulateExhibitionsDropDownList(exhibitionRoom.ExhibitionId);

            return View(exhibitionRoom);
        }


        /// <summary>
        /// POST: Exhibitions/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exhibitionRoomModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long? id, [Bind("RoomId,Title,Description,ExhibitionId,Order,Active,ScanRadius,ShowOnMap,VaGuid,Va,IconUrl,MapIconUrl,MapUrl,ThumbUrl,IconUrlFile,MapIconUrlFile,MapUrlFile,ThumbUrlFile")] ExhibitionRoomModel exhibitionRoomModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var exhibitionRoomToUpdate = await _context.ExhibitionRooms.Include(d => d.Exhibition)
                    .FirstOrDefaultAsync(m => m.RoomId == id);

                List<IFormFile> allFiles = new List<IFormFile> { exhibitionRoomModel.MapIconUrlFile, exhibitionRoomModel.MapUrlFile, exhibitionRoomModel.ThumbUrlFile, exhibitionRoomModel.IconUrlFile };

                if (await TryUpdateModelAsync<ExhibitionRoomModel>(
                    exhibitionRoomToUpdate,
                    "",
                    i => i.Title,
                    i => i.Description,
                    i => i.ExhibitionId,
                    i => i.Order, i =>
                    i.ScanRadius, i =>
                    i.ShowOnMap, i =>
                    i.VaGuid, i => i.Va,
                    i => i.MapIconUrl,
                    i => i.MapUrl,
                    i => i.ThumbUrl,
                    i => i.IconUrl))
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
                                                                            exhibitionRoomToUpdate.Title,
                                                                            string.Empty,
                                                                            Configuration.GetConnectionString("AccessKey"),
                                                                            Configuration.GetConnectionString("ContainerName"));

                            //create azure storace service
                            BlobStorageService objBlobService = new BlobStorageService(azureBlobFile.AccessKey, azureBlobFile.ContainerName);

                            //upload file to azure storage
                            string uploadedFileName = objBlobService.UploadFileToBlob(azureBlobFile);
                            //If necessary, open the remove option
                            //if the previous file exists delete
                            //string previousPropertyBlobUrlToUpdate = Helper.Helper.GetPropertyValue(exhibitionRoomToUpdate, azureBlobFile.PropertyName);

                            //if (!string.IsNullOrEmpty(previousPropertyBlobUrlToUpdate))
                            //{
                            //    objBlobService.DeleteBlobData(previousPropertyBlobUrlToUpdate, azureBlobFile.FilePath);
                            //}

                            //Set azure storage file nime into the model property
                            Helper.Helper.UpdateProperty(exhibitionRoomToUpdate, azureBlobFile.PropertyName, uploadedFileName);
                        }

                        #endregion

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        _logger.LogError($"ExhibitionRoom -> Edit -> exception: {ex.ToString()}");

                        //Log the error (uncomment ex variable name and write a log.)
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateExhibitionsDropDownList(exhibitionRoomModel.ExhibitionId);

            return View(exhibitionRoomModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exhibitionId"></param>
        private void PopulateExhibitionsDropDownList(long exhibitionId)
        {
            var exhibitions = from d in _context.Exhibitions
                              orderby d.Title
                              where d.ExhibitionId == exhibitionId
                              select d;

            ViewData["ExhibitionID"] = new SelectList(_context.Exhibitions, "ExhibitionId", "Title", exhibitionId);
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// GET: ExhibitionRoom/Delete/5
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
            ExhibitionRoomModel exhibition = await _context.ExhibitionRooms.FindAsync(id);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion Dispose and Error
    }
}
