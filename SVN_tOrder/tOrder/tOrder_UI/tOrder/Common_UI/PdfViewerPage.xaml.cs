using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.IO;

namespace tOrder
{
    public sealed partial class PdfViewerPage : Page
    {
        public PdfViewerPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string fileName)
            {
#if DEBUG
                string path = Path.Combine(AppContext.BaseDirectory, "Assets", "Docs", fileName);
                if (File.Exists(path))
                {
                    WebViewPdf.Source = new Uri($"file:///{path.Replace("\\", "/")}");
                    Debug.WriteLine($"[📄 DEBUG] Načítám PDF z: {path}");
                }
                else
                {
                    Debug.WriteLine($"[❌ DEBUG] Soubor neexistuje: {path}");
                }
#else
                var uri = new Uri($"ms-appx-web:///Assets/Docs/{fileName}");
                WebViewPdf.Source = uri;
                Debug.WriteLine($"[📄 RELEASE] Načítám PDF z: {uri}");
#endif
            }

            base.OnNavigatedTo(e);
        }
    }
}
