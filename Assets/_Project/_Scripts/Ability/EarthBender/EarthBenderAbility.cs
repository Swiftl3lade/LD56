using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBenderAbility : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject Ramp;

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        Debug.Log("Space");
        var _spawnPossition = spawnPoint.position;
        _spawnPossition.y = -0.2f;
        Instantiate(Ramp, _spawnPossition, transform.rotation);
    }
}
