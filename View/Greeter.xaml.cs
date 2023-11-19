using TronDuel.ViewModel;

namespace TronDuel.View;

public partial class Greeter : ContentPage
{
	public Greeter(GameSetupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

}