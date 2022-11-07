using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;
using HomeGardenShopAdmin.Moduls;
using Microsoft.VisualBasic;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
                Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
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
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Привет я бот админ магазина Макса, через меня ты можешь создавать новые продукты и категории к ним и много другого!🧙‍♀️");

                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Выбери категорию😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
            }
            else if (messageText == "1.Новости")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Ты выбрал категорию: Новости! 🤔");

                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Что нужно сделать?🫣", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetNewsButtons())));


            }
            else if (messageText == "2.Продукты")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Ты выбрал категорию: Продукты! 🤔");

                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Что нужно сделать?🫣", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetProductsButtons())));


            }
            else if (messageText == "3.Категории")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Ты выбрал категорию: Категории товаров! 🤔");

                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Что нужно сделать?🫣", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetCategorysButtons())));


            }
            else if (messageText == "4.Заказы")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Ты выбрал категорию: Заказы! 🤔");
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"К сожалению данная категория пока не доступна! 😰");
                // var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Что нужно сделать?🫣", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetNewsButtons())));


            }
            else if (messageText == "5.О нас")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Ты выбрал категорию: О нас! 🤔");

                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Что нужно сделать?🫣", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetAboutUsButtons())));


            }
           
            else if (messageText == "7.Показать все")
            {
                BotCommandsModel.GetAllNewsCommand(kernel, chatId, botClient, cancellationToken);

            }
            else if (messageText == "8.Найти")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Чтобы найти новость укажи тег get news id новости");
            }
            else if (messageText == "9.Создать")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Новости могут быть двух типов: с картинкой и без.\nЧто бы создать новость с картинкой выбери фото из галереи и сделай к нему подпись.\nДля новости без картинки можно использовать просто сообщение.\nВ тексте обязательно укажи теги create news nameRU:...;nameUA:...;nameEN:...;descRU:...;descUA:...;descEN:...; это название и описание новости. Можно указать  по одному тегу названия и описания тогда текст продублируется на остальные языки.😉");
            }
            else if (messageText == "10.Удалить")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Чтобы удалить новость укажи тег remove news и id новости.😉");
            }
            else if (messageText == "11.Изменить")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Чтобы редактировать новость укажи тег edit news id новости и любое поле например: nameRU:...;😉");
            }
            else if (messageText == "12.Назад")
            {
                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Выбери категорию😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
            }
            else if (messageText == "13.Показать все")
            {
                BotCommandsModel.GetAllProductCommand(kernel, chatId, botClient, cancellationToken);
            }
            else if (messageText == "14.Найти")
            {
                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Выбери фильтр поиска продукта!😊", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetProductsFilterButtons())));

            }
            else if (messageText == "14.1.Категория")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что бы найти продукт по категории укажи теги get prod category id категории.😉");
            }
            else if (messageText == "14.2.Название")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что бы найти продукт по названию укажи теги get prod name... ключевые слова для поиска.😉");
            }
            else if (messageText == "14.3.Цена")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что бы найти продукт по цене укажи теги get prod price ... диапазон цен, например 100 - 300.😉");
            }
            else if (messageText == "14.4.Id")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что бы найти продукт по категории укажи теги get prod id продукта.😉");
            }
            else if (messageText == "14.5.Назад")
            {
                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Выбери категорию😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.GetProductsButtons())));
            }
            else if (messageText == "15.Создать")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что бы создать продукт выбери фото из галереи и сделай к нему подпись.\nВ тексте обязательно укажи теги create prod nameRU:...;nameUA:...;nameEN:...;descRU:...;descUA:...;descEN:...;count:...;categoryId:...;price:...;discountPrice:...; Можно указать  по одному тегу названия и описания тогда текст продублируется на остальные языки.😉");
            }
            else if (messageText == "16.Удалить")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Чтобы удалить продукт укажи тег remove prod и id.😉");
            }
            else if (messageText == "17.Изменить")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Чтобы редактировать продукт укажи тег edit prod id продукта и любое поле например: nameRU:...;price:...;😉");
            }
            else if (messageText == "18.Назад")
            {
                var res = await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Выбери категорию😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
            }

            else if (messageText == "19.Показать все")
            {

                BotCommandsModel.GetAllCategoryCommand(kernel, chatId, botClient, cancellationToken);
              
            }
            else if (messageText == "20.Найти")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Чтобы найти категорию укажи тег get category id новости");
            }
            else if (messageText == "21.Создать")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что бы создать категорию укажи теги create category nameRU:...;nameUA:...;nameEN:...; Можно указать один тег названия тогда текст продублируется на остальные языки.😉");
            }
            else if (messageText == "22.Удалить")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Чтобы удалить категорию укажи тег remove category и id категории. Важно: при удалении категории все товары данной категории так же удаляются!😉");
            }
            else if (messageText == "23.Изменить")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Чтобы редактировать категорию укажи тег edit category id категории и любое поле например: nameRU:...;😉");
            }
            else if (messageText == "24.Назад")
            {
               await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Выбери категорию😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
            }


            else if (messageText == "25.Показать")
            {

                BotCommandsModel.GetAboutUsCommand(kernel, chatId, botClient, cancellationToken);

            }
            else if (messageText == "26.Создать")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Важно: при создании категории о нас, прошлая категория о нас удалиться!😉");
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что бы создать категорию о нас укажи теги create aboutUs nameCompany:...;descriptionRU:...;descriptionUA:...;descriptionEN:...; Можно указать один тег описания тогда текст продублируется на остальные языки.😉");
            }
            else if (messageText == "27.Удалить")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Чтобы удалить категорию о нас укажи тег remove aboutUs.😉");
            }
            else if (messageText == "28.Изменить")
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Чтобы редактировать категорию о нас укажи тег edit aboutUs и любое поле например: nameCompany:...;😉");
            }
            else if (messageText == "29.Назад")
            {
                await BotSendModel.SendTextWithReplyKeyboardButtonAsync(botClient, chatId, cancellationToken, "Выбери категорию😌", ReplyKeyboardChat.GetReplyKeyboardMarkupAsync(ReplyKeyboardChat.GetKeyboardMyltiButtonAsync(botRequest.StartButtons())));
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
                await BotSendModel.RemoveButtonsAsync(botClient, chatId, cancellationToken, "Чат закрыт, что бы начать заново напиши start. Буду тебя ждать!🥺");
            }
            else
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Данная команда не найдена;😢");
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
