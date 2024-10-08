using UnityEngine;

[System.Serializable]
public class DistanceSpring : SpringComponent
{
	private void Start()
	{
		if (Data != null) Data.OverMinAttraction = Mathf.Clamp(Data.OverMinAttraction, -1, 1);
	}

	protected override Vector3 SpringEval(Vector3 targetPosition)
	{
		var _dist = Vector3.Distance(checkOriginPoint.position, targetPosition);
		var _vector = (targetPosition - transform.position).normalized;
	/*	_vector.y = 0;
		_vector.Normalize();*/
		var _lerping =  (_dist >= 1 ? _dist : 1);


		if (_dist > Data.MinDistance && _dist < Data.MaxDistance)
		{
			_vector *= Data.OverMinAttraction / _lerping;
		}
		else
		{
			_vector *= -Data.UnderMinAttraction / _lerping;
		}

		return _vector;
	}
}
