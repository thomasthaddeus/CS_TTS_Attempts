using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace PowerPointExtractor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "PowerPoint Files|*.pptx" };
            if (openFileDialog.ShowDialog() == true)
            {
                var viewModel = (ExtractorViewModel)DataContext;
                viewModel.PptxFile = openFileDialog.FileName;
            }
        }

        private void BrowseFolderButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                var viewModel = (ExtractorViewModel)DataContext;
                viewModel.OutputDirectory = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
