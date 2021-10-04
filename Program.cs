using System;

namespace Polynomial
{
    class Program
    {
        static void Main()
        {
            Polynomial p = new Polynomial("3 + 4.78963*x^1 + 5.159*x^2 - 3*x^4 + -15*x^8");
            Console.WriteLine(p);
            Console.WriteLine(p - new Polynomial("4.78963*x^1"));
            Console.WriteLine(new Polynomial("1 + 2x^1") * new Polynomial("1 + 2x^1"));
            Console.Read();
        }
    }
}
