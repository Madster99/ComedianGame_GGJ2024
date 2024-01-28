using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public GameObject burstFX;
    public bool deleteOnImpact;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7) //ground layer for backwall
        {
            GameObject fxObj = Instantiate(burstFX, collision.contacts[0].point, Quaternion.identity);
            Destroy(fxObj, 1f);
            if (deleteOnImpact)
                Destroy(this.gameObject);
        }
    }
}
