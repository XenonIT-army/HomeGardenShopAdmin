using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopAdmin.BotModels
{
    public class BotRequestModel
    {
        public List<string[]> StartButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { "1.Новости", "2.Продукты", "3.Категории" };
            string[] button2 = { "4.Заказы", "5.О нас", "6.Выйти" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }
        public List<string[]> GetNewsButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { "7.Показать все", "8.Найти", "9.Создать" };
            string[] button2 = { "10.Удалить","11.Изменить",  "12.Назад" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }

        public List<string[]> GetProductsButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { "13.Показать все", "14.Найти", "15.Создать" };
            string[] button2 = { "16.Удалить", "17.Изменить", "18.Назад" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }
        public List<string[]> GetProductsFilterButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { "14.1.Категория", "14.2.Название", "14.3.Цена" };
            string[] button2 = { "14.4.Id", "14.5.Назад" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }
        public List<string[]> GetCategorysButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { "19.Показать все", "20.Найти", "21.Создать" };
            string[] button2 = { "22.Удалить", "23.Изменить", "24.Назад" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }
        public List<string[]> GetAboutUsButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { "25.Показать", "26.Создать", "27.Удалить" };
            string[] button2 = {  "28.Изменить", "29.Назад" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }
    }
}
