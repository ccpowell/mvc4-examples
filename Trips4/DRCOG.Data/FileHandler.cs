using System;
using System.IO;

namespace DRCOG.Data
{
    public class FileHandler
    {
        public static void Copy(String fileName, String newName, String sourcePath, String targetPath)
        {
            string sourceFile = Path.Combine(sourcePath, fileName);
            string destFile = Path.Combine(targetPath, newName);

            // To Copy a file to another location and
            // overwrite the destination file if it already exists

            if(File.Exists(sourceFile))
            {
                File.Copy(sourceFile, destFile, true);
            }
        }
        public static void Delete(String fileName, String path)
        {
            string filePath = Path.Combine(path, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
