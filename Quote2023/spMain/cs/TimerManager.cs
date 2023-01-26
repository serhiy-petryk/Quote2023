using System;
using System.Collections.Generic;
using System.Text;

namespace spMain {
  class csTimerManager {

    public delegate void DoTimerTick();

    static int _timerCnt = 0;// Global timer tick counter
    static int _timerTickIntervalInMiliSec = 1000;
    static int _minTimerTickIntervalInMiliSec = 100;
    static System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
    static Dictionary<object, TimerElement> _register = new Dictionary<object, TimerElement>();

    // Если при вызове в _timer_Tick процедуры _DoTick объект делает UnRegister, 
    //то удаление из словаря приводит к ошибке в цикле foreach(procedure _timer_tick)
    // Для этого введены _flagScan & _toBeRemove
    static bool _flagScan = false;
    static List<object> _toBeRemove = new List<object>();

    static csTimerManager() {
      _timer.Tick += new EventHandler(_timer_Tick);
      _timer.Interval = _timerTickIntervalInMiliSec; // in milisecs
    }

    // ===========================  Static Public Section =============================
    public static void Register(object o, int tickIntervalInMiliSec, DoTimerTick call) {
      lock (_timer) {
        UnRegister(o);
        _register.Add(o, new TimerElement(tickIntervalInMiliSec, call));
        _timer.Enabled = true;
      }
    }

    public static void UnRegister(object o) {
      if (_flagScan) {
        _toBeRemove.Add(o);
      }
      else {
        if (_register.ContainsKey(o)) _register.Remove(o);
        if (_register.Count > 0) _timer.Enabled = true;
      }
    }

    public static void TimerStop(object o) {
      TimerElement elem = _register[o];
      elem.StopFlag=true;
    }

    public static void TimerContinue(object o) {
      TimerElement elem = _register[o];
      elem.StopFlag=false;
    }

    public static int GetTimerCnt() { return _timerCnt; }

    // ========================  Private section ===========================
    static void _timer_Tick(object sender, EventArgs e) {
      _timer.Stop();
      _timerCnt++;
      lock (_timer) {
        _flagScan = true;
        _toBeRemove.Clear();
        foreach (TimerElement elem in _register.Values) {
          elem.DoTick();
        }
        _flagScan = false;
        foreach (object o in _toBeRemove) {
          if (_register.ContainsKey(o)) _register.Remove(o);
        }
        _toBeRemove.Clear();
        TimerRunCheck();
      }
    }

    static void TimerRunCheck() {
      bool stopFlag = true;
      DateTime nextRun = DateTime.MaxValue;
      foreach (TimerElement elem in _register.Values) {
        if (!elem.StopFlag && nextRun > elem.GetNextRunTime()) {
          stopFlag = false;
          nextRun = elem.GetNextRunTime();
        }
      }
      if (!stopFlag) {
        DateTime now = DateTime.Now;
        if (nextRun < now) {
          _timer.Interval = _minTimerTickIntervalInMiliSec;
        }
        else {
          TimeSpan ts = nextRun - now;
          _timer.Interval = Math.Max(_minTimerTickIntervalInMiliSec, Convert.ToInt32(ts.TotalMilliseconds));
        }
        _timer.Enabled = true;
      }
    }

    // =========================== SubClass TimerElement ==============================
    public class TimerElement {

//      public static List<string> _log = new List<string>();
      DateTime _nextRunTime;
      double _tickIntervalInMiliSec;
      DoTimerTick _call;
      bool _stopFlag = false;

      internal TimerElement(int tickIntervalInMiliSec, DoTimerTick call) {
        this._tickIntervalInMiliSec = Convert.ToDouble(tickIntervalInMiliSec); this._call = call;
        this._nextRunTime = DateTime.Now.AddMilliseconds(this._tickIntervalInMiliSec);
      }

      internal DateTime GetNextRunTime() { return this._nextRunTime; }

      internal bool StopFlag {
        get { return this._stopFlag; }
        set {
          this._stopFlag = value;
          csTimerManager.TimerRunCheck();
        }
      }

      internal void DoTick() {// return value: true= stop timer
        if (this._nextRunTime < DateTime.Now && !this._stopFlag) {
          this._call();
          this._nextRunTime = DateTime.Now.AddMilliseconds(this._tickIntervalInMiliSec);
//          _log.Add(DateTime.Now.ToString("HH:mm:ss.ff") + ":" + _tickIntervalInMiliSec.ToString() + ":" + this._call.Target.ToString());
        }
      }
    }


  }
}
