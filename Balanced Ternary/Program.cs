using System;

namespace BalancedTernary
{
    public class Program
    {
        public static void Main()
        {
            BalancedTernary a = new BalancedTernary("+-+0+");
            Console.WriteLine("a: " + a + " = " + a.ToInt());
            BalancedTernary b = new BalancedTernary(-37);
            Console.WriteLine("b: " + b + " = " + b.ToInt());
            BalancedTernary d = a * b;
            Console.WriteLine("a * b: " + d + " = " + d.ToInt());
            Console.ReadKey();
        }
    }
}