// ExtractorViewModel.cs
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PowerPointExtractor
{
    public class ExtractorViewModel : ObservableRecipient
    {
        private string pptxFile;
        private string outputDirectory;
        private string statusMessage;

        public string PptxFile
        {
            get => pptxFile;
            set => SetProperty(ref pptxFile, value);
        }

        public string OutputDirectory
        {
            get => outputDirectory;
            set => SetProperty(ref outputDirectory, value);
        }

        public ObservableCollection<string> OutputFiles { get; } = new ObservableCollection<string>();

        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public IAsyncRelayCommand ExtractTextCommand { get; }

        private PowerPointTextExtractor textExtractor = new PowerPointTextExtractor();

        public ExtractorViewModel()
        {
            ExtractTextCommand = new AsyncRelayCommand(ExtractTextAsync);
        }

        private async Task ExtractTextAsync()
        {
            // Call ExtractSlideNotes and update OutputFiles and StatusMessage
            // You should call SetProperty to update PptxFile and OutputDirectory to notify the UI of changes
            throw new NotImplementedException();
        }
    }
}
