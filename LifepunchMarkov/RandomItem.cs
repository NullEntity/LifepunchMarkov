﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomItemExtension
{
    class ItemExtension
    {
        private static Random _random = new Random();

        public static T GetRandomItem<T>(IEnumerable<T> itemsEnumerable, Func<T, int> weightKey)
        {
            var items = itemsEnumerable.ToList();

            var totalWeight = items.Sum(x => weightKey(x));
            var randomWeightedIndex = _random.Next(totalWeight);
            var itemWeightedIndex = 0;
            foreach (var item in items)
            {
                itemWeightedIndex += weightKey(item);
                if (randomWeightedIndex < itemWeightedIndex)
                    return item;
            }
            throw new ArgumentException("Collection count and weights must be greater than 0");
        }
    }
}
