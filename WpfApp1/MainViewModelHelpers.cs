using System.Threading.Tasks;

internal static class MainViewModelHelpers
{

    private static Task PlaySoundAsync(string soundFilePath)
    {
    // Add logic to play a sound file
    using var player = new System.Media.SoundPlayer(soundFilePath);
    player.Play();
    }
}