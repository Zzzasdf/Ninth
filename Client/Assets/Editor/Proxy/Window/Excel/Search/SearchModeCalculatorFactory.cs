using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ninth.Editor.Window
{

    public enum SearchMode
    {
        Equals,
        Contains,
    }

    public class SearchModeCalculatorFactory
    {
        private readonly static Dictionary<SearchMode, ISearchModeCalculator> factory = new Dictionary<SearchMode, ISearchModeCalculator>()
        {
            [SearchMode.Equals] = new SearchModeEqualsCalculator(),
            [SearchMode.Contains] = new SearchModeContainsCalculator(),
        };

        public static List<SearchMode> GetHeads()
        {
            return factory.Keys.ToList();
        }

        public static ISearchModeCalculator Get(SearchMode searchMode)
        {
            if (!factory.TryGetValue(searchMode, out ISearchModeCalculator result))
            {
                throw new NullReferenceException();
            }
            return result;
        }

        public interface ISearchModeCalculator
        {
            bool IsMatch(string cellValue, List<string> searchItems, StringComparison stringComparison);
        }

        public class SearchModeEqualsCalculator : ISearchModeCalculator
        {
            public bool IsMatch(string cellValue, List<string> searchItems, StringComparison stringComparison)
            {
                bool result = false;
                if (string.IsNullOrEmpty(cellValue)
                    || searchItems == null
                    || searchItems.Count == 0)
                {
                    return result;
                }
                for (int index = 0; index < searchItems.Count; index++)
                {
                    if (string.IsNullOrEmpty(searchItems[index]))
                    {
                        continue;
                    }
                    bool isEquals = cellValue.Trim().Equals(searchItems[index], stringComparison);
                    if (isEquals)
                    {
                        result = isEquals;
                        return result;
                    }
                }
                return false;
            }
        }

        public class SearchModeContainsCalculator : ISearchModeCalculator
        {
            public bool IsMatch(string cellValue, List<string> searchItems, StringComparison stringComparison)
            {
                bool result = false;
                if (string.IsNullOrEmpty(cellValue)
                    || searchItems == null
                    || searchItems.Count == 0)
                {
                    return result;
                }
                for (int index = 0; index < searchItems.Count; index++)
                {
                    if (string.IsNullOrEmpty(searchItems[index]))
                    {
                        continue;
                    }
                    bool isEquals = cellValue.Contains(searchItems[index], stringComparison);
                    if (isEquals)
                    {
                        result = isEquals;
                        return result;
                    }
                }
                return false;
            }
        }
    }
}

