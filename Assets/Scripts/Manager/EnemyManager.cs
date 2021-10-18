using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using VContainer;
using VContainer.Unity;
using System;

public class EnemyManager : MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    // Start is called before the first frame update

    [Inject] private IObjectResolver container { get; set; }

    private Camera _camera;
    private float z = -0.1f;// 適当なところから代入管理する

    [SerializeField] private int popProp;
    private int popPropInitial = 5;
    private float popUpwardRate = 1.2f; // ポップレートのフレーム上昇率
    private int popMaxNum = 50;// ポップアップする敵の上限

    [SerializeField] int p;

    void Start() {
        _camera = Camera.main;

        popProp = popPropInitial;

        this.UpdateAsObservable()
        .Subscribe(_ => {
            // 敵の出現確率
            var rnd = UnityEngine.Random.Range(0, 100);
            if (popProp > rnd) {
                popEnemy();
                popProp = popPropInitial;
            } else {
                popProp = (int)(popProp * popUpwardRate); //確率上昇
            }

        }).AddTo(this);
        popEnemy();
    }

    // Update is called once per frame
    void Update() {

        // フレームごとに敵の出現を制御する

    }

    private void popEnemy() {
        if (enemyPrefab == null) {
            throw new ArgumentNullException("ObjectManager needs enemyPrefab.");
        }
        var min = _camera.ViewportToWorldPoint(new Vector3(0, 0, -20 - z));
        var max = _camera.ViewportToWorldPoint(new Vector3(1, 1, -20 - z));

        var x = UnityEngine.Random.Range(min.x, max.x);
        var y = UnityEngine.Random.Range(min.y, max.y);

        var enemy = container.Instantiate(enemyPrefab);
        enemy.transform.position = new Vector3(x, y, z);

    }
}
