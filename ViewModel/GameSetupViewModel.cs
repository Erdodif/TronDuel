using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using TronDuel.View;

namespace TronDuel.ViewModel;

public partial class GameSetupViewModel : ObservableObject
{
    /*
    Command toGameCommand;
    public Command ToGameCommand => toGameCommand ??= new Command(() => Shell.Current.GoToAsync(nameof(Game)));*/

    [RelayCommand]
    public async Task ToGame()
    {
        await Shell.Current.GoToAsync(nameof(Game),true);
    }
}
