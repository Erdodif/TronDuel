using TronDuel.ViewModel;

namespace TronDuel.View;

public partial class Game : ContentPage
{
	public Game(GameViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}