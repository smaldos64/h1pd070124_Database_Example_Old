﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Example.ExtensionMethods
{
    public static class ObservableCollectionExtensions
    {
        public static int FindIndex<T>(this ObservableCollection<T> ts, Predicate<T> match)
        {
            return ts.FindIndex(0, ts.Count, match);
        }

        public static int FindIndex<T>(this ObservableCollection<T> ts, int startIndex, Predicate<T> match)
        {
            return ts.FindIndex(startIndex, ts.Count, match);
        }

        public static int FindIndex<T>(this ObservableCollection<T> ts, int startIndex, int count, Predicate<T> match)
        {
            if (startIndex < 0) startIndex = 0;
            if (count > ts.Count) count = ts.Count;

            for (int i = startIndex; i < count; i++)
            {
                if (match(ts[i])) return i;
            }

            return -1;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}
