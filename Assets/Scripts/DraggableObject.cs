using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

public class DraggableObject : MonoBehaviour {

    [Inject] private ISubscriber<DragEvent> dragSub { get; set; }
    [Inject] private ISubscriber<MouseUpEvent> mUpSub { get; set; }

    private ReactiveProperty<DragEvent> drag = new ReactiveProperty<DragEvent>(new DragEvent());
    private ReactiveProperty<bool> isMoving = new ReactiveProperty<bool>(false); // 動作中の場合、true
    private ReactiveProperty<Vector3> draggingVerocity = new ReactiveProperty<Vector3>(Vector3.zero); // ドラッグ中に得た速度の合計

    private MouseUpEvent mUpEvent;
    private MouseEvent mEvent;
    private Vector3 moveStartPosition;

    [SerializeField] private float thresholdMoving = 1; // TODO オブジェクトのスケールやフレームレートを考慮する必要があります
    [SerializeField] private float thresholdFlick = 50; // TODO 画面サイズに応じて計算する必要があります

    // デバッグ用
    private LineRenderer lineRenderer;
    private Material material;

    private void Awake() {
        material = GetComponent<Renderer>().material;
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start() {

        dragSub.Subscribe(d => {
            this.transform.position = d.position;
            drag.Value = d;
        }).AddTo(this);

        mUpSub.Subscribe(mUp => {
            StopTransform();
        }).AddTo(this);

        // ReactiveProperty(DragEvent) から平均速度を算出する
        drag.Where(x => x != null)
            .Buffer(15).Where(d => d.Count >= 2)
            .Select(d => {
                var a = d.First();
                var b = d.Last();
                var dx = b.position - a.position;
                var dt = b.time - a.time;
                var verocity = dx / dt;
                if (!isMoving.Value) {
                    moveStartPosition = a.position;// 動き出しのポジションをセット
                }
                return dx / dt;
            })
            .Subscribe(v => {
                draggingVerocity.Value = (draggingVerocity.Value + v) / 2;

                // 平均速度が閾値を超えている場合、isMoving
                isMoving.Value = v.magnitude > thresholdMoving;
            }).AddTo(this);

        // 動いている間は マテリアルを赤くする
        isMoving.Subscribe(x => {
            if (x) {
                material.color = Color.red;
            } else {
                material.color = Color.white;
                StopTransform();
            }
        }).AddTo(this);

        draggingVerocity
            .Where(v => v.magnitude > thresholdFlick && v != Vector3.zero)
            .Subscribe(_ => { Flick(); })
            .AddTo(this);
    }


    private void Flick() {
        if (moveStartPosition == Vector3.zero) return;

        var positions = new Vector3[] {
            moveStartPosition,
            drag.Value.position
        };
        lineRenderer.SetPositions(positions);

        moveStartPosition = drag.Value.position;
        draggingVerocity.Value = Vector3.zero;
    }

    private void StopTransform() {
        draggingVerocity.Value = Vector3.zero;
        moveStartPosition = Vector3.zero;
    }
}