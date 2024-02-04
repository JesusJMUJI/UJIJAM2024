using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePart : MonoBehaviour
{
	float health;
	PartAsset asset;
	public void AssignAsset(PartAsset _asset){
		asset = _asset;
		health = asset.health;
	}
	//On collison
		//if colliding with another creature part
		//Damage other creature part by damage
	
	//Damage method
		//if health > 0
		//reduce health by damage
		//if health == 0
			//destroy gameObject
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
