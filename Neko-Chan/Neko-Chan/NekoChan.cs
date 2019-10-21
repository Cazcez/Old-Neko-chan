using Discord.WebSocket;
using Neko_Chan.Sql;
using Neko_Chan.Structures;
using NekosSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neko_Chan
{
    static class NekoChanBot
    {
        public static Dictionary<ulong, DiscordServer> registeredServers = new Dictionary<ulong, DiscordServer>();
        public static NekoClient NekoAPI = new NekoClient("NekoClient");
        //public static TSql GuildTableSql = new TSql(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Dropbox\Dropbox\Neko-Chan\Neko-Chan\NekoBase.mdf;Integrated Security = True", "Guilds");
        private const string DatabasePath = @"NekoBase.sqlite";
        public static SQLite sqlManager;
        public static DiscordSocketClient Client;

        static NekoChanBot()
        {
            if (!File.Exists(DatabasePath))
            {
                SQLite.DatabaseCreationBlock dbCreationblock = new SQLite.DatabaseCreationBlock()
                {
                    DatabaseName = "NekoChan",
                    TableName = "Guilds",
                    TableParams = new string[] { "Id", "INTEGER", "GuildId", "INTEGER", "AnnocChannel", "INTEGER","NsfwChannel","INTEGER" }
                };

                bool result = SQLite.CreateDB(DatabasePath, dbCreationblock);

                if (result == false)
                {
                    throw new Exception("Database init failed");
                }
            }
            sqlManager = new SQLite(string.Format(@"Data Source={0};Version=3;", DatabasePath), "Guilds");

        }

    }
}
