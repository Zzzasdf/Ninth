using System.Collections.Generic;

namespace Ninth.Editor.Window
{
    public class TableStuct
    {
        // 支持多key
        public List<FieldDefine> Keys;

        // 字段组
        public Dictionary<string, FieldDefine> FieldDic;
    }

    // 字段定义
    public class FieldDefine
    {
        public string FieldName { get; private set; }
        public string FieldType { get; private set; }
        public string FieldDesc { get; private set; }

        // 集合类型, 储存方式，内 => 外
        public Dictionary<Collect, string> CollectDic { get; private set; }

        public FieldDefine(string fieldName, string fieldType, string fieldDesc)
        {
            FieldName = fieldName;
            FieldType = fieldType;
            FieldDesc = fieldDesc;

            // 查找类型是否为集合嵌套类型：支持多集合嵌套
            CollectDic = new Dictionary<Collect, string>();
            string[] collects = fieldType.Split("#");
            if(collects != null)
            {
                for (int index = 0; index < collects.Length; index++)
                {
                    string collectStr = collects[index];
                    Collect collect = CollectMap.GetCollect(collectStr);
                }
            }
        }
    }

    public static class CollectMap
    {
        private static Dictionary<string, Collect> cache = new Dictionary<string, Collect>()
        {
            ["Array"] = Collect.Array,
            ["List"] = Collect.List,
            ["Map"] = Collect.Map
        };

        public static Collect GetCollect(string collectStr)
        {
            try
            {
                return cache[collectStr];
            }
            catch(KeyNotFoundException e)
            {
                throw new KeyNotFoundException(string.Format("无法找到该字符串{0}的映射！！", collectStr), e);
            }
        }
    }

    public enum Collect
    {
        Null,
        Array,
        List,
        Map,
    }
}
