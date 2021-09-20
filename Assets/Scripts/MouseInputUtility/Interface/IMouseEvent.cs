using UnityEngine;

public interface IMouseEvent {
    Vector3 position { get; set; }
    float time { get; set; }
}