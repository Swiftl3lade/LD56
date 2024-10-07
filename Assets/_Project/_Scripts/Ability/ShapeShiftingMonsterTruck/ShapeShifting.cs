using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeShifting : MonoBehaviour
{
    private Vector3 initialScale = new Vector3(0.7f, 0.7f, 0.7f);
    private Vector3 targetScale = Vector3.one; // Scale to (1, 1, 1)
    [SerializeField] private float shapeshiftDuration = 10f; // Duration to stay at full size (10 seconds)
    [SerializeField] private Collider wheelsCollider; 


    private bool isShapeshifted = false; // To prevent multiple shapeshifts

    void Start()
    {
        transform.localScale = initialScale;
        wheelsCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isShapeshifted)
        {
            Shapeshift();
        }
    }

    private void Shapeshift()
    {
        transform.localScale = targetScale;
        isShapeshifted = true;
        wheelsCollider.enabled = true;
        GetComponent<Controller>().UpdateStats(1.05f, 1.45f);

        Invoke("RevertShapeshift", shapeshiftDuration);
    }

    private void RevertShapeshift()
    {
        transform.localScale = initialScale;
        isShapeshifted = false;
        wheelsCollider.enabled = false;
        GetComponent<Controller>().RevertStats();
    }
}
