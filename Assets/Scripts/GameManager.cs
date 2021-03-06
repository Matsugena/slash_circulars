using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class GameManager : MonoBehaviour {

    [SerializeField] private string reloadSceneName;

    [Inject] ISubscriber<GameEvent> gEvent;

    // Start is called before the first frame update
    void Start() {
        gEvent.Subscribe(ge => {
            var s = SceneManager.GetActiveScene().name;

            if (ge.Equals(GameEvent.DoReset())) {
                SceneManager.LoadScene(reloadSceneName);
            }
        });
    }

    // Update is called once per frame
    void Update() {

    }
}