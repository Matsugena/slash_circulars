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
            Debug.Log ("mds1 " + GetMouseEvent ().mousePosition.ToString ());
            mouse.Publish (GetMouseEvent ());
        });

        mouseDrag
            .Zip (mouseUp,
                (down, up) => {
                    CreateObject (Vector3.zero, down.mousePosition);
                    CreateObject (Vector3.zero, up.mousePosition);
                    Debug.Log ("mds2 " + down.mousePosition.ToString ());
                    Debug.Log ("mup " + up.mousePosition.ToString ());
                    return (down, up);
                })
            .Subscribe ();

    }
    // クリックされたマウス座標をメインカメラからみて適切な位置へ変換する
    // 今回は 2d として扱うので z = 5
    private MouseEvent GetMouseEvent () {
        var p = new Vector3 (
            Input.mousePosition.x,
            Input.mousePosition.y,
            5
        );

        return new MouseEvent {
            mousePosition = mainCamera.ScreenToWorldPoint (p), time = Time.time
        };
    }

    //  テスト用
    private void CreateObject (Vector3 from, Vector3 end) {
        var cube = GameObject.CreatePrimitive (
            PrimitiveType.Cube
        );
        // 2点の中心に移動
        cube.transform.position = (from + end) * 0.5f;
        // from の方向に物体を向ける
        cube.transform.forward = from;
        // サイズを拡大する
    }
}