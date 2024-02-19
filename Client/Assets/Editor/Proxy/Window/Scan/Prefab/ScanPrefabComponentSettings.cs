using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor.Window
{
    public abstract partial class ScanPrefabComponentSettings<TEnum, T>
        where TEnum : Enum
        where T : Component
    {
        public void OnDraw()
        {
            SetScanDirectory();
            SetScan();
            SetResult();
        }

        protected abstract void ClassifiesAssembler();

        protected void AssembleClassify(TEnum tEnum, Func<T, bool> func, string log, Action<T> handle)
        {
            if (classifies == null)
            {
                classifies = new List<(TEnum, Func<T, bool>, string, Action<T>)>();
            }
            classifies.Add((tEnum, func, log, handle));
        }

        protected abstract int EnumToInt(TEnum tEnum);

        protected abstract TEnum IntToEnum(int value);
    }

    public class ScanInfo<T>
    {
        public string PrefabPath { get; private set; }
        public string PrefabName { get; private set; }

        public bool IsFoldout { get; set; }

        // key => ScanLog
        private Dictionary<string, ScanInfoItem<T>> Cache { get; set; }

        public ScanInfo(string prefabPath, string prefabName)
        {
            PrefabPath = prefabPath;
            PrefabName = prefabName;

            IsFoldout = true;
            Cache = new Dictionary<string, ScanInfoItem<T>>();
        }

        public ScanInfoItem<T> this[string scanLog]
        {
            get => Cache[scanLog];
        }

        public bool ContainsKey(string scanLog)
        {
            return Cache.ContainsKey(scanLog);
        }

        public void Add(string scanLog, ScanInfoItem<T> scanInfoItem)
        {
            Cache.Add(scanLog, scanInfoItem);
        }

        public Dictionary<string, ScanInfoItem<T>>.KeyCollection Keys
        {
            get => Cache.Keys;
        }

        public Dictionary<string, ScanInfoItem<T>>.ValueCollection Values
        {
            get => Cache.Values;
        }
    }

    public class ScanInfoItem<T>
    {
        public string PrefabPath { get; private set; }
        public string PrefabName { get; private set; }
        public string ScanLog { get; private set; }
        public Action<T> Handle { get; private set; }

        public bool IsComponentObjsFoldout { get; set; }
        public List<string> ComponentObjsPath { get; set; }
        public List<string> ComponentObjsName { get; set; }

        public ScanInfoItem(string prefabPath, string prefabName, string scanLog, Action<T> handle)
        {
            PrefabPath = prefabPath;
            PrefabName = prefabName;
            ScanLog = scanLog;
            Handle = handle;

            IsComponentObjsFoldout = true;
            ComponentObjsPath = new List<string>();
            ComponentObjsName = new List<string>();
        }
    }
}

