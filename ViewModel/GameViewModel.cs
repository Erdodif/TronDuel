using TronLightCycle.GameObjects;

using Game = TronLightCycle.GameObjects.Game;
using Color = System.Drawing.Color;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TronDuel.ViewModel;

public class GameViewModel : ObservableObject
{
    private Game game;
    public Game Game { get => game; }

    private readonly MapViewModel mapViewModel;

    public MapViewModel MapViewModel { get => mapViewModel; }

    public GameViewModel(MapSize mapSize)
    {
        Player.ClearPlayerList();
        game = new Game((int)mapSize, new Player("Blue", Color.BlueViolet), new Player("Red", Color.IndianRed));
        mapViewModel = new MapViewModel(game.Map);
    }

    public GameViewModel() : this(MapSize.x12) { }
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
