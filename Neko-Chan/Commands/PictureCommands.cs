using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NekosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neko_Chan.Commands
{
    public partial class ComandSheet
    {
        #region Prank Pictures
        [Command("yaoi")]
        public async Task SendYaoi()
        {
            SocketTextChannel senderChannel = (SocketTextChannel)Context.Channel;

            //if (senderChannel.IsNsfw)
            //    await Context.Channel.SendFileAsync(yaoiPictureManager.RandomPicture(), "Here is your yaoi sir (｡◕‿‿◕｡)");
            //else
            //    await Context.Channel.SendMessageAsync("Sorry sir ;( this is not a nsfw chnannel!");
        }

        [Command("yaoi")]
        public async Task Test(IUser user)
        {
            var target = user as IUser; //etiketlenen kullanıcı 
            var senderid = Context.Message.Author.Id; //mesaj sahibi
            // TODO LEWDMANAGER.RANDOMLEWD DEN FOTO ÇEKEMİYOR VE DOSYADAN FOTO ÇEKEMİYOR FAKAT LEWDS'İ SİLERSEN FOTO ATAR
            // var fileName = "IMG_2574.jpg"; // BU ÇALISIYOR
            // var fileName = "lewds\\IMG_2574.jpg"; // BU ÇALISMIYOR TO DO

            //var fileName = yaoiPictureManager.RandomPicture();
            //Console.WriteLine($"attachment:\\{fileName}");
            //var embed = new EmbedBuilder
            //{
            //    ImageUrl = $"attachment://{fileName}",

            //};
            //// Or with methods
            //embed.AddField("Here is your yaoi master (｡◕‿‿◕｡)",
            //    "-")
            //    .WithFooter(new EmbedFooterBuilder().WithText("Sent by " + Context.Message.Author + " from " + Context.Guild.Name).WithIconUrl(Context.Message.Author.GetAvatarUrl())).WithColor(Color.Blue)
            //    .WithCurrentTimestamp();

            //await Discord.UserExtensions.SendFileAsync(target, fileName, embed: embed.Build());


            if (target.Id == 75295325118398481 || target.Id == 14358045046603776 || target.Id == 322880459585486848 || target.Id == 180306218043179009 || target.Id == 479755400908898304)
            {
                await Context.Channel.SendMessageAsync("Sorry master ;( i can't do this \nThis person is my comrade and comrades protect each other!");
            }
            else
            {
                try
                {
                    //await Discord.UserExtensions.SendFileAsync(target, LewdManager.RandomLewd(), "Here is your yaoi master (｡◕‿‿◕｡)");

                }
                catch (Exception e)
                {
                    await Context.Channel.SendMessageAsync($"<@{target.Id}> blocked me master! i can't send yaoi (｀◕‸◕´+)");

                }
            }

        }
        #endregion
        #region SFW Neko Pictures
        [Command("neko")]
        public async Task SendNeko()
        {
            Request Req = await NekoChanBot.NekoAPI.Image.Neko();

            EmbedFooterBuilder footerBuilder = new EmbedFooterBuilder();
            footerBuilder.WithText("Requested by " + Context.Message.Author);
            footerBuilder.WithIconUrl(Context.Message.Author.GetAvatarUrl());

            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.WithColor(0, 128, 0);
            embedBuilder.WithImageUrl(Req.ImageUrl);
            embedBuilder.WithFooter(footerBuilder);

            await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }
        [Command("neko")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task NekoSenderValue(int amount)
        {
            if (amount < 51)
            {
                EmbedFooterBuilder footerBuilder = new EmbedFooterBuilder();
                footerBuilder.WithText("Requested by " + Context.Message.Author);
                footerBuilder.WithIconUrl(Context.Message.Author.GetAvatarUrl());

                EmbedBuilder builder = new EmbedBuilder();
                builder.WithColor(0, 128, 0);
                builder.WithFooter(footerBuilder);


                for (int i = 1; i < amount; i++)
                {
                    Request image_request = await NekoChanBot.NekoAPI.Image.Neko();
                    builder.WithImageUrl(image_request.ImageUrl);

                    await Context.Channel.SendMessageAsync("", false, builder.Build());

                    //builder.WithImageUrl(image_request.ImageUrl);

                    await Task.Delay(1000);
                }

                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
            else
                await Context.Channel.SendMessageAsync("Sorry master :cry: maximum limit of neko is 50");

        }

        #endregion
        #region NSFW Pictures
        [Command("lewd")]
        public async Task SendNekoNsfw()
        {
            SocketTextChannel senderChannel = (SocketTextChannel)Context.Channel;

            if (senderChannel.IsNsfw)
            {
                Request request_image = await NekoChanBot.NekoAPI.Nsfw.Neko();

                EmbedFooterBuilder footerBuilder = new EmbedFooterBuilder();
                footerBuilder.WithText("Requested by " + Context.Message.Author);
                footerBuilder.WithIconUrl(Context.Message.Author.GetAvatarUrl());

                EmbedBuilder builder = new EmbedBuilder();
                builder.WithColor(0, 128, 0);
                builder.WithImageUrl(request_image.ImageUrl);
                builder.WithFooter(footerBuilder);

                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
            else
                await Context.Channel.SendMessageAsync("Sorry sir :cry: this is not a nsfw chnannel!");

        }
        [Command("lewd")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SendNekoNsfw(int amount)
        {
            SocketTextChannel senderChannel = (SocketTextChannel)Context.Channel;
            if (senderChannel.IsNsfw)
            {
                if (amount < 51)
                {

                    EmbedFooterBuilder footerBuilder = new EmbedFooterBuilder();
                    footerBuilder.WithText("Requested by " + Context.Message.Author);
                    footerBuilder.WithIconUrl(Context.Message.Author.GetAvatarUrl());

                    EmbedBuilder builder = new EmbedBuilder();
                    builder.WithColor(0, 128, 0);
                    builder.WithFooter(footerBuilder);

                    for (int i = 0; i < amount; i++)
                    {
                        Request image_request = await NekoChanBot.NekoAPI.Nsfw.Neko();
                        builder.WithImageUrl(image_request.ImageUrl);

                        await Context.Channel.SendMessageAsync("", false, builder.Build());
                        await Task.Delay(1000);
                    }
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Sorry master ;( max is 50!");
                }
            }
            else
                await Context.Channel.SendMessageAsync("Sorry master ;( this is not a nsfw chnannel!");
        }
        #endregion
    }
}
