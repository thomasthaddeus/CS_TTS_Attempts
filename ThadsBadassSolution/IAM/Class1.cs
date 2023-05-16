using Azure.Identity;

from azure.identity import DefaultAzureCredential 
    from azure.cognitiveservices.speech import SpeechConfig

    credential = DefaultAzureCredential()
speech_config = SpeechConfig(subscription="your_subscription_key", region="your_region", credential=credential)
