using Neko_Chan.Utily;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neko_Chan.MusicManager
{
    public static class MusicPlayer
    {
        private static string MusicStoragePath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Templates) + "\\Musics";
            }
        }
        static MusicPlayer()
        {
            if(Directory.Exists(MusicStoragePath))
            {
                foreach (var file in Directory.EnumerateFiles(MusicStoragePath))
                {
                    File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(MusicStoragePath);
            }

        }
        //public static async Task PlayMusic(string url)
        //{
        //    Task<string> downloadTask = DownloadMusic(url);

        //    await downloadTask;


        //}
        public static async Task<string> DownloadMusic(string url)
        {
            string fileHash = CryptoHelper.GetMD5String(url);

            if(CheckCache(fileHash + ".mp3"))
            {
                return MusicStoragePath + "\\" + fileHash + ".mp3";
            }

            Process youtubedl = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "youtube-dl.exe",
                    Arguments = $"--output \"{MusicStoragePath}\\{fileHash}.mp3\" --extract-audio --audio-format mp3 --prefer-ffmpeg {url}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            youtubedl.Start();
            youtubedl.WaitForExit();
            return  MusicStoragePath + "\\" +fileHash + ".mp3";
        }
        private static bool CheckCache(string fileName)
        {
            foreach (var file in Directory.EnumerateFiles(MusicStoragePath))
            {
                if (Path.GetFileName(file) == fileName)
                    return true;
            }
            return false;
        }
    }
}
