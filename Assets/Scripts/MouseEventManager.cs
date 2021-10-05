using MessagePipe;
using UnityEngine;
using VContainer;

public class MouseEventManager : MouseEventStream {
    [Inject] private IPublisher<MouseEvent> mouseDownPub { get; set; }

    [Inject] private IPublisher<MouseUpEvent> mouseUpPub { get; set; }

    [Inject] private IPublisher<DragEvent> dragPub { get; set; }

    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }

    protected override MouseEvent GetMouseEvent() {
        var p = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            12
        );

        return new MouseEvent {
            position = mainCamera.ScreenToWorldPoint(p), time = Time.time
        };
    }

    protected override void DoMouseDown(MouseEvent ev) {
        mouseDownPub.Publish(ev);
    }

    protected override void DoMouseUp(MouseEvent ev) {
        mouseUpPub.Publish(new MouseUpEvent(ev));
    }


    protected override void DoMouseDragg(MouseEvent ev) {
        dragPub.Publish(new DragEvent(ev));
    }
}