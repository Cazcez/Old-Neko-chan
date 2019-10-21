using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Neko_Chan.Utily;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Neko_Chan.Commands
{
    class BotOwnerCommands
    {
        public partial class ComandSheet : ModuleBase<SocketCommandContext>
        {
            #region Streaming Commands

            [Command("streaming")]
            public async Task StreamingStatus(string urlforstream)
            {
                if (Context.Message.Author.Id == 275295325118398481 || Context.Message.Author.Id == 214358045046603776)
                {
                    // TODO
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Sorry sir ;( i can't do this \n its a bot owner command!");

                }


                #endregion
            }
        }
    }
}
