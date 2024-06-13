using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureEditor : Environment
{	
	[SerializeField] bool CREATE_ENEMY;
	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] Image helpImage;
	
	public static bool isEditing;
	protected override void OnEnabled(){
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		//TODO reset camera
	}
	protected override void OnDisabled(){
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}
	[SerializeField] CameraPan cameraController;
	void ClearParts(){
		foreach (Transform child in partContainer) {
			Destroy(child.gameObject);
		}
	}

	void Start()
	{
		StartCoroutine(FadeHelp());
	}
	IEnumerator FadeHelp(){
		float time = 10;
		while(time >= 0){
			time -= Time.deltaTime;
			helpImage.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), 1 - Math.Clamp(time, 0, 1));
			yield return null;
		}
	}

	public void AssignCollection(PartPreview[] selectedParts, Vector2 relativePosition, float frameZoom){
		ClearParts();

		parts = new ConnectablePart[selectedParts.Length];
		for(int i = 0; i < selectedParts.Length; i++){
			PartPreview part = selectedParts[i];
			Vector2 localPosition = (Vector2)part.transform.localPosition;
			Quaternion rotation = part.transform.rotation;
			parts[i] = Instantiate(part.asset.editorObject, localPosition,rotation, partContainer);
		}
		cameraController.Reset(relativePosition, frameZoom);
	}
	void PropagateLayer(GameObject obj, int newLayer)
	{
		if (!obj){return;}
		obj.layer = newLayer;
	   
		foreach (Transform child in obj.transform){
			if (!child){continue;}
			PropagateLayer(child.gameObject, newLayer);
		}
	}
	[SerializeField] Transform partContainer;
	public void CompleteCreature(){
		Creature creature = ExtractCreature();
		creature.CompensateOffset();
		if(CREATE_ENEMY){
			PropagateLayer(creature.gameObject, 7);
			Debug.LogError("Enemy Created!", creature.gameObject);
		}
		GameManager.instance.SwitchToBattle(creature);
	}
	Creature ExtractCreature(){
		GameObject creatureObj = new GameObject("Creature");
		GameObject creature = Instantiate(creatureObj, transform.position, Quaternion.identity) as GameObject;
		Creature creatureComp = creature.AddComponent<Creature>();
		List<CreaturePart> creatureParts = new List<CreaturePart>();
		foreach(ConnectablePart part in parts){
			if(part.IsLocked()){
				CreaturePart creaturePart = part.ConvertToCreaturePart();
				creaturePart.transform.parent = creature.transform;
				creatureParts.Add(creaturePart);
			}
		}
		creatureComp.AssignParts(creatureParts.ToArray());
		return creatureComp; 
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
		isEditing = active && selectedPart != null;

		if(!active){return;}
		ProcessSelection();
		ProcessDrag();

		if(Input.GetKeyDown(KeyCode.Space)){
			if(BasePartSet()){
				if(selectedPart){
					RemoveJointOnSelected();
					selectedPart = null;
				}

				CompleteCreature();

			}
		}
		
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
		angleJoint.angularOffset = Mathf.DeltaAngle(selectedPart.transform.eulerAngles.z,0);


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
