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
    public class ExhibitionRoomItemController : Controller
    {
        private readonly ApplicationContext _context;

        private readonly ILogger<ExhibitionRoomController> _logger;

        public IConfiguration Configuration { get; }

        public ExhibitionRoomItemController(ApplicationContext context, IConfiguration configuration, ILogger<ExhibitionRoomController> logger)
        {
            _context = context;
            _logger = logger;
            Configuration = configuration;
        }

        #region Create

        // GET: ExhibitionRoomItem/Create
        public ActionResult Create(long? roomId)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            ExhibitionRoomItemModel roomItemModel = new ExhibitionRoomItemModel();
            roomItemModel.RoomId = (Int64)roomId;

            ViewBag.RoomId = roomId;
            ViewBag.ArtworkList = PopulateArtworkDropDownList(0);

            return View(roomItemModel);
        }

        /// <summary>
        /// POST: ExhibitionRoomItem/Create
        /// </summary>
        /// <param name="exhibitionRoomItemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtworkId,EditionNumber,Scale,RoomId,Order")] ExhibitionRoomItemModel exhibitionRoomItemModel)
        {
            if (ModelState.IsValid)
            {
                //default values
                exhibitionRoomItemModel.Scale = 1;
                exhibitionRoomItemModel.Offset = "0,0,0";
                exhibitionRoomItemModel.Axis = "0,1,0";

                _context.Add(exhibitionRoomItemModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ExhibitionRoomController.Edit), "ExhibitionRoom", new { id = exhibitionRoomItemModel.RoomId });
            }

            ViewBag.ArtworkList = PopulateArtworkDropDownList(0);

            return View(exhibitionRoomItemModel);
        }

        #endregion Create

        #region Edit

        /// <summary>
        /// GET: ExhibitionRoomItem/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exhibitionRoomItem = await _context.ExhibitionRoomItems
                .FirstOrDefaultAsync(m => m.RoomItemId == id);
            if (exhibitionRoomItem == null)
            {
                return NotFound();
            }

            ViewBag.ArtworkList = PopulateArtworkDropDownList(exhibitionRoomItem.ArtworkId);

            return View(exhibitionRoomItem);
        }


        /// <summary>
        /// POST: ExhibitionRoomItem/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exhibitionRoomModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long? id, [Bind("RoomItemId,ArtworkId,EditionNumber,Scale,RoomId,Order")] ExhibitionRoomItemModel exhibitionRoomItemModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var exhibitionRoomItemToUpdate = await _context.ExhibitionRoomItems.Include(d => d.Artwork)
                    .FirstOrDefaultAsync(m => m.RoomItemId == id);

                if (await TryUpdateModelAsync<ExhibitionRoomItemModel>(
                    exhibitionRoomItemToUpdate,
                    "",
                    i => i.ArtworkId,
                    i => i.EditionNumber,
                    i => i.Scale,
                    i => i.RoomId,
                    i => i.Order))
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        _logger.LogError($"ExhibitionRoomItem -> Edit -> exception: {ex.ToString()}");

                        //Log the error (uncomment ex variable name and write a log.)
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                    return RedirectToAction(nameof(ExhibitionRoomController.Edit), "ExhibitionRoom", new { id = exhibitionRoomItemToUpdate.RoomId });
                }
            }

            ViewBag.ArtworkList = PopulateArtworkDropDownList(exhibitionRoomItemModel.ArtworkId);

            return View(exhibitionRoomItemModel);
        }

        private IEnumerable<SelectListItem> PopulateArtworkDropDownList(long? artworkId)
        {
            // Initialization.
            SelectList lstobj = null;

            try
            {

                // Loading.
                var list = _context.Artworks.OrderBy(item => item.Name).ToList()
                                  .Select(p =>
                                            new SelectListItem
                                            {
                                                Value = p.ArtworkId.ToString(),
                                                Text = p.Name,
                                                Selected = (artworkId != null && artworkId > 0 && p.ArtworkId == artworkId) ? true : false
                                            });

                if (artworkId != null && artworkId > 0)
                {
                    lstobj = new SelectList(list, "Value", "Text", artworkId);
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

        #endregion Edit

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
