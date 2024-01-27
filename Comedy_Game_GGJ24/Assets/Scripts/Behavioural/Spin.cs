using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
	[SerializeField][Range(0, 2.0f)] float X, Y, Z;	

    // Update is called once per frame
    void Update()
    {
		Vector3 speed = new Vector3(X,Y,Z);
        transform.Rotate((speed * 1000) * Time.deltaTime);
    }
}
