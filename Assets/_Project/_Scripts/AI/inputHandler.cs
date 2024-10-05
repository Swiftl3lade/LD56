using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class InputHandler : MonoBehaviour
{

	public Vector3 inputVector { get; protected set; }
	protected SpringHandler springHandler;
	private void Start()
	{
		springHandler = new SpringHandler(GetComponents<SpringComponent>().Where(x => x.Data.SpringTag != SpringEnum.overrideE).ToList());
	}

	protected void SetYInput(float y)
	{
		inputVector = new Vector3(inputVector.x, y);

	}

	protected void SetXInput(float x)
	{
		inputVector = new Vector3(x, inputVector.y);

	}
}