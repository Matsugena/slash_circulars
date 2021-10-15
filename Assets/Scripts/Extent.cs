
using UniRx;
using UniRx.Triggers;
using UnityEngine;

// 伸び縮みするオブジェクト

public class Extent : MonoBehaviour {

    private float scaleY;
    private float maxScaleY = 10;
    private float minScaleY = 1;
    private float stepScaleY = 0.2f;
    private bool isExtent = false;

    private void Start() {
        this.UpdateAsObservable().Subscribe(_ => {
            ExtentTransform();
        }).AddTo(this);
    }

    private void ExtentTransform() {
        scaleY += isExtent ? stepScaleY : stepScaleY * -1;
        scaleY = scaleY < 0 ? 0 : scaleY;
        this.transform.localScale = new Vector3(1, scaleY, 1);
        this.transform.localPosition = new Vector3(0, scaleY / 2, 0);

        if (isExtent) {
            isExtent = scaleY > maxScaleY ? !isExtent : isExtent;
        } else {
            isExtent = scaleY < minScaleY ? !isExtent : isExtent;
        }

    }
}