using System;

namespace Polynomial
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine(new Polynomial("3 + 4.78963*x^1 + 5.159*x^2 - 3*x^4 + -15*x^8"));
            Console.Read();
        }
    }
}
