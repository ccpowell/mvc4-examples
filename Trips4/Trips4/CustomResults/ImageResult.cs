using System;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace DRCOG.Web.CustomResults
{
    public class ImageResult : ActionResult
    {
        public String ContentType { get; set; }
        public byte[] ImageBytes { get; set; }
        public String SourceFilename { get; set; }

        //This is used for times where you have a physical location
        public ImageResult(String sourceFilename, String contentType)
        {
            SourceFilename = sourceFilename;
            ContentType = contentType;
        }

        //This is used for when you have the actual image in byte form
        public ImageResult(byte[] sourceStream, String contentType)
        {
            ImageBytes = sourceStream;
            ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = ContentType;

            //Check to see if this is done from bytes or physical location
            if (ImageBytes != null)
            {
                var stream = new MemoryStream(ImageBytes);
                stream.WriteTo(response.OutputStream);
                stream.Dispose();
            }
            else
            {
                response.TransmitFile(SourceFilename);
            }
        }
    }
}
