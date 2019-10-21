using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Discord.Audio;
using System.Diagnostics;
using NekosSharp;
using Neko_Chan.Utily;
using NekoChan;
using Neko_Chan.Structures;

namespace Neko_Chan.Commands
{
    //Bu ana root class buraya sadece değişkenleri yaz metodları ayırdığım classlara sınıfına göre yaz ;)
    public partial class ComandSheet
    {
        private static PictureManager yaoiPictureManager = new PictureManager("yaoi");
        private int sended, tryingToSend;


        private const string URL = "https://nekos.life/api/lewd/neko";
        private const string DATA = @"{""object"":{""name"":""Name""}}";

        private static Dictionary<ulong, DateTime> guildCooldowns = new Dictionary<ulong, DateTime>();

        //What is Mew ?_? nya is better than this isnt it^
        //neko girls legion accepted your request Nya~ (=｀ェ´=)
        string[] nyaMessages = new string[] { "Mew!!~ (=⌒‿‿⌒=)", "Mew!!~ (=｀ェ´=)",
            "Mew!!~ b(=^‥^=)o", "Mew!!~ ଲ( ⓛ ω ⓛ *)ଲ", "Mew!!~ ヾ(=｀ω´=)ノ”" ,
            "Mew!!~ (=^･ｪ･^=)", "Mew!!~ (ノω<。)", "Mew!!~ (^人^)", "Mew!!~ (ↀДↀ)⁼³₌₃",
            "Mew!!~ ((ΦωΦ))", "Mew!!~ (=ↀωↀ=)", "Mew!!~ (=①ω①=)", "Mew!!~ (✦థ ｪ థ)",
            "Mew!!~ ( =①ω①=)", "Mew!!~ (≚ᄌ≚)ℒℴѵℯ:heart:", "Mew!!~ (ะ`♔´ะ)",
            "Mew!!~ (・∀・)", "Mew!!~ (,,◕　⋏　◕,,)", "Mew!!~ (ꀄꀾꀄ)", "Mew!!~ (ꀄꀾꀄ)",
            "Mew!!~ (=；ェ；=)", "Mew!!~ V(=^･ω･^=)v", "Mew!!~ (ٛ₌டுͩ ˑ̭ டுͩٛ₌)ฅ",
            "Mew!!~ ฅ(^ω^ฅ),", "Mew!!~ (=;ェ;=)", "Mew!!~ ლ(=ↀωↀ=)ლ", "Mew!!~ ㅇㅅㅇ",
            "Nya~ (=⌒‿‿⌒=)", "Nya~ (=｀ェ´=)",
            "Nya~ b(=^‥^=)o", "Nya~ ଲ( ⓛ ω ⓛ *)ଲ", "Nya~ ヾ(=｀ω´=)ノ”" ,
            "Nya~ (=^･ｪ･^=)", "Nya~ (ノω<。)", "Nya~ (^人^)", "Nya~ (ↀДↀ)⁼³₌₃",
            "Nya~ ((ΦωΦ))", "Nya~ (=ↀωↀ=)", "Nya~ (=①ω①=)", "Nya~ (✦థ ｪ థ)",
            "Nya~ ( =①ω①=)", "Nya~ (≚ᄌ≚)ℒℴѵℯ:heart:", "Nya~ (ะ`♔´ะ)",
            "Nya~ (・∀・)", "Nya~ (,,◕　⋏　◕,,)", "Nya~ (ꀄꀾꀄ)", "Nya~ (ꀄꀾꀄ)",
            "Nya~ (=；ェ；=)", "Nya~ V(=^･ω･^=)v", "Nya~ (ٛ₌டுͩ ˑ̭ டுͩٛ₌)ฅ",
            "Nya~ ฅ(^ω^ฅ),", "Nya~ (=;ェ;=)", "Nya~ ლ(=ↀωↀ=)ლ", "Nya~ ㅇㅅㅇ"};
        Random rnd = new Random();

        public string RandomNyaMessages
        {
            get
            {
                return nyaMessages[rnd.Next(0, nyaMessages.Length)];
            }
        }
    }
}
