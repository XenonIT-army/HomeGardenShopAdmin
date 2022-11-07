using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.IO;
using Telegram.Bot.Types.InputFiles;

namespace HomeGardenShopAdmin.BotModels
{
    public static class BotSendModel
    {
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task<bool> SendTextAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken, string text)
        {
            bool res = true;
            try
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendTextAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }


            return res;
        }

        public static async Task<bool> SendTextWithReplyKeyboardButtonAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken, string text, ReplyKeyboardMarkup replyKeyboardMarkup)
        {
            bool res = true;
            try
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: text,
               replyMarkup: replyKeyboardMarkup,
               cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendTextWithLinkAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }

        public static async Task<bool> SendTextWithInlineKeyboardButtonAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken, string text, InlineKeyboardMarkup inlineKeyboardMarkup)
        {
            bool res = true;
            try
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: text,
               replyMarkup: inlineKeyboardMarkup,
               cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendTextWithLinkAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }

        public static async Task<bool> SendTextWithLinkAsync(ITelegramBotClient botClient, ChatId chatId, Update update, CancellationToken cancellationToken, string text)
        {
            bool res = true;
            try
            {
                Message message = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: text,
               parseMode: ParseMode.MarkdownV2,
               disableNotification: false,
               replyToMessageId: update.Message.MessageId,
               replyMarkup: new InlineKeyboardMarkup(
               InlineKeyboardButton.WithUrl(
                   "Check sendMessage method",
                   "https://core.telegram.org/bots/api#sendmessage")),
               cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendTextWithLinkAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }

        public static async Task<bool> SendPhotoWithTextAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken, string text, byte[] image)
        {
            bool res = true;
            try
            {
                if (image != null)
                {
                    Stream stream = new MemoryStream(image);
                    Message message = await botClient.SendPhotoAsync(
                        chatId: chatId,
                                   photo: new InputOnlineFile(stream),
                                   caption: $"{text}",
                                   parseMode: ParseMode.Html,
                                   cancellationToken: cancellationToken);
                }

            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendPhotoWithTextAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }


        public static async Task<bool> SendVoiceAsync(ITelegramBotClient botClient, ChatId chatId, Update update, CancellationToken cancellationToken, string text)
        {
            bool res = true;
            try
            {
                Message message = await botClient.SendAudioAsync(
                    chatId: chatId,
                    audio: "https://github.com/TelegramBots/book/raw/master/src/docs/audio-guitar.mp3",
                    //performer: "Joel Thomas Hunger",
                    //title: "Fun Guitar and Ukulele",
                    //duration: 91, // in seconds
                    cancellationToken: cancellationToken);

                //using (var stream = System.IO.File.OpenRead("/path/to/voice-nfl_commentary.ogg"))
                //{
                //    message = await botClient.SendVoiceAsync(
                //        chatId: chatId,
                //        voice: stream,
                //        duration: 36,
                //        cancellationToken: cancellationToken);
                //}
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendVoiceAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }

        public static async Task<bool> SendVideoAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken, string text)
        {
            bool res = true;
            try
            {
                Message message = await botClient.SendVideoAsync(
                    chatId: chatId,

                    video: "https://raw.githubusercontent.com/TelegramBots/book/master/src/docs/video-countdown.mp4",
                    thumb: "https://raw.githubusercontent.com/TelegramBots/book/master/src/2/docs/thumb-clock.jpg",
                    supportsStreaming: true,
                      duration: 47,
                    cancellationToken: cancellationToken);

                //using (var stream = System.IO.File.OpenRead("/path/to/video-waves.mp4"))
                //{
                //    message = await botClient.SendVideoNoteAsync(
                //        chatId: chatId,
                //        videoNote: stream,
                //        duration: 47,
                //        length: 360, // value of width/height
                //        cancellationToken: cancellationToken);
                //}
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendVideoAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }

        public static async Task<bool> SendMediaGroupAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken, byte[] image)
        {
            bool res = true;
            try
            {
                if (image != null)
                {
                    Stream stream = new MemoryStream(image);

                    Message[] messages = await botClient.SendMediaGroupAsync(
                        chatId: chatId,
                        media: new IAlbumInputMedia[]
                        {
                        new InputMediaPhoto(new InputMedia(stream, "name"))
                        },
                        cancellationToken: cancellationToken);
                }
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendMediaGroupAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }

       

        public static async Task<bool> SendDocumentAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken, string text)
        {
            bool res = true;
            try
            {
                Message message = await botClient.SendDocumentAsync(
                    chatId: chatId,
                    document: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",
                    caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);


            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendDocumentAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }
        public static async Task<bool> SendPollAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken, string question, string[] options, bool isAnonymous = true)
        {
            bool res = true;
            try
            {
                Message pollMessage = await botClient.SendPollAsync(
                    chatId: chatId,
                    question: question,
                    options: options,
                    isAnonymous: isAnonymous,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendPollAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }

        public static async Task<bool> SendContactAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken, string phoneNumber, string firstName, string lastname, string email, string description)
        {
            bool res = true;
            try
            {
                Message message = await botClient.SendContactAsync(
                    chatId: chatId,
                    phoneNumber: $"{phoneNumber}",
                    firstName: $"{firstName}",
                    vCard: "BEGIN:VCARD\n" +
                    "VERSION:3.0\n" +
                    $"N:{lastname};Han\n" +
                    $"ORG:{description}\n" +
                    $"TEL;TYPE=voice,work,pref:{phoneNumber}\n" +
                    $"EMAIL:{email}\n" +
                    "END:VCARD",
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendContactAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }
        public static async Task<bool> SendPinAsync(ITelegramBotClient botClient, ChatId chatId, Update update, CancellationToken cancellationToken, string title, string addres)
        {
            bool res = true;
            try
            {
                Message message = await botClient.SendVenueAsync(
                    chatId: chatId,
                    latitude: 50.0840172f,
                    longitude: 14.418288f,
                    title: title,
                    address: addres,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendPinAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }
        public static async Task<bool> SendLocationAsync(ITelegramBotClient botClient, ChatId chatId, Update update, CancellationToken cancellationToken, string text)
        {
            bool res = true;
            try
            {
                Message message = await botClient.SendLocationAsync(
                    chatId: chatId,
                    latitude: 33.747252f,
                    longitude: -112.633853f,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(SendLocationAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }


        public static async Task<bool> RemoveButtonsAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken, string text)
        {
            bool res = true;
            try
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: text,
                   replyMarkup: new ReplyKeyboardRemove(),
                   cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"{nameof(RemoveButtonsAsync)} Received a '{ex.ToString()}' message in chat {chatId}.");
            }
            return res;
        }
    }
}
