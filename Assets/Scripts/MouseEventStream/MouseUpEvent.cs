using UnityEngine;

public class MouseUpEvent : IMouseEvent {
    public Vector3 position { get; set; }
    public float time { get; set; }
    public MouseUpEvent (IMouseEvent ime) {
        this.position = ime.position;
        this.time = ime.time;
    }

}