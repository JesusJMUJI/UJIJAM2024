using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
	[SerializeField] CreaturePart[] creatureParts;

	public void AssignParts(CreaturePart[] parts){
		creatureParts = parts;
	}
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
