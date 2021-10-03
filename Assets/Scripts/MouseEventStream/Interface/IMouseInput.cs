using UnityEngine;

public interface IMouseInput {
    public GameObject OnClickObject ();
    public MouseEvent OnClick ();
    public MouseEvent OnDrag ();
}