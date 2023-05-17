// ExtractorViewModel.cs
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PowerPointExtractor
{
    public class ExtractorViewModel : INotifyPropertyChanged
    {
        private string pptxFile;
        private string outputDirectory;
        private string statusMessage;

        public string PptxFile
        {
            get => pptxFile;
            set
            {
                pptxFile = value;
                OnPropertyChanged();
            }
        }

        public string OutputDirectory
        {
            get => outputDirectory;
            set
            {
                outputDirectory = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> OutputFiles { get; } = new ObservableCollection<string>();

        public string StatusMessage
        {
            get => statusMessage;
            set
            {
                statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand ExtractTextCommand { get; }

        public ExtractorViewModel()
        {
            ExtractTextCommand = new ExtractTextCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
