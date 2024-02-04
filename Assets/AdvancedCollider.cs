using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedCollider : MonoBehaviour
{
	public Rigidbody2D[] GetCollidingBodies(){
		return collidingBodies.ToArray();
	}
	public Collider2D[] GetCollisions(){
		return colliders.ToArray();
	}
	List<Rigidbody2D> collidingBodies = new List<Rigidbody2D>();
	List<Collider2D> colliders = new List<Collider2D>();
	void OnTriggerEnter2D(Collider2D collider){
		AddCol(collider);
		AddBody(collider);
	}
	void AddCol(Collider2D collider){
		for(int i = 0; i < colliders.Count; i++){
			if(colliders[i] == collider){
				return;
			}
		}
		colliders.Add(collider);
	}
	void AddBody(Collider2D collider){
		if(collider.attachedRigidbody == null){
			return;
		}
		for(int i = 0; i < collidingBodies.Count; i++){
			if(collidingBodies[i] == collider.attachedRigidbody){
				return;
			}
		}
		collidingBodies.Add(collider.attachedRigidbody);
	}
	void OnTriggerExit2D(Collider2D collider){
		RemoveCol(collider);
		RemoveBody(collider);
	}
	void RemoveBody(Collider2D collider){
		if(collider.attachedRigidbody == null){
			return;
		}
		for(int i = 0; i < collidingBodies.Count; i++){
			if(collidingBodies[i] == collider.attachedRigidbody){
				collidingBodies.RemoveAt(i);
				return;
			}
		}
	}
	void RemoveCol(Collider2D collider){
		for(int i = 0; i < colliders.Count; i++){
			if(colliders[i] == collider){
				colliders.RemoveAt(i);
				return;
			}
		}
	}
	void LateUpdate(){
		for(int i = collidingBodies.Count-1; i >=0; i--){
			if(collidingBodies[i] == null){
				collidingBodies.RemoveAt(i);
			}
		}
	}
}
