using System;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ObjectManager : MonoBehaviour {
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private float minLineLength = 0.2f;
    [SerializeField] private float lineWidth = 0.5f;
    [SerializeField] private float lineZ = -0.1f;

    [Inject] private ISubscriber<FlickEvent> flickSub;

    [Inject] private IObjectResolver container;

    public ObjectManager(IObjectResolver container) {
        this.container = container;
    }

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
            var line = container.Instantiate(linePrefab, this.transform);

            line.transform.position = new Vector3(fev.center.x, fev.center.y, lineZ);
            line.transform.right = fev.inverse.normalized;
            line.transform.localScale = new Vector3(fev.magnitude, lineWidth, lineWidth);
        }


    }
}
