using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AGameEvent<T> : ScriptableObject
{
    // Start is called before the first frame update
	private List<IGameEventListener<T>> listeners =
		new List<IGameEventListener<T>>();

	[SerializeField]
	[TextArea]
	private string explanation;

	public void Raise (T item)
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised(item);
	}

	public void RegisterListener (IGameEventListener<T> listener)
	{ listeners.Add(listener); }

	public void UnregisterListener (IGameEventListener<T> listener)
	{ listeners.Remove(listener); }
}
