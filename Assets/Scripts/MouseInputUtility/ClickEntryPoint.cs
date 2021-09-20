using System;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

// Click 回数をカウントします
public class ClickEntryPoint : ITickable, IInitializable {

    private Camera _camera = null;

    private string[] tagMask;

    [Inject] MessagePipe.IPublisher<DamageData> _damagePublisher;

    [Inject]
    ClickEntryPoint (IClickable c) {
        this.tagMask = c.TagMask;
    }

    public void Initialize () {
        _camera = Camera.main;

    }

    void ITickable.Tick () {
        if (Input.GetMouseButtonUp (0)) {
            GetRayHitPointFromCamera ();
        }
    }

    private void GetRayHitPointFromCamera () {
        Ray ray = _camera.ScreenPointToRay (Input.mousePosition);
        List<RaycastHit> hits = Physics.RaycastAll (ray)
            .ToList ()
            .FindAll (item =>
                tagMask.Contains (item.collider.tag)
            );

        if (hits.Count <= 0) return;

        SendDamege (hits[0].transform.gameObject);

    }
    private void SendDamege (GameObject obj) {
        var data = new DamageData {
            damage = 1
        };
        _damagePublisher.Publish (data);
    }
}