using System;
using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

// 入力に応じて通知を行わせるEntryPoint
public class InputEntryPoint : ITickable {

    [Inject] private IPlayerInput pInput { get; set; }

    [Inject] private IGameController gController { get; set; }

    [Inject] private IPublisher<Jump> jumpPub { get; set; }

    [Inject] private IPublisher<Move> movePub { get; set; }

    public void Tick () {

        // if (Input.GetMouseButtonDown (0)) {
        //     jumpPub.Publish (playereInput.jump);
        // }
        movePub.Publish (pInput.Move (Input.GetAxis ("Horizontal")));

        if (Input.GetKeyUp (KeyCode.Space)) {
            // when space key up then reload scene
            gController.Reset ();
            Debug.Log ("reset" + new DateTime ());
        }
    }
}