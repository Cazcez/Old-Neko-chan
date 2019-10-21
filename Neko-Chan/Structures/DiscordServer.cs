using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neko_Chan.Structures
{
    public class DiscordServer
    {
        public ITextChannel AnnouncementChannel;
        public ITextChannel NsfwChannel
        {
            get
            {
                return NekoChanBot.Client.GetGuild(ServerID).GetTextChannel(00);
            }
        }
        public ulong ServerID
        {
            get
            {
                return 00;
            }
        }
    }
}
