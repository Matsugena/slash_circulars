using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour {

    [Inject] private ISubscriber<Jump> jump { get; set; }

    [Inject] private ISubscriber<Move> move { get; set; }

    void Start() {
        // move.Subscribe (mv => { Move (mv); });
        // jump.Subscribe (j => { Jump (j); });
    }

    private void Move(Move m) {
        transform.Translate(m.axisDx * Time.deltaTime * 1.2f, 0, 0);
    }
    private void Jump(Jump j) {
        //rBody.AddForce(new Vector3(0, 10000, 0));
    }
}