using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour {

    [Inject] private ISubscriber<Jump> jump { get; set; }

    [Inject] private ISubscriber<Move> move { get; set; }

    [Inject] private ISubscriber<DragEvent> drag { get; set; }

    // Start is called before the first frame update
    void Start () {
        move.Subscribe (mv => { Move (mv); });
        jump.Subscribe (j => { Jump (j); });
        drag.Subscribe (d => {
            this.transform.position = d.position;
        });
    }

    private void Move (Move m) {
        transform.Translate (m.axisDx * Time.deltaTime * 1.2f, 0, 0);
    }
    private void Jump (Jump j) {
        transform.Translate (0, j.data, 0);
    }
}