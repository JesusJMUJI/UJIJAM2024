using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectablePart : MonoBehaviour
{
	List<Joint2D> connections = new List<Joint2D>();
	Rigidbody2D rb;
	AdvancedCollider connectionArea;
	
	bool locked = false;
	bool basePart = false;
	public bool IsLocked(){
		return locked || basePart;
	}
	public void SetBase(){
		basePart = true;
	}
	void Awake(){
		rb = GetComponent<Rigidbody2D>();
		connectionArea = GetComponentInChildren<AdvancedCollider>();
	}
	public void AddConnection(Joint2D joint){
		connections.Add(joint);
	}
	public void ConvertToCreature(){

	}
	public void Lock(){
		Rigidbody2D[] contacts = connectionArea.GetCollidingBodies();
		Debug.Log($"{contacts.Length} potential contacts");
		foreach(Rigidbody2D contact in contacts){
			CreateConnection(contact);
		}
	}
	void CreateConnection(Rigidbody2D other){
		ConnectablePart otherPart = other.gameObject.GetComponent<ConnectablePart>();
		if(otherPart == null){
			return;
		}
		if(!otherPart.IsLocked()){
			return;
		}
		Debug.Log($"Connection");
		Vector2 selfSurfacePoint = rb.ClosestPoint(other.position);
		Vector2 otherSurfacePoint = other.ClosestPoint(rb.position);
		Vector2 jointPos = (selfSurfacePoint + otherSurfacePoint) * 0.5f;


		HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();

		joint.connectedBody = other;
		joint.anchor = transform.InverseTransformPoint(jointPos);
		joint.autoConfigureConnectedAnchor = false;
		joint.connectedAnchor = other.transform.InverseTransformPoint(jointPos);
		AddConnection(joint);
		otherPart.AddConnection(joint);
	}
	public void Unlock(){
		foreach(Joint2D joint in connections){
			Destroy(joint);
		}
		basePart = false;
	}

	// Remove null connections
	void Update()
	{
		for(int i = connections.Count-1; i >=0; i--){
			if(connections[i] == null){
				connections.RemoveAt(i);
			}
		}

		Freeze(connections.Count > 0 || basePart);
	}
	void Freeze(bool toggleValue){
		if(toggleValue){
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		else{
			rb.constraints = RigidbodyConstraints2D.None;
		}
		locked = toggleValue;
	}
}
