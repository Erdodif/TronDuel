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
    public bool Paused { get => game.State == GameState.Paused || game.State == GameState.Fresh; }
    public bool MoveButtonsAvailable { get => game.State == GameState.Ongoing; }
    public bool Loadable { get => game.State == GameState.Fresh; }
    public bool CountDownOn { get => CountDown != 0; }
    #endregion

    #region Command
    [RelayCommand]
    private async Task ResumeGame()
    {
        this.OnPropertyChanging(nameof(Paused));
        this.OnPropertyChanging(nameof(Loadable));
        this.OnPropertyChanging(nameof(MoveButtonsAvailable));
        CountDown = 3;
        await Task.Delay(1000);
        CountDown = 2;
        await Task.Delay(1000);
        CountDown = 1;
        await Task.Delay(1000);
        CountDown = 0;
        _ = Task.Run(game.Start);
        this.OnPropertyChanged(nameof(Paused));
        this.OnPropertyChanged(nameof(Loadable));
        this.OnPropertyChanged(nameof(MoveButtonsAvailable));
        this.OnPropertyChanged(nameof(CountDownOn));
    }

    [RelayCommand]
    private void Pause()
    {
        game.Pause();
        this.OnPropertyChanged(nameof(Paused));
        this.OnPropertyChanged(nameof(MoveButtonsAvailable));
        this.OnPropertyChanged(nameof(CountDownOn));
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
        game = new Game((int)mapSize, 500, new Player("Blue", Color.BlueViolet), new Player("Red", Color.IndianRed));
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
