using System.Collections;
using System.Collections.Generic;
using _Project._Scripts;
using UnityEngine;

public class WheelBumper : MonoBehaviour
{
    private GameObject _parent;
    private CarStats _stats;
    
    private void Awake()
    {
        _parent = transform.parent.gameObject;
        _stats = _parent.GetComponent<CarStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (other.CompareTag("FrontBumper"))
        {
            _stats.DealDamage(other, true);
            return;
        }

        _stats.DealDamage(other, false);
    }
}
