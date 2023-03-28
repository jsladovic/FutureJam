using Assets.Scripts.GameEvents.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameEvents.Listeners
{
	public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T>
		where E : BaseGameEvent<T>
		where UER : UnityEvent<T>
	{
		[SerializeField]
		public E GameEvent;
		[SerializeField]
		private UER UnityEventReponse;

		private void OnEnable()
		{
			if (GameEvent == null)
				return;
			GameEvent.RegisterListener(this);
		}

		private void OnDisable()
		{
			if (GameEvent == null)
				return;
			GameEvent.UnregisterListener(this);
		}

		public void OnEventRaised(T item)
		{
			if (UnityEventReponse == null)
				return;
			UnityEventReponse.Invoke(item);
		}
	}

	public interface IGameEventListener<T>
	{
		void OnEventRaised(T item);
	}
}
