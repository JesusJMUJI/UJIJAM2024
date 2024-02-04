using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFrameController : MonoBehaviour
{
	[SerializeField] BoxCollider2D cameraArea;
	[SerializeField] SpriteRenderer cameraViewVisual;
	[SerializeField] float speed;
	[SerializeField] float size;
	[SerializeField] AdvancedCollider frameArea;
	public float GetSize(){
		return size;
	}
	void Start()
	{
		
	}
	public PartPreview[] GetPartsInFrame(){
		Collider2D[] cols = frameArea.GetCollisions();
		List<PartPreview> previews = new List<PartPreview>();
		foreach(Collider2D col in cols){
			PartPreview preview = col.gameObject.GetComponent<PartPreview>();
			if(preview != null){
				previews.Add(preview);
			}
		}
		return previews.ToArray();
	}

	void FixedUpdate()
	{
		cameraViewVisual.size = new Vector2(size*WorldCamera.GetCamera().aspect, size);
		cameraArea.size = new Vector2(size*WorldCamera.GetCamera().aspect, size);
		Vector2 targetPos = WorldCamera.GetWorldMousePos();

		transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z), speed*Time.fixedDeltaTime);
	}
}