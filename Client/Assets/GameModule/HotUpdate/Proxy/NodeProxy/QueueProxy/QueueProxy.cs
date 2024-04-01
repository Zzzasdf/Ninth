using System.Collections.Generic;
using System;
using System.Linq;

namespace Ninth.HotUpdate
{
    public class QueueProxy<TKey, TValue>: IQueueProxy<TKey, TValue>
        where TValue: class, INode
    {
        private int orderId;

        private readonly Dictionary<TKey, Queue<TValue>> singles = new();
        private readonly int singleCountMax;
        private readonly Dictionary<TKey, int> singleCounts = new();
        private readonly int singleWeightMax;
        private readonly Dictionary<TKey, int> singleWeights = new();

        private readonly SortedDictionary<int, TValue> totals = new();
        private readonly int totalCountMax;
        private int totalCount;
        private readonly int totalWeightMax;
        private int totalWeight;

        public QueueProxy((int? countMax, int? weightMax)? single, (int? countMax, int? weightMax)? total)
        {
            singleCountMax = single?.countMax ?? int.MaxValue;
            singleWeightMax = single?.weightMax ?? int.MaxValue;
            totalCountMax = total?.countMax ?? int.MaxValue;
            totalWeightMax = total?.weightMax ?? int.MaxValue;
        }

        TValue? IQueueProxy<TKey, TValue>.Peek<TValueExpand>(TKey key)
        {
            TValue? result = null;
            if (!singles.TryGetValue(key, out var queue)
                || queue.Count == 0)
            {
                return result;
            }
            result = queue.Peek();
            return result;
        }
        
        TValue? IQueueProxy<TKey, TValue>.Dequeue<TValueExpand>(TKey key)
        {
            TValue? result = null;
            if (!singles.TryGetValue(key, out var queue)
                || queue.Count == 0)
            {
                return result;
            }
            result = queue.Dequeue();
            singleCounts[key]--;
            singleWeights[key] -= result.Weight;
            totals.Remove(result.OrderId);
            totalCount--;
            totalWeight -= result.Weight;
            return result;
        }

        void IQueueProxy<TKey, TValue>.Enqueue<TValueExpand>(TKey key, TValueExpand value, bool isEnqueue, Action<TValue>? removeFunc)
        {
            if (!isEnqueue)
            {
                removeFunc?.Invoke(value);
                return;
            }
            var preQueueWeight = value.Weight;
            if (singleCounts.ContainsKey(key))
            {
                while (totalCount + 1 > totalCountMax 
                       || totalWeight + preQueueWeight > totalWeightMax
                       || singleCounts[key] + 1 > singleCountMax 
                       || singleWeights[key] + preQueueWeight > singleWeightMax)
                {
                    var keyFirst = totals.Keys.First();
                    totals.Remove(keyFirst, out var item);
                    totalCount--;
                    var removeWeights = item.Weight;
                    totalWeight -= removeWeights;

                    singles[key].Dequeue();
                    singleCounts[key]--;
                    singleWeights[key] -= removeWeights;
                    removeFunc?.Invoke(item);
                }
            }
            if (!singles.TryGetValue(key, out var queue))
                singles.Add(key, queue = new Queue<TValue>());
            queue.Enqueue(value);
            singleCounts.TryAdd(key, 0);
            singleCounts[key]++;
            singleWeights.TryAdd(key, 0);
            singleWeights[key] += preQueueWeight;
            
            value.OrderId = orderId++;
            totals.Add(value.OrderId, value);
            totalCount++;
            totalWeight += preQueueWeight;
        }

        void IQueueProxy<TKey, TValue>.Clear<TValueExpand>(TKey key)
        {
            if (!singles.Remove(key, out var queue))
            {
                return;
            }
            singleCounts.Remove(key);
            singleWeights.Remove(key);
            foreach (var node in queue)
            {
                totals.Remove(node.OrderId);
                totalCount--;
                totalWeight -= node.Weight;
            }
        }

        void IQueueProxy<TKey, TValue>.Clear()
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