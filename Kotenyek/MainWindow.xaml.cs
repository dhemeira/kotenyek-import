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
using System.Diagnostics;

namespace Kotenyek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly List<Product> Products = new();
        List<CategoriesResponse> Categories = new();
        List<ColorsResponse> Colors = new();
        readonly List<string> checkedColors = new();
        readonly List<string> checkedCategories = new();
        readonly MainView mainView = new();
        static readonly HttpClient client = new();
        object Spinner { get; set; }
        string images;

        public static bool IsUserNotLoggedIn => string.IsNullOrWhiteSpace(Properties.Settings.Default.AuthToken) || string.IsNullOrWhiteSpace(Properties.Settings.Default.SiteURL);

        public MainWindow()
        {
            InitializeComponent();          
            DataContext = mainView;
            mainView.ImageURL = "";
            images = "";
            this.Spinner = imageUploadBT.Content;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.ResizeMode = ResizeMode.CanMinimize;
            await Task.Delay(1);
            await LoginUser();
            this.MinWidth = this.Width;
            this.MinHeight = this.Height;
            this.ResizeMode = ResizeMode.CanResize;
        }

        private async Task LoadAndEnable()
        {
            mainView.Categories.Clear();
            mainView.Colors.Clear();
            mainView.AvailableColors.Clear();
            await LoadDataToList(mainView.Categories, $"{Properties.Settings.Default.SiteURL}/wp-json/wc/v3/products/categories");
            await LoadDataToList(mainView.AvailableColors, $"{Properties.Settings.Default.SiteURL}/wp-json/wc/v3/products/attributes/9/terms");
            await LoadDataToList(mainView.Colors, $"{Properties.Settings.Default.SiteURL}/wp-json/wc/v3/products/attributes/1/terms");
            this.Categories = mainView.Categories.ToList();
            this.Colors = mainView.Colors.ToList();

            using (StreamReader r = new("Assets/washingInstructions.txt", Encoding.UTF8))
            {
                while (!r.EndOfStream)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    string[] s = r.ReadLine().Split(';');
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    mainView.WashingInstruction.Add(new Washes() { Name = s[0], Url = s[1] });
                }
            }

            mainView.WashingInstruction.Add(new Washes() { Name = "teszt", Url = "teszturl" });
            mainView.WashingInstruction.Add(new Washes() { Name = "teszt2", Url = "teszt2url" });

            mainDockPanel.IsEnabled = true;
            loginSpinner.Visibility = Visibility.Hidden;
            productName.Focus();
        }

        private async Task LoadDataToList<T>(ObservableCollection<T> list, string requestUri)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AuthToken);
            var response = await client.SendAsync(request);

            if (response == null)
            {
                Helpers.ShowMessage(this, "Az oldal jelenleg nem elérhető, próbáld újra később!", "Hiba");
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
                Helpers.ShowMessage(this, "Ismeretlen hiba!", "Hiba");
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
                var imageUrl = System.Text.RegularExpressions.Regex.Unescape(responseContent.Split(',')[3].Split('"')[5]);
                if (mainView.ImageURL == "")
                {
                    images = imageUrl;
                    mainView.ImageURL = Path.GetFileName(imageUrl);

                }
                else
                {
                    images += "," + imageUrl;
                    mainView.ImageURL += ", " + Path.GetFileName(imageUrl);
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
                else await LoadAndEnable();
            }
            else
            {
                await ValidateToken();
                await LoadAndEnable();
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
                Title = "KÖTÉNYEK kép feltöltés",
                Multiselect = true,
            };

            bool? result = fileDialog.ShowDialog();

            if (result == true)
            {
                await ValidateToken();
                foreach (var filename in fileDialog.FileNames)
                {
                    using var request = new HttpRequestMessage(HttpMethod.Post, $"{Properties.Settings.Default.SiteURL}/wp-json/wp/v2/media");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AuthToken);
                    FileStream FS = new(filename, FileMode.Open, FileAccess.Read);
                    request.Content = new StreamContent(FS);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    var teszt = Path.GetFileName(filename);
                    request.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = Path.GetFileName(filename)};
                    var response = await client.SendAsync(request);
                    await HandleResponse(response);
                }              
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
            if (string.IsNullOrWhiteSpace(productName.Text)) errors.AppendLine("Nincs megadva a termék neve\n");
            else if (!productName.Text.ToUpper().Contains($" ({productUID.Text.ToUpper()})")) errors.AppendLine("A termék neve nem tartalmazza a cikkszámot\n");

            if (!string.IsNullOrWhiteSpace(productLength.Text) && !int.TryParse(productLength.Text, out length)) errors.AppendLine("Nincs megadva a termék hossza\n");
            else length = -1;

            if (!string.IsNullOrWhiteSpace(productWidth.Text) && !int.TryParse(productWidth.Text, out width)) errors.AppendLine("Nincs megadva a termék szélessége\n");
            else width = -1;

            if (!int.TryParse(productPrice.Text, out price)) errors.AppendLine("Nincs megadva a termék ára\n");

            if (checkedCategories.Count == 0) errors.AppendLine("Nincs megadva a termék kategóriája\n");

            if (checkedColors.Count > 0 && string.IsNullOrWhiteSpace(productColor.Text)) errors.AppendLine("Kötelező színt megadni ha ki van választva kapható szín\n");
            else if (checkedColors.Count > 0 && !string.IsNullOrWhiteSpace(productColor.Text) && !checkedColors.Contains(productColor.Text))
                errors.AppendLine("A kiválasztott szín nem szerepel a kapható színek között\n");

            if (string.IsNullOrWhiteSpace(productUID.Text)) errors.AppendLine("Nincs megadva a termék cikkszáma\n");
            else if (checkedColors.Count > 0 && !productUID.Text.ToUpper().EndsWith((this.Colors.Find(x => x.Name == productColor.Text)?.Slug?.ToUpper()) ?? ""))
                errors.AppendLine("A termék cikkszámának tartalmaznia kell a színkódot\n");

            return errors.ToString().Trim();
        }

        private bool AddProductToList()
        {
            string errorList = CheckForErrors(out int length, out int width, out int price);
            if (string.IsNullOrEmpty(errorList))
            {
                var washingInstruction = mainView.WashingInstruction.ToList().Find(x => x.Name == productDescription.Text) ?? new Washes() { Name = "", Url = "" };
                Products.Add(new Product(productName.Text.ToUpper(),
                            productShortDescription.Text.Replace("\r", "").Replace("\n", "\\n"),
                            (productDescription.Text != "" ? $"Mosási útmutató:\\n<img src=\"{washingInstruction.Url}\" title=\"{washingInstruction.Name}\" alt=\"Mosási útmutató\" width=\"166\" height=\"30\" class=\"alignnone size-full wp-image-1186\"/>" : ""),
                            length,
                            width,
                            price,
                            string.Join(", ", checkedCategories),
                            images,
                            productColor.Text,
                            string.Join(", ", checkedColors),
                            productUID.Text.ToUpper()));
                return true;
            }
            else
            {
                Helpers.ShowMessage(this, errorList, "Beviteli hiba");
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
                images = "";
                productColor.Text = "";
                foreach (var color in mainView.AvailableColors)
                {
                    color.Checked = false;
                }
                checkedColors.Clear();
                productUID.Text = "";
            }
            productName.Focus();
        }

        private void SaveCSV_Click(object sender, RoutedEventArgs e)
        {
            if (Products.Count <= 0) {
                Helpers.ShowMessage(this, "Még nem adtál hozzá terméket a listához", "Import fájl mentése", false);
                return;
            }
            if (!Helpers.ShowMessage(this, $"Biztosan szeretnéd menteni az import fájlt?\nA következő termékek kerülnek hozzáadásra:\n\n{string.Join("\n", Products.Select(x => x.Name))}", "Import fájl mentése", true))
                return;
            mainDockPanel.IsEnabled = false;
            loginSpinner.Visibility = Visibility.Visible;
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "UTF-8 CSV fájl (*.csv)|*.csv",
                Title = "KÖTÉNYEK import fájl mentés"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    StreamWriter streamWriter = new(saveFileDialog.FileName, false, Encoding.UTF8);
                    streamWriter.WriteLine("Név;Rövid leírás;Leírás;Hosszúság (cm);Szélesség (cm);Normál ár;Kategória;Képek;Attribute 1 name;Tulajdonság (1) értéke(i);Attribute 1 global;Attribute 2 name;Tulajdonság (2) értéke(i);Attribute 2 global;Cikkszám");
                    foreach (var item in Products)
                    {
                        streamWriter.WriteLine($"{item.Name};{item.ShortDescription};{item.Description};{(item.Length >= 0 ? item.Length : "")};{(item.Width >= 0 ? item.Width : "")};{item.Price};{item.Category};{item.Images};Szín;{item.Color};1;Kapható színek;{item.AvailableColors};1;{item.UID}");
                    }
                    streamWriter.Close();
                    Products.Clear();
                    productName.Text = "";
                    productShortDescription.Text = "";
                    productDescription.Text = "";
                    productLength.Text = "";
                    productWidth.Text = "";
                    productPrice.Text = "";
                    foreach (var category in mainView.Categories)
                    {
                        category.Checked = false;
                    }
                    checkedCategories.Clear();
                    mainView.ImageURL = "";
                    images = "";
                    productColor.Text = "";
                    foreach (var color in mainView.AvailableColors)
                    {
                        color.Checked = false;
                    }
                    checkedColors.Clear();
                    productUID.Text = "";
                }
                catch (System.Exception)
                {
                    Helpers.ShowMessage(this,"A fájl már használatban van! Zárd be mielőtt felülírnád!", "Import fájl mentése");
                }                                          
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
                images = "";
                productColor.Text = "";
                productUID.Text = "";
            }
            productName.Focus();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if(Helpers.ShowMessage(this, "Biztosan ki szeretnél jelentkezni?", "Kijelentkezés", true))
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
                        if (parent != null)
                        {
                            parent.CheckedChildCount++;
                            parent.Checked = true;
                            parent.Enabled = false;
                            checkedCategories.Add($"{parent.Name} > {name}");
                            return;
                        }
                    }
                    checkedCategories.Add(name);
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
                        if (parent != null)
                        {
                            parent.CheckedChildCount--;
                            if (parent.CheckedChildCount <= 0)
                            {
                                parent.Checked = false;
                                parent.Enabled = true;
                                checkedCategories.Remove($"{parent.Name} > {name}");
                                return;
                            }
                        }
                    }
                    checkedCategories.Remove(name);
                }
            }
        }

        private void AddSymbol_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if(button.Name == "AddSquaredSymbol")
                productShortDescription.Text += "<sup>2</sup> ";

                if (button.Name == "AddCubedSymbol")
                    productShortDescription.Text += "<sup>3</sup> ";
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var sInfo = new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true,
            };
            Process.Start(sInfo);
            e.Handled = true;
        }
    }
}
