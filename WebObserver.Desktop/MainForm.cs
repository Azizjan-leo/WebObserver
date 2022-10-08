using System;
using System.Windows;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Handler;
using CefSharp.WinForms;

namespace WebObserver.Desktop
{
    public partial class MainForm : Form
    {
        string USER_AGENT = "";
        string CACHE_PATH = "";
        string PROXY_URL = "";
        int PROXY_PORT; 

        public MainForm(string[] args)
        {
            PROXY_URL = args[0].Split(':')[0];
            PROXY_PORT = int.Parse(args[0].Split(':')[1]);
            
            InitializeComponent();
            InitializeChromium();

        }
        public ChromiumWebBrowser chromeBrowser;

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void InitializeChromium()
        {
            if (string.IsNullOrEmpty(USER_AGENT) || string.IsNullOrWhiteSpace(USER_AGENT))
                USER_AGENT = "Mozilla/5.0 (Linux; Android 6.0.1; Nexus 5X Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.5195.125 Mobile Safari/537.36 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)\r\n";

            if (string.IsNullOrEmpty(CACHE_PATH) || string.IsNullOrWhiteSpace(CACHE_PATH))
                CACHE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";

            CefSettings settings = new CefSettings
            {
                CachePath = CACHE_PATH,
                UserAgent = USER_AGENT
            };

            var requestContextHander = new RequestContextHandler()
                .SetProxyOnContextInitialized(PROXY_URL, PROXY_PORT);

            var requestContext = new RequestContext(requestContextHander);

            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("https://whois.domaintools.com/whois/msn.com", requestContext);
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }
    }
}
