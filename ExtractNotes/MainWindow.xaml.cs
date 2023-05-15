// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using GemBox.Presentation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Text;

namespace ExtractNotes
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                FileTypeFilter = { ".pptx" }
            };

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                txtFilePath.Text = file.Path;
            }
        }

        private async void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            string pptxFilePath = txtFilePath.Text;

            if (string.IsNullOrEmpty(pptxFilePath))
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please select a PPTX file.",
                    CloseButtonText = "OK"
                };

                await errorDialog.ShowAsync();
                return;
            }

            try
            {
                ComponentInfo.SetLicense("FREE-LIMITED-KEY"); // You may need to set your GemBox.Presentation license key here.
                var presentation = PresentationDocument.Load(pptxFilePath);
                string outputDirectory = Path.GetDirectoryName(pptxFilePath);

                var extractedFiles = ExtractNotes(presentation, pptxFilePath, outputDirectory);
                lstFiles.Items.Clear();
                lstFiles.Items.Add(extractedFiles);

                ContentDialog successDialog = new ContentDialog
                {
                    Title = "Success",
                    Content = "Notes extracted successfully.",
                    CloseButtonText = "OK"
                };

                await successDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Error: {ex.Message}",
                    CloseButtonText = "OK"
                };

                await errorDialog.ShowAsync();
            }
        }

        private static List<string> ExtractNotes(PresentationDocument presentation, string presentationFilePath, string outputDirectory)
        {
            var extractedFiles = new List<string>();
            int slideNumber = 1;

            foreach (Slide slide in presentation.Slides)
            {
                var notesSlide = slide.Notes;
                if (notesSlide != null)
                {
                    var notesText = new StringBuilder();
                    foreach (TextParagraph paragraph in notesSlide.TextContent.Paragraphs)
                    {
                        foreach (var element in paragraph.Elements)
                        {
                            if (element is TextRun textRun)
                            {
                                notesText.AppendLine(textRun.Text);
                            }
                        }
                    }

                    if (notesText.Length > 0)
                    {
                        string fileName = $"{Path.GetFileNameWithoutExtension(presentationFilePath)}_slide_{slideNumber}.txt";
                        string filePath = Path.Combine(outputDirectory, fileName);

                        File.WriteAllText(filePath, notesText.ToString());
                        extractedFiles.Add(filePath);
                    }
                }

                slideNumber++;
            }

            return extractedFiles;
        }
    }
}