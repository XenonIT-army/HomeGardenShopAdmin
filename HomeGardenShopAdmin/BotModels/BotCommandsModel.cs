using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;
using HomeGardenShopAdmin.Moduls;
using HomeGardenShopDAL.Entities;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HomeGardenShopAdmin.BotModels
{
    public static class BotCommandsModel
    {
        public static async void CreateNewsCommand(IKernel kernel, ChatId chatId, string messageText,  ITelegramBotClient botClient, CancellationToken cancellationToken, byte[] image)
        {
            IService<News> newsService = kernel.Get<IService<News>>();
            var news = new News();
            try
            {
                string? nameRes = messageText.Split(';').FirstOrDefault(x => x.Contains("nameRU"));
                string? nameResUA = messageText.Split(';').FirstOrDefault(x => x.Contains("nameUA"));
                string? nameResEN = messageText.Split(';').FirstOrDefault(x => x.Contains("nameEN"));
                string? descRes = messageText.Split(';').FirstOrDefault(x => x.Contains("descRU"));
                string? descResUA = messageText.Split(';').FirstOrDefault(x => x.Contains("descUA"));
                string? descResEN = messageText.Split(';').FirstOrDefault(x => x.Contains("descEN"));
                if (nameRes != null)
                {
                    string nameRU = nameRes.Split("nameRU:").Last();
                    news.Name = nameRU;
                }
                if (nameResUA != null)
                {
                    string nameUA = nameResUA.Split("nameUA:").Last();
                    news.NameUA = nameUA;
                }
                if (nameResEN != null)
                {
                    string nameEN = nameResEN.Split("nameEN:").Last();
                    news.NameEN = nameEN;
                }
                if (descRes != null)
                {
                    string descRU = descRes.Split("descRU:").Last();
                    news.Description = descRU;
                }
                if (descResUA != null)
                {
                    string descUA = descResUA.Split("descUA:").Last();
                    news.DescriptionUA = descUA;
                }
                if (descResEN != null)
                {
                    string descEN = descResEN.Split("descEN:").Last();
                    news.DescriptionEN = descEN;
                }
                if (image != null)
                {
                    news.Image = image;
                }

                List<string> strings = new List<string>();
                strings.Add(news.Name);
                strings.Add(news.NameUA);
                strings.Add(news.NameEN);
                if (news.Name == null)
                {
                    news.Name = strings.Where(x=> x != null).First();
                }
                if (news.NameUA == null)
                {
                    news.NameUA = strings.Where(x => x != null).First();
                }
                if (news.NameEN == null)
                {
                    news.NameEN = strings.Where(x => x != null).First();
                }
                strings.Clear();
                strings.Add(news.Description);
                strings.Add(news.DescriptionUA);
                strings.Add(news.DescriptionEN);
                if (news.Description == null)
                {
                    news.Description = strings.Where(x => x != null).First();
                }
                if (news.DescriptionUA == null)
                {
                    news.DescriptionUA = strings.Where(x => x != null).First();
                }
                if (news.DescriptionEN == null)
                {
                    news.DescriptionEN = strings.Where(x => x != null).First();
                }

                news.DateTime = DateTime.Now;
                if(image != null)
                {
                    news.Image = image;
                }
                await newsService.AddOrUpdate(news);
                await newsService.Save();
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Новость успешо добавлена.😏");
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }
        public static async void GetNewsCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<News> newsService = kernel.Get<IService<News>>();
            var news = new News();
            try
            {
                string res = messageText.Split("get news").Last();
                int id = Convert.ToInt32(res);
                var item = await newsService.Get(id);
                if (item != null)
                {
                    if (item.Image != null && item.Image.Length > 0)
                    {
                        await BotSendModel.SendPhotoWithTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\nКартинка:", item.Image);
                    }
                    else
                    {
                        await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\nКартинка: нет");
                    }
                }
                else
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Новость с таким id не найдена!😢");
                }
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }
        public static async void RemoveNewsCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<News> newsService = kernel.Get<IService<News>>();
            var news = new News();
            try
            {
                string res = messageText.Split("remove news").Last();
                int id = Convert.ToInt32(res);
                var item = await newsService.Get(id);
                if (item != null)
                {
                    await newsService.Delete(item);
                    await newsService.Save();
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Новость успешо удалена.😥");
                }
                else
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Новость с таким id не найдена!😢");
                }
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }
        public static async void EditNewsCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken, byte[] image)
        {
            IService<News> newsService = kernel.Get<IService<News>>();
            try
            {
                string res = messageText.Split(" ")[2];
                int id = Convert.ToInt32(res);
                var item = await newsService.Get(id);
                if (item != null)
                {

                    string? nameRes = messageText.Split(';').FirstOrDefault(x => x.Contains("nameRU"));
                    string? nameResUA = messageText.Split(';').FirstOrDefault(x => x.Contains("nameUA"));
                    string? nameResEN = messageText.Split(';').FirstOrDefault(x => x.Contains("nameEN"));
                    string? descRes = messageText.Split(';').FirstOrDefault(x => x.Contains("descRU"));
                    string? descResUA = messageText.Split(';').FirstOrDefault(x => x.Contains("descUA"));
                    string? descResEN = messageText.Split(';').FirstOrDefault(x => x.Contains("descEN"));
                    if (nameRes != null)
                    {
                        string nameRU = nameRes.Split("nameRU:").Last();
                        item.Name = nameRU;
                    }
                    if (nameResUA != null)
                    {
                        string nameUA = nameResUA.Split("nameUA:").Last();
                        item.NameUA = nameUA;
                    }
                    if (nameResEN != null)
                    {
                        string nameEN = nameResEN.Split("nameEN:").Last();
                        item.NameEN = nameEN;
                    }
                    if (descRes != null)
                    {
                        string descRU = descRes.Split("descRU:").Last();
                        item.Description = descRU;
                    }
                    if (descResUA != null)
                    {
                        string descUA = descResUA.Split("descUA:").Last();
                        item.DescriptionUA = descUA;
                    }
                    if (descResEN != null)
                    {
                        string descEN = descResEN.Split("descEN:").Last();
                        item.DescriptionEN = descEN;
                    }
                    if (image != null)
                    {
                        item.Image = image;
                    }
                    await newsService.AddOrUpdate(item);
                    await newsService.Save();
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Новость успешо отредактирована.😏");
                }
                else
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Новость с таким id не найдена!😢");
                }
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }
        public static async void GetAllNewsCommand(IKernel kernel, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationTokene)
        {
            IService<News> newsService = kernel.Get<IService<News>>();
            var list = await newsService.GetAll();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    if (item.Image != null && item.Image.Length > 0)
                    {
                        await BotSendModel.SendPhotoWithTextAsync(botClient, chatId, cancellationTokene, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\nКартинка:", item.Image);
                    }
                    else
                    {
                        await BotSendModel.SendTextAsync(botClient, chatId, cancellationTokene, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\nКартинка: нет");
                    }
                }
            }
            else
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationTokene, $"Список новостей пуст!😢");
            }
        }


            public static async void CreateProductCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken, byte[] image)
        {
            IService<Product> productService = kernel.Get<IService<Product>>();
            IService<Category> categoryService = kernel.Get<IService<Category>>();
            var prod = new Product();
            try
            {
                string? nameRes = messageText.Split(';').FirstOrDefault(x => x.Contains("nameRU"));
                string? nameResUA = messageText.Split(';').FirstOrDefault(x => x.Contains("nameUA"));
                string? nameResEN = messageText.Split(';').FirstOrDefault(x => x.Contains("nameEN"));
                string? descRes = messageText.Split(';').FirstOrDefault(x => x.Contains("descRU"));
                string? descResUA = messageText.Split(';').FirstOrDefault(x => x.Contains("descUA"));
                string? descResEN = messageText.Split(';').FirstOrDefault(x => x.Contains("descEN"));
                string? priceRes = messageText.Split(';').FirstOrDefault(x => x.Contains("price"));
                string? countRes = messageText.Split(';').FirstOrDefault(x => x.Contains("count"));
                string? discountPriceRes = messageText.Split(';').FirstOrDefault(x => x.Contains("discountPrice"));
                string? categoryIdRes = messageText.Split(';').FirstOrDefault(x => x.Contains("categoryId"));
                if (nameRes != null)
                {
                    string nameRU = nameRes.Split("nameRU:").Last();
                    prod.Name = nameRU;
                }
                if (nameResUA != null)
                {
                    string nameUA = nameResUA.Split("nameUA:").Last();
                    prod.NameUA = nameUA;
                }
                if (nameResEN != null)
                {
                    string nameEN = nameResEN.Split("nameEN:").Last();
                    prod.NameEN = nameEN;
                }
                if (descRes != null)
                {
                    string descRU = descRes.Split("descRU:").Last();
                    prod.Description = descRU;
                }
                if (descResUA != null)
                {
                    string descUA = descResUA.Split("descUA:").Last();
                    prod.DescriptionUA = descUA;
                }
                if (descResEN != null)
                {
                    string descEN = descResEN.Split("descEN:").Last();
                    prod.DescriptionEN = descEN;
                }

                List<string> strings = new List<string>();
                strings.Add(prod.Name);
                strings.Add(prod.NameUA);
                strings.Add(prod.NameEN);
                if (prod.Name == null)
                {
                    prod.Name = strings.Where(x => x != null).First();
                }
                if (prod.NameUA == null)
                {
                    prod.NameUA = strings.Where(x => x != null).First();
                }
                if (prod.NameEN == null)
                {
                    prod.NameEN = strings.Where(x => x != null).First();
                }
                strings.Clear();
                strings.Add(prod.Description);
                strings.Add(prod.DescriptionUA);
                strings.Add(prod.DescriptionEN);
                if (prod.Description == null)
                {
                    prod.Description = strings.Where(x => x != null).First();
                }
                if (prod.DescriptionUA == null)
                {
                    prod.DescriptionUA = strings.Where(x => x != null).First();
                }
                if (prod.DescriptionEN == null)
                {
                    prod.DescriptionEN = strings.Where(x => x != null).First();
                }

                if (priceRes != null)
                {
                    string price = priceRes.Split("price:").Last();
                    prod.Price = Convert.ToDouble(price); 
                }
                if (discountPriceRes != null)
                {
                    string discountPrice = discountPriceRes.Split("discountPrice:").Last();
                    prod.DiscountPrice = Convert.ToDouble(discountPrice);
                }
                if (countRes != null)
                {
                    string count = countRes.Split("count:").Last();
                    prod.Count = Convert.ToDouble(count);
                }
                if (categoryIdRes != null)
                {
                    string categoryId = categoryIdRes.Split("categoryId:").Last();
                    var category = await  categoryService.Get(Convert.ToInt32(categoryId));
                    if(category != null)
                    {
                        prod.CategoryId = Convert.ToInt32(categoryId);
                    }
                    else
                    {
                        await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория с таким id не найдена!😢");
                        return;
                    }
                }
                if (image != null)
                {
                    prod.Image = image;
                }
                await productService.AddOrUpdate(prod);
                await productService.Save();
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Продукт успешо добавлен.😏");
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов и формат данных😢");
            }
        }
        public static async void GetAllProductCommand(IKernel kernel, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<Product> products = kernel.Get<IService<Product>>();
            IService<Category> categorys = kernel.Get<IService<Category>>();
            var list = await products.GetAll();
            var categoryList = await categorys.GetAll();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var category = categoryList.Where(x => x.Id == item.CategoryId).FirstOrDefault();
                    string name = "";
                    if (category != null)
                    {
                        name = category.Name;
                    }
                    if (item.Image != null && item.Image.Length > 0)
                    {
                        await BotSendModel.SendPhotoWithTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\ncount: {item.Count}\nprice: {item.Price}\ndiscountPrice: {item.DiscountPrice}\ncategory: {name}\n", item.Image);
                    }
                    else
                    {
                        await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\ncount: {item.Count}\nprice: {item.Price}\ndiscountPrice: {item.DiscountPrice}\ncategory: {name}\n");
                    }
                }
            }
            else
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Список продуктов пуст!😢");
            }
        }

        public static async void RemoveProductCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<Product> productService = kernel.Get<IService<Product>>();
            try
            {
                string res = messageText.Split("remove prod").Last();
                int id = Convert.ToInt32(res);
                var item = await productService.Get(id);
                if (item != null)
                {
                    await productService.Delete(item);
                    await productService.Save();
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Продукт успешо удалена.😥");
                }
                else
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Продукт с таким id не найдена!😢");
                }
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }
        public static async void EditProductCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken, byte[] image)
        {
            IService<Product> productService = kernel.Get<IService<Product>>();
            IService<Category> categoryService = kernel.Get<IService<Category>>();
            try
            {
                string res = messageText.Split(" ")[2];
                int id = Convert.ToInt32(res);
                var item = await productService.Get(id);
                if (item != null)
                {
                    string? nameRes = messageText.Split(';').FirstOrDefault(x => x.Contains("nameRU"));
                    string? nameResUA = messageText.Split(';').FirstOrDefault(x => x.Contains("nameUA"));
                    string? nameResEN = messageText.Split(';').FirstOrDefault(x => x.Contains("nameEN"));
                    string? descRes = messageText.Split(';').FirstOrDefault(x => x.Contains("descRU"));
                    string? descResUA = messageText.Split(';').FirstOrDefault(x => x.Contains("descUA"));
                    string? descResEN = messageText.Split(';').FirstOrDefault(x => x.Contains("descEN"));
                    string? priceRes = messageText.Split(';').FirstOrDefault(x => x.Contains("price"));
                    string? discountPriceRes = messageText.Split(';').FirstOrDefault(x => x.Contains("discountPrice"));
                    string? countRes = messageText.Split(';').FirstOrDefault(x => x.Contains("count"));
                    string? categoryIdRes = messageText.Split(';').FirstOrDefault(x => x.Contains("categoryId"));


                    if (nameRes != null)
                    {
                        string nameRU = nameRes.Split("nameRU:").Last();
                        item.Name = nameRU;
                    }
                    if (nameResUA != null)
                    {
                        string nameUA = nameResUA.Split("nameUA:").Last();
                        item.NameUA = nameUA;
                    }
                    if (nameResEN != null)
                    {
                        string nameEN = nameResEN.Split("nameEN:").Last();
                        item.NameEN = nameEN;
                    }
                    if (descRes != null)
                    {
                        string descRU = descRes.Split("descRU:").Last();
                        item.Description = descRU;
                    }
                    if (descResUA != null)
                    {
                        string descUA = descResUA.Split("descUA:").Last();
                        item.DescriptionUA = descUA;
                    }
                    if (descResEN != null)
                    {
                        string descEN = descResEN.Split("descEN:").Last();
                        item.DescriptionEN = descEN;
                    }
                    if (priceRes != null)
                    {
                        string price = priceRes.Split("price:").Last();
                        item.Price = Convert.ToDouble(price);
                    }
                    if (discountPriceRes != null)
                    {
                        string discountPrice = discountPriceRes.Split("discountPrice:").Last();
                        item.DiscountPrice = Convert.ToDouble(discountPrice);
                    }
                    if (countRes != null)
                    {
                        string count = countRes.Split("count:").Last();
                        item.Count = Convert.ToDouble(count);
                    }
                    if (categoryIdRes != null)
                    {
                        string categoryId = categoryIdRes.Split("categoryId:").Last();
                        var category = await categoryService.Get(Convert.ToInt32(categoryId));
                        if (category != null)
                        {
                            item.CategoryId = Convert.ToInt32(categoryId);
                        }
                        else
                        {
                            await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория с таким id не найдена!😢");
                            return;
                        }
                    }
                    if (image != null)
                    {
                        item.Image = image;
                    }
                    await productService.AddOrUpdate(item);
                    await productService.Save();
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Продукт успешо отредактирован.😏");
                }
                else
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Продукт с таким id не найден!😢");
                }
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов и формат данных😢");
            }

        }

        public static async void GetProductCommand(IKernel kernel, ChatId chatId, string messageText, GetProductEnum prodEnum, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<Product> productService = kernel.Get<IService<Product>>();
            IService<Category> categorys = kernel.Get<IService<Category>>();
            var prod = new Product();
            try
            {
                if(prodEnum == GetProductEnum.GetByID)
                {
                    string res = messageText.Split("get prod").Last();
                    int id = Convert.ToInt32(res);
                    var item = await productService.Get(id);
                    var categoryList = await categorys.GetAll();
                    if (item != null)
                    {
                        var category = categoryList.Where(x => x.Id == item.CategoryId).FirstOrDefault();
                        string namecat = "";
                        if (category != null)
                        {
                            namecat = category.Name;
                        }
                        if (item.Image != null && item.Image.Length > 0)
                        {
                            await BotSendModel.SendPhotoWithTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\ncount: {item.Count}\nprice: {item.Price}\ndiscountPrice: {item.DiscountPrice}\ncategory: {namecat}\n", item.Image);
                        }
                        else
                        {
                            await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\ncount: {item.Count}\nprice: {item.Price}\ndiscountPrice: {item.DiscountPrice}\ncategory: {namecat}\n");
                        }
                    }
                    else
                    {
                        await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Продукт не найден!😢");
                    }
                }
                if (prodEnum == GetProductEnum.GetByCategory)
                {
                    string res = messageText.Split("get prod category").Last();
                    int id = Convert.ToInt32(res);
                    var list = await productService.GetAll();
                    var categoryList = await categorys.GetAll();
                    var items = list.Where(x => x.CategoryId == id);
                    if (items != null && items.Count() > 0)
                    {
                        foreach (var item in items)
                        {
                            var category = categoryList.Where(x => x.Id == item.CategoryId).FirstOrDefault();
                            if(category != null)
                            {
                                string namecat = "";
                                if (category != null)
                                {
                                    namecat = category.Name;
                                }
                                if (item.Image != null && item.Image.Length > 0)
                                {
                                    await BotSendModel.SendPhotoWithTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\ncount: {item.Count}\nprice: {item.Price}\ndiscountPrice: {item.DiscountPrice}\ncategory: {namecat}\n", item.Image);
                                }
                                else
                                {
                                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\ncount: {item.Count}\nprice: {item.Price}\ndiscountPrice: {item.DiscountPrice}\ncategory: {namecat}\n");
                                }
                            }
                            else
                            {
                                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория не найдена!😢");
                            }
                        }
                    }
                    else
                    {
                        await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Продукт не найден!😢");
                    }
                }
                if (prodEnum == GetProductEnum.GetByPrice)
                {
                    string res = messageText.Split("get prod price").Last();
                    double min = Convert.ToDouble(res.Split('-').First());
                    double max = Convert.ToDouble(res.Split('-').Last());
                    var categoryList = await categorys.GetAll();


                    var list = await productService.GetAll();
                    var listWithDiscount = list.Where(x => x.DiscountPrice > 0);
                    var listWithoutDiscount = list.Where(x => x.DiscountPrice == 0);
                    List<Product> products =  new List<Product>();
                    products.AddRange(listWithDiscount.Where(x => x.DiscountPrice >= min && x.DiscountPrice <= max));
                    products.AddRange(listWithoutDiscount.Where(x => x.Price >= min && x.Price <= max));
                    if (products.Count > 0)
                    {
                        foreach (var item in products)
                        {
                            var category = categoryList.Where(x => x.Id == item.CategoryId).FirstOrDefault();
                            string namecat = "";
                            if (category != null)
                            {
                                namecat = category.Name;
                            }
                            if (item.Image != null && item.Image.Length > 0)
                            {
                                await BotSendModel.SendPhotoWithTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\ncount: {item.Count}\nprice: {item.Price}\ndiscountPrice: {item.DiscountPrice}\ncategory: {namecat}\n", item.Image);
                            }
                            else
                            {
                                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\ncount: {item.Count}\nprice: {item.Price}\ndiscountPrice: {item.DiscountPrice}\ncategory: {namecat}\n");
                            }
                        }
                    }
                    else
                    {
                        await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Продукт не найден!😢");
                    }
                }
                if (prodEnum == GetProductEnum.GetByName)
                {
                    string res = messageText.Split("get prod name ").Last();
                    string name = res.ToLower();
                    var list = await productService.GetAll();
                    var categoryList = await categorys.GetAll();

                    var items = list.Where(x => x.Name.ToLower().Contains(name) || x.NameUA.ToLower().Contains(name) || x.NameEN.ToLower().Contains(name));
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            var category = categoryList.Where(x => x.Id == item.CategoryId).FirstOrDefault();
                            string namecat = "";
                            if (category != null)
                            {
                                namecat = category.Name;
                            }
                            if (item.Image != null && item.Image.Length > 0)
                            {
                                await BotSendModel.SendPhotoWithTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\ncount: {item.Count}\nprice: {item.Price}\ndiscountPrice: {item.DiscountPrice}\ncategory: {namecat}\n", item.Image);
                            }
                            else
                            {
                                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}\ndescRU: {item.Description}\ndescUA: {item.DescriptionUA}\ndescEN: {item.DescriptionEN}\ncount: {item.Count}\nprice: {item.Price}\ndiscountPrice: {item.DiscountPrice}\ncategory: {namecat}\n");
                            }
                        }
                    }
                    else
                    {
                        await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Продукт не найден!😢");
                    }
                }

            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }

        }
        public static async void CreateCategoryCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<Category> categoryService = kernel.Get<IService<Category>>();
            var category = new Category();
            try
            {
                string? nameRes = messageText.Split(';').FirstOrDefault(x => x.Contains("nameRU"));
                string? nameResUA = messageText.Split(';').FirstOrDefault(x => x.Contains("nameUA"));
                string? nameResEN = messageText.Split(';').FirstOrDefault(x => x.Contains("nameEN"));
                if (nameRes != null)
                {
                    string nameRU = nameRes.Split("nameRU:").Last();
                    category.Name = nameRU;
                }
                if (nameResUA != null)
                {
                    string nameUA = nameResUA.Split("nameUA:").Last();
                    category.NameUA = nameUA;
                }
                if (nameResEN != null)
                {
                    string nameEN = nameResEN.Split("nameEN:").Last();
                    category.NameEN = nameEN;
                }

                List<string> strings = new List<string>();
                strings.Add(category.Name);
                strings.Add(category.NameUA);
                strings.Add(category.NameEN);
                if (category.Name == null)
                {
                    category.Name = strings.Where(x => x != null).First();
                }
                if (category.NameUA == null)
                {
                    category.NameUA = strings.Where(x => x != null).First();
                }
                if (category.NameEN == null)
                {
                    category.NameEN = strings.Where(x => x != null).First();
                }
              
                await categoryService.AddOrUpdate(category);
                await categoryService.Save();
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория успешо добавлена.😏");
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }
        public static async void GetCategoryCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {

            IService<Category> categoryService = kernel.Get<IService<Category>>();
            try
            {
                string res = messageText.Split("get category").Last();
                int id = Convert.ToInt32(res);
                var item = await categoryService.Get(id);
                if (item != null)
                {
                        await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}");
                }
                else
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория с таким id не найдена!😢");
                }
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }
        public static async void GetAllCategoryCommand(IKernel kernel, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<Category> categoryService = kernel.Get<IService<Category>>();
            var list = await categoryService.GetAll();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameRU: {item.Name}\nnameUA: {item.NameUA}\nnameEN: {item.NameEN}");
                }
            }
            else
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Список категорий пуст!😢");
            }
        }
        public static async void RemoveCategoryCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<Category> categoryService = kernel.Get<IService<Category>>();
            IService<Product> productsService = kernel.Get<IService<Product>>();
            IService<Category> categorys = kernel.Get<IService<Category>>();
            try
            {
                string res = messageText.Split("remove category").Last();
                int id = Convert.ToInt32(res);
                var item = await categoryService.Get(id);
                if (item != null)
                {
                    var list = await productsService.GetAll();
                    foreach(var prod in list)
                    {
                        if(prod.CategoryId == item.Id)
                        {
                            await productsService.Delete(prod);
                        }
                    }
                    await productsService.Save();
                    await categoryService.Delete(item);
                    await categoryService.Save();
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория успешо удалена.😥");
                }
                else
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория с таким id не найдена!😢");
                }
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }
        public static async void EditCategoryCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<Category> categoryService = kernel.Get<IService<Category>>();
            try
            {
                string res = messageText.Split(" ")[2];
                int id = Convert.ToInt32(res);
                var item = await categoryService.Get(id);
                if (item != null)
                {

                    string? nameRes = messageText.Split(';').FirstOrDefault(x => x.Contains("nameRU"));
                    string? nameResUA = messageText.Split(';').FirstOrDefault(x => x.Contains("nameUA"));
                    string? nameResEN = messageText.Split(';').FirstOrDefault(x => x.Contains("nameEN"));
                    if (nameRes != null)
                    {
                        string nameRU = nameRes.Split("nameRU:").Last();
                        item.Name = nameRU;
                    }
                    if (nameResUA != null)
                    {
                        string nameUA = nameResUA.Split("nameUA:").Last();
                        item.NameUA = nameUA;
                    }
                    if (nameResEN != null)
                    {
                        string nameEN = nameResEN.Split("nameEN:").Last();
                        item.NameEN = nameEN;
                    }
                   
                    await categoryService.AddOrUpdate(item);
                    await categoryService.Save();
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория успешо отредактирована.😏");
                }
                else
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория с таким id не найдена!😢");
                }
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }

        public static async void GetAboutUsCommand(IKernel kernel, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<AboutUs> aboutUsService = kernel.Get<IService<AboutUs>>();
            var list = await aboutUsService.GetAll();
            var item = list.FirstOrDefault();
            if (item != null)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Id: {item.Id}\nnameCompany: {item.NameCompany}\ndescriptionRU: {item.Description}\ndescriptionUA: {item.DescriptionUA}\ndescriptionEN: {item.DescriptionEN}");
            }
            else
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Данные не найдены!😢");
            }
        }

        public static async void CreateAboutUsCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<AboutUs> aboutUsService = kernel.Get<IService<AboutUs>>();
            var aboutUs = new AboutUs();
            try
            {
                string? name = messageText.Split(';').FirstOrDefault(x => x.Contains("nameCompany"));
                string? desc = messageText.Split(';').FirstOrDefault(x => x.Contains("descriptionRU"));
                string? descUA = messageText.Split(';').FirstOrDefault(x => x.Contains("descriptionUA"));
                string? descEN = messageText.Split(';').FirstOrDefault(x => x.Contains("descriptionEN"));
                if (name != null)
                {
                    string res = name.Split("nameCompany:").Last();
                    aboutUs.NameCompany = res;
                }
                if (desc != null)
                {
                    string res = desc.Split("descriptionRU:").Last();
                    aboutUs.Description = res;
                }
                if (descUA != null)
                {
                    string resUA = descUA.Split("descriptionUA:").Last();
                    aboutUs.DescriptionUA = resUA;
                }
                if (descEN != null)
                {
                    string resEN = descEN.Split("descriptionEN:").Last();
                    aboutUs.DescriptionEN = resEN;
                }
                List<string> strings = new List<string>();
                strings.Add(aboutUs.Description);
                strings.Add(aboutUs.DescriptionUA);
                strings.Add(aboutUs.DescriptionEN);
                if (aboutUs.Description == null)
                {
                    aboutUs.Description = strings.Where(x => x != null).First();
                }
                if (aboutUs.DescriptionUA == null)
                {
                    aboutUs.DescriptionUA = strings.Where(x => x != null).First();
                }
                if (aboutUs.DescriptionEN == null)
                {
                    aboutUs.DescriptionEN = strings.Where(x => x != null).First();
                }
                var list = await aboutUsService.GetAll();
                var item = list.FirstOrDefault();
                if(item != null)
                {
                    await aboutUsService.Delete(item);
                }
                await aboutUsService.AddOrUpdate(aboutUs);
                await aboutUsService.Save();
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Раздел о нас успешо создан.😏");
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }
        public static async void RemoveAboutUsCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<AboutUs> aboutUsService = kernel.Get<IService<AboutUs>>();
            try
            {
                string res = messageText.Split("remove aboutUs").Last();
                var list = await aboutUsService.GetAll();
                var item = list.FirstOrDefault();
                if (item != null)
                {
                    await aboutUsService.Delete(item);
                    await aboutUsService.Save();
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория о нас успешо удалена.😥");
                }
                else
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория о нас не найдена!😢");
                }
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }

        public static async void EditAboutUsCommand(IKernel kernel, ChatId chatId, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            IService<AboutUs> aboutUsService = kernel.Get<IService<AboutUs>>();
            try
            {
                var list = await aboutUsService.GetAll();
                var item = list.FirstOrDefault();
                if (item != null)
                {

                    string? name = messageText.Split(';').FirstOrDefault(x => x.Contains("nameCompany"));
                    string? desc = messageText.Split(';').FirstOrDefault(x => x.Contains("descriptionRU"));
                    string? descUA = messageText.Split(';').FirstOrDefault(x => x.Contains("descriptionUA"));
                    string? descEN = messageText.Split(';').FirstOrDefault(x => x.Contains("descriptionEN"));
                    if (name != null)
                    {
                        string res = name.Split("nameCompany:").Last();
                        item.NameCompany = res;
                    }
                    if (desc != null)
                    {
                        string res = desc.Split("descriptionRU:").Last();
                        item.Description = res;
                    }
                    if (descUA != null)
                    {
                        string resUA = descUA.Split("descriptionUA:").Last();
                        item.DescriptionUA = resUA;
                    }
                    if (descEN != null)
                    {
                        string resEN = descEN.Split("descriptionEN:").Last();
                        item.DescriptionEN = resEN;
                    }

                    await aboutUsService.AddOrUpdate(item);
                    await aboutUsService.Save();
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория о нас успешо отредактирована.😏");
                }
                else
                {
                    await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Категория о нас не найдена!😢");
                }
            }
            catch (Exception ex)
            {
                await BotSendModel.SendTextAsync(botClient, chatId, cancellationToken, $"Что то пошло не так... Проверь правильность написания тегов😢");
            }
        }
    }

    public enum GetProductEnum
    {
        GetByName,
        GetByID,
        GetByPrice,
        GetByCategory
    }
}
