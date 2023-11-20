using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using TronLightCycle.GameObjects.BoardElements;

using Map = TronLightCycle.GameObjects.BoardElements.Map;

namespace TronDuel.ViewModel;

public abstract class PropertyNotifier : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class CallColl<T> : ObservableCollection<T>
{
    public void UpdateManual()
    {
        //Needs attention!
        Dispatcher.GetForCurrentThread()?.DispatchAsync(() =>
        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)));
    }
}

public class MapViewModel : ObservableObject
{
    private CallColl<TileViewModel> InitMap(Map map)
    {
        Height = map.Height;
        Width = map.Width;
        CallColl<TileViewModel> row = new();
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
        FlatMap.UpdateManual();
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
        FlatMap.UpdateManual();
        OnPropertyChanged();
    }

    public int Width { get; private set; }
    public int Height { get; private set; }
    public CallColl<TileViewModel> FlatMap { get; private set; }

    public ColumnDefinitionCollection Columns { get; private set; }

    public RowDefinitionCollection Rows { get; private set; }

    public MapViewModel(Map map)
    {
        FlatMap = InitMap(map);
        Columns = new();
        Rows = new();
        for (int y = 0; y < map.Height; y++) Columns.Add(new(GridLength.Star)); 
        for (int y = 0; y < map.Width; y++) Rows.Add(new(GridLength.Star)); 
    }
}

