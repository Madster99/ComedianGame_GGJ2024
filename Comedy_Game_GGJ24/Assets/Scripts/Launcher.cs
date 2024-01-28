using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject[] throwablePrefabs;

    [SerializeField] private float launchForce;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Launch();
        }
    }

    public void Launch()
    {
        GameObject currentLaunchedObject = Instantiate(throwablePrefabs[Random.Range(0, throwablePrefabs.Length)], this.transform);
        Rigidbody rb = currentLaunchedObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * launchForce);
        Destroy(currentLaunchedObject, 4f);
    }
}
