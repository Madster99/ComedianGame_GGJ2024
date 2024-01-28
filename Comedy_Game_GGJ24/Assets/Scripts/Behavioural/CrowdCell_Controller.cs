using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CrowdCell_Controller : MonoBehaviour
{
	[SerializeField] List<Image>	crowdMembers;
	//public List<Sprite>				crowdSprites;
	public int crowdMax;
	[SerializeField] Image imagePrefab;

	#region start and update
	//// Start is called before the first frame update
	void Start()
	{
		crowdMembers = new List<Image>();
		foreach (Transform child in gameObject.transform)
			Destroy(child.gameObject);
	}	
	#endregion

	public void UpdateCrowd(int max)
	{		
		crowdMax = max;

		if (crowdMembers.Count < crowdMax)
			AddMembers();
		else
			RemoveMembers();
	}

	void AddMembers()
	{
		while(transform.childCount < crowdMax && crowdMembers.Count < crowdMax)
		{		
			Image img = Instantiate(imagePrefab, transform);
			Crowd_Controller crowdCTRL = this.gameObject.transform.parent.GetComponent<Crowd_Controller>();
			List<Sprite> crowdSprites = new List<Sprite>(crowdCTRL.GetCrowdSpriteList());

			img.gameObject.transform.localPosition = new Vector3(transform.position.x, transform.position.y, Random.Range(-0.5f, 0.5f)); 

			while (true && crowdSprites != null)
			{				
				Sprite spr = crowdCTRL.GetCrowdSprite_AtIndex( UnityEngine.Random.Range(0, crowdSprites.Count-1));
				bool found = false;
				foreach (Image i in crowdMembers)
				{
					if (i.sprite == spr && (crowdSprites.Count >= crowdMax))
						found = true; break;
				}

				if (!found)
				{
					img.sprite = spr;
					break;
				}					
			}
			crowdMembers.Add(img);
		}
	}

	void RemoveMembers()
	{
		for (int i = transform.childCount - 1; i >= crowdMax; i--)
		{
			Destroy(gameObject.transform.GetChild(i).gameObject);
			crowdMembers.RemoveAt(i);
		}
	}

	void Crowd_Angry_Movement()
	{

	}

	void Crowd_Happy_Movement()
	{

	}
}