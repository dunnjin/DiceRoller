using DiceRoller.Common;
using DiceRoller.Core;
using DiceRoller.Models;
using DiceRoller.Storage;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SimpleExpressionEvaluator;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace DiceRoller.ViewModels
{
    public class ListDiceViewModel : BindableBase, INavigationAware
    {
        private Random _random;
        private readonly IEntityRepository _repository;
        private readonly INavigationService _navigationService;

        public ListDiceViewModel(
            IEntityRepository repository,
            INavigationService navigationService)
        {
            _repository = repository;
            _navigationService = navigationService;

            this.DiceCategories = new ObservableCollection<DiceCategory>();

            _random = new Random();
            _rollCommand = new DelegateCommand<Guid?>(this.Roll);
            _removeCommand = new DelegateCommand<Guid?>(this.Remove);
        }

        private ObservableCollection<DiceCategory> _diceCategories;
        public ObservableCollection<DiceCategory> DiceCategories
        {
            get => _diceCategories;
            set => base.SetProperty(ref _diceCategories, value);
        }

        private object _selectedDiceRoll;
        public object SelectedDiceRoll
        {
            get => _selectedDiceRoll;
            set => base.SetProperty(ref _selectedDiceRoll, value);
        }

        private ICommand _rollCommand;
        public ICommand RollCommand => _rollCommand;

        private ICommand _removeCommand;
        public ICommand RemoveCommand => _removeCommand;


        public void Roll(Guid? id)
        {
            try
            {
                var diceRoll = this.DiceCategories
                    .SelectMany(dc => dc)
                    .First(dr => dr.Id == id);

                // TODO: Abstract out
                var rollEquation = diceRoll.Equation;
                rollEquation
                    .RegexMatches(@"(\d+)d(\d+)")
                    .ToList()
                    .ForEach(e =>
                    {
                        var splitDice = e.Split('d');
                        var diceCount = int.Parse(splitDice.First());
                        var diceSides = int.Parse(splitDice.Last());
                        var diceResult = Enumerable
                            .Range(1, diceCount)
                            .Sum(c => _random.Next(1, diceSides + 1));

                        var regex = new Regex(e);

                        rollEquation = regex.Replace(rollEquation, $"({diceResult})", 1); 
                    });


                diceRoll.RollEquation = rollEquation;
                diceRoll.RollResult = new ExpressionEvaluator()
                    .Evaluate(rollEquation)
                    .ToString();
            }
            catch(Exception exception)
            {

            }
        }

        public async void Remove(Guid? id)
        {
            try
            {
                var diceRoll = this.DiceCategories
                    .SelectMany(dc => dc)
                    .First(dr => dr.Id == id);

                var dice = (await _repository.GetByIdAsync<Dice>(id.Value))
                    ?? throw new NullReferenceException(nameof(Dice));

                await _repository.RemoveAsync(dice);

                var diceCategory = this.DiceCategories.First(dc => dc.Any(d => d.Id == diceRoll.Id));
                diceCategory.Remove(diceRoll);

                if (!diceCategory.Any())
                    this.DiceCategories.Remove(diceCategory);
            }
            catch(Exception exception)
            {

            }
        }
         
        public async void EditDice(Guid? id)
        {
            try
            {
                if (!id.HasValue)
                    return;

                await _navigationService.NavigateAsync($"{Constants.Views.CREATE_DICE}?diceId={id.Value}");
            }
            catch(Exception exception)
            {

            }
        }

        #region INavigationAware
        public void OnNavigatedFrom(NavigationParameters parameters) { }
        public void OnNavigatingTo(NavigationParameters parameters) { }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                this.DiceCategories = (await _repository
                    .GetAllAsync<Dice>())
                    .OrderBy(d => d.Category)
                    .GroupBy(d => d.Category)
                    .Select(g =>
                    {
                        var diceCategory = new DiceCategory
                        {
                            Title = g.Key,
                        };

                        diceCategory.AddRange(g
                            .OrderBy(d => d.Name)
                            .Select(d =>
                                new DiceRoll
                                {
                                    Id = d.Id,
                                    Category = g.Key,
                                    Equation = d.Equation,
                                    Name = d.Name,
                                    RollEquation = d.Equation,
                                    RollResult = "0",
                                }));

                        return diceCategory;
                    })
                    .ToObservableCollection();
            }
            catch(Exception exception)
            {

            }
        }
        #endregion
    }
}
