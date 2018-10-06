using Prism.Mvvm;
using System;

namespace DiceRoller.Core
{
    public class DiceRoll : BindableBase
    {
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => base.SetProperty(ref _id, value);
        }

        private string _rollResult;
        public string RollResult
        {
            get => _rollResult;
            set => base.SetProperty(ref _rollResult, value);
        }

        private string _rollEquation;
        public string RollEquation
        {
            get => _rollEquation;
            set => base.SetProperty(ref _rollEquation, value);
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
    }
}
