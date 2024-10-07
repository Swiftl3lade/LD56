using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.Ability;
using UnityEngine;

public class ShapeShifting : Ability
{
    private Vector3 initialScale = new Vector3(0.7f, 0.7f, 0.7f);
    private Vector3 targetScale = Vector3.one; // Scale to (1, 1, 1)
    [SerializeField] private float shapeshiftDuration = 10f; // Duration to stay at full size (10 seconds)
    [SerializeField] private Collider wheelsCollider; 

    private bool isShapeshifted = false; // To prevent multiple shapeshifts

    public override void Start()
    {
        base.Start();
        transform.localScale = initialScale;
        wheelsCollider.enabled = false;
    }

    public override void ActivateAbility()
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
