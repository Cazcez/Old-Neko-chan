using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.Webhook;
using Neko_Chan.Commands;

namespace Neko_Chan.Events
{
    public static class UserEvents
    {
        public static async Task OnUserJoined(SocketGuildUser user) //Welcomes the new user
        {
            //await ComandSheet._announcementChannel.SendMessageAsync($"Nya~ {user.Mention}, Are you my new master? b(=^‥^=)o"); //Welcomes the new user
        }
        public static async Task OnUserLeft(SocketGuildUser user) //Welcomes the new user
        {
            //await ComandSheet._announcementChannel.SendMessageAsync($"Nya~ master, {user.Mention} left");
        }
        public static async Task OnUserBanned(SocketGuildUser user) //Welcomes the new user
        {
            //await ComandSheet._announcementChannel.SendMessageAsync($"Nya~ master, {user.Mention} banned b(=^‥^=)o"); //Welcomes the new user
        }
        public static async Task OnUserUnBanned(SocketGuildUser user) //Welcomes the new user
        {
            //await ComandSheet._announcementChannel.SendMessageAsync($"Nya~ master, {user.Mention} unbanned b(=^‥^=)o"); //Welcomes the new user
        }
    }

}
