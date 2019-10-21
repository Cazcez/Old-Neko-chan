using Discord;
using Discord.Commands;
using NekoChan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neko_Chan.Commands
{
    public partial class ComandSheet : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Commands");
            builder.WithFooter($"Requested by {Context.Client.CurrentUser.Username}", Context.Client.CurrentUser.GetAvatarUrl());
            List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>(4);


            fields[0].WithName("Game");
            fields[0].WithValue("its suprise");

            builder.WithFields(fields[0]);

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
        [Command("Game")]
        public async Task GameInfo()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTimestamp(DateTime.Now);
            builder.WithAuthor("PhantomCloak", "https://cdn.discordapp.com/avatars/214358045046603776/4ab5c91ee259cebd432cbe0472a272f0.png");
            builder.WithColor(40, 200, 150);
            builder.WithTitle($"Hentai Nekos : Multiplayer On Steam!");
            builder.WithDescription("World's first multiplayer hentai puzzle game now avaible on [Steam store](https://steam.com) \n\n");

            EmbedFieldBuilder efb = new EmbedFieldBuilder();
            efb.WithName("\n\n**Multiplayer**");
            efb.WithValue("You can set up lobby with your own level or default level up to 4 players! :smirk:");

            EmbedFieldBuilder efb1 = new EmbedFieldBuilder();

            efb1.WithName("**Customize Puzzles**");
            efb1.WithValue("You can create any kind of puzzle from anywhere with a just link of image!\n\n");

            EmbedFieldBuilder efb2 = new EmbedFieldBuilder();

            efb2.WithName("**Sharable Puzzle Levels**");
            efb2.WithValue("As well Hentai Neko's allow to player made Levels you can save your level or even share with your friends");

            builder.WithFields(efb, efb1, efb2);

            builder.WithThumbnailUrl(NekoChanBot.Client.CurrentUser.GetAvatarUrl());
            builder.WithImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/ba/Bundesarchiv_Bild_101I-299-1805-16%2C_Nordfrankreich%2C_Panzer_VI_%28Tiger_I%29.2.jpg");
            //builder.WithFooter("This is footer", Context.Guild.Owner.GetAvatarUrl());

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }


    }
}
