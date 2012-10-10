using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Common.Services;
using DRCOG.Data;
using DRCOG.Domain.Interfaces;
using System.Transactions;

namespace DRCOG.TIP.Services
{
    public static class ImageServiceExtender
    {
        public static readonly IFileRepositoryExtender FileRepository = new FileRepository();

        public static void Delete(this ImageService i, int id, int projectVersionId)
        {
            FileRepository.Delete(id, projectVersionId);
        }

        public static Image Load(this ImageService i, int id)
        {
            FileRepository repo = new FileRepository();
            return repo.Load(id);
        }

        public static Image Load(this ImageService i, int id, int maxWidth, int maxHeight)
        {
            Image file = i.Load(id);

            file.Data = i.ProportionallyResize(i.Parse<System.Drawing.Bitmap>(file.Data), maxWidth, maxHeight);

            return file;
        }

        public static Guid Save(this ImageService i, Image image, int projectVersionId)
        {
            Guid guid = default(Guid);
            guid = FileRepository.Save(image, projectVersionId);
            return guid;
        }

    }
}
