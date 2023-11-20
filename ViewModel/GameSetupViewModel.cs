using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using TronDuel.View;

namespace TronDuel.ViewModel;

public partial class GameSetupViewModel : ObservableObject
{

    [RelayCommand]
    public async Task ToGame12x12()
    {
        await Shell.Current.GoToAsync("//Game", true);
    }
}
