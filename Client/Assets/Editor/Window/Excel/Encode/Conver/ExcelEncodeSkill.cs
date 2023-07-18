using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HX
{
    public class ExcelEncodeSkill
    {
        // 转换
        private Dictionary<string, (string, Type)> Conver = new Dictionary<string, (string, Type)>()
        {
            ["ID"] = ("ID", typeof(int)),
            ["名称"] = ("Name", typeof(string)),
            ["描述"] = ("Desc", typeof(string)),
            ["功能类型"] = ("FuncType", typeof(int)),
            ["传递的转换率"] = ("TransferRate", typeof(int)),
        };

        // 检表
        private Dictionary<string, Func<bool>> Check = new Dictionary<string, Func<bool>>()
        {
            ["ID"] = CheckID,
        };

        // 映射
        private Dictionary<string, Dictionary<string, int>> Map = new Dictionary<string, Dictionary<string, int>>()
        {
            ["功能类型"] = new Dictionary<string, int>()
            {
                ["特效"] = 1,
                ["转换器"] = 2,
                ["增强器"] = 3,
            },
            ["功能参数"] = new Dictionary<string, int>()
            {
                ["双接口"] = 1,
                ["三接口"] = 2,
            },
        };

        private static bool CheckID()
        {
            return true;
        }
    }
}
