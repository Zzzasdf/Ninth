using System.Linq;
using System.Text.RegularExpressions;
using Ninth.HotUpdate;
using NUnit.Framework;

// 正则表达式
// http://www.cnblogs.com/chaowang/p/6274852.html
namespace Ninth.Editor
{
    public class RegularExample
    {
        // 括号
        // [] => 需要匹配的字符
        // {} => 指定匹配字符的数量
        // () => 用来分组

        // ^ => 正则表达式的开始
        // $ => 正则表达式的阶数

        // 实际命令 => 快捷命令
        // [0-9] => d (digit, 十进制)
        // [a-z][0-9][_] => w
        // 0次或多次发生 => * 
        // 至少一次发生 => + (默认先自增，所以至少一次)
        // 0次或一次发生 => ? (true or false, 发不发生)

        // RegexOptions
        // RegexOptions枚举值 => 内敛标志 => 简单说明
        // ExplicitCapture => n => 只有定义了命名或编号的组才捕获
        // IgnoreCase => i => 不区分大小写
        // IgnorePatternWhitespace => x => 消除模式中的非转义空白并启用由#标记的注释
        // MultiLine => m => 多行模式，其原理是修改了 ^ 和 $ 的含义
        // SingleLine => s => 单行模式，和 MultiLine 相对应
        // 内联标志可以更小力度（一组为单位）的定义匹配选项

        // @ => 不让编辑器去解析其中的转义字符，而作为正则表达式的语法（元字符）存在

        // 定位元字符
        // \b => 匹配单词的开始或结束
        // \B => 匹配非单词的开始或结束
        // ^ => 匹配必须出现在字符串的开头或行开头
        // $ => 匹配必须出现在以下位置：字符串结尾、字符串结尾处的 \n 之前或行的结尾
        // \A => 指定匹配必须出现在字符串的开头（忽略 MultiLine 选项）
        // \z => 指定匹配必须出现在字符串的结尾（忽略 MultiLine 选项）
        // \Z => 指定匹配必须出现在字符串的结尾或字符串结尾处的 \n 之前（忽略 MultiLine 选项）
        // \G => 指定匹配必须出现在上一个匹配结束的地方。与 Match.NextMatch() 一起使用时，此断言确保所有匹配都是连续的

        // 基本语法元字符
        // . => 匹配除换行符以外的任意字符
        // \w => 匹配字符、数字、下划线、汉字
        // \W => \w的补集 (除\w定义的字符之外)
        // \s => 匹配任意空白符（包括换行符/n、回车符/r、制表符/t、垂直制表符/v、换页符/f)
        // \S => \s的补集 (除\s定义的字符之外)
        // \d => 匹配字符 (0-9数字)
        // \D => 表示\d的补集 (除0-9数字之外)
        // 在正则表达式中，\是转义字符，*是元字符，如果要表示一个\.*字符的话，需要使用\\\.\*

        // 反义字符
        // \B => 匹配不是单词开头或结束的位置
        // [ab] => 匹配中括号中的字符
        // [a-c] => a字符到c字符之前的字符
        // [^x] => 匹配除了x以外的任意字符
        // [^adwz] => 匹配除了adwz这几个字符以外的任意字符

        // 重复描述字符
        // {n} => 匹配前面的字符n次
        // {n,} => 匹配前面的字符n次或多于n次
        // {n,m} => 匹配前面的字符n到m次
        // ? => 重复零次或一次
        // + => 重复一次或更多次
        // * => 重复零次或更多次

        // 择一匹配
        // | => 将两个匹配条件进行逻辑 “ 或 ” (Or) 运算】

        [Test]
        public void MatchLetter()
        {
            // 搜索长度为10的a-z的英文字母
            Regex obj = new Regex("[a-z]{10}"); 
            string test = "aqwwga1vzq";
            obj.IsMatch(test).Log();
        }

        [Test]
        public void MatchUrl()
        {
            // 验证简单的网址URL
            // 第一步：检查是否存在 www: ^www.
            // 第二步：域名必须是长度在 1-15 的英文字母：.[a-z]{1,15}
            // 第三步：以 .com 或者 .org结束：.(com|org)$
            Regex url = new Regex("^www[.][a-z]{1,15}[.](com|org)$");
            string test = "www.baidu.com";
            url.IsMatch(test).Log();
        }

        [Test]
        public void MatchHeadTailChar()
        {
            // 匹配开始 ^
            string str = "I am Blue cat";
            Regex.Replace(str, "^", "准备开始：").Log();
            // 匹配结束 $
            string str2 = "I am Blue cat";
            Regex.Replace(str2, "$", "结束了！").Log();
        }
        
        [Test]
        public void MatchOutside()
        {
            // 查找除ahou这之外的所有字符
            string strFind1 = "I am a Cat!", strFind2 = "My Name's Blue cat!";
            ("除ahou这之外的所有字符,原字符为:" + strFind1 + "替换后:" + Regex.Replace(strFind1, @"[^ahou]", "*")).Log();
            ("除ahou这之外的所有字符,原字符为:" + strFind2 + "替换后:" + Regex.Replace(strFind2, @"[^ahou]", "*")).Log();
        }

        [Test]
        public void MatchQQ()
        {
            // 校验输入内容是否为合法QQ号（备注：QQ号为5-12位数字）
            string isQq1 = "1233", isQq2 = "a1233", isQq3 = "0123456789123", isQq4 = "556878554";
            string regexQq = @"^\d{5,12}$";
            (isQq1 + "是否为合法QQ号(5-12位数字):" + Regex.IsMatch(isQq1, regexQq)).Log();
            (isQq2 + "是否为合法QQ号(5-12位数字):" + Regex.IsMatch(isQq2, regexQq)).Log();
            (isQq3 + "是否为合法QQ号(5-12位数字):" + Regex.IsMatch(isQq3, regexQq)).Log();
            (isQq4 + "是否为合法QQ号(5-12位数字):" + Regex.IsMatch(isQq4, regexQq)).Log();
        }

