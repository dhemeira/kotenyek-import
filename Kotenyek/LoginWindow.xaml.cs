using Kotenyek.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json.Nodes;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Kotenyek
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            siteURLTB.Focus();
        }

        public class TokenResponse
        {
            [JsonPropertyName("token")]
            public string? Token { get; set; }
        }

        private async void LoginUser()
        {
            if (!string.IsNullOrWhiteSpace(siteURLTB.Text) && !string.IsNullOrWhiteSpace(siteUsernameTB.Text) && !string.IsNullOrWhiteSpace(sitePasswordTB.Password))
            {
                mainLoginStackPanel.IsEnabled = false;
                loginSpinner.Visibility = Visibility.Visible;
                var siteURL = "https://" + siteURLTB.Text.Trim().Split("/").ToList().Find(x => x.Contains(".hu") || x.Contains(".com"));
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, $"{siteURL}/wp-json/jwt-auth/v1/token")
                {
                    Content = new StringContent($"{{\"username\":\"{siteUsernameTB.Text.Trim()}\",\"password\":\"{sitePasswordTB.Password.Trim()}\"}}", Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseContent = await response.Content.ReadFromJsonAsync<TokenResponse>();
                    Properties.Settings.Default.SiteURL = siteURL;
                    Properties.Settings.Default.AuthToken = responseContent?.Token;
                    Properties.Settings.Default.Save();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hibás adatok!", "Hiba");
                }
            }
            else
            {
                MessageBox.Show("Hibás adatok!", "Hiba");
            }
        }

        private void LoginUser_Click(object sender, RoutedEventArgs e)
        {
            LoginUser();
        }

        private void sitePasswordTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginUser();
            }
        }

        private void siteURLTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                siteUsernameTB.Focus();
            }
        }

        private void siteUsernameTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sitePasswordTB.Focus();
            }
        }
    }
}
