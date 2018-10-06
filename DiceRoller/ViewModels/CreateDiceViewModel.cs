using DiceRoller.Common;
using DiceRoller.Models;
using DiceRoller.Storage;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class CreateDiceViewModel : BindableBase, INavigationAware
    {
        private readonly IEntityRepository _entityRepository;
        private readonly INavigationService _navigationService;

        private Dice _selectedDice;

        public CreateDiceViewModel(
            IEntityRepository entityRepository,
            INavigationService navigationService)
        {
            _entityRepository = entityRepository;
            _navigationService = navigationService;

            _createCommand = new DelegateCommand(this.Create)
                .ObservesProperty(() => this.Name)
                .ObservesProperty(() => this.Equation)
                .ObservesProperty(() => this.Category)
                .ObservesCanExecute(() => this.CanCreate);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => base.SetProperty(ref _name, value);
        }

        private string _equation;
        public string Equation
        {
            get => _equation;
            set => base.SetProperty(ref _equation, value);
        }

        private string _category;
        public string Category
        {
            get => _category;
            set => base.SetProperty(ref _category, value);
        }

        private bool CanCreate =>
           !string.IsNullOrEmpty(this.Name) &&
           !string.IsNullOrEmpty(this.Equation) &&
           !string.IsNullOrEmpty(this.Category);

        private ICommand _createCommand;
        public ICommand CreateCommand => _createCommand;

        public async void Create()
        {
            try
            {
                var dice = Dice.Create(this.Name, this.Equation, this.Category);

                if(_selectedDice == null)
                {
                    await _entityRepository.AddAsync(dice);
                }
                else
                {
                    dice.Id = _selectedDice.Id;
                    await _entityRepository.UpdateAsync(dice);
                }

                await _navigationService.NavigateAsync($"{Constants.Views.DICE_MAIN_MENU}?selectedTab={Constants.Views.LIST_DICE}");
            }
            catch(Exception exception)
            {

            }
        }

        private void Clear()
        {
            _selectedDice = null;

            this.Name = string.Empty;
            this.Category = string.Empty;
            this.Equation = string.Empty;
        }

        #region INavigationAware
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            this.Clear();
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                var diceId = parameters.GetValue<Guid?>("diceId");
                if (!diceId.HasValue)
                    return;

                _selectedDice = (await _entityRepository.GetByIdAsync<Dice>(diceId.Value))
                    ?? throw new NullReferenceException(nameof(Dice));

                this.Name = _selectedDice.Name;
                this.Category = _selectedDice.Category;
                this.Equation = _selectedDice.Equation;
            }
            catch(Exception exception)
            {

            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}
