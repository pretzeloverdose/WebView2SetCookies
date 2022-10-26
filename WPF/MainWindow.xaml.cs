using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System;

namespace WPFSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Auth0Client client;
        readonly string[] _connectionNames = new string[]
        {
            "Username-Password-Authentication",
            "google-oauth2",
            "twitter",
            "facebook",
            "github",
            "windowslive"
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            string domain = ConfigurationManager.AppSettings["Auth0:Domain"];
            string clientId = ConfigurationManager.AppSettings["Auth0:ClientId"];

            client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "dev-j2m8shog10tezb18.us.auth0.com",
                ClientId = "cXhmwnyx986yInpJAR4O6CF8U6Tl7oGO"
            });

            DisplayResult(await client.LoginAsync());
        }

        private void DisplayResult(LoginResult loginResult)
        {
            // Display error
            if (loginResult.IsError)
            {
                return;
            }

            try
            {
                string uri = @"http://localhost:3000/secret";
                var cookie = webView.CoreWebView2.CookieManager.CreateCookie("auth._token.auth0", "Bearer%20"+loginResult.AccessToken, "localhost:3000", "/");
                cookie.IsHttpOnly = true;
                webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
                webView.CoreWebView2.Navigate("http://localhost:3000");

            }
            catch (Exception oEx)
            {
                // handle exception
                Console.WriteLine(oEx);
            }


            
            
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            BrowserResultType browserResult = await client.LogoutAsync();

            if (browserResult != BrowserResultType.Success)
            {
                return;
            }

            logoutButton.Visibility = Visibility.Collapsed;
            loginButton.Visibility = Visibility.Visible;
        }
    }
}