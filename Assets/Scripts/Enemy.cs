using System;
using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class Enemy : MonoBehaviour {

    private int _HitPoint = 3;
    [Inject] private ISubscriber<DamageData> _damageData { get; set; }
    private IDisposable disposable;

    // Update is called once per frame
    void Update () {
        if (_HitPoint <= 0) {
            Destroy (this.gameObject);
        }
    }
    private void Start () {

        var bag = DisposableBag.CreateBuilder ();

        _damageData.Subscribe (damageData => {
            this.supplyDamage (damageData);
        }).AddTo (bag);

        disposable = bag.Build ();
    }

    private void supplyDamage (DamageData d) {
        Debug.Log (d.damage);
        _HitPoint -= d.damage;
    }

    private void OnDestroy () {
        disposable?.Dispose ();
    }

}