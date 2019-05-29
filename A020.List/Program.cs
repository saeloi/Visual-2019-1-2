using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//교재 p.197 리스트 클래스 참고해서 읽어보기

namespace A020.List
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> a = new List<int>();
            a.Add(3);
            a.Add(13);
            a.Add(32);
            a.Add(24);
            a.Add(12);
            a.Add(5);
            a.Add(87);
            a.Add(54);
            a.Add(6);
            a.Add(90);
            foreach (var item in a)
            {
                Console.WriteLine(item);
            }
            a.Sort();
            foreach (var item in a)
            {
                Console.WriteLine(item);
            }
        }
    }
}
