using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using System.IO;

namespace PDFuse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Collection = new ObservableCollection<string>();
        public MainWindow()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            FileList.DataContext = Collection;
        }

        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { 
                DefaultExt = "*.pdf",
                Filter = "Portable Document Format |*.pdf",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                foreach(string file in dialog.FileNames)
                {
                    Collection.Add(file);
                }   
                
            }
        }

        private void ExportFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog() {
                FileName = "result.pdf",
                DefaultExt = ".pdf",
                Filter = "Portable Document Format |*.pdf",
                Title = "Dump a phat load"
            };

            if (dialog.ShowDialog() == false)
                return;

            string[] filenames = Collection.ToArray<string>();

            PdfDocument result = new PdfDocument();

            foreach(string file in filenames)
            {
                PdfDocument pdf = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                foreach (PdfPage page in pdf.Pages)
                {
                    result.AddPage(page);
                }
            }

            result.Save(dialog.FileName);

            using Process fileopener = new Process();

            fileopener.StartInfo.FileName = "explorer";
            fileopener.StartInfo.Arguments = "\"" + dialog.FileName;
            fileopener.Start();
        }
    }
}
