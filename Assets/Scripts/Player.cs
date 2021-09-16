using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour {

    [Inject] private ISubscriber<Jump> _jump { get; set; }

    [Inject] private ISubscriber<Move> _move { get; set; }

    // Start is called before the first frame update
    void Start () {
        _move.Subscribe (mv => { Move (mv); });
        _jump.Subscribe (j => { Jump (j); });
    }

    private void Move (Move m) {
        transform.Translate (m.axisDx * Time.deltaTime * 1.2f, 0, 0);
    }
    private void Jump (Jump j) {
        transform.Translate (0, j.data, 0);
    }
}