using System.Text.Json;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using TronDuel.View;

using TronLightCycle.GameObjects;
using TronLightCycle.Serialization;

namespace TronDuel.ViewModel;

public partial class GameSetupViewModel : ObservableObject
{
    private GVMService gvmservice;

    [ObservableProperty]
    private bool loading = false;


    [RelayCommand]
    private async Task ToGame12x12()
    {
        Loading = true;
        gvmservice.GameViewModel = new GameViewModel(MapSize.x12);
        await Shell.Current.GoToAsync($"//Game", true);
    }

    [RelayCommand]
    private async Task ToGame24x24()
    {
        Loading = true;
        gvmservice.GameViewModel = new GameViewModel(MapSize.x24);
        await Shell.Current.GoToAsync($"//Game", true);
    }

    [RelayCommand]
    private async Task ToGame36x36()
    {
        Loading = true;
        gvmservice.GameViewModel = new GameViewModel(MapSize.x36);
        await Shell.Current.GoToAsync($"//Game", true);
    }

    [RelayCommand]
    private async Task LoadSaved()
    {
        var page = Application.Current!.MainPage!;
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string[]? files = null;
        if (Directory.Exists(folder))
        {
            files = Directory.GetFiles(folder, "*.tron.savedgame");
        }
        if ((files?.Length ?? 0) == 0)
        {
            await page.DisplayAlert("No game for you",
#if WINDOWS
                "There is no savefile found on this microwave",
#else
                "There is no savefile found on this toaster",
#endif
                "shoot");
            return;
        }
        string file = await page.DisplayActionSheet("Select save", "no bueno", null, files);
        try
        {
            Loading = true;
            gvmservice.GameViewModel = new GameViewModel(GameFileTransfer.ReadFromFile(Path.Combine(folder,file))!);
            await Shell.Current.GoToAsync($"//Game", true);
        }
        catch
        {
            await page.DisplayAlert("Something went horribly wrong", "🤩", "boi");
        }
    }

    public GameSetupViewModel(GVMService gvm)
    {
        this.gvmservice = gvm;
    }
}
