#if WINDOWS
using KeyboardHookLite;
#endif

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

#if WINDOWS
    static private KeyboardHook kbh = new KeyboardHook();
    static private bool acceptInput = true;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        kbh.KeyboardPressed += KeyDown;
    }

    private void KeyDown(object? sender, KeyboardHookEventArgs e)
    {
        if (!acceptInput) return;
        acceptInput = false;
        _ = Task.Run(async () =>
        {
            var key = e.InputEvent.Key;
            var vm = (BindingContext as GameViewModel)!;
            if (key == System.Windows.Input.Key.A && vm.TP2LCommand.CanExecute(null))
            {
                vm.TP2LCommand.Execute(null);
            }
            if (key == System.Windows.Input.Key.D && vm.TP2RCommand.CanExecute(null))
            {
                vm.TP2RCommand.Execute(null);
            }
            if (key == System.Windows.Input.Key.J && vm.TP1LCommand.CanExecute(null))
            {
                vm.TP1LCommand.Execute(null);
            }
            if (key == System.Windows.Input.Key.L && vm.TP1RCommand.CanExecute(null))
            {
                vm.TP1RCommand.Execute(null);
            }
            await Task.Delay(40);
            acceptInput = true;
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        kbh.Dispose();
    }
#endif
}