using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Util
{
    public static IEnumerator Delay(float time, Action onCompleted)
    {
        yield return new WaitForSeconds(time);
        onCompleted?.Invoke();
    }
    
    public static IEnumerator DelayOneFrame(Action onCompleted)
    {
        yield return new WaitForEndOfFrame();
        onCompleted?.Invoke();
    }
}
