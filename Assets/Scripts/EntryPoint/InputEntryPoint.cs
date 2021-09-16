using System;
using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InputEntryPoint : ITickable {

    [Inject] private IPlayerInput _player { get; set; }

    [Inject] private IPublisher<Jump> _jump { get; set; }

    [Inject] private IPublisher<Move> _move { get; set; }

    private void Jump () {
        _jump.Publish (_player.Jump ());
    }

    private void Move (float x) {
        _move.Publish (_player.Move (x));

    }

    public void Tick () {

        if (Input.GetMouseButtonDown (0)) {
            Jump ();
        }

        Move (Input.GetAxis ("Horizontal"));
    }
}