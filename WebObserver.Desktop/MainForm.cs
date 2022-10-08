using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Handler;
using CefSharp.WinForms;

namespace WebObserver.Desktop
{
    public partial class MainForm : Form
    {
        // setup user agent here. leave empty for default
        private const string USER_AGENT = "Mozilla/5.0 (Linux; Android 6.0.1; Nexus 5X Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.5195.125 Mobile Safari/537.36 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)\r\n";

        private readonly string _cachePath;

        private readonly string _proxyUrl;
        private readonly int _proxyPort;

        public MainForm(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                _proxyUrl = args[0].Split(':')[0];
                _proxyPort = int.Parse(args[0].Split(':')[1]);
            }

            _cachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";

            InitializeComponent();
            InitializeChromium();

        }
        public ChromiumWebBrowser chromeBrowser;

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void InitializeChromium()
        {

            CefSettings settings = new CefSettings();

            if (!string.IsNullOrWhiteSpace(_cachePath))
                settings.CachePath = _cachePath;

            if (!string.IsNullOrWhiteSpace(USER_AGENT))
                settings.UserAgent = USER_AGENT;

            settings.DisableGpuAcceleration();

            RequestContextHandler requestContextHander = new RequestContextHandler();

            if (!string.IsNullOrEmpty(_proxyUrl) && !string.IsNullOrWhiteSpace(_proxyUrl))
            {
                requestContextHander = new RequestContextHandler()
                    .SetProxyOnContextInitialized(_proxyUrl, _proxyPort);
            }

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
