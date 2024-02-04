using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
	[SerializeField] CreaturePart[] creatureParts;
	public CreaturePart[] GetParts(){
		return creatureParts.Clone() as CreaturePart[];
	}

	public void AssignParts(CreaturePart[] parts){
		creatureParts = parts;
	}
	public Vector2 GetCenter(){
		Vector2 center = Vector2.zero;
		foreach(CreaturePart part in creatureParts){
			center += (Vector2)part.position;
		}
		center /= creatureParts.Length;
		return center;
	}
	public void PushTowards()
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
