using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using Prism.Services.Dialogs;
using Azure.Security.KeyVault.Secrets;
using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using System.Windows;

namespace WpfApp1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private List<Voice> _voices;
        private Voice _selectedVoice;
        private string _selectedDirectory;

        public ICommand OpenWebsiteCommand { get; }
        public ICommand DisplayDateCommand { get; }
        public ICommand CalculateSumCommand { get; }
        public ICommand ShowMessageCommand { get; }
        public ICommand LogTimeCommand { get; }
        public ICommand ChangeColorCommand { get; }
        public ICommand LoadTextCommand { get; }
        public ICommand SaveTextCommand { get; }
        public ICommand PlaySoundCommand { get; }
        public ICommand CloseAppCommand { get; }

        public ICommand SelectDirectoryCommand { get; }
        public ICommand RunExecutableCommand { get; }
        public ICommand SendFilesCommand { get; }
        public ICommand AddAudioCommand { get; }

        public MainViewModel()
        {
            LoadVoices();
            SelectDirectoryCommand = new RelayCommand(SelectDirectory);
            RunExecutableCommand = new AsyncCommand(RunExecutableAsync);
            SendFilesCommand = new AsyncCommand(() => TextToSpeechAsync("text", "voice"));
            AddAudioCommand = new AsyncCommand(() => AddAudioToSlideAsync("filePath", 1));
            OpenWebsiteCommand = new RelayCommand(OpenWebsiteCommand);
            DisplayDateCommand = new RelayCommand(DisplayDate);
            CalculateSumCommand = new RelayCommand<int, int>(CalculateSum);
            ShowMessageCommand = new RelayCommand<string>(ShowMessage);
            LogTimeCommand = new AsyncCommand(LogTimeAsync);
            ChangeColorCommand = new RelayCommand<string>(ChangeColor);
            LoadTextCommand = new AsyncCommand<string>(LoadTextAsync);
            SaveTextCommand = new AsyncCommand<string, string>(SaveTextAsync);
            PlaySoundCommand = new AsyncCommand<string>(PlaySoundAsync);
            CloseAppCommand = new RelayCommand(CloseApp);
        }
     
        public async Task SendFilesCommandExecute()
        {
            var audioFilePath = await TextToSpeechAsync("text", "voice");
            // Do something with audioFilePath...
        }

        
        private void OpenWebsite(object obj)
        {
            // Add logic to open a website
            Process.Start(new ProcessStartInfo("cmd", $"/c start https://www.google.com"));
        }

        private void DisplayDate(object obj)
        {
            // Add logic to display the current date
            MessageBox.Show(DateTime.Now.ToString());
        }

        private void CalculateSum(int num1, int num2)
        {
            // Add logic to calculate the sum of two numbers
            var sum = num1 + num2;
            MessageBox.Show($"The sum of {num1} and {num2} is {sum}");
        }

        private void ShowMessage(string message)
        {
            // Add logic to show a message box with a custom message
            MessageBox.Show(message);
        }

        private async Task LogTimeAsync(object obj)
        {
            // Add logic to log the current time to a file
            using StreamWriter writer = new StreamWriter("log.txt", append: true);
            await writer.WriteLineAsync(DateTime.Now.ToString());
        }

        private void ChangeColor(string color)
        {
            // Add logic to change the window background color
            // This needs to access the view, and might require a more complex implementation
        }

        private async Task LoadTextAsync(string filePath, string content)
        {
            // Add logic to load a text file into a text box
            using StreamReader reader = new StreamReader(filePath);
            string Text = await reader.ReadToEndAsync();
            // Assume there's a Text property that's bound to a TextBox.Text in the view
            // Text = content;
        }

        private async Task SaveTextAsync(string filePath, string content)
        {
            // Add logic to save text from a text box to a file
            using StreamWriter writer = new StreamWriter(filePath);
            await writer.WriteAsync(content);
        }

        private void CloseApp(object obj)
        {
            // Add logic to close the application
            Application.Current.Shutdown();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Voice> Voices
        {
            get { return _voices; }
            set
            {
                _voices = value;
                OnPropertyChanged();
            }
        }

        public Voice SelectedVoice
        {
            get { return _selectedVoice; }
            set
            {
                _selectedVoice = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDirectory
        {
            get { return _selectedDirectory; }
            set
            {
                _selectedDirectory = value;
                OnPropertyChanged();
            }
        }

        private void LoadVoices()
        {
            Voices = File.ReadAllLines("voices.txt")
                .Select(line => new Voice { Name = line })
                .ToList();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<string> TextToSpeechAsync(string text, string voice)
        {
            return await Task.Run(() => TextToSpeech(text, voice));
        }

        public async Task AddAudioToSlideAsync(string filePath, int slideNumber)
        {
            await Task.Run(() => AddAudioToSlide(filePath, slideNumber));
        }

        public void SelectDirectory()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
        
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                SelectedDirectory = dialog.SelectedPath;
            }
        }

        // Your properties and methods here...
        public async Task AddAudioCommandExecute()
        {
            await AddAudioToSlideAsync("filePath", 1);
        }
        
        public static async Task RunExecutableCommandExecute()
        {
            await RunExecutableAsync();
        }

        public static async Task RunExecutableAsync()
        {
            await Task.Run(() =>
            {
                // Your existing RunExecutable code here...
            });
        }

        private void RunExecutable()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"C:\Users\Thaddeus Maximus\finished_application\app.publish\PptxScraperGui.exe",
                        // Add any necessary arguments
                    }
                };
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                // Log or display the exception
                Debug.WriteLine(ex.ToString());
            }
        }

        private static async Task<string?> GetSecretFromKeyVaultAsync(string keyVaultUrl, string secretName)
        {
            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            var secretResponse = await client.GetSecretAsync(secretName);
            return secretResponse.Value.Value;
        }

        private async Task<string?> TextToSpeech(string text, string voice)
        {
            try
            {
                var config = SpeechConfig.FromSubscription(speechKey, speechRegion);
                config.SpeechSynthesisVoiceName = voice;
                SpeechConfig.setSpeechSynthesesOutputFormat(SpeechSynthesisOutputFormat.Riff48Khz16BitMonoPcm);
        
                using var synthesizer = new SpeechSynthesizer(config, null);
                var result = await synthesizer.SpeakTextAsync(text);

                if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                {

                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                    // Handle the error
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception
                Debug.WriteLine(ex.ToString());
            }
            return null;
        }

        private static async Task SynthesizeAudioAsync(SpeechSynthesisResult? result)
        {
        using var stream = AudioDataStream.FromResult(result);
        await stream.SaveToWaveFileAsync("%Documents%/PptxAudioFiles/audio_file.wav");
        }

        private static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text,
            Exception argumentOutOfRangeException)
        {
            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    Console.WriteLine($"Speech synthesized for text: [{text}]");
                    break;

                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");
                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        Console.WriteLine("CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;

                default:
                    throw argumentOutOfRangeException;
                    // throw new ArgumentOutOfRangeException(nameof(speechSynthesisResult.Reason), speechSynthesisResult.Reason, null);
            }
        }

        private void AddAudioToSlide(string filePath, int slideNumber)
        {
            PowerPoint.Application powerpoint = null;
            PowerPoint.Presentation presentation = null;
            PowerPoint.Slide slide = null;

            try
            {
                powerpoint = new PowerPoint.Application();
                presentation = powerpoint.Presentations.Open("Your PowerPoint file");
                slide = presentation.Slides[slideNumber];
            object value = slide.Shapes.AddMediaObject2(filePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 0, 0);
                presentation.Save();
                presentation.Close();
                powerpoint.Quit();
            }
            catch (Exception ex)
            {
                // Log or display the exception
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                if (slide != null)
                {
                    Marshal.ReleaseComObject(slide);
                }
                if (presentation != null)
                {
                    Marshal.ReleaseComObject(presentation);
                }
                if (powerpoint != null)
                {
                    Marshal.ReleaseComObject(powerpoint);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }



        /*
        public static async Task Master()
        {
            /*
            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential(true));
            // Replace with the URL of your Azure Key Vault instance.

            // Retrieve the secrets from Azure Key Vault.
            speechKey = await GetSecretFromKeyVaultAsync(keyVaultUrl, "speechKey");
            speechRegion = await GetSecretFromKeyVaultAsync(keyVaultUrl, "speechRegion");
            ///
            const string secretName = "SpeechKey";
            // const string keyVaultUrl = @"https://key-vault.vault.azure.net/";
            const string resourceLocation = "westus2";
            var speechConfig = (secretName, resourceLocation);

            // The language of the voice that speaks.
            speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";
            using var speechSynthesizer = new SpeechSynthesizer(SpeechConfig);
            // Get text from the console and synthesize to the default speaker.
            Console.WriteLine("Enter some text that you want to speak >");
            var inputText = Console.ReadLine() ?? throw new InvalidOperationException();

            var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(inputText);
            OutputSpeechSynthesisResult(speechSynthesisResult, inputText, new ArgumentOutOfRangeException(
                nameof(speechSynthesisResult.Reason),
                speechSynthesisResult.Reason, null));
        }*/
    }
}