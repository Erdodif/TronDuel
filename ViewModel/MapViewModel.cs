using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using CommunityToolkit.Mvvm.ComponentModel;

using TronLightCycle.GameObjects.BoardElements;

using Map = TronLightCycle.GameObjects.BoardElements.Map;

namespace TronDuel.ViewModel;

public class MapViewModel : ObservableObject
{
    #region Properties
    public int Width { get; private set; }
    public int Height { get; private set; }
    public ObservableCollection<TileViewModel> FlatMap { get; private set; }

    public ColumnDefinitionCollection Columns { get; private set; }

    public RowDefinitionCollection Rows { get; private set; }
    #endregion

    #region Constructor
    public MapViewModel(Map map)
    {
        FlatMap = InitMap(map);
        Columns = new();
        Rows = new();
        for (int y = 0; y < map.Height; y++) Columns.Add(new(GridLength.Star)); 
        for (int y = 0; y < map.Width; y++) Rows.Add(new(GridLength.Star)); 
    }
    #endregion

    #region Methods
    private ObservableCollection<TileViewModel> InitMap(Map map)
    {
        Height = map.Height;
        Width = map.Width;
        ObservableCollection<TileViewModel> row = [];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                row.Add(new(map[y, x], x, y));
            }
        }
        return row;
    }

    public async Task SyncHeatMap(Map map, HashSet<(int, int)> heatmap)
    {
        await Task.Run(() =>
        {
            foreach ((int y, int x) in heatmap)
            {
                FlatMap[(y * Width) + x].Tile = map[y, x];
            }
        });
    }

    public void SyncMap(Map map)
    {
        Height = map.Height;
        Width = map.Width;
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                FlatMap[(y * Width) + x].Tile = map[y, x];
            }
        }
        OnPropertyChanged();
    }
    #endregion
}

