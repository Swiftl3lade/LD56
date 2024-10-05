using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Spring", menuName = "ScriptableObjects/Spring")]
public class SpringData : ScriptableObject
{
	[SerializeField] public SpringEnum SpringTag = SpringEnum.player;
	[Range(-1, 1)]
	[Tooltip("Kicks in while distance is in between max and min")]
	[SerializeField] public float OverMinAttraction;
	[Range(-1, 1)]
	[Tooltip("Kicks in while distance is outside max and min")]
	[SerializeField] public float UnderMinAttraction;
	[SerializeField] public float MinDistance = 0f;
	[SerializeField] public float MaxDistance = Single.PositiveInfinity; // spring only kicks in when between these distances


	[field: SerializeField] public Color MaxColor { get; private set; } = Color.blue;
	[field: SerializeField] public Color MinColor { get; private set; } = Color.white;
}