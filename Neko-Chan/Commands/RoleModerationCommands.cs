using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neko_Chan.Commands
{
    public partial class ComandSheet
    {
        [Command("role")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RoleGiveManager(IGuildUser roleuser, [Remainder]IRole roleguild)
        {
            try
            {
                await roleuser.AddRoleAsync(roleguild);
                await Context.Channel.SendMessageAsync($"{roleguild} gave succesfully master! (◕‿◕✿) Am I fast today?");
            }
            catch
            {
                await Context.Channel.SendMessageAsync($"I can't do master! Please, check my perms (｀◕‸◕´+)");
            }

        }
        [Command("addrole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RoleCreator([Remainder]string roleguildcreate)
        {

            Console.WriteLine(roleguildcreate);
            try
            {
                await Context.Guild.CreateRoleAsync(roleguildcreate);
                await Context.Channel.SendMessageAsync($"{roleguildcreate} created succesfully master! (◕‿◕✿) Am I fast today?");
            }
            catch
            {
                await Context.Channel.SendMessageAsync($"I can't do master! Please, check my perms (｀◕‸◕´+)");
            }

        }
        public async Task AnnounceJoinedUser(SocketGuildUser user) //Welcomes the new user
        {
            var channel = Context.Channel as SocketTextChannel; // Gets the channel to send the message in
            //wtf about new master are you traitor your nazi scum ?????
            await Context.Guild.GetTextChannel(592303821376716802).SendMessageAsync($"<@{user.Id}> joined! welcome master! ");
        }

    }
}
