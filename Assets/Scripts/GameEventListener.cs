using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// Explanation: Gameevent (scriptable object file)


// T= type, E = GameEvent, UER = Unity Event Response.
public abstract class GameEventListener <T,E,UER> : MonoBehaviour, IGameEventListener<T> where E : AGameEvent<T> where UER : UnityEvent<T>
{
    [SerializeField]
    private E gameEvent;
    [SerializeField]
    private UER unityEventResponse;

    private void OnEnable ()
    {
        if(gameEvent != null)
        {
            gameEvent.RegisterListener(this);
        }
    }

    private void OnDisable ()
    {
        if (gameEvent != null)
        {
            gameEvent.UnregisterListener(this);
        }
    }


    public void OnEventRaised (T data)
    {
        if(unityEventResponse != null)
        {
            unityEventResponse.Invoke(data);
        }
    }
}
