using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;
using HomeGardenShopAdmin.Moduls;
using Microsoft.VisualBasic;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace HomeGardenShopAdmin.BotModels
{
    public class ChatBotModel
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient(AppInfo.BotId);

        private static CancellationTokenSource cts = new CancellationTokenSource();

        private static ChatId chatId;

        private static BotRequestModel botRequest = new BotRequestModel();

        private static IKernel kernel;
       
        public ChatBotModel()
        {
            try
            {
                kernel = new StandardKernel(new ProductNinjectModule());
            }
            catch (Exception ex)

            {

            }
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                BotSendModel.HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);
            var me = bot.GetMeAsync();


            //Console.WriteLine($"Start listening for @{me.Id}");
            Console.ReadLine();
            cts.Cancel();
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            //if (update.Type != UpdateType.Message)
            //    return;
            //// Only process text messages
            //if (update.Message!.Type != MessageType.Text)
            //    return;


            if (update.Type == UpdateType.ChannelPost)
            {
                chatId = update.ChannelPost.Chat.Id;
                var messageText = update.ChannelPost.Text; 
                //List<string[]> list = new List<string[]>();
                //string[] button = { "first", "second", "thrird" };
                //string[] button2 = { "i", "am", "maks" };
                //list.Add(button);
                //list.Add(button2);
                //var res = ReplyKeyboardChat.GetReplyKeyboardAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(list));


                //await SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "give me your number", res);
                List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                Dictionary<string, string> line1 = new Dictionary<string, string>();
                Dictionary<string, string> line2 = new Dictionary<string, string>();
                line1.Add("name", "Maks");
                line1.Add("old", "22");
                line2.Add("cat", "Chiz");
                line2.Add("old", "12");
                list.Add(line1);
                list.Add(line2);
                var res = InlineKeyboardChat.GetInlineKeyboardMarkupAsync(InlineKeyboardChat.GetInlineKeyboardMyltiButtonAsync(list));
                await BotSendModel.SendTextWithInlineKeyboardButtonAsync(botClient, chatId, cancellationToken, "info", res);

            }
            else if (update.Type == UpdateType.Message)
            {

                chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;
                var caption = update.Message.Caption;
                //Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
                if(messageText !=null)
                {
                    GetMessage(messageText, botClient, cancellationToken);
                }
                if (caption != null)
                {
                    var photo = update.Message.Photo.LastOrDefault();
                    if(photo != null)
                    {
                        GetCaption(caption, botClient, cancellationToken, photo);
                    }
                    else
                    {
                        await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
                    }
                }
                
                //else
                //{
                //    List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                //    Dictionary<string, string> line1 = new Dictionary<string, string>();
                //    Dictionary<string, string> line2 = new Dictionary<string, string>();
                //    line1.Add("name", "Maks");
                //    line1.Add("old", "22");
                //    line2.Add("cat", "Chiz");
                //    line2.Add("old", "12");
                //    list.Add(line1);
                //    list.Add(line2);
                //    //var res = InlineKeyboardChat.GetInlineKeyboardMarkupAsync(InlineKeyboardChat.GetInlineKeyboardMyltiButtonAsync(list));
                //    //if(update.Message.Text == "hi")
                //    //await botClient.DeleteMessageAsync(chatId, update.Message.MessageId, cancellationToken);

                //    string[] options = { "Maks", "Andrey", "Misha" };
                //    await SendPollAsync(botClient, chatId, cancellationToken, "My name?", options);
                //    // await SendTextWithInlineKeyboardButtonAsync(botClient, chatId, cancellationToken, "info", res);

                //    await bot.SendTextMessageAsync(360569879, "hi");
                //}

            }
            else if (update.Type == UpdateType.Poll)
            {
                //chatId = update.Poll.;
                foreach (var item in update.Poll.Options)
                {
                    Console.WriteLine($"Received a Answer: {item.Text} Count: {item.VoterCount} message in chat {chatId}.");
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Answer: {item.Text} Count: {item.VoterCount}");
                }
            }
            else
            {
                return;
            }

            //List<string[]> list = new List<string[]>();
            //string[] button = { "first", "second", "thrird" };
            //string[] button2 = { "i", "am", "maks" };
            //list.Add(button);
            //list.Add(button2);
            //var res = GetReplyKeyboardAsync(GetKeyboardMyltiButtonAsync(list));

            //var res = GetReplyKeyboardAsync(GetKeyboardButtonAsync(button));

            //Message sentMessage = await botClient.SendTextMessageAsync(
            //    chatId: chatId,
            //    text: "Who or Where are you?",
            //    replyMarkup: GetLocationReplyKeyboardAsync(),
            //    cancellationToken: cancellationToken);

            //Message sentMessage2 = await botClient.SendTextMessageAsync(
            //    chatId: chatId,
            //    text: "Choose a response",
            //    replyMarkup: res,
            //    cancellationToken: cancellationToken);

        }
      
        public static async void GetMessage(string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            if (messageText.ToLower().Contains("start"))
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Input Admin password🧙‍♀️");

                //var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Input admin password", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
            }
            else if(messageText == "admin123")
            {
                AppInfo.Admin = chatId;
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Hi Admin!✌️");

                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Select language", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardButtonAsync(botRequest.LanguageButtons())));

            }
           
            else  if (AppInfo.Admin == chatId)
            {
                if (AppInfo.CurrentUICulture !=null)
                {
                    CultureInfo.CurrentUICulture = AppInfo.CurrentUICulture;
                }

                if (messageText == "ru-RU" || messageText == "uk-UA" || messageText == "en-US")
            {
                    CultureInfo newCulture = new CultureInfo(messageText);
                    AppInfo.CurrentUICulture = newCulture;
                    CultureInfo.CurrentUICulture = AppInfo.CurrentUICulture;
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String1}🧙‍♀️");

                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String2}😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
            }
            else if (messageText == $"1.{Properties.Resource.String32}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String3} 🤔");
                    var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String4}🫣", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetNewsButtons())));

            }
            else if (messageText == $"2.{Properties.Resource.String33}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String6} 🤔");

                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String4}🫣", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetProductsButtons())));


            }
            else if (messageText == $"3.{Properties.Resource.String34}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String4} 🤔");

                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String4}🫣", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetCategorysButtons())));


            }
            else if (messageText == $"4.{Properties.Resource.String35}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String7} 🤔");
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String8} 😰");
                // var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Что нужно сделать?🫣", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetNewsButtons())));


            }
            else if (messageText == $"5.{Properties.Resource.String36}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String9} 🤔");

                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String4}🫣", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetAboutUsButtons())));


            }
           
            else if (messageText == $"7.{Properties.Resource.String38}")
            {
                BotCommandsModel.GetAllNewsCommand(kernel, chatId, botClient, cancellationToken);

            }
            else if (messageText == $"8.{Properties.Resource.String39}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String10}");
            }
            else if (messageText == $"9.{Properties.Resource.String40}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String11}😉");
            }
            else if (messageText == $"10.{Properties.Resource.String41}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String12}😉");
            }
            else if (messageText == $"11.{Properties.Resource.String42}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String13}😉");
            }
            else if (messageText == $"12.{Properties.Resource.String43}")
            {
                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String2}😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
            }
            else if (messageText == $"13.{Properties.Resource.String38}")
            {
                BotCommandsModel.GetAllProductCommand(kernel, chatId, botClient, cancellationToken);
            }
            else if (messageText == $"14.{Properties.Resource.String39}")
            {
                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String14}😊", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetProductsFilterButtons())));

            }
            else if (messageText == $"14.1.{Properties.Resource.String45}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String15}😉");
            }
            else if (messageText == $"14.2.{Properties.Resource.String46}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String16}😉");
            }
            else if (messageText == $"14.3.{Properties.Resource.String47}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String17}😉");
            }
            else if (messageText == $"14.4.Id")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String18}😉");
            }
            else if (messageText == $"14.5.{Properties.Resource.String43}")
            {
                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String2}😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetProductsButtons())));
            }
            else if (messageText == $"15.{Properties.Resource.String40}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String19}😉");
            }
            else if (messageText == $"16.{Properties.Resource.String41}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String20}😉");
            }
            else if (messageText == $"17.{Properties.Resource.String42}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String21}😉");
            }
            else if (messageText == $"18.{Properties.Resource.String43}")
            {
                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String2}😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
            }

            else if (messageText == $"19.{Properties.Resource.String38}")
            {

                BotCommandsModel.GetAllCategoryCommand(kernel, chatId, botClient, cancellationToken);
              
            }
            else if (messageText == $"20.{Properties.Resource.String39}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String22}😉");
            }
            else if (messageText == $"21.{Properties.Resource.String40}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String23}😉");
            }
            else if (messageText == $"22.{Properties.Resource.String41}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String24}😉");
            }
            else if (messageText == $"23.{Properties.Resource.String42}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String25}😉");
            }
            else if (messageText == $"24.{Properties.Resource.String43}")
            {
               await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String2}😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
            }


            else if (messageText == $"25.{Properties.Resource.String44}")
            {

                BotCommandsModel.GetAboutUsCommand(kernel, chatId, botClient, cancellationToken);

            }
            else if (messageText == $"26.{Properties.Resource.String40}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String26}😉");
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String27}😉");
            }
            else if (messageText == $"27.{Properties.Resource.String41}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String28}😉");
            }
            else if (messageText == $"28.{Properties.Resource.String42}")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String29}😉");
            }
            else if (messageText == $"29.{Properties.Resource.String43}")
            {
                await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String2}😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
            }



            else if (messageText.Contains("create news"))
            {
                BotCommandsModel.CreateNewsCommand(kernel, chatId, messageText, botClient, cancellationToken,null);
            }
            else if (messageText.Contains("get news"))
            {
                BotCommandsModel.GetNewsCommand(kernel, chatId, messageText, botClient, cancellationToken);
            }
            else if (messageText.Contains("remove news"))
            {
                BotCommandsModel.RemoveNewsCommand(kernel, chatId, messageText, botClient, cancellationToken);
            }
            else if (messageText.Contains("edit news"))
            {
                BotCommandsModel.EditNewsCommand(kernel, chatId, messageText, botClient, cancellationToken, null);
            }
            else if (messageText.Contains("create prod"))
            {
                BotCommandsModel.CreateProductCommand(kernel, chatId, messageText, botClient, cancellationToken, null);
            }
            else if (messageText.Contains("remove prod"))
            {
                BotCommandsModel.RemoveProductCommand(kernel, chatId, messageText, botClient, cancellationToken);
            }
            else if (messageText.Contains("edit prod"))
            {
                BotCommandsModel.EditProductCommand(kernel, chatId, messageText, botClient, cancellationToken, null);
            }

            else if (messageText.Contains("get prod category"))
            {
                BotCommandsModel.GetProductCommand(kernel, chatId, messageText, GetProductEnum.GetByCategory, botClient, cancellationToken);
            }
            else if (messageText.Contains("get prod name"))
            {
                BotCommandsModel.GetProductCommand(kernel, chatId, messageText, GetProductEnum.GetByName, botClient, cancellationToken);
            }
            else if (messageText.Contains("get prod price"))
            {
                BotCommandsModel.GetProductCommand(kernel, chatId, messageText, GetProductEnum.GetByPrice, botClient, cancellationToken);
            }
            else if (messageText.Contains("get prod"))
            {
                BotCommandsModel.GetProductCommand(kernel, chatId, messageText, GetProductEnum.GetByID, botClient, cancellationToken);
            }

            else if (messageText.Contains("create category"))
            {
                BotCommandsModel.CreateCategoryCommand(kernel, chatId, messageText, botClient, cancellationToken);
            }
            else if (messageText.Contains("get category"))
            {
                BotCommandsModel.GetCategoryCommand(kernel, chatId, messageText, botClient, cancellationToken);
            }
            else if (messageText.Contains("remove category"))
            {
                BotCommandsModel.RemoveCategoryCommand(kernel, chatId, messageText, botClient, cancellationToken);
            }
            else if (messageText.Contains("edit category"))
            {
                BotCommandsModel.EditCategoryCommand(kernel, chatId, messageText, botClient, cancellationToken);
            }

            else if (messageText.Contains("create aboutUs"))
            {
                BotCommandsModel.CreateAboutUsCommand(kernel, chatId, messageText, botClient, cancellationToken);
            }
            else if (messageText.Contains("remove aboutUs"))
            {
                BotCommandsModel.RemoveAboutUsCommand(kernel, chatId, messageText, botClient, cancellationToken);
            }
            else if (messageText.Contains("edit aboutUs"))
            {
                BotCommandsModel.EditAboutUsCommand(kernel, chatId, messageText, botClient, cancellationToken);
            }

            else if (messageText.ToLower().ToLower() == "exit" || messageText == "6.Выйти")
            {
                await BotSendModel.RemoveButtonsAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String30}🥺");
            }
            else
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"{Properties.Resource.String31}😢");
            }
            }
            else
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Incorrect password😔");
            }
        }
        public static async void GetCaption(string caption, ITelegramBotClient botClient, CancellationToken cancellationToken,PhotoSize photo)
        {
           
            byte[] bytes = { };
            var file = await bot.GetFileAsync(photo.FileId);
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    await botClient.DownloadFileAsync(file.FilePath, stream);
                    bytes = stream.ToArray();
                }
            }
            catch(Exception ex)
            {

            }
           
            if (caption.ToLower().Contains("create news"))
            {
              
                BotCommandsModel.CreateNewsCommand(kernel, chatId, caption, botClient, cancellationToken, bytes);
            }
            else if (caption.ToLower().Contains("edit news"))
            {
               
                BotCommandsModel.EditNewsCommand(kernel, chatId, caption, botClient, cancellationToken, bytes);
            }
            else if (caption.ToLower().Contains("create prod"))
            {
               
                BotCommandsModel.CreateProductCommand(kernel, chatId, caption, botClient, cancellationToken, bytes);
            }
            else if (caption.ToLower().Contains("edit prod"))
            {

                BotCommandsModel.EditProductCommand(kernel, chatId, caption, botClient, cancellationToken, bytes);
            }
        }


    }
}
