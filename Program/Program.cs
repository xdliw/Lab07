using Calculations;


IntegralCalculation calc = new IntegralCalculation();
calc.Step = 0.00000001;
calc.CompletionPercentEvent += (d) =>
{
    int id = Thread.CurrentThread.ManagedThreadId;
    int progBarCells = (int)(d + 0.01) / 5;
    lock (Console.Out)
    {
        Console.SetCursorPosition(0, id);
        Console.Write($"Stream {id}: [{"=".Repeat(progBarCells)}{">".Repeat((progBarCells < 20) ? 1 : 0)}{" ".Repeat(19 - progBarCells)}]{d:F1}%");
    }
};

calc.EndEvent += (result) =>
{
    int id = Thread.CurrentThread.ManagedThreadId;
    lock (Console.Out)
    {
        Console.SetCursorPosition(0, id);
        Console.Write($"Stream {id}: Ended with result: {result}");
    }
};

calc.ElapsedTimeEvent += (time) =>
{
    int id = Thread.CurrentThread.ManagedThreadId;
    lock (Console.Out)
    {
        Console.SetCursorPosition(60, id);
        Console.Write($"Consumed time: {time}ms");
        if (Thread.CurrentThread.Priority == ThreadPriority.Lowest)
        {
            Console.Write(" Low priority");
        }
        if (Thread.CurrentThread.Priority == ThreadPriority.Highest)
        {
            Console.Write(" High priority");
        }
    }
};

Thread thread1 = new Thread(calc.Integrate);
thread1.Priority = ThreadPriority.Highest;
thread1.Start();

Thread thread2 = new Thread(calc.Integrate);
thread2.Priority = ThreadPriority.Lowest;
thread2.Start();

Thread thread3 = new Thread(calc.Integrate);
thread3.Start();

Thread thread4 = new Thread(calc.Integrate);
thread4.Start();

Thread thread5 = new Thread(calc.Integrate);
thread5.Start();



public static class StringExtensions
{
    public static string Repeat(this string value, int count)
    {
        count = Math.Max(0, count);
        return string.Concat(Enumerable.Repeat(value, count));
    }
}