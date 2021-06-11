using System;
using System.Threading;

namespace Integral
{
    class Program
    {
        public delegate double Function(double x);

        static void Main(string[] args)

        {
            double startPoint = 1;

            double finalpoint = 2;

            var integral = new Calculation(f, startPoint, finalpoint, 0.001);

            integral.StartCalculate();

            Thread.Sleep(300);

            Console.WriteLine($"Результат: {integral.result}");

            int n = 1000;

            double[] y = new double[n];

            double h = (finalpoint - startPoint) / (n - 1);

            for (int i = 0; i < n; i++)

            {
                double x = i * h;

                y[i] = (5 * Math.Pow(x, 4)) + (2 * Math.Sqrt(x)) - (4 / x);
            }

            var result = Trapezoidal(f, startPoint, finalpoint, n);

            Console.WriteLine("Результат: {0}", result);

            Console.ReadKey();
        }

        static double f(double x)

        {
            return (5 * Math.Pow(x, 4)) + (2 * Math.Sqrt(x)) - (4 / x);
        }

        public static double Trapezoidal(Function f, double a, double b, int n)

        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            double sum = 0;

            double h = (b - a) / n;

            for (int i = 0; i < n; i++)

            {
                sum += 0.5 * h * (f(a + i * h) + f(a + (i + 1) * h));
            }

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("Время в однопоточном режиме: " + Convert.ToSingle(elapsedMs));

            return sum;
        }
    }
}