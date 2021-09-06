using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// Mouse操作のタイミングを通知するManager
/// </summary>
public class MouseEventManager : SingletonMonoBehaviour<MouseEventManager> {
    public IObservable<long> OnClickedSubject = Observable.EveryUpdate ().Where (_ => Input.GetMouseButtonDown (0));
    public IObservable<long> IsClickedSubject = Observable.EveryUpdate ().Where (_ => Input.GetMouseButton (0));
}