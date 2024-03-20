using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public enum MagicWeaponEvent
    {
        AAA = 1,
        BBB = 2,
    }

    public enum VoucherEvent
    {
        VVV = 3,
        CCC = 4,
    }
    public class MagicWeaponMgr : Singleton<MagicWeaponMgr>
    {
        public bool EventAAA(int a) => false;
        public bool EventBBB(int a) => false;
    }

    public class VoucherMgr : Singleton<VoucherMgr>
    {
        public bool EventCCC(long a) => false;
        public bool EventVVV(string a) => false;
    }

    public class RedTipMgr : Singleton<RedTipMgr>
    {
        public RedTipMgr()
        {
            RedTipGroups<int>.Instance.TryAdds(
                (MagicWeaponEvent.AAA, MagicWeaponMgr.Instance.EventAAA),
                (MagicWeaponEvent.BBB, MagicWeaponMgr.Instance.EventBBB)
            );

            RedTipGroups<long>.Instance.TryAdds(
                (VoucherEvent.CCC, VoucherMgr.Instance.EventCCC)
            );

            RedTipGroups<string>.Instance.TryAdds(
                (VoucherEvent.VVV, VoucherMgr.Instance.EventVVV)
            );
        }
        public bool TryRemove<T>(Enum redTipEnum, T tKey)
        {
            if(!RedTipGroups<T>.Instance.TryGetValue(redTipEnum, out var group))
            {
                return false;
            }
            return group.TryRemove(tKey);
        }
        public void SetDirty<T>(Enum redTipEnum, bool dirty)
        {
            if(!RedTipGroups<T>.Instance.TryGetValue(redTipEnum, out var group))
            {
                return;
            }
            group.SetDirty(dirty);
        }
        public void SetDirty<T>(Enum redTipEnum, T tKey, bool dirty)
        {
            if(!RedTipGroups<T>.Instance.TryGetValue(redTipEnum, out var group))
            {
                return;
            }
            group.SetDirty(tKey, dirty);
        }
        public bool IsDisplay<T>(Enum redTipEnum, T tKey)
        {
            if(!RedTipGroups<T>.Instance.TryGetValue(redTipEnum, out var group))
            {
                return false;
            }
            return group.IsDisplay(tKey);
        }

        public class RedTipGroups<T>: Singleton<RedTipGroups<T>>
        {
            private Dictionary<Enum, RedTipGroup<T>> container = new();

            public void TryAdds(params (Enum enumKey, Func<T, bool> funcCheck)[] groups)
            {
                foreach(var item in groups)
                {
                    TryAdd(item.enumKey, item.funcCheck);
                }
            }

            private void TryAdd(Enum enumKey, Func<T, bool> funcCheck)
            {
                if(container.ContainsKey(enumKey))
                {
                    $"已存在此枚举{enumKey}, 请勿重复添加！！".Error();
                    return;
                }
                container.Add(enumKey, new RedTipGroup<T>(funcCheck));
            }

            public bool TryGetValue(Enum enumKey, out RedTipGroup<T> group)
            {
                return container.TryGetValue(enumKey, out group);
            }
        }

        public class RedTipGroup<T>
        {
            private Func<T, bool> funcCheck;
            private Dictionary<T, RedTipItem<T>> container;

            public RedTipGroup(Func<T, bool> funcCheck)
            {
                this.funcCheck = funcCheck;
                this.container = new Dictionary<T, RedTipItem<T>>();
            }

            public bool TryRemove(T tKey)
            {
                if (!container.ContainsKey(tKey))
                {
                    return false;
                }
                container.Remove(tKey);
                return true;
            }

            public void SetDirty(bool dirty)
            {
                foreach (var item in container)
                {
                    item.Value.SetDirty(dirty);
                }
            }

            public void SetDirty(T tKey, bool dirty)
            {
                if (!container.ContainsKey(tKey))
                {
                    return;
                }
                container[tKey].SetDirty(dirty);
            }

            public bool IsDisplay(T tKey)
            {
                if (!container.TryGetValue(tKey, out RedTipItem<T> value))
                {
                    value = new RedTipItem<T>();
                    container.Add(tKey, value);
                }
                return value.IsDisplay(this.funcCheck, tKey);
            }
        }

        public class RedTipItem<T>
        {
            private bool dirty;
            private bool display;
            public RedTipItem()
            {
                dirty = true;
                display = true;
            }

            public void SetDirty(bool dirty)
            {
                this.dirty = dirty;
            }
            public bool IsDisplay(Func<T, bool> funcCheck, T tArg)
            {
                if (dirty)
                {
                    dirty = !dirty;
                    display = funcCheck?.Invoke(tArg) ?? false;
                }
                return display;
            }
        }
    }
}
