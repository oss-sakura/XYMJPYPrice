using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace XYMJPYPrice.Common
{
    public class IntervalTimer
    {
        private Timer _intervalTimer = null;
        private int _interval;

        public ConcurrentDictionary<string, bool> OverIntervalTime = new ConcurrentDictionary<string, bool>();

        public IntervalTimer(int interval)
        {
            Init(interval);
        }

        ~IntervalTimer()
        {
            Stop();
            Dispose();
        }

        private void Init(int interval)
        {
            _interval = interval;
            CreateTimer(interval);
        }

        private void CreateTimer(int interval)
        {
            _intervalTimer = new Timer(interval);
            _intervalTimer.Elapsed += (sender, e) =>
            {
                if (OverIntervalTime != null && OverIntervalTime.Count != 0)
                {
                    foreach (KeyValuePair<string, bool> item in OverIntervalTime)
                    {
                        if (!item.Value) { OverIntervalTime[item.Key] = true; }
                    }
                }
            };
        }

        public void ChangeIntervalTimer(int interval)
        {
            if (_interval != interval)
            {
                Stop();
                Dispose();

                _interval = interval;
                CreateTimer(_interval);
                Start();
            }
        }

        public void Add(string name, bool condition = true)
        {
            _ = OverIntervalTime.TryAdd(name, condition);
        }

        public void Reset(string name)
        {
            OverIntervalTime[name] = false;
        }

        public void Start()
        {
            if (_intervalTimer != null) { _intervalTimer.Start(); }
        }

        public void Stop()
        {
            if (_intervalTimer != null) { _intervalTimer.Stop(); }
        }

        public void Dispose()
        {
            if (_intervalTimer != null) { _intervalTimer.Dispose(); }
        }
    }

}
