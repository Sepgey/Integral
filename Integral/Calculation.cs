using System;
using System.Threading;

namespace Integral
{
    public class Calculation
    {
        private Func<double, double> func;

        private double _leftPoint;

        private double _rightPoint;

        private double _eps;

        private Semaphore _semaphore;

        public double result { get; set; }
        
        private volatile uint _started;
        private volatile uint _finished;


        public bool HasFinished => _started == _finished;
        public Calculation(Func<double, double> function, double startPoint, double finalPoint, double e)

        {

            func = function;

            _semaphore = new Semaphore(4, 4);

            _leftPoint = startPoint;

            _rightPoint = finalPoint;

            _eps = e;

        }

        private void Calculate(object args)

        {

            _semaphore.WaitOne();

            var left = (double)((object[])args)[0];

            var right = (double)((object[])args)[1];

            var length = right - left;

            var h = length / 2;

            var middle = left + h;

            if (length < _eps)

            {

                result += func(middle) * length;

            }

            else

            {

                var threadLeft = new Thread(Calculate);

                threadLeft.Start(new object[] { left, middle });

                var threadRight = new Thread(Calculate);

                threadRight.Start(new object[] { middle, right });

            }

            _semaphore.Release();

        }

        public void StartCalculate()

        {

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var thread = new Thread(Calculate);

            thread.Start(new object[] { _leftPoint, _rightPoint });

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds; 
            Console.WriteLine($"Время в многопоточном режиме: {watch.ElapsedMilliseconds}");

        }
    }
}