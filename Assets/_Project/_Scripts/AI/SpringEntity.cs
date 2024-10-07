using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpringEntity : MonoBehaviour
{
	[field: SerializeField] public SpringEnum springTag { get; set; } = SpringEnum.walls;
	private void Start()
	{
		if (springTag == SpringEnum.overrideE)
		{
			SpringManager.OverrideSprings.Concat(GetComponents<SpringComponent>());
			return;
		}

		SpringManager.EntityDict[springTag]?.Add(this);
	}

	void OnDestroy()
	{
		if (springTag == SpringEnum.overrideE) return;

		SpringManager.EntityDict[springTag]?.Remove(this);
	}
}

[System.Serializable]
public enum SpringEnum
{
	overrideE,
	player,
	walls,
	danger,
	solid,
	pickup,
	ramp,
	AI,
	bumper
}
