using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace HomeGardenShopAdmin.BotModels
{
    public class ReplyKeyboardChat
    {

        public static ReplyKeyboardMarkup GetReplyKeyboardMarkupAsync(List<KeyboardButton> buttons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(buttons)
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }

        public static ReplyKeyboardMarkup GetReplyKeyboardMarkupAsync(List<List<KeyboardButton>> buttons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(buttons)
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }
        public static ReplyKeyboardMarkup GetLocationReplyKeyboardAsync(string nameButton)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(KeyboardButton.WithRequestLocation(nameButton))
            {
                ResizeKeyboard = true,
            };
            return replyKeyboardMarkup;
        }
        public static ReplyKeyboardMarkup GetContactReplyKeyboardAsync(string nameButton)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(KeyboardButton.WithRequestContact(nameButton))
            {
                ResizeKeyboard = true,
            };
            return replyKeyboardMarkup;
        }
        public static ReplyKeyboardMarkup GetPollReplyKeyboardAsync(string nameButton)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(KeyboardButton.WithRequestPoll(nameButton))
            {
                ResizeKeyboard = true,
            };
            return replyKeyboardMarkup;
        }
        public static List<KeyboardButton> GetKeyboardButtonAsync(string[] name)
        {
            List<KeyboardButton> buttons =
            new List<KeyboardButton>();
            int count = name.Length;
            for (int i = 0; i < count; i++)
            {
                buttons.Add(name[i]);
            }
            return buttons;
        }

        public static List<List<KeyboardButton>> GetKeyboardMyltiButtonAsync(List<string[]> name, int countLine = 2)
        {
            List<List<KeyboardButton>> buttons =
           new List<List<KeyboardButton>>();

            int count = name.Count;

            if (count > 0)
            {

                for (int i = 0; i < countLine; i++)
                {
                    List<KeyboardButton> item =
                        new List<KeyboardButton>();

                    foreach (var res in name[i])
                    {
                        item.Add(res);
                    }
                    buttons.Add(item);
                }
            }
            return buttons;
        }
    }
}
