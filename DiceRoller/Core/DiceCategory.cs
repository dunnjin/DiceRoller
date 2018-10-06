using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiceRoller.Core
{
    public class DiceCategory : ObservableCollection<DiceRoll>
    {
        public string Title { get; set; }

        public ObservableCollection<DiceRoll> DiceRolls => this;
    }
}
