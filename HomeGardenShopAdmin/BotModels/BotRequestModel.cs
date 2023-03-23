using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopAdmin.BotModels
{
    public class BotRequestModel
    {
        public string[] LanguageButtons()
        {
            string[] button = { "uk-UA", "en-US", "ru-RU" };
            return button;
        }
        public List<string[]> StartButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { $"1.{Properties.Resource.String32}", $"2.{Properties.Resource.String33}", $"3.{Properties.Resource.String34}" };
            string[] button2 = { $"4.{Properties.Resource.String35}", $"5.{Properties.Resource.String36}", $"6.{Properties.Resource.String37}" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }
        public List<string[]> GetNewsButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { $"7.{Properties.Resource.String38}", $"8.{Properties.Resource.String39}", $"9.{Properties.Resource.String40}" };
            string[] button2 = { $"10.{Properties.Resource.String41}",$"11.{Properties.Resource.String42}",  $"12.{Properties.Resource.String43}" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }

        public List<string[]> GetProductsButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { $"13.{Properties.Resource.String38}", $"14.{Properties.Resource.String39}", $"15.{Properties.Resource.String40}" };
            string[] button2 = { $"16.{Properties.Resource.String41}", $"17.{Properties.Resource.String42}", $"18.{Properties.Resource.String43}" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }
        public List<string[]> GetProductsFilterButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { $"14.1.{Properties.Resource.String45}", $"14.2.{Properties.Resource.String46}", $"14.3.{Properties.Resource.String47}" };
            string[] button2 = { $"14.4.Id", $"14.5.{Properties.Resource.String43}" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }
        public List<string[]> GetCategorysButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { $"19.{Properties.Resource.String38}", $"20.{Properties.Resource.String39}", $"21.{Properties.Resource.String40}" };
            string[] button2 = { $"22.{Properties.Resource.String41}", $"23.{Properties.Resource.String42}", $"24.{Properties.Resource.String43}" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }
        public List<string[]> GetAboutUsButtons()
        {
            List<string[]> repyButtons = new List<string[]>();
            string[] button = { $"25.{Properties.Resource.String44}", $"26.{Properties.Resource.String40}", $"27.{Properties.Resource.String41}" };
            string[] button2 = {  $"28.{Properties.Resource.String42}", $"29.{Properties.Resource.String43}" };
            repyButtons.Add(button);
            repyButtons.Add(button2);
            return repyButtons;
        }
    }
}
