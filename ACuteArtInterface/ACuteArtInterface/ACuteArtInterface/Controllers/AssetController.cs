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
    public class AssetController : Controller
    {
        private readonly ApplicationContext _context;
        public IConfiguration Configuration { get; }
        private readonly ILogger<ArtistController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        public AssetController(ApplicationContext context, IConfiguration configuration, ILogger<ArtistController> logger)
        {
            _context = context;
            _logger = logger;
            Configuration = configuration;
        }

        #region Edit

        /// <summary>
        /// Artist/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit()
        {
            string key = "hero_intro_image";

            var asset = await _context.Assets.FirstOrDefaultAsync(m => m.AssetKey == key);

            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        /// <summary>
        /// POST: Asset/Edit/5
        /// </summary>
        /// <param name="key"></param>
        /// <param name="assetModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AssetModel assetModel)
        {
            string key = "hero_intro_image";

            //getting the artist
            var assetToUpdate = await _context.Assets.FirstOrDefaultAsync(m => m.AssetKey == key);
            if (ModelState.IsValid && assetModel.AssetValueFile != null)
            {
                //preparing the properties to update
                if (await TryUpdateModelAsync<AssetModel>(
                    assetToUpdate,
                    "",
                    i => i.AssetKey,
                    i => i.AssetValue))
                {
                    try
                    {
                        #region Upload File to azure storage

                        //create file class to use uploading files
                        AzureBlobFile azureBlobFile = new AzureBlobFile(Enums.InterfaceFileTypes.Asset,
                                                                        assetModel.AssetValueFile,
                                                                        await assetModel.AssetValueFile.GetBytes(),
                                                                        assetToUpdate.AssetKey,
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
                        Helper.Helper.UpdateProperty(assetToUpdate, azureBlobFile.PropertyName, uploadedFileName);


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
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }

            return View(assetToUpdate);
        }

        #endregion Edit

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
