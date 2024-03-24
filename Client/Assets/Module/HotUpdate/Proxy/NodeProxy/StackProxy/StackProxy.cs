using System.Collections.Generic;
using System;
using System.Linq;

namespace Ninth.HotUpdate
{
    public class StackProxy<TKey, TValue>: IStackProxy<TKey, TValue>
        where TValue: class, INode
    {
        private int orderId;

        private readonly Dictionary<TKey, LinkedList<TValue>> singles = new();
        private readonly int singleCountMax;
        private readonly Dictionary<TKey, int> singleCounts = new();
        private readonly int singleWeightMax;
        private readonly Dictionary<TKey, int> singleWeights = new();

        private readonly SortedDictionary<int, TValue> totals = new();
        private readonly int totalCountMax;
        private int totalCount;
        private readonly int totalWeightMax;
        private int totalWeight;

        public StackProxy((int? countMax, int? weightMax)? single, (int? countMax, int? weightMax)? total)
        {
            singleCountMax = single?.countMax ?? int.MaxValue;
            singleWeightMax = single?.weightMax ?? int.MaxValue;
            totalCountMax = total?.countMax ?? int.MaxValue;
            totalWeightMax = total?.weightMax ?? int.MaxValue;
        }

        TValue? IStackProxy<TKey, TValue>.Peek(TKey key)
        {
            TValue? result = null;
            if (!singles.TryGetValue(key, out var linkedList)
                || linkedList.Count == 0)
            {
                return result;
            }
            result = linkedList.Last.Value;
            return result;
        }
        
        TValue? IStackProxy<TKey, TValue>.Pop(TKey key)
        {
            TValue? result = null;
            if (!singles.TryGetValue(key, out var linkedList)
                || linkedList.Count == 0)
            {
                return result;
            }
            result = linkedList.Last.Value;
            linkedList.RemoveLast();
            singleCounts[key]--;
            singleWeights[key] -= result.Weight;
            totals.Remove(result.OrderId);
            totalCount--;
            totalWeight -= result.Weight;
            return result;
        }

        void IStackProxy<TKey, TValue>.Push<TValueExpand>(TKey key, TValueExpand value, bool isPush, Action<TValue>? removeFunc)
        {
            if (!isPush)
            {
                removeFunc?.Invoke(value);
                return;
            }
            var preStackWeight = value.Weight;
            if (singleCounts.ContainsKey(key))
            {
                while (totalCount + 1 > totalCountMax 
                       || totalWeight + preStackWeight > totalWeightMax
                       || singleCounts[key] + 1 > singleCountMax 
                       || singleWeights[key] + preStackWeight > singleWeightMax)
                {
                    var keyFirst = totals.Keys.First();
                    totals.Remove(keyFirst, out var item);
                    totalCount--;
                    var removeWeights = item.Weight;
                    totalWeight -= removeWeights;

                    singles[key].RemoveFirst();
                    singleCounts[key]--;
                    singleWeights[key] -= removeWeights;
                    removeFunc?.Invoke(item);
                }
            }
            if (!singles.TryGetValue(key, out var linkedList))
                singles.Add(key, linkedList = new LinkedList<TValue>());
            linkedList.AddLast(value);
            singleCounts.TryAdd(key, 0);
            singleCounts[key]++;
            singleWeights.TryAdd(key, 0);
            singleWeights[key] += preStackWeight;
            
            value.OrderId = orderId++;
            totals.Add(value.OrderId, value);
            totalCount++;
            totalWeight += preStackWeight;
        }

        void IStackProxy<TKey, TValue>.Clear<TValueExpand>(TKey key)
        {
            if (!singles.Remove(key, out var linkedList))
            {
                return;
            }
            singleCounts.Remove(key);
            singleWeights.Remove(key);
            foreach (var node in linkedList)
            {
                totals.Remove(node.OrderId);
                totalCount--;
                totalWeight -= node.Weight;
            }
        }

        void IStackProxy<TKey, TValue>.Clear()
        {
            singles.Clear();
            singleCounts.Clear();
            singleWeights.Clear();
            totals.Clear();
            totalCount = 0;
            totalWeight = 0;
        }
    }
}