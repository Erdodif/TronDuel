using TronDuel.ViewModel;

namespace TronDuel.View;
public partial class Game : ContentPage
{

    public Game(GVMService vms)
    {
        BindingContext = vms.GameViewModel;
        InitializeComponent();
    }
}