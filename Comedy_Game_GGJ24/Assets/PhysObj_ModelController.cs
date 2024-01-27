using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysObj_ModelController : MonoBehaviour
{
	[SerializeField] private List<GameObject> modelPrefabs;

    // Start is called before the first frame update
    void Start()
    {
		ChooseRandomModel();
    }

	private void ChooseRandomModel()
	{
		int index = 0;
		if (modelPrefabs.Count > 0)
		{
			int modelIndex = Random.Range(0, modelPrefabs.Count);
			Instantiate(modelPrefabs[modelIndex], this.transform);
		}
	}
}
