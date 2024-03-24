using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public class RecycleProxy<TValue>: IRecycleProxy<TValue>
        where TValue: INode
    {
        private int orderId;

        private readonly Dictionary<Type, Queue<TValue>> singles = new();
        private readonly int singleCountMax;
        private readonly Dictionary<Type, int> singleCounts = new();
        private readonly int singleWeightMax;
        private readonly Dictionary<Type, int> singleWeights = new();

        private readonly SortedDictionary<int, TValue> totals = new();
        private readonly int totalCountMax;
        private int totalCount;
        private readonly int totalWeightMax;
        private int totalWeight;

        public RecycleProxy((int? countMax, int? weightMax)? single, (int? countMax, int? weightMax)? total)
        {
            singleCountMax = single?.countMax ?? int.MaxValue;
            singleWeightMax = single?.weightMax ?? int.MaxValue;
            totalCountMax = total?.countMax ?? int.MaxValue;
            totalWeightMax = total?.weightMax ?? int.MaxValue;
        }

        async UniTask<TValue> IRecycleProxy<TValue>.GetOneAsync<TValueExpand>(Func<UniTask<TValueExpand>> newOneFunc)
        {
            TValue result;
            var key = typeof(TValueExpand);
            if (!singles.TryGetValue(key, out var queue)
                || queue.Count == 0)
            {
                result = await newOneFunc.Invoke();
                return result;
            }
            result = queue.Dequeue();
            singleCounts[key]--;
            singleWeights[key] -= result.Weight;
            totals.Remove(result.OrderId);
            totalCount--;
            totalWeight -= result.Weight;
            return await UniTask.FromResult(result);
        }

        TValue IRecycleProxy<TValue>.GetOne<TValueExpand>(Func<TValueExpand> newOneFunc)
        {
            TValue result;
            var key = typeof(TValueExpand);
            if (!singles.TryGetValue(key, out var queue)
                || queue.Count == 0)
            {
                result = newOneFunc.Invoke();
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

        public void Recycle<TValueExpand>(TValueExpand value, bool isRecycle, Action<TValue>? removeFunc = null) where TValueExpand: TValue
        {
            var key = typeof(TValueExpand);
            Recycle(key, value, isRecycle, removeFunc);
        }

        public void Recycle(Type key, TValue value, bool isRecycle, Action<TValue>? removeFunc)
        {
            if (!isRecycle)
            {
                removeFunc?.Invoke(value);
                return;
            }
            var preRecycleWeight = value.Weight;
            if (singleCounts.ContainsKey(key))
            {
                while (totalCount + 1 > totalCountMax 
                       || totalWeight + preRecycleWeight > totalWeightMax
                       || singleCounts[key] + 1 > singleCountMax 
                       || singleWeights[key] + preRecycleWeight > singleWeightMax)
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
            singleWeights[key] += preRecycleWeight;
            
            value.OrderId = orderId++;
            totals.Add(value.OrderId, value);
            totalCount++;
            totalWeight += preRecycleWeight;
        }

        void IRecycleProxy<TValue>.Clear<TValueExpand>()
        {
            var key = typeof(TValueExpand);
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

        void IRecycleProxy<TValue>.Clear()
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
