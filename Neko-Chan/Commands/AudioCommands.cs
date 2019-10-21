using Discord;
using Discord.Audio;
using Discord.Commands;
using Neko_Chan.MusicManager;
using Neko_Chan.Utily;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neko_Chan.Commands
{
    public partial class ComandSheet
    {
        [Command("play")]
        public async Task JoinChannel(string url,IVoiceChannel channel = null)
        {
            // Get the audio channel
            channel = channel ?? ((IGuildUser)Context.Message.Author)?.VoiceChannel;
            if (channel == null) { await Context.Message.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

            // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
            var audioClient = await channel.ConnectAsync();

            Task<string> downloadTask = MusicPlayer.DownloadMusic(url);

            await downloadTask;

            await SendAsync(audioClient,downloadTask.Result);
        }
        #region Stream
        private Process CreateStream(string path)
        {
            try
            {
                return Process.Start(new ProcessStartInfo
                {
                    FileName = "ffmpeg.exe",
                    Arguments = $"-i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }
        private async Task SendAsync(IAudioClient client, string path)
        {
            try
            {
                // Create FFmpeg using the previous example
                using (var ffmpeg = CreateStream(path))
                using (var output = ffmpeg.StandardOutput.BaseStream)
                using (var discord = client.CreatePCMStream(AudioApplication.Music))
                {
                    try { await output.CopyToAsync(discord); }
                    finally { await discord.FlushAsync(); }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        } 
        #endregion
    }
}
