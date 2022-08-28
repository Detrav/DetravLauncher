using Microsoft.Web.WebView2.WinForms;
using System.Diagnostics;
using System.Text.Json;

namespace Detrav.Launcher.Client
{
    public partial class MainForm : Form
    {
        private string appUrl;
        private WebView2 webView;

        public MainForm()
        {
            InitializeComponent();

            appUrl = Environment.GetCommandLineArgs()[2];
            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            webView.Parent = this;
            Shown += MainForm_Shown;
            webView.NavigationStarting += WebView_NavigationStarting;
            webView.NavigationCompleted += WebView_NavigationCompleted;
            Load += MainForm_LoadAsync;
        }

        private async void MainForm_LoadAsync(object? sender, EventArgs e)
        {
            await webView.EnsureCoreWebView2Async();
            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        }

        private void WebView_CoreWebView2InitializationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            webView.CoreWebView2.OpenDevToolsWindow();
        }

        private void WebView_NavigationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.WebErrorStatus != Microsoft.Web.WebView2.Core.CoreWebView2WebErrorStatus.OperationCanceled)
            {
                if (!e.IsSuccess)
                {
                    webView.CoreWebView2.NavigateToString(File.ReadAllText("Pages/NotFound.html").Replace("__main_uri__", appUrl));
                }
            }
        }

        private void WebView_NavigationStarting(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            if (e.Uri.StartsWith("data:text/html;"))
            {

            }
            else if (e.Uri.StartsWith(appUrl))
            {

            }
            else
            {
                DialogResult result;
                try
                {
                    result = MessageBox.Show(String.Format("Do you want to go external link `{0}`?", new Uri(e.Uri).Host), "Go Out!", MessageBoxButtons.YesNo);
                }
                catch
                {
                    result = MessageBox.Show(String.Format("Do you want to go external link ?", new Uri(e.Uri).Host), "Go Out!", MessageBoxButtons.YesNo);
                }

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = e.Uri,
                        UseShellExecute = true
                    });
                }
                e.Cancel = true;
            }
        }

        private void MainForm_Shown(object? sender, EventArgs e)
        {
            webView.Source = new Uri(appUrl);
        }
    }
}