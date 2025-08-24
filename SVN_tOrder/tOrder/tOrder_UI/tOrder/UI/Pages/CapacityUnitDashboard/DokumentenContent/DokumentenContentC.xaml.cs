using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.ApplicationModel;

namespace tOrder.UI
{
    public sealed partial class DokumentenContentC : UserControl
    {
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

        public string? SelectedPdfPath { get; set; }

        public DokumentenContentC()
        {
            InitializeComponent();
            CheckAssetsExistence();
            LoadPdfListAsync();
        }

        private void CheckAssetsExistence()
        {
            try
            {
                string debugPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Docs");
                Debug.WriteLine($"[📁] Debug assets path: {debugPath}");
                if (!Directory.Exists(debugPath))
                {
                    Debug.WriteLine("[❌] Složka 'Assets/Docs' neexistuje v debug adresáři.");
                }
                else
                {
                    Debug.WriteLine("[✅] Složka 'Assets/Docs' existuje v debug adresáři.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[⚠️] Chyba při kontrole složky v debug režimu: {ex.Message}");
            }
        }

        private async void LoadPdfListAsync()
        {
            try
            {
                List<string> pdfFiles;
#if DEBUG
                string path = Path.Combine(AppContext.BaseDirectory, "Assets", "Docs");
                pdfFiles = Directory.EnumerateFiles(path, "*.pdf")
                                    .Select(Path.GetFileName)
                                    .ToList();
#else
                var assetsFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                var docsFolder = await assetsFolder.GetFolderAsync("Docs");
                var files = await docsFolder.GetFilesAsync();
                pdfFiles = files.Where(f => f.FileType.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                                .Select(f => f.Name)
                                .ToList();
#endif
                PdfList.ItemsSource = pdfFiles;
            }
            catch (Exception ex)
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Chyba při načítání PDF",
                    Content = ex.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                _ = dialog.ShowAsync();
            }
        }

        private void PdfButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is string fileName)
            {
#if DEBUG
                string debugPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Docs", fileName);
                if (File.Exists(debugPath))
                {
                    SelectedPdfPath = debugPath;
                    PdfViewerControl.Source = new Uri($"file:///{debugPath.Replace("\\", "/")}");
                    Debug.WriteLine($"[📄 DEBUG] Otevřen soubor: {debugPath}");
                }
                else
                {
                    Debug.WriteLine($"[❌ DEBUG] Soubor neexistuje: {debugPath}");
                }
#else
                var uri = new Uri($"ms-appx-web:///Assets/Docs/{fileName}");
                SelectedPdfPath = uri.ToString();
                PdfViewerControl.Source = uri;
                Debug.WriteLine($"[📄 RELEASE] Otevřen soubor: {uri}");
#endif
            }
        }
    }
}
