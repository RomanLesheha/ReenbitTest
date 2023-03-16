using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ReenbitTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ReenbitTest.Controllers
{
    public class HomeController : Controller
    {
        public readonly string blobStorageConnectingString = "DefaultEndpointsProtocol=https;AccountName=reenbitcampteststorage;AccountKey=V8L4BGxEGM8H8jUxGoelNkfP+NffrymeyIlDOSJ1A+e2VTV20GAB/56CGWAHiOuWlRrPhAy/rsOu+AStwkH/+g==;EndpointSuffix=core.windows.net";
        public readonly string blobStorageContainerNAme = "files";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            if (user.File  == null || user.UserEmail == null)
            {
                ViewBag.ErrorMessage = "Please choose the file or email";
                return View();
            }
            if (ModelState.IsValid)
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(blobStorageConnectingString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer containers = blobClient.GetContainerReference(blobStorageContainerNAme);
                CloudBlockBlob blob = containers.GetBlockBlobReference(user.File.FileName);
                bool blobExists = await blob.ExistsAsync();
                if (blobExists)
                {
                    ViewBag.ErrorMessage = " File already exist";
                    return View();
                }
                else
                {
                    var stream = user.File.OpenReadStream();
                    var container = new BlobContainerClient(blobStorageConnectingString, blobStorageContainerNAme);
                    await container.UploadBlobAsync(user.File.FileName, stream);
                    ViewBag.InfoMessage = "Your file succesfully uploaded";
                }
            }
            return View("Index");
        }
       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
