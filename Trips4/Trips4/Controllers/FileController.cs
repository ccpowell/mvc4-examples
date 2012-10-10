using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DRCOG.Web.CustomResults;
using System.Web;
using DRCOG.Common.Services;
using DRCOG.Common.Services.Interfaces;
using DRCOG.Common.Web.MvcSupport.Attributes;

namespace DRCOG.Web.Controllers
{
    /// <summary>
    /// Controller that will manage the Account Administration aspects of the DRCOG application.
    /// Actual login validation is handled in the <see cref="LoginController"/>
    /// </summary>
    [HandleError]
    //[RemoteRequireHttps]
    public class FileController : ControllerBase
    {
        protected readonly ImageService ImageService;
        public FileController(IFileRepository FileRepository)
        {
            ImageService = new ImageService(FileRepository);
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[Authorize]
        //public ActionResult Upload()
        //{
        //    return View();
        //}

        //[Authorize]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Upload(Image image)
        //{
        //    Image newImage = new Image();
        //    HttpPostedFileBase file = Request.Files["fileUpload"];

        //    if (file.ContentLength < 524288)
        //    {

        //        newImage.Name = image.Name;
        //        newImage.AlternateText = image.AlternateText;

        //        newImage.ContentType = file.ContentType;

        //        Int32 length = file.ContentLength;
        //        byte[] tempImage = new byte[length];
        //        file.InputStream.Read(tempImage, 0, length);
        //        newImage.Data = tempImage;

        //        int id = ImageService.Save(newImage);
        //        return View();
        //    }
        //    ViewData["Message"] = "Image must be smaller then 4mb.";

        //    return View();
        //}

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult ShowPhoto(Int32 id)
        //{
        //    //This is my method for getting the image information
        //    // including the image byte array from the image column in
        //    // a database.
        //    Image image = ImageService.GetById(id);
        //    //As you can see the use is stupid simple.  Just get the image bytes and the
        //    //  saved content type.  See this is where the contentType comes in real handy.
        //    ImageResult result = new ImageResult(image.Data, image.ContentType);

        //    return result;
        //}

    }

}
