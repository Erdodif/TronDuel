using TronLightCycle.GameObjects;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Map = TronLightCycle.GameObjects.BoardElements.Map;
using Game = TronLightCycle.GameObjects.Game;
using Color = System.Drawing.Color;
using TronLightCycle.GameObjects.BoardElements;
using System.Text.Json;
using System.Text;

namespace TronDuel.ViewModel;

public partial class GameViewModel : ObservableObject
{
    private static readonly int GAME_SPEED = 250;
    #region Properties
    private Game game;
    public Game Game { get => game; }

    [ObservableProperty]
    private MapViewModel mapViewModel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CountDownOn))]
    private int countDown = 0;

    #region Toogles
    [ObservableProperty]
    private bool fresh = true;

    [ObservableProperty]
    private bool paused = true;
    [ObservableProperty]
    private bool moveButtonsAvailable = false;
    [ObservableProperty]
    private bool loadable = true;

    public bool CountDownOn { get => CountDown != 0; }
    #endregion
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
        Fresh = false;
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
    private void Shuffle()
    {
        if (game.State == GameState.Ongoing) return;
        this.game.ResetGame();
        Paused = true;
        Fresh = true;
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

    [RelayCommand]
    private async Task SaveGame()
    {
        var page = Application.Current!.MainPage!;
        string name = $"savegame_{DateTime.Now.DayOfYear}_{DateTime.Now.Hour}_{DateTime.Now.Minute}";
        name = await page.DisplayPromptAsync("Save game", "give a name", initialValue: name);
        var invalids = Path.GetInvalidFileNameChars();
        StringBuilder boby = new();
        foreach (var item in name)
        {
            if(!invalids.Contains(item)) boby.Append(item);
        }
        name = boby.ToString();
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        if(!Path.Exists(path)) Directory.CreateDirectory(path);
        if (File.Exists(Path.Combine(path, name + ".tron.savedgame")) && !await page.DisplayAlert("Savefile exists", "This name is occupied.\nDo you want to overrite it?", "On god", "God no"))
        {
            return;
        }
        try
        {
            File.WriteAllText(Path.Combine(path, name + ".tron.savedgame"), JsonSerializer.Serialize(this.Game));
            await page.DisplayAlert("Game saved","This will wait until you have more time","okay, okay");
            App.Current.Quit();
        }
        catch
        {
            await page.DisplayAlert("Something went horribly wrong", "🥰", "boi");
        }
    }

    [RelayCommand]
    private void Exit()
    {
        Application.Current?.Quit();
    }
    #endregion

    #region Game Events
    private void GameUpdated(Map map, HashSet<(int, int)> heatmap)
    {
        _ = Task.Run(async () =>
        {
            await MapViewModel.SyncHeatMap(map, heatmap);
            OnPropertyChanged(nameof(MapViewModel));
        });
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

    public GameViewModel(Game game)
    {
        this.game = game;
        game.Pause();
        mapViewModel = new MapViewModel(game.Map);
        game.UpdateEvent += GameUpdated;
        game.EndEvent += GameEnded;
    }

    public GameViewModel(MapSize mapSize)
    {
        Player.ClearPlayerList();
        game = new Game((int)mapSize, GAME_SPEED, new Player("Blue", Color.BlueViolet), new Player("Red", Color.IndianRed));
        mapViewModel = new MapViewModel(game.Map);
        game.UpdateEvent += GameUpdated;
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
