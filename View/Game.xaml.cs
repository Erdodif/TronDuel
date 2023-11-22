using TronDuel.ViewModel;

namespace TronDuel.View;
public partial class Game : ContentPage
{

    public Game(GVMService vms)
    {
        BindingContext = vms.GameViewModel;
        InitializeComponent();
        Map.SizeChanged += (_, _) =>
        {
            Dispatcher.Dispatch(() => { Map.WidthRequest = Map.Height; Map.ForceLayout(); });
        };
    }
}