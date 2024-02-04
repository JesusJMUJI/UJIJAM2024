using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFrameController : MonoBehaviour
{

	[SerializeField] Transform cameraViewVisual;
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
		cameraViewVisual.localScale = new Vector3(size*WorldCamera.GetCamera().aspect, size,1);
		Vector2 targetPos = WorldCamera.GetWorldMousePos();

		transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z), speed*Time.fixedDeltaTime);
	}
}