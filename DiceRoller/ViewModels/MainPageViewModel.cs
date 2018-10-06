using DiceRoller.Common;
using DiceRoller.Storage;
using Prism.Mvvm;
using Prism.Navigation;
using System;

namespace DiceRoller.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region INavigationAware
        public void OnNavigatedFrom(NavigationParameters parameters) { }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            await _navigationService.NavigateAsync(Constants.Views.DICE_MAIN_MENU);
        }

        public void OnNavigatingTo(NavigationParameters parameters) { }
        #endregion
    }
}
