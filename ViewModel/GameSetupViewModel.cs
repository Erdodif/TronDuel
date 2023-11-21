using System.Text.Json;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using TronDuel.View;

namespace TronDuel.ViewModel;

public partial class GameSetupViewModel : ObservableObject
{
    private GVMService gvmservice;

    [ObservableProperty]
    private bool loading = false;


    [RelayCommand]
    public async Task ToGame12x12()
    {
        Loading = true;
        gvmservice.GameViewModel = new GameViewModel(MapSize.x12);
        await Shell.Current.GoToAsync($"//Game", true);
    }

    [RelayCommand]
    public async Task ToGame24x24()
    {
        Loading = true;
        gvmservice.GameViewModel = new GameViewModel(MapSize.x24);
        await Shell.Current.GoToAsync($"//Game", true);
    }

    [RelayCommand]
    public async Task ToGame36x36()
    {
        Loading = true;
        gvmservice.GameViewModel = new GameViewModel(MapSize.x36);
        await Shell.Current.GoToAsync($"//Game", true);
    }

    public GameSetupViewModel(GVMService gvm)
    {
        this.gvmservice = gvm;
    }
}
