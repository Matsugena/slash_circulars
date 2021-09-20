using System;
using MessagePipe;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

public class MouseEventManager : MonoBehaviour {

    [Inject] private IPublisher<MouseEvent> mouse { get; set; }

    [Inject] private IPublisher<DragEvent> drag { get; set; }

    private IObservable<MouseEvent> mouseDown { get; set; }
    private IObservable<MouseEvent> mouseUp { get; set; }
    private IObservable<MouseEvent> mouseDrag { get; set; }

    private Camera mainCamera;
    private IDisposable dragDisposable;

    private void Start () {

        mainCamera = Camera.main;

        // Stream の作成
        mouseDown = this.UpdateAsObservable ()
            .Where (_ => Input.GetMouseButtonDown (0))
            .Select (_ => { return GetMouseEvent (); });

        mouseUp = this.UpdateAsObservable ()
            .Where (_ => Input.GetMouseButtonUp (0))
            .Select (_ => { return GetMouseEvent (); });

        // ドラッグ中マウス座標をストリームし続ける
        mouseDrag = this.UpdateAsObservable ()
            .SkipUntil (mouseDown)
            .TakeUntil (mouseUp)
            .Repeat ()
            .Select (_ => { return GetMouseEvent (); });

        // SubScribe
        mouseDown.Subscribe (_ => {
            mouse.Publish (GetMouseEvent ());
        }).AddTo (this);

        dragDisposable = mouseDrag.Subscribe (d => {
            drag.Publish (new DragEvent {
                position = d.position, time = d.time
            });
        });

    }
    // クリックされたマウス座標をメインカメラからみて適切な位置へ変換する
    // 今回は 2d として扱うので z = 5
    private MouseEvent GetMouseEvent () {
        var p = new Vector3 (
            Input.mousePosition.x,
            Input.mousePosition.y,
            12
        );

        return new MouseEvent {
            position = mainCamera.ScreenToWorldPoint (p), time = Time.time
        };
    }

    private void OnDestroy () {
        dragDisposable?.Dispose ();
    }
}