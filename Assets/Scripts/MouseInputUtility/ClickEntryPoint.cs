using System;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

// Click 回数をカウントします
public class ClickEntryPoint : ITickable, IInitializable {

    private Camera _camera = null;

    [SerializeField] private int _clickCount = 0;
    [SerializeField] private string[] tagMask;

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

            _clickCount++;

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

        foreach (RaycastHit item in hits) {
            Debug.Log ("hitObject" + item.transform.name);
            _damagePublisher.Publish (
                new DamageData {
                    damage = 1,
                        targetInstanceId = item.transform.gameObject.GetInstanceID ()
                }
            );
        }

    }
}