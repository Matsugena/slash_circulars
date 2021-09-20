using UnityEngine;

public class DragEvent : IMouseEvent {
    public Vector3 mousePosition {
        get;
        set;
    }

    public float time {
        get { return time; }
        set { time = Time.time; }
    }
}