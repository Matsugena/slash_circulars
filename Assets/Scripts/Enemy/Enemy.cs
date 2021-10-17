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

    [Inject] private ISubscriber<AttackData> _onAttack { get; set; }
    private HasDestoryAnimator _hasDestroyAnimator;

    [SerializeField] private int hitPoint = 10;

    private ReactiveProperty<bool> hasDestroy = new ReactiveProperty<bool>(false);

    void Start() {
        // move.Subscribe (mv => { Move (mv); });
        // jump.Subscribe (j => { Jump (j); });
        _onAttack.AsObservable()
        .Where(d => { return d.objectId == this.gameObject.GetInstanceID(); })
        .Subscribe(d => {
            applyDamage(d);
        }).AddTo(this);

        hasDestroy.Where(x => x)
        .Subscribe(_ => {
            // Destroy(this.gameObject);
            _hasDestroyAnimator.DoAnimation();
        }).AddTo(this);

        this.tag = "Enemy";
        _hasDestroyAnimator = this.gameObject.AddComponent<HasDestoryAnimator>();
    }

    private void Move(Move m) {
        transform.Translate(m.axisDx * Time.deltaTime * 1.2f, 0, 0);
    }
    private void Jump(Jump j) {
        //rBody.AddForce(new Vector3(0, 10000, 0));
    }
    private void applyDamage(AttackData data) {
        this.hitPoint -= data.Damage;
        hasDestroy.Value = this.hitPoint <= 0;
    }

}