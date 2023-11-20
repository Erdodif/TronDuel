using CommunityToolkit.Mvvm.ComponentModel;

using TronLightCycle.GameObjects;
using TronLightCycle.GameObjects.BoardElements;

namespace TronDuel.ViewModel;

public partial class TileViewModel(ITile tile, int x, int y) : ObservableObject
{
    public int X { get; private set; } = x;

    public int Y { get; private set; } = y;

    public ITile Tile { get; set; } = tile;

    public bool Visible => Tile is TrailBike;

    public Color BackgroundColor
    {
        get
        {
            if (Tile is Wall)
            {
                return Colors.Black;
            }
            if (Tile is PlayerWall pw)
            {
                return Color.FromRgb(pw.Player.Color.R, pw.Player.Color.G, pw.Player.Color.B);
            }
            return Colors.Transparent;
        }
    }

    public Color BorderColor
    {
        get
        {
            if (Tile is Wall)
            {
                return Colors.Black;
            }
            if (Tile is PlayerWall pw)
            {
                return Color.FromRgb(pw.Player.Color.R, pw.Player.Color.G, pw.Player.Color.B);
            }
            if (Tile is TrailBike tb)
            {
                return Color.FromRgb(tb.Player.Color.R, tb.Player.Color.G, tb.Player.Color.B);
            }
            return Colors.Transparent;
        }
    }

    public int Angle => (Tile is TrailBike bike) ? Directions.AngleOf(bike.Direction) : 0;
}
