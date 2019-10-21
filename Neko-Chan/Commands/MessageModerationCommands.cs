using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Neko_Chan.Utily;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Neko_Chan.Commands
{
    public partial class ComandSheet
    {
        #region Administrator Commands
        [Command("alert")]
        [Summary("Alert <.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task Alert([Remainder]string text)
        {
            if (guildCooldowns.ContainsKey(Context.Guild.Id) == true)
            {
                DateTime cooldownDate = guildCooldowns[Context.Guild.Id];
                TimeSpan span = DateTime.Now.Subtract(cooldownDate);
                if (span != TimeSpan.Zero)
                {
                    await Context.Channel.SendMessageAsync("cooldown so wait you little faggot");
                    DateTime cooldownDateCalc = DateTime.Now;
                    TimeSpan value = cooldownDate.Subtract(cooldownDateCalc);
                    await Context.Channel.SendMessageAsync($"{value.Seconds} seconds left master!");
                    //TODO CHANGE THIS TEXT
                }
            }
            else
            {
                guildCooldowns.Add(Context.Guild.Id, DateTime.Now.AddMinutes(1));
                await Context.Channel.SendMessageAsync("1 minute cooldown started"); //TODO CHANGE THIS TEXT
                EmbedBuilder builder = new EmbedBuilder();
                EmbedFooterBuilder footerBuilder = new EmbedFooterBuilder();
                footerBuilder.WithText("Sent by " + Context.Message.Author + " from " + Context.Guild.Name);
                footerBuilder.WithIconUrl(Context.Message.Author.GetAvatarUrl());
                builder.WithFooter(footerBuilder);
                builder.WithColor(0, 128, 0);
                builder.WithTitle(text);
                foreach (var user in Context.Guild.Users)
                {
                    await Task.Delay(250);

                    VisualConsole.Write("TRY TO SEND==>> " + user, ConsoleColor.Yellow);
                    tryingToSend += 1;
                    if (user.IsBot != true && user.Status == UserStatus.Online || user.Status == UserStatus.DoNotDisturb || user.IsBot != true && user.Status == UserStatus.Idle)
                    {
                        try
                        {
                            VisualConsole.Write("ELIGHABLE USER==>> " + user, ConsoleColor.Cyan);
                            // await Discord.UserExtensions.SendMessageAsync(user, text);
                            await Discord.UserExtensions.SendMessageAsync(user, "", false, builder.Build());

                        }
                        catch
                        {
                            await Context.Channel.SendMessageAsync($"<@{user.Id}> blocked me sir! i can't send messages (｀◕‸◕´+)");
                        }
                        VisualConsole.Write("MESSAGE SENDED TO==>> " + user, ConsoleColor.Green);
                        sended += 1;
                    }
                    VisualConsole.Write($"TRY TO SEND {tryingToSend} SENDED {sended}", ConsoleColor.Red);

                }
                await Context.Channel.SendMessageAsync("Your message sent all online users sir");
                await Task.Delay(60000);
                guildCooldowns.Remove(Context.Guild.Id);

            }


        }
        [Command("purge")]
        [Summary("Deletes the specified amount of messages.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeChat(int amount)
        {

            var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync(); //defualt is 100
            await ((SocketTextChannel)Context.Channel).DeleteMessagesAsync(messages);

        }
        #endregion
        #region User Commands

        [Command("nya")]
        public async Task NyaSender()
        {
            string msg = RandomNyaMessages;
            await Context.Channel.SendMessageAsync(msg);
        }
        [Command("nya")]
        public async Task NyaSenderArg([Remainder]string text)
        {
            await NyaSender();
        }
        [Command("meow")]
        public async Task MeowSenderArg([Remainder]string text)
        {
            await NyaSender();
        }
        [Command("meow")]
        public async Task MeowSender()
        {
            await NyaSender();
        }

        [Command("say")]
        public async Task Echo([Remainder]string text)
        {
            await Context.Channel.SendMessageAsync(text);
            //Console.WriteLine(yaoiPictureManager.RandomPicture());
        }
        #endregion
        #region Bot Commands
        [Command("announcements")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ChannelSet(ITextChannel channel)
        {
            ulong senderGuild = channel.GuildId;
            ulong senderTextChannel = channel.Id;

            //check if guild is registered
            if (NekoChanBot.sqlManager.RowIsExist($"GuildId = {senderGuild}") == false)
            {
                NekoChanBot.sqlManager.AddSingleRow(new string[] { "Id", "GuildId", "AnnocChannel", "NsfwChannel" }, 0, senderGuild, senderTextChannel, 0);
            }

            //Same channel
            bool IsAlreadyRegistered = NekoChanBot.sqlManager.ConditionIsMet($"WHERE GuildId = {senderGuild} AND AnoncChannel = {senderTextChannel}");
            if (IsAlreadyRegistered)
            {
                //channel already registered im passing
                return;
            }
            else
            {
                NekoChanBot.sqlManager.RunSqlQuery($"UPDATE Guilds SET AnoncChannel = {senderTextChannel} WHERE GuildId = {senderGuild}");
            }

            //_announcementChannel = channel; // TODO ünal bunu public yap altakiler de görsün la
        }
        [Command("welcome")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task WelcomeMessageSet([Remainder]string text )
        {
            ulong senderGuild = Context.Guild.Id;

            //check if guild is registered
            if (NekoChanBot.sqlManager.RowIsExist($"GuildId = {senderGuild}") == false)
            {
                NekoChanBot.sqlManager.AddSingleRow(new string[] { "Id", "GuildId", "AnnocChannel", "NsfwChannel", "WelcomeMessage" }, 0, senderGuild, 0, 0, text);
            }

            //Same channel
            bool IsAlreadyRegistered = NekoChanBot.sqlManager.ConditionIsMet($"WHERE GuildId = {senderGuild} AND WelcomeMessage = {text}");
            if (IsAlreadyRegistered)
            {
                //channel already registered im passing
                return;
            }
            else
            {
                NekoChanBot.sqlManager.RunSqlQuery($"UPDATE Guilds SET WelcomeMessage = {text} WHERE GuildId = {senderGuild}");
            }

            //_announcementChannel = channel; // TODO ünal bunu public yap altakiler de görsün la
        }
        //[Command("welcometest")]
        //[RequireUserPermission(GuildPermission.Administrator)]
        //public async Task WelcomeMessageTest([Remainder]string text)
        //{
        //    await Context.Channel.SendMessageAsync(NekoChanBot.sqlManager.AddSingleRow(text));
        //}

        #endregion
    }
}
