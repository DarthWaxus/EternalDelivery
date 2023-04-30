using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventManager
{
    public static UnityEvent<Box, Copter> BoxCaptured = new UnityEvent<Box, Copter>();
    public static void SendBoxCaptured(Box box, Copter copter) { BoxCaptured.Invoke(box, copter); }
    public static UnityEvent<Box> BoxRemoved = new UnityEvent<Box>();
    public static void SendBoxRemoved(Box box) { BoxRemoved.Invoke(box); }

    public static UnityEvent<Box> BoxInBot = new UnityEvent<Box>();
    public static void SendBoxInBot(Box box) { BoxInBot.Invoke(box); }

    public static UnityEvent<Copter> CopterRemoved = new UnityEvent<Copter>();
    public static void SendCopterRemoved(Copter copter) { CopterRemoved.Invoke(copter); }

}