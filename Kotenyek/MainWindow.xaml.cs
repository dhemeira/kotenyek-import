using Microsoft.Win32;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using Kotenyek.View;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Kotenyek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly List<Product> Products = new();
        readonly MainView mainView = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = mainView;
            mainView.ImageURL = "";
            this.Spinner = imageUploadBT.Content;
            imageUploadBT.Content = "Képek feltöltése";
        }

        public object Spinner { get; private set; }
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

        private async void AddImage_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = "Képek (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "KÖTÉNYEK kép feltöltés"
            };

            bool? result = fileDialog.ShowDialog();

            if (result == true)
            {
                imageUploadBT.IsEnabled = false;
                imageUploadBT.Content = this.Spinner;

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, $"{Properties.Settings.Default.SiteURL}/wp-json/wp/v2/media");
                //token generálás után token mentése:
                //Properties.Settings.Default.AuthToken = authtoken ide;  
                //Properties.Settings.Default.Save();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AuthToken);

                request.Content = new StreamContent(fileDialog.OpenFile());
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                request.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileDialog.SafeFileName };
                var response = await client.SendAsync(request);

                if (response != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var imageUrl = responseContent.Split(',')[3].Split('"')[5];
                    if (mainView.ImageURL == "")
                    {
                        mainView.ImageURL = System.Text.RegularExpressions.Regex.Unescape(imageUrl);
                    }
                    else
                    {
                        mainView.ImageURL += "," + System.Text.RegularExpressions.Regex.Unescape(imageUrl);
                    }
                }

                imageUploadBT.Content = "Képek feltöltése";             
                imageUploadBT.IsEnabled = true;
            }
        }

        private void AddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder hibak = new();
            //munka>c#>wpf gyakorlo>asztali tenisz gui-ban további minta
            if (string.IsNullOrWhiteSpace(productName.Text)) hibak.AppendLine("Nincs megadva a termék neve");
            //string.IsNullOrWhiteSpace(productShortDescription.Text) ||
            //    string.IsNullOrWhiteSpace(productDescription.Text) ||
            //    string.IsNullOrWhiteSpace(productLength.Text) ||
            //    string.IsNullOrWhiteSpace(productWidth.Text) ||
            //    string.IsNullOrWhiteSpace(productPrice.Text) ||
            //    //kategória checkbox pipák kivétel
            //    string.IsNullOrWhiteSpace(mainView.ImageURL) ||
            //    //szín radiobutton jelölés kivétel
            //    //kapható színek checkbox pipák kivétele
            //    string.IsNullOrWhiteSpace(productUID.Text)
            string hiba = hibak.ToString();
            if (string.IsNullOrEmpty(hiba))
            {
                Products.Add(new Product()
                {
                    Name = productName.Text,
                    ShortDescription = productShortDescription.Text,
                    Description = productDescription.Text,
                    Length = int.Parse(productLength.Text),
                    Width = int.Parse(productWidth.Text),
                    Price = int.Parse(productPrice.Text),
                    Category = "Kötény", //Kicserélni checkbox valuera
                    Images = mainView.ImageURL,
                    Color = "Fekete", //Kicserélni radiobutton valuera
                    AvailableColors = "Fekete, Fehér", //Kicserélni checkbox valuera
                    UID = productUID.Text
                });
                productName.Text = "";
                productShortDescription.Text = "";
                productDescription.Text = "";
                productLength.Text = "";
                productWidth.Text = "";
                productPrice.Text = "";
                //kategória checkbox pipák kivétel
                mainView.ImageURL = "";
                //szín radiobutton jelölés kivétel
                //kapható színek checkbox pipák kivétele
                productUID.Text = "";
            }
            else
            {
                MessageBox.Show(hiba, "Beviteli hiba");
            }
        }

        private void SaveCSV_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "UTF-8 CSV fájl (*.csv)|*.csv",
                Title = "KÖTÉNYEK import fájl mentés"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                StreamWriter streamWriter = new(saveFileDialog.FileName, false, Encoding.UTF8);
                streamWriter.WriteLine("Név;Rövid leírás;Leírás;Hosszúság (cm);Szélesség (cm);Normál ár;Kategória;Képek;Attribute 1 name;Tulajdonság (1) értéke(i);Attribute 1 global;Attribute 2 name;Tulajdonság (2) értéke(i);Attribute 2 global;Cikkszám");
                foreach (var item in Products)
                {
                    streamWriter.WriteLine($"{item.Name};{item.ShortDescription};{item.Description};{item.Length};{item.Width};{item.Price};{item.Category};{item.Images};Szín;{item.Color};1;Kapható színek;{item.AvailableColors};1;{item.UID}");
                }
                streamWriter.Close();
            }
        }

        private void AddNewColor_Click(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}
