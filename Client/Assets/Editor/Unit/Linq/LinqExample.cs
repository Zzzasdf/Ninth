using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using Random = System.Random;

namespace Ninth.Editor
{
    public class LinqExample
    {
        [Test]
        public void Intersect()
        {
            List<int> a = new();
            List<int> b = new();
            var result = a.Intersect(b);
            Debug.Assert(result != null);
        }

        [Test]
        public void GroupBy()
        {
            List<int> a = new() { 1, 3, 4, 5, 4};
            // var result = a.GroupBy(x => x).Select(g => new { g.Key, Count = g.Count()});
            var result = from x in a
                         group x by x into g
                         select new { g.Key, Count = g.Count() };
            foreach(var item in result)
            {
                $"{item.Key}, {item.Count}".Log();
            }
        }

        // 延迟执行（defer）与消耗（exhaust）
        [Test]
        public void DeferAndExhaust()
        {
            var lst = new List<int> { 1, 2, 3, 4, 5 };
            
            // var query = lst.Select(x => Mathf.Pow(x, 2));
            var query = lst.Select(x => 
            {
                Thread.Sleep(500);
                return Mathf.Pow(x, 2);
            }); // 没有消耗，不真正参与运算
            "finish".Log();
            query.Log();
            
            lst.Add(6);
            query.Log();
        }
        
        // PLinq并行查询
        [Test]
        public void ParallelQuery()
        {
            var arr = Enumerable
                .Range(1, 10)
                .ToArray()
                .AsParallel() // 多线程执行
                .AsOrdered()
                .Select(x => 
                {
                    Thread.Sleep(500);
                    return Mathf.Pow(x, 2).Log();
                })
                .AsSequential() // 恢复单线程
                .Log();
        }

        // 展平
        [Test]
        public void Flattening()
        {
            var mat = new int[][]{
                new[]{ 1,2,3,4},
                new[]{5,6,7,8},
                new[]{9,10,11,12}
            };

            // var res = 
            //     from row in mat
            //     from n in row
            //     select n;
            var res = mat
                .SelectMany(n => n);
            res.Log();
        }

        // 笛卡尔积
        [Test]
        public void CartesianProduct()
        {
            // for (int i = 0; i < 5; i++)
            // {
            //     for (int j = 0; j < 4; j++)
            //     {
            //         for (int k = 0; k < 3; k++)
            //         {
            //             $"{i},{j},{k}".Log();
            //         }
            //     }
            // }

            // var prods =
            //     from i in Enumerable.Range(0, 5)
            //     from j in Enumerable.Range(0, 4)
            //     from k in Enumerable.Range(0, 3)
            //     select $"{i},{j},{k}";

            var prods = Enumerable
                .Range(0, 5)
                .SelectMany(r => Enumerable.Range(0, 4), (l, r) => (l, r))
                .SelectMany(r => Enumerable.Range(0, 3), (l, r) => (l.l, l.r, r))
                .Select(x => x.ToString());

            prods.Log();
        }

        // 字母频率
        [Test]
        public void LetterFrequency()
        {
            var words = new string[] { "tom", "jerry", "spike", "tyke", "butch", "quacker" };
            // var query =
            //     from w in words
            //     from c in w
            //     group c by c into g
            //     select new { g.Key, Count = g.Count() } into a
            //     orderby a.Count descending
            //     select a;
            var query = words
                .SelectMany(c => c)
                .GroupBy(c => c)
                .Select(g => new { g.Key, Count = g.Count()})
                .OrderByDescending(g => g.Count);

            query.Log();
        }

        // 批量下载文件
        [Test]
        public async void BatchDownloadOfFiles()
        {
            var urls = new string[]
            {
                "http://www.example/com/pic1.jpg",
                "http://www.example/com/pic2.jpg",
                "http://www.example/com/pic3.jpg",
            };

            // var tasks = new List<Task>();
            // foreach(var url in urls)
            // {
            //     tasks.Add(DownloadAsync(url, url.Split('/').Last()));
            // }

            // var tasks = urls
            //     .Select(url => DownloadAsync(url, url.Split('/').Last()));

            var tasks =
                from url in urls
                let filename = url.Split('/').Last()
                where filename != "pic2.jpg"
                select DownloadAsync(url, filename);

            await Task.WhenAll(tasks);
            "finish".Log();

            async Task DownloadAsync(string url, string filename)
            {
                await Task.Delay(1000);
                $"{filename} downloaded.".Log(); 
            }
        }

        // 寻找派生类
        [Test]
        public void FindDerivedClasses()
        {
            var types = Assembly
                .GetAssembly(typeof(Exception))
                .GetTypes();
            types
                .Where(t => t.IsSubclassOf(typeof(Exception)))
                .Select(t => t.Name)
                .OrderBy(t => t.Length)
                .Log();
        }

        // 开销预警
        [Test]
        public void OverheadWarning()
        {
            QueryExceeds();
            ReplaceOrderBy();

            // 查询是否有超过某个数
            void QueryExceeds()
            {
                const int Size = 100_000_000;
                var arr = GetRandomArray();
                bool res;
                // res = arr.Where(x => x > 5000).Count() > 0;
                res = arr.Any(x => x > 5000);

                IEnumerable<int> GetRandomArray()
                {
                    var rnd = new Random(1334);
                    return Enumerable
                        .Range(0, Size)
                        .Select(_ => rnd.Next(1000));
                }
            }

            // 替换OrderBy方法
            void ReplaceOrderBy()
            {
                var arr1 = new List<int> { 1, 3, 5, 7, 9, 2, 4, 6, 8 };
                // arr1 = arr.OrderBy(x => x).ToList(); // 新生成了一个集合, 而且Lamda表达式本质上是个委托，要占用托管堆
                // arr1 = arr.OrderByDescending(x => x).ToList();
                arr1.Sort(); // 在原数组操作
                arr1.Reverse(); // 反转         
                arr1.Log();

                var arr2 = new [] { 1, 3, 5, 7, 9, 2, 4, 6, 8 };
                Array.Sort(arr2);
                arr2.Log();
            }
        }

        [Test]
        public void CommonAPI()
        {
            var arr = new List<int> { 1, 3, 5, 7, 9, 2, 4, 6, 8, 1 };
            arr.First(x => (x & 1) == 0).Log("First => {0}");
            arr.FirstOrDefault(x => x == 10).Log("FirstOrDefault => {0}");
            arr.Last(x => (x & 1) == 0).Log("Last => {0}");
            arr.LastOrDefault(x => x == 10).Log("LastOrDefault => {0}");
            arr.Count(x => (x & 1) == 0).Log("Count => {0}");
            arr.Min().Log("Min => {0}");
            arr.Max().Log("Max => {0}");
            arr.Average().Log("Average => {0}");
            arr.Sum().Log("Sum => {0}");
            arr.Distinct().Log("Distinct => {0}"); // 去重
            arr.Distinct().Single(x => x == 1).Log("Single => {0}"); // 查找的目标不存在或存在多个，则返回异常System.InvalidOperationException,  需先去重
        }
    }
}