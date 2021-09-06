using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// カメラからのクリックされたオブジェクト上の座標を保持する
/// </summary>

public class ObjectClickManager : SingletonMonoBehaviour<ObjectClickManager> {

    [SerializeField] private Camera mCamera = null;
    private Vector3 currentPosition = Vector3.zero;
    private Subject<Vector3> clickedPosition = new Subject<Vector3> ();

    // 購読
    public IObservable<Vector3> OnClickedPosition {
        get { return clickedPosition; }
    }
    // ray のフィルター
    // [SerializeField] LayerMask layerMask;
    [SerializeField] List<String> tagMask;

    // Start is called before the first frame update
    void Start () {
        // MouseEventManagerの購読を行う
        MouseEventManager.Instance.OnClickedSubject.Subscribe (_ => {
            // UpdateCurrentPositionFromClickePosition ();
            // GetRayHitPointFromCamera ();
            // clickedPosition.OnNext (currentPosition);
        });
        MouseEventManager.Instance.IsClickedSubject.Subscribe (_ => {
            GetRayHitPointFromCamera ();
            clickedPosition.OnNext (currentPosition);
        });
        // 初期化チェック
        if (mCamera == null) {
            Debug.Log ("カメラがアタッチされていません@ObjectClickManager");
        }
    }

    /// <summary>
    /// デバッグ用
    /// </summary>
    private void OnDrawGizmos () {
        if (currentPosition != Vector3.zero) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube (currentPosition, new Vector3 (1.0f, 1.0f, 1.0f));
        }
    }

    /// <summary>
    /// mCameraから見たクリックした座標の位置を返す
    /// </summary>
    private void UpdateCurrentPositionFromClickePosition () {
        var ray = mCamera.ScreenPointToRay (Input.mousePosition);
        var hitlist = Physics.RaycastAll (ray).ToList ();

        var hitFromTagMask = hitlist.FindAll (item => tagMask.Contains (item.transform.tag));

        if (hitFromTagMask.Count > 0) {
            var distance = Vector3.Distance (mCamera.transform.position, hitFromTagMask.First ().point);
            var mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance);

            currentPosition = mCamera.ScreenToWorldPoint (mousePosition);
        }

    }

    private void GetRayHitPointFromCamera () {
        Ray ray = mCamera.ScreenPointToRay (Input.mousePosition);
        List<RaycastHit> hits = Physics.RaycastAll (ray)
            .ToList ()
            .FindAll (item =>
                tagMask.Contains (item.collider.tag)
            );

        if (hits.Count == 0) return;

        var distance = Vector3.Distance (
            mCamera.transform.position, hits.First ().point);
        var mousePosition = new Vector3 (
            Input.mousePosition.x, Input.mousePosition.y, distance);

        currentPosition = hits.First ().point;

        Debug.DrawRay (ray.origin, ray.direction * 1000f); // debug
    }

}