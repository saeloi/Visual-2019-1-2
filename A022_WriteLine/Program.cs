using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A022_WriteLine
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 10, y = 20;

            Console.WriteLine(x.ToString() + ", " + y); //문자열을 출력한다

            //printf("%d %d", x, y); 
            Console.WriteLine("{0}, {1}", x, y); //0번째, 1번째 나오는 출력값
            Console.WriteLine($"{x}, {y}");

            string s = string.Format($"{x}, {y}"); //해당 문자열이 s로 들어감
            Console.WriteLine(s);

            string t = string.Format("{0}, {1}", x, y); // 이 방법이 가장 일반적
            Console.WriteLine(t);
        }
    }
}
