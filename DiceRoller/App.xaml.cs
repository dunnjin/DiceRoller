﻿using Prism;
using Prism.Ioc;
using DiceRoller.ViewModels;
using DiceRoller.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Autofac;
using DiceRoller.Storage;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DiceRoller
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<DiceMainMenuView>();
            containerRegistry.RegisterForNavigation<CreateDiceView>();
            containerRegistry.RegisterForNavigation<ListDiceView>();
            containerRegistry.Register<IEntityRepository, EntityRepository>();
        }
    }
}
