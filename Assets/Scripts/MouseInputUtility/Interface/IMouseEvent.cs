using UnityEngine;

public interface IMouseEvent {
    Vector3 mousePosition { get; set; }
    float time { get; set; }
}