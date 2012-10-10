using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using DRCOG.Utility.Interfaces;

namespace DRCOG.Utility.Services.Utility
{
    public class ImageService
    {
        private readonly IFileRepository FileRepository;

        public ImageService(IFileRepository _fileRepository)
        {
            FileRepository = _fileRepository;
        }

        public Int32 Save(DRCOG.Utility.Domain.Image image)
        {
            image.File = ProportionallyResize(Parse<Bitmap>(image.File), 348, 480);
            return FileRepository.Save(image);
        }

        public T Parse<T>(object value)
        {
            try
            {
                return
                    (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
            }
            catch (Exception) { return default(T); }
        }

        public T ConvertTo<T>(object value)
        {
            try
            {
                return
                    (T)TypeDescriptor.GetConverter(value.GetType()).ConvertTo(value, typeof(T));
            }
            catch (Exception) { return default(T); }
        }

        public Byte[] ProportionallyResize(Bitmap src, int maxWidth, int maxHeight)
        {
            // original dimensions
            int w = src.Width;
            int h = src.Height;

            // Longest and shortest dimension
            int longestDimension = (w > h) ? w : h;
            int shortestDimension = (w < h) ? w : h;

            // propotionality
            float factor = ((float)longestDimension) / shortestDimension;

            // default width is greater than height
            double newWidth = maxWidth;
            double newHeight = maxWidth / factor;

            // if height greater than width recalculate
            if (w < h)
            {
                newWidth = maxHeight / factor;
                newHeight = maxHeight;
            }

            // Create new Bitmap at new dimensions
            Bitmap result = new Bitmap((int)newWidth, (int)newHeight);
            using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
                g.DrawImage(src, 0, 0, (int)newWidth, (int)newHeight);

            Byte[] test = ConvertTo<Byte[]>(result) ?? null;

            return test;
        }


        public DRCOG.Utility.Domain.Image GetById(int id)
        {
            return FileRepository.GetById(id);
        }


    }
}
