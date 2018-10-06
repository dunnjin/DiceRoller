using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiceRoller.Common
{
    public static class WpfHelpers
    {
        public static ObservableCollection<TSource> ToObservableCollection<TSource>(this IEnumerable<TSource> source) =>
            new ObservableCollection<TSource>(source);
        
        public static void AddRange<TSource>(this ObservableCollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
                source.Add(item);
        }
    }
}
