using Microsoft.Win32;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using Kotenyek.View;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;

namespace Kotenyek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly List<Product> Products = new();
        List<CategoriesResponse> Categories = new();
        readonly List<string> checkedColors = new();
        readonly List<string> checkedCategories = new();
        readonly MainView mainView = new();
        static readonly HttpClient client = new();
        object Spinner { get; set; }

        public static bool IsUserNotLoggedIn => string.IsNullOrWhiteSpace(Properties.Settings.Default.AuthToken) || string.IsNullOrWhiteSpace(Properties.Settings.Default.SiteURL);

        public MainWindow()
        {
            InitializeComponent();          
            DataContext = mainView;
            mainView.ImageURL = "";
            this.Spinner = imageUploadBT.Content;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoginUser();

            if (!IsUserNotLoggedIn)
            {
                await LoadDataToList(mainView.Categories, $"{Properties.Settings.Default.SiteURL}/wp-json/wc/v3/products/categories");
                await LoadDataToList(mainView.AvailableColors, $"{Properties.Settings.Default.SiteURL}/wp-json/wc/v3/products/attributes/9/terms");
                await LoadDataToList(mainView.Colors, $"{Properties.Settings.Default.SiteURL}/wp-json/wc/v3/products/attributes/1/terms");
                this.Categories = mainView.Categories.ToList();

                mainDockPanel.IsEnabled = true;
                loginSpinner.Visibility = Visibility.Hidden;
                productName.Focus();
            }
        }

        private static async Task LoadDataToList<T>(ObservableCollection<T> list, string requestUri)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AuthToken);
            var response = await client.SendAsync(request);

            if (response == null)
            {
                Helpers.ShowMessage("Az oldal jelenleg nem elérhető, próbáld újra később!", "Hiba");
                return;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadFromJsonAsync<List<T>>();
                if (responseContent != null)
                {
                    foreach (var item in responseContent)
                    {
                        list.Add(item);
                    }
                }
            }
            else
            {
                Helpers.ShowMessage("Ismeretlen hiba!", "Hiba");
            }
        }

        private async Task ValidateToken()
        {
            var siteURL = Properties.Settings.Default.SiteURL;
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{siteURL}/wp-json/jwt-auth/v1/token/validate");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AuthToken);
            var response = await client.SendAsync(request);

            if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogOut();
            }
        }

        private async Task HandleResponse(HttpResponseMessage? response)
        {
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
        }

        private async Task LoginUser()
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
                    loginSpinner.Visibility = Visibility.Hidden;
                    productName.Focus();
                }
            }
            else
            {
                await ValidateToken();            
            }
        }
 
        private async void AddImage_Click(object sender, RoutedEventArgs e)
        {
            imageUploadBT.IsEnabled = false;
            addAndColorBT.IsEnabled = false;
            addAndNewBT.IsEnabled = false;
            logOutBT.IsEnabled = false;
            saveImportBT.IsEnabled = false;
            imageUploadBT.Content = this.Spinner;
            var fileDialog = new OpenFileDialog()
            {
                Filter = "Képek (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "KÖTÉNYEK kép feltöltés"
            };

            bool? result = fileDialog.ShowDialog();

            if (result == true)
            {
                await ValidateToken();

                using var request = new HttpRequestMessage(HttpMethod.Post, $"{Properties.Settings.Default.SiteURL}/wp-json/wp/v2/media");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AuthToken);
                request.Content = new StreamContent(fileDialog.OpenFile());
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                request.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileDialog.SafeFileName };
                var response = await client.SendAsync(request);
                await HandleResponse(response);
            }
            imageUploadBT.Content = "Képek feltöltése";
            imageUploadBT.IsEnabled = true;
            addAndColorBT.IsEnabled = true;
            addAndNewBT.IsEnabled = true;
            logOutBT.IsEnabled = true;
            saveImportBT.IsEnabled = true;
        }

        private string CheckForErrors(out int length, out int width, out int price)
        {
            StringBuilder errors = new();
            if (string.IsNullOrWhiteSpace(productName.Text))
            {
                errors.AppendLine("Nincs megadva a termék neve");
                
            }
            else
            {
                if (!productName.Text.ToUpper().Contains($" ({productUID.Text.ToUpper()})")) errors.AppendLine("A termék neve nem tartalmazza a cikkszámot");
            }
            if (!string.IsNullOrWhiteSpace(productLength.Text) && !int.TryParse(productLength.Text, out length)) errors.AppendLine("Nincs megadva a termék hossza");
            else length = -1;
            if (!string.IsNullOrWhiteSpace(productWidth.Text) && !int.TryParse(productWidth.Text, out width)) errors.AppendLine("Nincs megadva a termék szélessége");
            else width = -1;
            if (!int.TryParse(productPrice.Text, out price)) errors.AppendLine("Nincs megadva a termék ára");
            if (checkedCategories.Count == 0) errors.AppendLine("Nincs megadva a termék kategóriája");
            if (string.IsNullOrWhiteSpace(productUID.Text)) errors.AppendLine("Nincs megadva a termék cikkszáma");
            return errors.ToString();
        }

        private bool AddProductToList()
        {
            string errorList = CheckForErrors(out int length, out int width, out int price);
            if (string.IsNullOrEmpty(errorList))
            {
                Products.Add(new Product(productName.Text.ToUpper(),
                            productShortDescription.Text.Replace("\r", "").Replace("\n", "\\n"),
                            (productDescription.Text != "" ? $"Mosási útmutató:\\n<img src=\"{productDescription.Text}\" alt=\"Mosási útmutató\" width=\"166\" height=\"30\" class=\"alignnone size-full wp-image-1186\"/>" : ""),
                            length,
                            width,
                            price,
                            string.Join(", ", checkedCategories.ToArray()),
                            mainView.ImageURL,
                            productColor.Text,
                            string.Join(", ", checkedColors.ToArray()),
                            productUID.Text.ToUpper()));
                return true;
            }
            else
            {
                Helpers.ShowMessage(errorList, "Beviteli hiba");
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
                foreach (var category in mainView.Categories) {
                    category.Checked = false;
                }
                checkedCategories.Clear();
                mainView.ImageURL = "";
                productColor.Text = "";
                foreach (var color in mainView.AvailableColors)
                {
                    color.Checked = false;
                }
                checkedColors.Clear();
                productUID.Text = "";
            }         
        }

        private void SaveCSV_Click(object sender, RoutedEventArgs e)
        {
            mainDockPanel.IsEnabled = false;
            loginSpinner.Visibility = Visibility.Visible;
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
                    streamWriter.WriteLine($"{item.Name};{item.ShortDescription};{item.Description};{(item.Length >= 0 ? item.Length : "")};{(item.Width >= 0 ? item.Width : "")};{item.Price};{item.Category};{item.Images};Szín;{item.Color};1;Kapható színek;{item.AvailableColors};1;{item.UID}");
                }
                streamWriter.Close();
            }
            mainDockPanel.IsEnabled = true;
            loginSpinner.Visibility = Visibility.Hidden;
            productName.Focus();
        }

        private void AddNewColor_Click(object sender, RoutedEventArgs e)
        {
            if (AddProductToList())
            {
                productName.Text = "";
                mainView.ImageURL = "";
                productColor.Text = "";
                productUID.Text = "";
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if(Helpers.ShowMessage("Biztosan ki szeretnél jelentkezni?", "Kijelentkezés", true))
            {
                LogOut();
            }
        }

        private async void LogOut()
        {
            Properties.Settings.Default.AuthToken = "";
            Properties.Settings.Default.SiteURL = "";
            Properties.Settings.Default.Save();
            await LoginUser();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox)
            {
                string name = checkbox.Content.ToString() ?? "";
                if (productAvailableColors.IsAncestorOf(checkbox))
                {
                    checkedColors.Add(name);
                }
                if (productCategories.IsAncestorOf(checkbox))
                {
                    var parentId = this.Categories.Find(x => x.Name == name)?.Parent;
                    if (parentId != 0)
                    {
                        var parent = this.Categories.Find(x => x.Id == parentId);
                        parent.CheckedChildCount++;
                        parent.Checked = true;
                        parent.Enabled = false;
                        checkedCategories.Add($"{parent.Name} > {name}");
                    } else
                    {
                        checkedCategories.Add(name);
                    }               
                }    
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox)
            {
                string name = checkbox.Content.ToString() ?? "";
                if (productAvailableColors.IsAncestorOf(checkbox))
                {
                    checkedColors.Remove(name);
                }
                if (productCategories.IsAncestorOf(checkbox))
                {
                    var parentId = this.Categories.Find(x => x.Name == name)?.Parent;
                    if (parentId != 0)
                    {
                        var parent = this.Categories.Find(x => x.Id == parentId);
                        parent.CheckedChildCount--;
                        if (parent.CheckedChildCount <= 0)
                        {
                            parent.Checked = false;
                            parent.Enabled = true;
                            checkedCategories.Remove($"{parent.Name} > {name}");
                        }
                        else
                        {
                            checkedCategories.Remove(name);
                        }
                    }
                    else
                    {
                        checkedCategories.Remove(name);
                    }
                }
            }
        }
    }
}
