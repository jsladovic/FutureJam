using Assets.Scripts.GameEvents.Listeners;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameEvents.Events
{
	public abstract class BaseGameEvent<T> : ScriptableObject
	{
		private readonly List<IGameEventListener<T>> EventListeners = new List<IGameEventListener<T>>();

		public void Raise(T item)
		{
			for (int i = EventListeners.Count - 1; i >= 0; i--)
			{
				EventListeners[i].OnEventRaised(item);
			}
		}

		public void RegisterListener(IGameEventListener<T> listener)
		{
			if (EventListeners.Contains(listener) == true)
				return;
			EventListeners.Add(listener);
		}

		public void UnregisterListener(IGameEventListener<T> listener)
		{
			if (EventListeners.Contains(listener) == false)
				return;
			EventListeners.Remove(listener);
		}
	}
}
