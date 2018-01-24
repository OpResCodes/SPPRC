using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation
{
    public class Timekeeper
    {
        public TimeSpan TotalTimeSinceReset { get; private set; }

        public Timekeeper()
        {
            ResetTotalTime();
        }

        private TimeSpan MeasureAndChoose(Action operation, bool warmUp)
        {
            if (warmUp)
            {
                operation();
            }

            var watch = new Stopwatch();
            watch.Start();
            operation();
            watch.Stop();
            TotalTimeSinceReset = TotalTimeSinceReset.Add(watch.Elapsed);
            return watch.Elapsed;
        }

        public TimeSpan Measure(Action operation)
        {
            return MeasureAndChoose(operation, false);
        }

        public TimeSpan MeasureWithWarmUp(Action operation)
        {
            return MeasureAndChoose(operation, true);
        }

        public void ResetTotalTime()
        {
            TotalTimeSinceReset = new TimeSpan();
        }

    }
}
