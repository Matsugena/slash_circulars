using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour {

    [Inject] private ISubscriber<Jump> jump { get; set; }

    [Inject] private ISubscriber<Move> move { get; set; }

    [Inject] private ISubscriber<DragEvent> drag { get; set; }

    [Inject] private ISubscriber<MouseEvent> mouseDown { get; set; }

    [Inject] private ISubscriber<MouseUpEvent> mouseUp { get; set; }

    private MouseUpEvent mUpEvent;
    private MouseEvent mEvent;
    private float flickDeltaTime = 5000f; // フリック受付時間
    private float flickForceMagnitude = 1000; // 計算を省略
    [SerializeField] private Rigidbody rBody;

    // Start is called before the first frame update
    void Start () {
        move.Subscribe (mv => { Move (mv); });
        jump.Subscribe (j => { Jump (j); });
        drag.Subscribe (d => {
            this.transform.position = d.position;
        });
        mouseUp.Subscribe (v => {
            mUpEvent = v;
        }).AddTo (this);

        mouseDown.Subscribe (v => {
            mEvent = v;
            Flick ();
        }).AddTo (this);

        //
        rBody = GetComponent<Rigidbody> ();
    }
    // MouseUp と MouseDown の値を利用してフリック入力的な AddForce を加える
    private void Flick () {
        if (mUpEvent == null || mEvent == null) return;
        Debug.Log ("1");
        // 入力時間閾値
        if (mUpEvent.time - mEvent.time > flickDeltaTime) return;

        Debug.Log ("2");
        // 正規化ベクトル抽出(大きさが1)
        var v = (mUpEvent.position - mEvent.position).normalized;
        Debug.Log ("3 : " + v);

        // 単純にAddForce
        rBody.AddForce (new Vector3 (0, 10, 0) * flickForceMagnitude, ForceMode.Impulse);
    }

    private void Move (Move m) {
        transform.Translate (m.axisDx * Time.deltaTime * 1.2f, 0, 0);
    }
    private void Jump (Jump j) {
        transform.Translate (0, j.data, 0);
    }
}