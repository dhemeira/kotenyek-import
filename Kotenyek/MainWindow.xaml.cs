using Microsoft.Win32;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using Kotenyek.View;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Kotenyek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly List<Product> Products = new();
        readonly MainView mainView = new();

        public static bool IsUserNotLoggedIn => string.IsNullOrWhiteSpace(Properties.Settings.Default.AuthToken) || string.IsNullOrWhiteSpace(Properties.Settings.Default.SiteURL);
        public MainWindow()
        {
            InitializeComponent();          
            DataContext = mainView;
            mainView.ImageURL = "";
            this.Spinner = imageUploadBT.Content;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoginUser();
        }

        private async Task ValidateToken()
        {
            var siteURL = Properties.Settings.Default.SiteURL;
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{siteURL}/wp-json/jwt-auth/v1/token/validate");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AuthToken);
            var response = await client.SendAsync(request);

            if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogOut();
            }
        }

        private async void LoginUser()
        {
            imageUploadBT.Content = "Képek feltöltése";
            imageUploadBT.IsEnabled = true;
            mainDockPanel.IsEnabled = false;
            loginSpinner.Visibility = Visibility.Visible;
            if (IsUserNotLoggedIn)
            {
                LoginWindow loginWindow = new()
                {
                    Owner = this
                };
                loginWindow.ShowDialog();
                if (IsUserNotLoggedIn) Application.Current.Shutdown();
                else
                {
                    mainDockPanel.IsEnabled = true;
                    loginSpinner.Visibility = Visibility.Hidden;
                    productName.Focus();
                }
            }
            else
            {
                await ValidateToken();
                mainDockPanel.IsEnabled = true;
                loginSpinner.Visibility = Visibility.Hidden;
                productName.Focus();
            }
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

                await ValidateToken();

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, $"{Properties.Settings.Default.SiteURL}/wp-json/wp/v2/media");
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

        private bool AddProductToList()
        {
            StringBuilder hibak = new();
            if (string.IsNullOrWhiteSpace(productName.Text))
            {
                hibak.AppendLine("Nincs megadva a termék neve");
                if (!productName.Text.ToUpper().Contains($"({productUID.Text.ToUpper()})")) hibak.AppendLine("A termék neve nem tartalmazza a cikkszámot");
            }
            if (string.IsNullOrWhiteSpace(productShortDescription.Text)) hibak.AppendLine("Nincs megadva a termék leírása");
            if (string.IsNullOrWhiteSpace(productDescription.Text)) hibak.AppendLine("Nincs megadva a termék mosási útmutatója");
            if (!int.TryParse(productLength.Text, out int length)) hibak.AppendLine("Nincs megadva a termék hossza");
            if (!int.TryParse(productWidth.Text,out int width)) hibak.AppendLine("Nincs megadva a termék szélessége");
            if (!int.TryParse(productPrice.Text, out int price)) hibak.AppendLine("Nincs megadva a termék ára");
            //kategória checkbox
            if (string.IsNullOrWhiteSpace(mainView.ImageURL)) hibak.AppendLine("Nincsenek megadva a termék képei");
            //szín radiobutton
            //kapható színek checkbox
            if (string.IsNullOrWhiteSpace(productUID.Text)) hibak.AppendLine("Nincs megadva a termék cikkszáma");

            string hiba = hibak.ToString();
            if (string.IsNullOrEmpty(hiba))
            {
                
                Products.Add(new Product()
                {
                    Name = productName.Text.ToUpper(),
                    ShortDescription = productShortDescription.Text.Replace("\r", ""),
                    Description = $"Mosási útmutató:\n<img src=\"{productDescription.Text}\" alt=\"Mosási útmutató\" width=\"166\" height=\"30\" class=\"alignnone size-full wp-image-1186\"/>",
                    Length = length,
                    Width = width,
                    Price = price,
                    Category = "Kötény", //Kicserélni checkbox vagy dropdown valuera
                    Images = mainView.ImageURL,
                    Color = "Fekete", //Kicserélni dropdown valuera
                    AvailableColors = "Fekete, Fehér", //Kicserélni checkbox valuera
                    UID = productUID.Text.ToUpper()
                });
                return true;
            }
            else
            {
                MessageBox.Show(hiba, "Beviteli hiba");
                return false;
            }
        }

        private void AddNewProduct_Click(object sender, RoutedEventArgs e)
        {      
            if (AddProductToList())
            {
                productName.Text = "";
                productShortDescription.Text = "";
                productDescription.Text = "";
                productLength.Text = "";
                productWidth.Text = "";
                productPrice.Text = "";
                //kategória checkbox pipák vagy dropdown kivétel
                mainView.ImageURL = "";
                //szín dropdown kivétel
                //kapható színek checkbox pipák kivétele
                productUID.Text = "";
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
            if (AddProductToList())
            {
                productName.Text = "";
                mainView.ImageURL = "";
                //szín radiobutton jelölés kivétel
                productUID.Text = "";
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Biztosan ki szeretnél jelentkezni?", "Kijelentkezés", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                LogOut();
            }
        }

        private void LogOut()
        {
            Properties.Settings.Default.AuthToken = "";
            Properties.Settings.Default.SiteURL = "";
            Properties.Settings.Default.Save();
            LoginUser();
        }
    }
}
