using System;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

public class ObjectManager : MonoBehaviour {
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private float minLineLength = 0.2f;
    [SerializeField] private float lineWidth = 0.5f;

    [Inject] private ISubscriber<FlickEvent> flickSub;

    void Start() {
        this.transform.position = Vector3.zero;

        flickSub.Subscribe(fev => DrawLine(fev)).AddTo(this);
    }

    void DrawLine(FlickEvent fev) {
        if (linePrefab == null) {
            throw new ArgumentNullException("ObjectManager needs linePrefab.");
        }



        var magnitude = fev.magnitude;

        if (magnitude > minLineLength) {
            var line = Instantiate(linePrefab);

            line.transform.position = fev.center;
            line.transform.right = fev.inverse.normalized;
            line.transform.localScale = new Vector3(fev.magnitude, lineWidth, lineWidth);
        }


    }
}
