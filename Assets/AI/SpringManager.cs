using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpringManager : MonoBehaviour, ISerializationCallbackReceiver
{
	public static Transform playerTransform;

	public static Dictionary<SpringEnum, List<SpringEntity>> EntityDict { get; private set; }
	public static List<SpringComponent> OverrideSprings;

	[SerializeField] private Transform PlayerTransform;                  //used only for serialization


	private void Awake()
	{
		EntityDict = new Dictionary<SpringEnum, List<SpringEntity>>();
		OverrideSprings = new List<SpringComponent>();

		foreach (var tag in Enum.GetValues(typeof(SpringEnum)).Cast<SpringEnum>())
		{
			EntityDict.Add(tag, new List<SpringEntity>());
		}
	}

	public void OnAfterDeserialize()
	{
		playerTransform = PlayerTransform;
	}

	public void OnBeforeSerialize()
	{
		PlayerTransform = playerTransform;
	}

	public static event EventHandler<OnEnemyDeathArgs> onEnemyDeath;
	public class OnEnemyDeathArgs : EventArgs
	{
		public int scoreIncrease;
	}

	public static void OnEnemyDeath(object sender, OnEnemyDeathArgs args)
	{
		onEnemyDeath?.Invoke(sender, args);
	}
}
