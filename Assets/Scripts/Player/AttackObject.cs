
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using MessagePipe;
using VContainer;

/* 攻撃用オブジェクト 後々抽象化する*/
public class AttackObject : MonoBehaviour {
    private float blinkingLimitTime = 5.0f; // 点滅開始後にオブジェクトが消滅するまでの時間
    private float blinkInterval = 0.015f * 15; // Renderer のenabled が trueになっている時間
    private float disappearingTime = 0.015f * 1; // Renderer のenabled が falseになっている時間
    private bool isBlinking = false;
    private float objectRemainingTime = float.MaxValue;
    private float nextBlinkTime = float.MinValue;
    private Renderer rend;

    //
    [Inject] private IPublisher<AttackData> atackPub;
    private int damage = 1;

    // Start is called before the first frame update
    void Start() {
        rend = this.GetComponent<Renderer>();

        this.UpdateAsObservable()
        .Where(_ => { return isBlinking; })
        .Where(_ => { return nextBlinkTime < Time.time; })
        .Subscribe(_ => {
            Blink();
        });

        this.UpdateAsObservable()
        .Where(_ => { return objectRemainingTime < Time.time; })
        .Subscribe(_ => {
            Destroy(this.gameObject);
        });

        // 出現と同時に点滅フラグ着火
        startBlink();
    }


    void startBlink() {
        isBlinking = true;
        objectRemainingTime = Time.time + blinkingLimitTime;
    }

    /* 点滅の制御 */
    private void Blink() {
        rend.enabled = !rend.enabled; // 反転する
        nextBlinkTime = Time.time;
        if (rend.enabled) {
            nextBlinkTime += blinkInterval;
        } else {
            nextBlinkTime += disappearingTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag != "Enemy") return;

        atackPub.Publish(
            new AttackData(damage, other.gameObject)
        );

    }
}
