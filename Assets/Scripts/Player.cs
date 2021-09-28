using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour {

    [Inject] private ISubscriber<Jump> jump { get; set; }

    [Inject] private ISubscriber<Move> move { get; set; }

    [Inject] private ISubscriber<DragEvent> dragSub { get; set; }

    [Inject] private ISubscriber<MouseEvent> mouseDown { get; set; }

    [Inject] private ISubscriber<MouseUpEvent> mouseUp { get; set; }

    private MouseUpEvent mUpEvent;
    private MouseEvent mEvent;
    private float flickDeltaTime = 16 * 20; // フリック受付時間
    private float flickForceMagnitude = 20; // 計算を省略
    private bool isFlicked = false;

    [SerializeField] private Rigidbody rBody;

    private ReactiveProperty<DragEvent> drag = new ReactiveProperty<DragEvent> (new DragEvent ());
    private ReactiveProperty<bool> isMoving = new ReactiveProperty<bool> (false); // 動作中の場合、true
    private Material material; // デバッグ用 このオブジェクトのマテリアル

    private void Awake () {
        material = GetComponent<Renderer> ().material;
        rBody = GetComponent<Rigidbody> ();
    }

    // Start is called before the first frame update
    void Start () {
        move.Subscribe (mv => { Move (mv); });
        jump.Subscribe (j => { Jump (j); });
        dragSub.Subscribe (d => {
            if (!isFlicked) {
                this.transform.position = d.position;
                drag.Value = d;
            }
        });
        mouseUp.Subscribe (v => {
            mUpEvent = v;
            Flick ();
        }).AddTo (this);

        mouseDown.Subscribe (v => {
            mEvent = v;
        }).AddTo (this);

        // ReactiveProperty から平均速度を算出する
        drag.Where (x => x != null)
            .Buffer (15).Where (vero => vero.Count >= 2)
            .Select (d => {
                var a = d.First ();
                var b = d.Last ();
                var dt = b.time - a.time;
                var verocity = (b.position - a.position) / dt;
                return verocity;
            })
            .Subscribe (a => {
                // 平均速度が閾値を超えている場合、動いている判定
                isMoving.Value = (a.magnitude > 0.5);
                // 
            })
            .AddTo (this);

        isMoving.Subscribe (x => {
            if (x) material.color = Color.red;
            else material.color = Color.white;
        });
    }
    // MouseUp と MouseDown の値を利用してフリック入力的な AddForce を加える
    private void Flick () {
        if (mUpEvent == null || mEvent == null) return;
        // 入力時間閾値
        if (mUpEvent.time - mEvent.time > flickDeltaTime) return;

        // 正規化ベクトル抽出(大きさが1)
        var v = (mUpEvent.position - mEvent.position).normalized;

        // 単純にAddForce
        rBody.AddForce (v * flickForceMagnitude, ForceMode.Impulse);
    }

    private void Move (Move m) {
        transform.Translate (m.axisDx * Time.deltaTime * 1.2f, 0, 0);
    }
    private void Jump (Jump j) {
        rBody.AddForce (new Vector3 (0, 10000, 0));
    }
}