using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

abstract public class MouseEventStream : MonoBehaviour {

    public int mouseButton { get; } = 0;

    private protected IObservable<MouseEvent> mouseDown { get; set; }
    private protected IObservable<MouseEvent> mouseUp { get; set; }
    private protected IObservable<MouseEvent> mouseDrag { get; set; }

    private IDisposable dragDisposable;

    private void Start() {

        // Stream の作成
        mouseDown = this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(mouseButton))
            .Select(_ => { return GetMouseEvent(); });

        mouseUp = this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonUp(mouseButton))
            .Select(_ => { return GetMouseEvent(); });

        // ドラッグ中マウス座標をストリームし続ける
        mouseDrag = this.UpdateAsObservable()
            .SkipUntil(mouseDown)
            .TakeUntil(mouseUp)
            .Repeat()
            .Select(_ => { return GetMouseEvent(); });

        mouseDown.Subscribe(mDw => DoMouseDown(mDw)).AddTo(this);
        mouseUp.Subscribe(mUp => DoMouseUp(mUp)).AddTo(this);
        dragDisposable = mouseDrag.Subscribe(mDr => DoMouseDragg(mDr));

    }
    abstract protected MouseEvent GetMouseEvent();
    abstract protected void DoMouseDown(MouseEvent ev);
    abstract protected void DoMouseUp(MouseEvent ev);
    abstract protected void DoMouseDragg(MouseEvent ev);

    private void OnDestroy() {
        dragDisposable?.Dispose();
    }
}