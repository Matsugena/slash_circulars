using UnityEngine;

public class DragEvent : IMouseEvent {
    public Vector3 position { get; set; }
    public float time { get; set; }
}