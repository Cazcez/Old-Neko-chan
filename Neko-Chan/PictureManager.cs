using System;
using System.IO;

namespace Neko_Chan
{
    class PictureManager
    {
        private static Random rngSeed;
        private string[] loadedPictures;

        private string RootDir;
        public PictureManager(string root)
        {
            RootDir = root;
            Init();
        }
        public void Init()
        {
            if (Directory.Exists(RootDir) != true)
                Directory.CreateDirectory(RootDir);

            loadedPictures = Directory.GetFiles(RootDir, "*", SearchOption.TopDirectoryOnly);
            rngSeed = new Random();
        }
        public string RandomPicture()
        {
            int randomIndex = rngSeed.Next(0, loadedPictures.Length);
            string fileName = loadedPictures[randomIndex];
            return fileName;
        }

    }
}