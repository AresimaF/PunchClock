using PersonalPunchClock.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPunchClock.Events
{
    public class ClockEventArgs : EventArgs
    {
        public string ID { get; set; }
        public bool Activate { get; set; }
    }

    public class ClockEvents
    {
        public event EventHandler<ClockEventArgs> ClockEvent;

        public void RaiseClockEvent(ClockEventArgs e)
        {
            ClockEvent?.Invoke(this, e);
        }
    }

    public class KillEventArgs : EventArgs
    {
        public string ID { get; set; }
    }

    public class KillEvents
    {
        public event EventHandler<KillEventArgs> KillEvent;

        public void RaiseKillEvent(KillEventArgs e)
        {
            KillEvent?.Invoke(this, e);
        }
    }

    public class ButtonEventArgs : EventArgs
    {
        public string ID { get; set; }
    }

    public class ButtonEvents
    {
        public event EventHandler<ButtonEventArgs> ButtonEvent;

        public void RaiseButtonEvent(ButtonEventArgs e)
        {
            ButtonEvent?.Invoke(this, e);
        }
    }

}

