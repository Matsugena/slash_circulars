// 開発用
using MessagePipe;
using UnityEngine;
using VContainer;

public class DevelopGameController : IGameController {

    [Inject] IPublisher<GameEvent> gEventPub;

    public void Pause () {
        throw new System.NotImplementedException ();
    }

    public void Reset () {
        gEventPub.Publish (GameEvent.DoReset ());
    }
}