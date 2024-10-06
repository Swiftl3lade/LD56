using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpringHandler
{
	[SerializeField] public List<SpringComponent> springs;

	Vector3 previousVector = Vector3.zero;
	float lerp = 1f;

	public SpringHandler(List<SpringComponent> _springs)
	{
		springs = _springs.Concat(SpringManager.OverrideSprings).ToList();
	}

	public Vector3 CalculateDirectionVector()
	{
		var _vector = Vector3.zero;
		foreach (var spring in springs)
		{
			_vector += spring.Evaluate();
		}

		//_vector = Vector3.Lerp(previousVector, _vector.normalized, lerp*Time.deltaTime).normalized;
		previousVector = _vector;

		return _vector.normalized;
	}
}
