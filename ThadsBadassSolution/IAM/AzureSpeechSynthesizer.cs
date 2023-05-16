using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace IAM
{
    public class AzureSpeechSynthesizer
    {
        private readonly SpeechConfig _speechConfig;

        public AzureSpeechSynthesizer(string subscriptionKey, string region)
        {
            _speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
        }

        public async Task SynthesizeAudioAsync(string text, string voiceName, string outputFileName, bool useSsml = false)
        {
            using var synthesizer = new SpeechSynthesizer(_speechConfig, null);
            _speechConfig.SpeechSynthesisVoiceName = voiceName;

            using var result = useSsml
                ? await synthesizer.SpeakSsmlAsync($"<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\"><voice name=\"{voiceName}\">{text}</voice></speak>")
                : await synthesizer.SpeakTextAsync(text);

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                using var fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
                await result.AudioData.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                throw new SpeechSynthesisException($"CANCELED: Reason={cancellation.Reason}, ErrorCode={cancellation.ErrorCode}, ErrorDetails={cancellation.ErrorDetails}");
            }
        }

        public static List<string> GetNeuralVoiceNames()
        {
            return new List<string>
            {
                "en-US-JessaNeural",
                "en-US-GuyNeural",
                "en-US-AriaNeural",
                // Add more neural voices here
            };
        }
    }

    public class SpeechSynthesisException : Exception
    {
        public SpeechSynthesisException(string message) : base(message) { }
    }
}
