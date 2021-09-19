using MessagePipe;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

public class MouseEventManager : MonoBehaviour {

    [Inject] private IPublisher<MouseEvent> mouse { get; set; }

    [Inject] private IPublisher<DragEvent> drag { get; set; }

    private System.IObservable<MouseEvent> mouseDownStream { get; set; }
    private System.IObservable<MouseEvent> mouseUpStream { get; set; }

    private Camera mainCamera;

    private void Start () {

        mainCamera = Camera.main;

        mouseDownStream =
            this.UpdateAsObservable ()
            .Where (_ => Input.GetMouseButtonDown (0))
            .Select (_ => { return GetMouseEvent (); });

        mouseDownStream.Subscribe (_ => {
            mouse.Publish (GetMouseEvent ());
        });

        mouseUpStream =
            this.UpdateAsObservable ()
            .Where (_ => Input.GetMouseButtonUp (0))
            .Select (_ => { return GetMouseEvent (); });

        mouseDownStream
            .Zip (mouseUpStream,
                (down, up) => {
                    CreateObject (Vector3.zero, down.mousePosition);
                    CreateObject (Vector3.zero, up.mousePosition);
                    return (down, up);
                })
            .Subscribe (a => {
                var up = a.up;
                var down = a.down;
                var dt = up.time - down.time; // 時間
                var dm = Vector3.SqrMagnitude (up.mousePosition - down.mousePosition); //距離
                // TODO クリックして待機した状態から、フリックした場合も処理を行いたい
                // フリックした基準タイムから動作させたい
                // CreateObject (down.mousePosition, up.mousePosition);
            });

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
            mousePosition = mainCamera.ScreenToWorldPoint (p)
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