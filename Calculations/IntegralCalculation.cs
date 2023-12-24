using System.Diagnostics;

namespace Calculations
{
    public class IntegralCalculation
    {
        public static Semaphore semaphore = new Semaphore(2, 2);
        public delegate double Function(double x);
        private Function function;

        public delegate void NotifyDelegate(double x);
        public event NotifyDelegate? CompletionPercentEvent;
        public event NotifyDelegate? EndEvent;
        public event NotifyDelegate? ElapsedTimeEvent;
        public double A { get; set; }
        public double B { get; set; }

        public double Step { get; set; }

        public void Integrate()
        {
            semaphore.WaitOne();
            double result = 0;
            var cw = Stopwatch.StartNew();
            double completionPercent = 0;
            for (double i = A; i <= B; i += Step)
            {
                result += function(i) * Step;
                if (completionPercent + 0.1 < (i - A) / (B - A) * 100)
                {
                    completionPercent = (i - A) / (B - A) * 100;
                    CompletionPercentEvent?.Invoke(completionPercent);
                }
                
            }
            cw.Stop();
            ElapsedTimeEvent?.Invoke(cw.ElapsedMilliseconds);
            EndEvent?.Invoke(result);
            semaphore.Release();
        }

        public IntegralCalculation()
        {
            this.function = Math.Sin;
            A = 0;
            B = 1;
            Step = 0.00000001;
        }

        public void SetFunction(Function function)
        {
            this.function = function;
        }
    }
}