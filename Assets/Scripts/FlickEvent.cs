
using UnityEngine;

/*
  フリックされたポジション(2点) とその発生時間を持つ
*/
public class FlickEvent {
    public Vector3 from { get; }
    public Vector3 to { get; }
    public float time;
    public FlickEvent(Vector3 from, Vector3 to) {
        this.from = from;
        this.to = to;
        this.time = Time.time;
    }

    public float magnitude {
        get { return (this.from - this.to).magnitude; }
    }
    public Vector3 center {
        get { return (this.from + this.to) / 2; }
    }
    public Vector3 inverse {
        get { return (this.from - this.to) / 2; }
    }
}