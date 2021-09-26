using UnityEngine;

public class FlickEvent : IMouseEvent {
    public Vector3 position { get; set; }
    public float time { get; set; }
    public Vector3 verocity { get; set; } // フリック先の速度ベクトル
}