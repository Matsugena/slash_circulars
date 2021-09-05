using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private float countAfterMaxScale = 0;

    [SerializeField]
    private float restDestroiSecond = 10.0f;
    [SerializeField]
    private float tickScaleUp = 0.2f;
    [SerializeField]
    private float maxScaleUp = 5;

    // 出現後最大サイズまで膨らむ
    // 最大サイズまで膨らむとカウントダウンを開始する
    // デストロイする
    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

        if (this.transform.localScale.x > maxScaleUp) {
            countDown ();
        } else {
            this.transform.localScale = calcScale ();
        }

        if (countAfterMaxScale > restDestroiSecond) {
            Debug.Log ("destroyed!!");
        }
    }

    private Vector3 calcScale () {
        var scale = this.transform.localScale.x + tickScaleUp * Time.deltaTime;
        if (scale > maxScaleUp) scale = maxScaleUp;

        return new Vector3 (
            scale, scale, this.transform.localScale.z
        );

    }

    private void countDown () {
        countAfterMaxScale += Time.deltaTime;
    }
}