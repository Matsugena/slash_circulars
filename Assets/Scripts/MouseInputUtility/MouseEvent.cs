using UnityEngine;

public class MouseEvent : IMouseEvent {
    public Vector3 position { get; set; }
    public float time { get; set; }
}