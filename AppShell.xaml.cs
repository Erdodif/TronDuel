﻿using TronDuel.View;

namespace TronDuel
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            _ = GoToAsync("//Greeter");
        }
    }
}
