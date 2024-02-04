using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
	[SerializeField] List<CreaturePart> creatureParts = new List<CreaturePart>();
	
	public void CompensateOffset(){
		Vector2 delta = GetCenter() - (Vector2)transform.position;
		foreach(CreaturePart part in creatureParts){
			part.transform.position -= (Vector3)delta;
		}
	} 
	public CreaturePart[] GetParts(){
		return creatureParts.ToArray();
	}

	public void AssignParts(CreaturePart[] parts){
		creatureParts = new List<CreaturePart>(parts);
	}
	public Vector2 GetCenter(){
		if(creatureParts.Count == 0){
			return transform.position;
		}
		Vector2 center = Vector2.zero;
		foreach(CreaturePart part in creatureParts){
			center += (Vector2)part.transform.position;
		}
		center /= creatureParts.Count;
		return center;
	}
	public void PullTowards(Vector2 target, float force){
		foreach(CreaturePart part in creatureParts){
			Vector2 delta = target-(Vector2)part.transform.position;
			part.GetRigidbody().velocity += delta.normalized*force*Time.deltaTime;
		}
	}
	public void UpdateParts()
	{
		for(int i = creatureParts.Count-1; i >= 0; i--){
			if(creatureParts[i] == null){
				creatureParts.RemoveAt(i);
			}
		}
	}
}
