using System;
using UnityEngine;

[System.Serializable]
public class NemesisSpring : SpringComponent
{
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
		if (UnityEngine.Random.Range(0, 4) > 0) return Vector3.zero;
		var _dist = Vector3.Distance(checkOriginPoint.position, targetPosition);
		var _vector = (targetPosition - transform.position).normalized;

		if (_dist > Data.MinDistance && _dist < Data.MaxDistance)
		{
			_vector *= Data.OverMinAttraction;
		}
		else
		{
			_vector *= Data.UnderMinAttraction;
		}

		return _vector;
	}
}
