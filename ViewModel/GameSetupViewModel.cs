using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using TronDuel.View;

namespace TronDuel.ViewModel;

public partial class GameSetupViewModel : ObservableObject
{
    [ObservableProperty]
    private bool loading = false;

    [RelayCommand]
    public async Task ToGame12x12()
    {
        Loading = true;
        await Shell.Current.GoToAsync("//Game", true, [new("mapSize", MapSize.x12)]);
    }

    [RelayCommand]
    public async Task ToGame24x24()
    {
        Loading = true;
        await Shell.Current.GoToAsync("//Game", true, [new("mapSize", MapSize.x24)]);
    }

    [RelayCommand]
    public async Task ToGame36x36()
    {
        Loading = true;
        await Shell.Current.GoToAsync("//Game", true, [new("mapSize", MapSize.x36)]);
    }
}