        [Test]
        public void MatchNumOrLetter()
        {
            // 查找数字或字母
            string findStr1 = "ad(d2)-df";
            string regexFindStr = @"[a-z]\d";
            string newStrFind = string.Empty;
            MatchCollection newStr = Regex.Matches(findStr1, regexFindStr);
            newStr.Cast<Match>().Select(m => m.Value).ToList().ForEach(i => newStrFind += i);
            (findStr1 + "中的字母和数字组成的新字符串为:" + newStrFind).Log();
        }

        [Test]
        public void MatchName()
        {
            // 将人名输出 (zhangsan;lisi,wangwu.zhaoliu")
            string strSplit = "zhangsan;lisi,wangwu.zhaoliu";
            string regexSplitstr = @"[;][,][.]";
            Regex.Split(strSplit, regexSplitstr).ToList().ForEach(i => i.Log());
        }

        [Test]
        public void MatchTelephoneNumber()
        {
            // 校验国内电话号码（支持四种写法校验 A. 010-87654321 B. (010)87654321 C. 01087654321 D. 010 87654321）
            string TelNumber1 = "(010)87654321", TelNumber2 = "010-87654321", TelNumber3 = "01087654321",
            TelNumber4 = "09127654321", TelNumber5 = "010)87654321", TelNumber6 = "(010-87654321",
            TelNumber7 = "91287654321";
            Regex RegexTelNumber3 = new Regex(@"\(0\d{2,3}\)[-]?\d{7,8}|^0\d{2,3}[-]?\d{7,8}$");
            ("电话号码 " + TelNumber1 + " 是否合法:" + RegexTelNumber3.IsMatch(TelNumber1)).Log();
            ("电话号码 " + TelNumber2 + " 是否合法:" + RegexTelNumber3.IsMatch(TelNumber2)).Log();
            ("电话号码 " + TelNumber3 + " 是否合法:" + RegexTelNumber3.IsMatch(TelNumber3)).Log();
            ("电话号码 " + TelNumber4 + " 是否合法:" + RegexTelNumber3.IsMatch(TelNumber4)).Log();
            ("电话号码 " + TelNumber5 + " 是否合法:" + RegexTelNumber3.IsMatch(TelNumber5)).Log();
            ("电话号码 " + TelNumber6 + " 是否合法:" + RegexTelNumber3.IsMatch(TelNumber6)).Log();
            ("电话号码 " + TelNumber7 + " 是否合法:" + RegexTelNumber3.IsMatch(TelNumber7)).Log();
        }

        [Test]
        public void MatchRepeat()
        {
            // 用小括号来指定子表达式（也叫做分组）
            // 重复单字符 和 重复分组字符
            string inputStr = "31321412424";
            string strGroup1 = @"a{2}";
            ("单字符重复2两次替换为22, 结果为：" + Regex.Replace(inputStr, strGroup1, "22")).Log();
            // 重复 多个字符 使用（ abcd ）{n}进行分组限定
            string strGroup2 = @"(ab\w{2}){2}";
            ("分组字符重复2两次替换为5555, 结果为：" + Regex.Replace(inputStr, strGroup2, "5555")).Log();
        }

        [Test]
        public void MatchIP()
        {
             // 校验IP4地址 (如：192.168.1.4，为四段，每段最多三位，每段最大数字为255，并且第一位不能为0)
            string regexStrIp4 = @"^(((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?))$";
            string inputStrIp4 = "192.168.1.4";
            (inputStrIp4 + " 是否为合法的IP4地址:" + Regex.IsMatch(inputStrIp4, regexStrIp4)).Log();
        }

        [Test]
        public void MatchZeroWidthAssertion()
        {
            // 零宽断言
            Regex reg = new Regex(@"A(\w+)A"); // (exp)匹配exp
            reg.Match("dsA123A").Log();
            reg.Match("dsA123A").Groups[1].Log(); // 捕获文本到自命名的数值里

            // (?<name>exp) 匹配exp, 并捕获文本到名称为name的数组里
            Regex reg2 = new Regex(@"A(?<num>\w+)A");
            reg2.Match("dsA123A").Groups["num"].Log(); // 123
            Regex reg3 = new Regex(@"A(?:\w+A)");
            reg3.Match("dsA123A").Log();

            Regex reg4 = new Regex(@"sing(?=ing)"); // 在sing的后面会有ing, 如果sing后你那紧跟ing, 那么这个sing才匹配成功
            reg4.Match("ksingkksingingkkkk").Log(); // single
            reg4.Match("singddddsingsingd").Index.Log(); // 0

            Regex reg5 = new Regex(@"(?<=wo)man"); // (?<exp) 匹配exp后面的位置
            reg5.Match("Hi man Hi woman").Log(); // man
            reg5.Match("Hi man Hi woman").Index.Log(); // 12

            Regex reg6 = new Regex(@"sing(?!ing)"); // (?!exp) 匹配后面跟的不是exp的位置
            reg6.Match("singing-singabc").Log(); //sing
            reg6.Match("singing-singabc").Index.Log(); // 8

            Regex reg7 = new Regex(@"(?<!wo)man"); // (?!exp) 匹配前面不是exp的位置
            reg7.Match("Hi woman Hi man").Log(); // man
            reg7.Match("Hi woman Hi man").Index.Log(); // 12

            Regex reg8 = new Regex("ABC(?#这是一段注释。)DEF");
            reg8.Match("ABCDEF").Log();
        }
    }
}
