using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureEditor : Environment
{
	protected override void OnEnabled(){
	}
	protected override void OnDisabled(){
	}

	void AssignParts(ConnectablePart[] _parts){

	}
	[SerializeField] ConnectablePart[] parts;
	bool BasePartSet(){
		foreach(ConnectablePart part in parts){
			if(part.IsLocked()){
				return true;
			}
		}
		return false;
	}
	ConnectablePart selectedPart;
	RelativeJoint2D angleJoint;
	TargetJoint2D targetJoint;
	[SerializeField] float rotationSpeed = 4;
	void Update(){
		if(!active){return;}
		ProcessSelection();
		ProcessDrag();
		
	}
	void ProcessSelection(){
		if(Input.GetMouseButtonDown(0)){
			if(selectedPart == null){
				ConnectablePart part = SelectPart();
				
				if(part != null){
					part.Unlock();
					selectedPart = part;
					CreateJointOnSelected();
				}
			}
			else{
				selectedPart.Lock();
				if(!BasePartSet()){
					selectedPart.SetBase();
				}
				RemoveJointOnSelected();
				selectedPart = null;
			}
		}
	}
	ConnectablePart SelectPart(){
		RaycastHit2D hit = Physics2D.Raycast(WorldCamera.GetWorldMousePos(), -Vector2.up);
		if(hit.collider != null){
			ConnectablePart part = hit.collider.gameObject.GetComponentInParent<ConnectablePart>();
			if(part != null){
				return part;
			}
		}
		return null;
	}
	void RemoveJointOnSelected(){
		Destroy(angleJoint);
		Destroy(targetJoint);
	}
	void CreateJointOnSelected(){
		angleJoint = selectedPart.gameObject.AddComponent<RelativeJoint2D>();
		angleJoint.autoConfigureOffset = false;
		angleJoint.maxForce = 0;
		// angleJoint.linearOffset = 
		angleJoint.angularOffset = -selectedPart.transform.eulerAngles.z;


		targetJoint = selectedPart.gameObject.AddComponent<TargetJoint2D>();
		targetJoint.autoConfigureTarget = false;
		targetJoint.target = WorldCamera.GetWorldMousePos();
	}
	void ProcessDrag(){
		if(angleJoint != null){
			targetJoint.target = WorldCamera.GetWorldMousePos();
			angleJoint.angularOffset += Input.mouseScrollDelta.y*Time.deltaTime*rotationSpeed;
		}
	}
}
