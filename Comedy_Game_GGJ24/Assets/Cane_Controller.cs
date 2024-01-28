using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Cane_Controller : MonoBehaviour
{
	public enum HookStateType { WAITING, REACHING, ATTACKING, RETREATING }

	[SerializeField, Range(0.0f, 1.15f)] float caneAggro;
	[SerializeField] Transform startMarker, endMarker;

	Vector3 distanceVector;

	float baseYRot = -90f, hookingRot = 7.2f, maxAggro = 1.15f;

	[SerializeField] HookStateType hookState;	

	[InspectorButton("OnButtonClicked")]
	[SerializeField, InspectorName("Hook 'Em!")] bool hookEm;

	// Start is called before the first frame update
	void Start()
    {
		caneAggro = 0.0f;
		hookState = HookStateType.WAITING;		
		distanceVector = endMarker.position - startMarker.position;
    }

    // Update is called once per frame
    void Update()
    {
		if (distanceVector == null)
			return;

		HookStateMachine();
	}

	private void OnButtonClicked()
	{
		Debug.Log("Clicked The Auto-Hook Button!");
		ReleaseTheHook();
	}

	public async void ReleaseTheHook()
	{
		while (caneAggro <= 1.0f && hookState < HookStateType.ATTACKING)
		{
			switch(hookState)
			{
				case HookStateType.WAITING:
					caneAggro += 0.005f;
					break;
				case HookStateType.REACHING:
					caneAggro += 0.0025f;
					//await Task.Delay(TimeSpan.FromSeconds(0.1f));
					break;
			}
			await Task.Yield();			
		}
	}

	void HookStateMachine()
	{

		Vector3 percentDistanceVector = caneAggro * distanceVector;
		percentDistanceVector.y = 0;
		float yRot = baseYRot, rotSpeed = 4f;

		switch(hookState)
		{
			case HookStateType.WAITING:
				if (caneAggro > 0.85f)
					hookState = HookStateType.REACHING;				
				break;
			case HookStateType.REACHING:
				if (caneAggro > 1.0f)
					hookState = HookStateType.ATTACKING;
				yRot += (caneAggro * hookingRot);
				break;
			case HookStateType.ATTACKING:
				if (caneAggro >= maxAggro)
					hookState = HookStateType.RETREATING;
				else
					caneAggro += 0.003f;
				rotSpeed = 2.5f;
				yRot -= ((caneAggro) * hookingRot * 1.05f);
				break;
			case HookStateType.RETREATING:
				if (caneAggro <= 0.0f)
					hookState = HookStateType.WAITING;
				caneAggro -= 0.0375f;
				break;
		}



		Quaternion rotation = Quaternion.Euler(0, yRot, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);


		Vector3 newPos = startMarker.position + percentDistanceVector;
		transform.position = Vector3.Slerp(transform.position, newPos, 7 * Time.deltaTime);
	}
}
