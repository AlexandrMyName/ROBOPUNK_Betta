using Abstracts;
using System;
using UnityEngine;

public class TimerPool : MonoBehaviour
{
    public ITimer CreateCallBackTimer(string name)
    {
        GameObject timerObject = new(name);
        timerObject.transform.SetParent(this.transform, false);
        ITimer timer = timerObject.AddComponent<CallBackTimer>();
        return timer;
    }
    public void RunTimer(ITimer timer,float seconds, Action callback)
    {
        timer.AwaiteFor(seconds, callback);
        
    }

}
