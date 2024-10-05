using UnityEngine;

public class OverrideSpring : SpringComponent
{
	protected override Vector3 SpringEval(Vector3 _targetPosition)
	{
		throw new System.NotImplementedException();
	}
}