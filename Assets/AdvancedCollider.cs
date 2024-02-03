using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedCollider : MonoBehaviour
{
	public Rigidbody2D[] GetCollidingBodies(){
		return collidingBodies.ToArray();
	}
	List<Rigidbody2D> collidingBodies = new List<Rigidbody2D>();
	void OnTriggerEnter2D(Collider2D collider){
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
	void LateUpdate(){
		for(int i = collidingBodies.Count-1; i >=0; i--){
			if(collidingBodies[i] == null){
				collidingBodies.RemoveAt(i);
			}
		}
	}
}
