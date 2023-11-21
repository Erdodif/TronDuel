using TronLightCycle.GameObjects;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Map = TronLightCycle.GameObjects.BoardElements.Map;
using Game = TronLightCycle.GameObjects.Game;
using Color = System.Drawing.Color;
using TronLightCycle.GameObjects.BoardElements;

namespace TronDuel.ViewModel;

public partial class GameViewModel : ObservableObject
{
    #region Properties
    private Game game;
    public Game Game { get => game; }

    [ObservableProperty]
    private MapViewModel mapViewModel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CountDownOn))]
    private int countDown = 0;
    #endregion

    #region Toogles
    [ObservableProperty]
    private bool paused = true;
    [ObservableProperty]
    private bool moveButtonsAvailable = false;
    [ObservableProperty]
    private bool loadable = true;

    public bool CountDownOn { get => CountDown != 0; }
    #endregion

    #region Command
    [RelayCommand]
    private async Task ResumeGame()
    {
        CountDown = 3;
        await Task.Delay(1000);
        CountDown = 2;
        await Task.Delay(1000);
        CountDown = 1;
        await Task.Delay(1000);
        CountDown = 0;
        this.OnPropertyChanged(nameof(CountDownOn));
        _ = Task.Run(game.Start);
        Paused = false;
        Loadable = false;
        MoveButtonsAvailable = true;
    }

    [RelayCommand]
    private void Pause()
    {
        game.Pause();
        Paused = true;
        Loadable = true;
        MoveButtonsAvailable = false;
    }

    [RelayCommand]
    private void TP1L() => game.PrepareTurnPlayer(game.Players[0], TurnDirection.Left);
    [RelayCommand]
    private void TP1R() => game.PrepareTurnPlayer(game.Players[0], TurnDirection.Right);
    [RelayCommand]
    private void TP2L() => game.PrepareTurnPlayer(game.Players[1], TurnDirection.Left);
    [RelayCommand]
    private void TP2R() => game.PrepareTurnPlayer(game.Players[1], TurnDirection.Right);
    #endregion

    #region Game Events
    private async Task GameUpdated(Map map, HashSet<(int, int)> heatmap)
    {
        await MapViewModel.SyncHeatMap(map, heatmap);
        OnPropertyChanged(nameof(MapViewModel));
    }

    private void GameEnded(Player? player)
    {
        MoveButtonsAvailable = false;
        Loadable = false;
        if (player is null)
        {
            Application.Current?.Dispatcher.Dispatch(async () =>
            {
                await Application.Current!.MainPage!.DisplayAlert("Draw", "It is a draw", "wow");
                Application.Current.Quit();
            });
        }
        else
        {
            Application.Current?.Dispatcher.Dispatch(async () =>
            {
                await Application.Current!.MainPage!.DisplayAlert("Win", $"The winner is {player.Name}!", "Horray!");
                Application.Current.Quit();
            });
        }
    }
    #endregion

    #region Constructor

    public GameViewModel(MapSize mapSize)
    {
        Player.ClearPlayerList();
        game = new Game((int)mapSize, 800, new Player("Blue", Color.BlueViolet), new Player("Red", Color.IndianRed));
        mapViewModel = new MapViewModel(game.Map);
        game.UpdateEvent += (m, hm) => _ = GameUpdated(m, hm);
        game.EndEvent += GameEnded;
    }

    public GameViewModel() : this(MapSize.x12) { }

    #endregion
}

/// <summary>
/// A Baked set of available map sizes
/// 
/// This is needed because of the limitation of mobile displays
/// </summary>
public enum MapSize
{
    x12 = 12,
    x24 = 24,
    x36 = 36,
}
