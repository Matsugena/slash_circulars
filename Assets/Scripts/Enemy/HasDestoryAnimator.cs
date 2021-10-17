
using DG.Tweening;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

// Enemyオブジェクトの破壊時のアニメーションを定義します

public class HasDestoryAnimator : MonoBehaviour {
    private bool _isRunning = false;
    private Vector3 _rotateAxis = new Vector3(0, 0, 1);
    private float _distance = 10;
    private float _duration = 0.8f;

    public void DoAnimation() {
        if (!_isRunning) {
            this.transform.DOMove(GetRandomVector3(_distance), _duration).OnComplete(() => {
                Destroy(this.gameObject); // どこかしらでも自らをDestroyする例はあまりよろしくない
            });
        }
        _isRunning = true;
    }

    // ランダムに distance分離れた距離の点を計算する
    private Vector3 GetRandomVector3(float distance) {
        var n = UnityEngine.Random.insideUnitCircle; // 同心円上の適当な座標
        return n * distance;
    }
}