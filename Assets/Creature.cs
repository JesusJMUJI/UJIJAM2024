using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
	[SerializeField] List<CreaturePart> creatureParts = new List<CreaturePart>();
	float maxImpulseDistance = 4;
	float maxImpulseForce = 80;
	float attackDuration = 0.4f;
	float attackCooldown = 0.5f;
	bool attackOnCooldown = false;
	bool isAttacking = false;
	public void CompensateOffset(){
		Vector2 delta = GetCenter() - (Vector2)transform.position;
		foreach(CreaturePart part in creatureParts){
			part.transform.position -= (Vector3)delta;
		}
	} 
	public CreaturePart[] GetParts(){
		return creatureParts.ToArray();
	}
	public void ApplyImpulse(Vector2 dir, Vector2 point){
		dir = dir.normalized;
		foreach(CreaturePart part in creatureParts){
			Vector2 delta = point-(Vector2)part.transform.position;
			float force = Mathf.Lerp(maxImpulseForce, 0, delta.magnitude/maxImpulseDistance);
			part.GetRigidbody().velocity = dir*force;
		}
	}
	public void AssignParts(CreaturePart[] parts){
		creatureParts = new List<CreaturePart>(parts);
		foreach(CreaturePart part in parts){
			part.AssignCreature(this);
		}
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
	public void Attack(Vector2 target)
	{
		StartCoroutine(AttackRoutine(target));
	}
	IEnumerator AttackRoutine(Vector2 target)
	{
		if(attackOnCooldown){
			yield break;
		}
		foreach(CreaturePart part in creatureParts){
			Vector2 delta = target - (Vector2)part.transform.position;
			part.GetRigidbody().velocity = delta.normalized*50*Random.Range(0,2);
		}
		attackOnCooldown = true;
		ToggleAttack(true);
		yield return new WaitForSeconds(attackDuration);
		ToggleAttack(false);
		yield return new WaitForSeconds(attackCooldown);
		attackOnCooldown = false;
	}
	void ToggleAttack(bool val){
		isAttacking = val;
		foreach(CreaturePart part in creatureParts){
			part.ToggleAttack(val);
		}
	}
	public void UpdateParts(){
		for(int i = creatureParts.Count-1; i >= 0; i--){
			if(creatureParts[i] == null){
				creatureParts.RemoveAt(i);
			}
		}
	}
}
