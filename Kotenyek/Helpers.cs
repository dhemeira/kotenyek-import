using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace Kotenyek
{
    class Helpers
    {
        public static bool ShowMessage(string message, string title)
        {
            //plug the custom dialog here
            MessageBox.Show(message, title);
            return true;
        }
        public static bool ShowMessage(string message, string title, bool isYesNoDialog)
        {
            if (isYesNoDialog)
            {
                //plug the custom dialog here
                var result = MessageBox.Show(message, title, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes) return true;
                else return false;
            }
            return ShowMessage(message, title);
        }
    }

    public class Product
    {
        public string? Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Price { get; set; }
        public string? Category { get; set; }
        public string? Images { get; set; }
        public string? Color { get; set; }
        public string? AvailableColors { get; set; }
        public string? UID { get; set; }
    }

    public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }
}
