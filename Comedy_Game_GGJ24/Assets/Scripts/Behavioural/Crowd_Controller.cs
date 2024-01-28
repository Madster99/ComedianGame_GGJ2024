using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crowd_Controller : MonoBehaviour
{
	[SerializeField] List<Transform> crowdParents;
	[SerializeField] List<Sprite>	 crowdSprites;
	//start 1, max 5
	[SerializeField, Range(0,5)] int currCrowdStage;
	int crowdMax = 5;

	[InspectorButton("GenerateCrowd")]
	public bool Generate;

    // Start is called before the first frame update
    void Start()
    {
		currCrowdStage = 1;
		//foreach (Transform child in crowdParents)
		//{
		//	if (child.TryGetComponent(out CrowdCell_Controller c))			
		//		c.crowdSprites = new List<Sprite>(crowdSprites);			
		//}			
		GenerateCrowd();
    }

	private void GenerateCrowd()
	{
		if (crowdParents.Count > 0 && crowdSprites.Count > 0)
			UpdateCrowd();
	}

	//private void FixedUpdate()
	//{
	//	if (crowdParents.Count > 0 && crowdSprites.Count > 0)
	//		UpdateCrowd();
	//}

	public void UpdateCrowd()
    {
		int min, max;
		max = currCrowdStage;
		min = max;
		if (currCrowdStage > 1 && currCrowdStage != crowdMax)
			min -= 1;

		foreach (Transform t in crowdParents)
		{
			if (t.TryGetComponent(out CrowdCell_Controller c))
			{
				int crowdCap = Random.Range(min, max + 1);
				crowdCap = System.Math.Clamp(crowdCap, crowdCap, crowdMax);
				c.UpdateCrowd(crowdCap);
			}				
		}
    }

	//needs nulkl checks but whatever
	public Sprite GetCrowdSprite_AtIndex(int index) { return crowdSprites[index]; }
	public List<Sprite> GetCrowdSpriteList()		{ return crowdSprites; }

	public List<Image> GetAllCrowdMembers()
	{
		List<Image> crowdMembers = new List<Image>();
		foreach(Transform crowdParent in crowdParents)
		{
			if (crowdParent.TryGetComponent(out CrowdCell_Controller c))
				foreach (Image img in c.GetCrowdMembers())
					crowdMembers.Add(img);						
		}

		return crowdMembers;
	}
}
