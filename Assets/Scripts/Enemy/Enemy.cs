using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

public class Enemy : MonoBehaviour {

    [Inject] private ISubscriber<AttackData> atackSub { get; set; }

    [SerializeField] private int hitPoint = 10;


    void Start() {
        // move.Subscribe (mv => { Move (mv); });
        // jump.Subscribe (j => { Jump (j); });
        atackSub.AsObservable()
        .Where(d => { return d.objectId == this.gameObject.GetInstanceID(); })
        .Subscribe(d => {
            applyDamage(d);
        }).AddTo(this);

        this.UpdateAsObservable()
        .Where(_ => { return this.hitPoint <= 0; })
        .Subscribe(_ => {
            Destroy(this.gameObject);
        }).AddTo(this);

        this.tag = "Enemy";
    }



    private void Move(Move m) {
        transform.Translate(m.axisDx * Time.deltaTime * 1.2f, 0, 0);
    }
    private void Jump(Jump j) {
        //rBody.AddForce(new Vector3(0, 10000, 0));
    }
    private void applyDamage(AttackData data) {
        this.hitPoint -= data.Damage;
    }
}