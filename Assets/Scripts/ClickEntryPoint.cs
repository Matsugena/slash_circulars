using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer.Unity;

// Click 回数をカウントします
public class ClickEntryPoint : ITickable, IInitializable {

    private Camera _camera = null;
    private Vector3 _currentPosition = Vector3.zero;

    [SerializeField] private int _clickCount = 0;
    [SerializeField] List<String> tagMask; // ここをDIします

    public void Initialize () {
        _camera = Camera.main;
    }

    void ITickable.Tick () {
        if (Input.GetMouseButtonUp (0)) {
            GetRayHitPointFromCamera ();

            _clickCount++;
            Debug.Log (_clickCount);

        }
    }

    private void GetRayHitPointFromCamera () {
        Ray ray = _camera.ScreenPointToRay (Input.mousePosition);
        List<RaycastHit> hits = Physics.RaycastAll (ray)
            .ToList ();
        // .FindAll (item =>
        //     tagMask.Contains (item.collider.tag)
        // );

        if (hits.Count == 0) {
            Debug.Log (ray);
            return;
        }

        foreach (RaycastHit item in hits) {
            Debug.Log (item.collider.gameObject);
        }

        var distance = Vector3.Distance (
            _camera.transform.position, hits.First ().point);
        var mousePosition = new Vector3 (
            Input.mousePosition.x, Input.mousePosition.y, distance);

        _currentPosition = hits.First ().point;

        Debug.DrawRay (ray.origin, ray.direction * 1000f); // debug
    }
}