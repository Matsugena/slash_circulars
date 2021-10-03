using System;
using MessagePipe;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

public class MouseEventManager : MouseEventStream {
    [Inject] private IPublisher<MouseEvent> mouseDownPub { get; set; }

    [Inject] private IPublisher<MouseUpEvent> mouseUpPub { get; set; }

    [Inject] private IPublisher<DragEvent> dragPub { get; set; }

    private Camera mainCamera;

    private void Start() {

        mainCamera = Camera.main;

        // Stream の作成
        mouseDown = this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => { return GetMouseEvent(); });

        mouseUp = this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonUp(0))
            .Select(_ => { return GetMouseEvent(); });

        // ドラッグ中マウス座標をストリームし続ける
        mouseDrag = this.UpdateAsObservable()
            .SkipUntil(mouseDown)
            .TakeUntil(mouseUp)
            .Repeat()
            .Select(_ => { return GetMouseEvent(); });

    }


    protected override MouseEvent GetMouseEvent() {
        var p = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            12
        );

        return new MouseEvent {
            position = mainCamera.ScreenToWorldPoint(p), time = Time.time
        };
    }

    protected override void DoMouseDown(MouseEvent ev) {
        mouseDownPub.Publish(ev);
    }

    protected override void DoMouseUp(MouseEvent ev) {
        mouseUpPub.Publish(new MouseUpEvent(ev));
    }


    protected override void DoMouseDragg(MouseEvent ev) {
        dragPub.Publish(new DragEvent(ev));
    }
}