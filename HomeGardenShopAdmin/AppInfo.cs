using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HomeGardenShopAdmin
{
    public static class AppInfo
    {
        public static string BotId { get; set; } = "5661186200:AAGxKtGlUpKjQzk713-478DUTa4xwL10_X8";
        public static ChatId Admin { get; set; }

        public static CultureInfo CurrentUICulture { get; set; }
    }
}
