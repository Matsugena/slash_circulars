using UnityEngine;
using VContainer.Unity;

// Click 回数をカウントします
public class ClickEntryPoint : ITickable {

    [SerializeField]
    private int clickCount = 0;

    void ITickable.Tick () {
        if (Input.GetMouseButtonUp (0)) {
            clickCount++;
            Debug.Log (clickCount);

        }
    }
}