using System;
using System.Globalization;

namespace projekt
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string funkcja;
            double x, xMin, xMax;
            int n;

            // if(args[0]==null || args[1]==null || args[2]==null || args[3]==null || args[4]==null){
            //     Console.WriteLine("nie podano wszystkich argumentów");
            //     Environment.Exit(0);
            // }

            funkcja = args[0];
            x = double.Parse(args[1]);
            xMin = double.Parse(args[2]);
            xMax = double.Parse(args[3]);
            n = int.Parse(args[4]);

            RPN uchwyt = new RPN(funkcja,x,xMin,xMax,n);
            uchwyt.Walidacja();
            uchwyt.Postfix();
            Console.WriteLine(uchwyt.ObliczX());
            uchwyt.ObliczWieleX();
        }
    }
}
