using UnityEngine;

public class DragEvent : IMouseEvent {
    public Vector3 position { get; set; }
    public float time { get; set; }
    public DragEvent (IMouseEvent ime) {
        this.position = ime.position;
        this.time = ime.time;
    }
    public DragEvent () {

    }
}