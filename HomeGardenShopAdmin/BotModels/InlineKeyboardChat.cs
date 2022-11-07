using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace HomeGardenShopAdmin.BotModels
{
    public class InlineKeyboardChat
    {
        public static InlineKeyboardMarkup GetInlineKeyboardMarkupAsync(List<InlineKeyboardButton> buttons)
        {
            InlineKeyboardMarkup replyKeyboardMarkup = new InlineKeyboardMarkup(buttons)
            {
            };
            return replyKeyboardMarkup;
        }

        public static InlineKeyboardMarkup GetInlineKeyboardMarkupAsync(List<List<InlineKeyboardButton>> buttons)
        {
            InlineKeyboardMarkup replyKeyboardMarkup = new InlineKeyboardMarkup(buttons)
            {
            };
            return replyKeyboardMarkup;
        }

        public static List<InlineKeyboardButton> GetInlineKeyboardButtonAsync(Dictionary<string, string> values)
        {
            List<InlineKeyboardButton> buttons =
            new List<InlineKeyboardButton>();

            foreach (var item in values)
            {
                var button = InlineKeyboardButton.WithCallbackData(text: item.Key, callbackData: item.Value);
                buttons.Add(button);
            }
            return buttons;
        }

        public static List<List<InlineKeyboardButton>> GetInlineKeyboardMyltiButtonAsync(List<Dictionary<string, string>> values, int countLine = 2)
        {
            List<List<InlineKeyboardButton>> buttons =
           new List<List<InlineKeyboardButton>>();

            for (int i = 0; i < countLine; i++)
            {
                List<InlineKeyboardButton> itemList =
                    new List<InlineKeyboardButton>();

                foreach (var item in values[i])
                {
                    var button = InlineKeyboardButton.WithCallbackData(text: item.Key, callbackData: item.Value);
                    itemList.Add(button);
                }
                buttons.Add(itemList);
            }
            return buttons;
        }

        public static InlineKeyboardMarkup GetInlineKeyboardWithLinkAsync(string text, string url)
        {
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: text,
                    url: url)
                });
            return inlineKeyboard;
        }
        public static InlineKeyboardMarkup GetInlineKeyboardWithSwitchAsync(string copyText, string shareText)
        {
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    InlineKeyboardButton.WithSwitchInlineQuery(copyText),
                    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(shareText),
                });

            return inlineKeyboard;
        }
    }
}
