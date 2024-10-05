using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoVSpring : DistanceSpring
{
	[SerializeField] private LayerMask layersToCheck;

	protected override Vector3 SpringEval(Vector3 _targetPosition)
	{
		var hit = Physics2D.Raycast(checkOriginPoint.position, (_targetPosition - (Vector3)checkOriginPoint.position), Data.MaxDistance, layersToCheck);
		if (!hit) return Vector3.zero;

		if (hit.collider.gameObject.GetComponentInParent<SpringEntity>().springTag == Data.SpringTag) return base.SpringEval(_targetPosition);

		return Vector3.zero;
	}

	private void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();

		if (!Application.isPlaying) return;

		foreach (var entity in SpringManager.EntityDict[Data.SpringTag])
		{
			var hit = Physics2D.Raycast(checkOriginPoint.position, (entity.transform.position - checkOriginPoint.position), Data.MaxDistance, layersToCheck);
			if (!hit) return;


			if (hit.collider.gameObject.GetComponentInParent<SpringEntity>().springTag == Data.SpringTag) Gizmos.color = Color.green;
			else Gizmos.color = Color.red;
			Gizmos.DrawLine(checkOriginPoint.position, transform.position + (entity.transform.position - checkOriginPoint.position));
		}
	}
}
