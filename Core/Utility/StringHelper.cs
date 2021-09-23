﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NonsensicalKit.Utility
{
    public static class StringHelper
    {
        #region PublicMethod

        public static string ToLongString(this Vector3 v3)
        {
            return $"({v3.x:f5},{v3.y:f5},{v3.z:f5})";
        }

        public static string TrimBOM(string str)
        {
            return str.Substring(1);
        }
        public static string TrimClone(string str)
        {
            int le = str.Length;
            return str.Substring(0, le - 7);
        }


        /// <summary>
        /// 算式运算(仅支持加减乘除)
        /// </summary>
        /// <param name="_s"></param>
        /// <returns></returns>
        public static double? Calculation(string _s)
        {
            List<string> ls = Incision(_s);

            if (ls == null || ls.Count == 0)
            {
                return null;
            }
            ls = Brackets(ls);
            if ((ls = Exclude(ls)) == null)
            {
                return null;
            }

            ls = Multiplication_And_Division(ls);
            return Addition_And_Subtraction(ls);
        }

        /// <summary>
        /// 获取集合的可读字符串
        /// </summary>
        /// <param name="ienumerable"></param>
        /// <returns></returns>
        public static string GetSetString(IEnumerable ienumerable)
        {
            if (ienumerable==null)
            {
                return "not a set";
            }
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            foreach (var item in ienumerable)
            {
                if (item.GetType() == typeof(Vector3))
                {
                    Vector3 temp = (Vector3)item;
                    sb.Append($"({temp.x},{temp.y},{temp.z})");
                }
                else if (item.GetType() == typeof(Vector4))
                {
                    Vector4 temp = (Vector4)item;
                    sb.Append($"({temp.x},{temp.y},{temp.z},{temp.w})");
                }
                else
                {
                    sb.Append(item.ToString());
                }
                sb.Append(",");
            }
            if (sb[sb.Length - 1] != '[')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// 根据路径字符串获取文件名
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <param name="withSuffix">返回的结果是否携带后缀</param>
        /// <returns></returns>
        public static string GetFileNameByPath(string path, bool withSuffix = false)
        {
            if (path == null)
            {
                return string.Empty;
            }

            string[] fullNameTemp = path.Split(new char[] { '/', '\\' });

            string nameWithSuffix = fullNameTemp[fullNameTemp.Length - 1];

            string filename = nameWithSuffix;

            if (withSuffix == false)
            {
                string[] nameWithSuffixTemp = nameWithSuffix.Split(new char[] { '.' });

                filename = nameWithSuffixTemp[0];
            }

            return filename;
        }

        /// <summary>
        /// 根据路径字符串获取文件夹字符串
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <returns></returns>
        public static string GetDirpathByPath(string path)
        {
            if (path == null)
            {
                return string.Empty;
            }

            string[] fullNameTemp = path.Split(new char[] { '/', '\\' });

            int fileNameLength = fullNameTemp[fullNameTemp.Length - 1].Length;


            return path.Substring(0, path.Length - fileNameLength - 1);
        }

        /// <summary>
        /// 根据长度切分字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static string[] SplitStringByLength(string target, int limit = 10000)
        {
            if (target.Length < limit) return new string[] { target };
            int count = target.Length / limit;
            var array = new string[count + 1];
            for (int i = 0; i < count; i++)
            {
                array[i] = target.Substring(i * limit, limit) + "\n";
            }
            array[count] = target.Substring(count * limit, target.Length % limit);
            return array;
        }

        /// <summary>
        /// 判断字符串数组中是否包含目标字符串
        /// </summary>
        /// <param name="stringArray"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool Contains(this string[] stringArray, string check)
        {
            foreach (var item in stringArray)
            {
                if (item == check)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        ///  将传入的字符串以算数符号为界切开，并放入链表中
        /// </summary>
        /// <param name="_s">要切分的字符串</param>
        /// <returns></returns>
        private static List<string> Incision(string _s)
        {
            List<string> ls = new List<string>();
            bool flag = false;

            while (true)
            {
                for (int i = 0; i < _s.Length; i++)
                {
                    if (_s[i] == '+' || _s[i] == '-' || _s[i] == '*' || _s[i] == '/')
                    {
                        string temp1 = _s[i].ToString();
                        char[] temp2 = { _s[i] };
                        string[] ss = _s.Split(temp2, 2);
                        if (ss[0] != null && ss[0] != "")
                        {
                            ls.Add(ss[0]);
                        }
                        ls.Add(temp1);
                        if (ss[1] == "")
                        {
                            return null;
                        }
                        _s = ss[1];
                        flag = true;
                        break;
                    }
                    else if (_s[i] == '(' || _s[i] == '（')
                    {
                        if (i == 0)
                        {
                            if (!char.IsNumber(_s[i + 1]))
                            {
                                return null;
                            }
                        }
                        else
                        {
                            if ((_s[i - 1] != '+' && _s[i - 1] != '-' && _s[i - 1] != '*' && _s[i - 1] != '/') || !char.IsNumber(_s[i + 1]))
                            {
                                return null;
                            }
                        }

                        ls.Add(_s[i].ToString());
                        _s = _s.Substring(1);
                        flag = true;
                        break;
                    }
                    else if (_s[i] == ')' || _s[i] == '）')
                    {
                        if (i == _s.Length - 1)
                        {
                            if (!char.IsNumber(_s[i - 1]))
                            {
                                return null;
                            }
                        }
                        else
                        {
                            if (!char.IsNumber(_s[i - 1]) || (_s[i + 1] != '+' && _s[i + 1] != '-' && _s[i + 1] != '*' && _s[i + 1] != '/'))
                            {
                                return null;
                            }
                        }

                        string temp1 = _s[i].ToString();
                        char[] temp2 = { _s[i] };
                        string[] ss = _s.Split(temp2, 2);
                        ls.Add(ss[0]);
                        ls.Add(temp1);
                        _s = ss[1];
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    flag = false;
                    continue;
                }
                if (_s != null && _s != "")
                {
                    ls.Add(_s);
                }
                break;
            }
            return ls;
        }

        /// <summary>
        /// 将所有非数字或运算符号的字符排除
        /// </summary>
        /// <param name="_ls"></param>
        /// <returns></returns>
        private static List<string> Exclude(List<string> _ls)
        {
            for (int i = 0; i < _ls.Count; i++)
            {
                if (_ls[i] != "+" && _ls[i] != "-" && _ls[i] != "*" && _ls[i] != "/" && _ls[i] != "(" && _ls[i] != ")" && _ls[i] != "（" && _ls[i] != "）")
                {
                    for (int j = 0; j < _ls[i].Length; j++)
                    {
                        if (!char.IsNumber(_ls[i][j]) && _ls[i][j] != '.')
                        {
                            _ls[i] = _ls[i].Replace(_ls[i][j].ToString(), "");
                            j = -1;
                        }
                    }
                    if (_ls[i] == "")
                    {
                        return null;
                    }
                }
            }

            //Console.Write("输入的式子为：");
            //for (int i = 0; i < _ls.Count; i++)
            //{
            //    Console.Write(_ls[i]);
            //}
            //Console.WriteLine();

            return _ls;
        }

        /// <summary>
        /// 括弧运算
        /// </summary>
        /// <param name="_ls"></param>
        /// <returns></returns>
        private static List<string> Brackets(List<string> _ls)
        {
            for (int i = 0; i < _ls.Count; i++)
            {
                if (_ls[i] == "(" || _ls[i] == "（")
                {
                    int count = 1;
                    for (int j = i + 1; j < _ls.Count; j++)
                    {
                        if (_ls[j] == "(" || _ls[j] == "（")
                        {
                            count++;
                        }
                        else if ((_ls[j] == ")" || _ls[j] == "）"))
                        {
                            count--;
                            if (count == 0)
                            {
                                _ls[i] = Calculation(string.Join("", _ls.GetRange(i + 1, j - i - 1))).ToString();
                                _ls.RemoveRange(i + 1, j - i);
                                break;
                            }

                        }
                    }
                }
            }
            return _ls;
        }

        /// <summary>
        /// 进行乘除运算
        /// </summary>
        /// <param name="_ls"></param>
        /// <returns></returns>
        private static List<string> Multiplication_And_Division(List<string> _ls)
        {
            int i = 0;
            bool flag = false;
            double d = 0;

            while (true)
            {
                for (; i < _ls.Count; i++)
                {
                    if (_ls[i] == "*")
                    {
                        d = double.Parse(_ls[i - 1]) * double.Parse(_ls[i + 1]);
                        flag = true;
                        break;
                    }
                    else if (_ls[i] == "/")
                    {
                        d = double.Parse(_ls[i - 1]) / double.Parse(_ls[i + 1]);
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    _ls[i - 1] = d.ToString();
                    _ls.RemoveRange(i, 2);
                    flag = false;
                }
                else
                {
                    return _ls;
                }
            }
        }

        /// <summary>
        /// 进行加减运算
        /// </summary>
        /// <param name="_ls"></param>
        /// <returns></returns>
        private static double Addition_And_Subtraction(List<string> _ls)
        {
            double result = double.Parse(_ls[0]);

            for (int i = 1; i < _ls.Count; i += 2)
            {
                if (_ls[i] == "+")
                {
                    result += double.Parse(_ls[i + 1]);
                }
                else if (_ls[i] == "-")
                {
                    result -= double.Parse(_ls[i + 1]);
                }
            }
            return result;
        }
        #endregion
    }
}