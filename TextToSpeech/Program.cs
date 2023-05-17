// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Windows.Media.SpeechSynthesis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpeechSynthesizer = Microsoft.CognitiveServices.Speech.SpeechSynthesizer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.



namespace TextToSpeech
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string subscriptionKey = "<Your-Azure-Speech-Service-Subscription-Key>";
            string region = "<Your-Azure-Speech-Service-Region>";

            foreach (string textFilePath in args)
            {
                try
                {
                    string text = await File.ReadAllTextAsync(textFilePath);

                    var config = SpeechConfig.FromSubscription(subscriptionKey, region);
                    using var synthesizer = new SpeechSynthesizer(config, null);

                    var result = await synthesizer.SpeakTextAsync(text);

                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        string audioFilePath = Path.ChangeExtension(textFilePath, ".wav");
                        await File.WriteAllBytesAsync(audioFilePath, result.AudioData);
                        Console.WriteLine($"Audio file created: {audioFilePath}");
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}

