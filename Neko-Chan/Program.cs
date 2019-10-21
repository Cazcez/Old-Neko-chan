#define DEBUG_VERBOSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using Neko_Chan.Events;
using Neko_Chan;

namespace NekoChan
{
    class Program
    {
        public static Program Instance;
        CommandService commands;
        Random rnd = new Random();

        static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        string[] abusementMessages = new string[] { "Nya~~ >.< dont do that", "Im hungry >~< feed me!","i guess i caught ¯\\_(ツ)_/¯",
        "Awwww you scared me!","ฅ^•ﻌ•^ฅ","(◕‿◕✿) Am I fast today?","Do you want to feed me ༼ つ ◕_◕ ༽つ",
        "(=ↀωↀ=) Grrrrrr",":cat:","How are you today?	(｡◕‿‿◕｡)","Nya what is this 人◕ __ ◕人","(｀◕‸◕´+)"};


        string RandomAbuseMessage
        {
            get
            {
                return abusementMessages[rnd.Next(0, abusementMessages.Length)];
            }
        }

        private async Task MainAsync()
        {
            Instance = this;

            NekoChanBot.Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
            });

            NekoChanBot.Client.UserJoined += UserEvents.OnUserJoined;
            NekoChanBot.Client.UserLeft += UserEvents.OnUserLeft;

            commands = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug,
            });
            //AudioModule mod = new AudioModule(new AudioService());
            NekoChanBot.Client.MessageReceived += Client_MessageReceived;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);


            NekoChanBot.Client.Ready += Client_Ready;
            NekoChanBot.Client.Log += Client_Log;

            await NekoChanBot.Client.LoginAsync(TokenType.Bot, "NTkxOTg5NTUxNjg2MjIxODU2.XQ40NA.-Wt-tcjadz-K-PQt8lztKbGGVso");
            await NekoChanBot.Client.StartAsync();
            await Task.Delay(-1);
        }





        private async Task Client_Log(LogMessage arg)
        {
            Console.WriteLine($"{DateTime.Now} at {arg.Source} : {arg.Message}");
        }

        private async Task Client_Ready()
        {
            var Games = NekoChanBot.Client.Activity as Game;
            Game gameStatus = new Game("lewd things", ActivityType.Streaming);
            await NekoChanBot.Client.SetActivityAsync(gameStatus);
            //  await client.SetGameAsync("Hentai Nekos : Multiplayer ",null,ActivityType.Playing);
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {

            SocketUserMessage message = (SocketUserMessage)arg;
            SocketCommandContext contex = new SocketCommandContext(NekoChanBot.Client, message);
            if (contex.IsPrivate)
            {
                var u = contex.Message.Author;  // Or some other means of getting user_id
                string msg = RandomAbuseMessage;
                await Discord.UserExtensions.SendMessageAsync(u, msg);
                return;
            }

            if (contex.Message == null || contex.Message.Content == "")
                return;
            if (contex.User.IsBot == true)
                return;

            int argPos = 0;

            if (!(message.HasStringPrefix("n!", ref argPos) || message.HasMentionPrefix(NekoChanBot.Client.CurrentUser, ref argPos)))
                return;

            Console.WriteLine($"SENDER => {contex.User.Username} FROM {contex.Guild.Name} VIA {contex.Channel.Name}");
            var Result = await commands.ExecuteAsync(contex, argPos, null);

            //SearchResult res = commands.Search(contex, "nya");
            //var reeeeeee = await res.Commands[0].Command.ExecuteAsync(contex, null, null, commands.);

            if (!Result.IsSuccess)
                Console.WriteLine($"{DateTime.Now} Something went wrong with executing command. Text : {contex.Message.Content} Error : {Result.ErrorReason}");
        }
    }
}
