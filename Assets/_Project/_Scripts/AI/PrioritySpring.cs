using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrioritySpring : SpringComponent
{
	[SerializeField] List<SpringComponent> springs;
	private void Start()
	{
		try
		{
			Data.OverMinAttraction = Mathf.Clamp(Data.OverMinAttraction, -1, 1);
		}
		catch (NullReferenceException)
		{
			Debug.LogError("No Spring Data Object Added");
		}
	}

	protected override Vector3 SpringEval(Vector3 targetPosition)
	{
		var _dist = Vector3.Distance(checkOriginPoint.position, targetPosition);
		var _vector = (targetPosition - transform.position).normalized;

		if (_dist > Data.MinDistance && _dist < Data.MaxDistance)
		{
			_vector *= Data.OverMinAttraction;
            foreach (var item in springs)
            {
				item.isDisabled = true;
            }
		}
		else
		{
			_vector *= Data.UnderMinAttraction;
			foreach (var item in springs)
			{
				item.isDisabled = false;
			}
		}

		return _vector;
	}
}
