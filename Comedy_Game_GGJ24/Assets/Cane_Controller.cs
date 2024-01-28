using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cane_Controller : MonoBehaviour
{
	[SerializeField, Range(0.0f, 1.1f)] float caneAggro;
	[SerializeField] Transform startMarker, endMarker;

	Vector3 distanceVector;

	float baseYRot = -90f, hookingRot = 7.1f;

	[SerializeField] bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
		caneAggro = 0.0f;
		//transform.eulerAngles = Vector3.zero;

		distanceVector = endMarker.position - startMarker.position;
    }

    // Update is called once per frame
    void Update()
    {
		if (distanceVector == null)
			return;

		if (caneAggro <= 0.60 && caneAggro < 1.0f)
			attacking = false;
		else if (caneAggro >= 1.0f)
			attacking = true;

		Vector3 percentDistanceVector = caneAggro * distanceVector;
		percentDistanceVector.y = 0;

		float yRot = baseYRot;		
		if (caneAggro > 0.75f && !attacking)
			yRot += (caneAggro * hookingRot);
		else if (attacking)
		{
			attacking = true;
			yRot -= ((caneAggro) * hookingRot*1.1f);
		}			

		Quaternion rotation = Quaternion.Euler(0, yRot, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);


		Vector3 newPos = startMarker.position + percentDistanceVector;
		transform.position = Vector3.Slerp(transform.position, newPos, 7*Time.deltaTime);
    }
}
