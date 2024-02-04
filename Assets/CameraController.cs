using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[System.Serializable]
	public struct CameraConfig
	{
		[SerializeField] public float baseZoom;
		[SerializeField] public float velocityZoom;
		[SerializeField] public float lerpSpeed;
		[SerializeField] public float zoomLerpSpeed;
		[SerializeField] public bool followCursor;
		[SerializeField] [Range(0,1f)] public float cursorPositionInfluence;
		
	}
	public List<Transform> trackedObjects = new List<Transform>();
	[SerializeField] Camera camera;
	[SerializeField] CameraConfig cameraConfig;

	//Cam controlling
	void FixedUpdate(){
		for(int i = trackedObjects.Count-1; i >= 0; i--){
			if(trackedObjects[i] == null){
				trackedObjects.RemoveAt(i);
			}
		}
		// Calclulating Average point
		Vector2 targetPosition = Vector2.zero;
		for (int i = 0; i < trackedObjects.Count; i++){
			targetPosition += (Vector2)trackedObjects[i].position;
		}
		if(cameraConfig.followCursor){
			Vector2 mousePos = WorldCamera.GetWorldMousePos();
			targetPosition += mousePos * cameraConfig.cursorPositionInfluence;
			targetPosition /= trackedObjects.Count + cameraConfig.cursorPositionInfluence;
		}
		else{
			targetPosition /= trackedObjects.Count;
		}
		


		//Computing Max distance from avarage point
		float maxDistSqr = 0;
		for (int i = 0; i < trackedObjects.Count; i++){
			Vector2 delta = (Vector2)trackedObjects[i].position - targetPosition;
			float aspectAdjustedMagnitudeSqr = Mathf.Pow(delta.y,2) + Mathf.Pow(delta.x/camera.aspect,2);
			if(maxDistSqr < aspectAdjustedMagnitudeSqr){
				maxDistSqr = aspectAdjustedMagnitudeSqr;
			}
		}
		float objectFramingZoom = Mathf.Sqrt(maxDistSqr);

		// //Mouse handling
		// if(cameraConfig.followCursor){
		// 	Vector2 mousePos = camera.ScreenToWorldPoint(mousePositionAction.ReadValue<Vector2>());
		// 	float mouseFramingZoom = (mousePos - targetPosition).magnitude;
		// 	objectFramingZoom = Mathf.Max(objectFramingZoom, mouseFramingZoom*cameraConfig.cursorZoomInfluence);
		// }

		transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), cameraConfig.lerpSpeed*Time.unscaledDeltaTime);

		//Computing frame velocity
		Vector2 targetDelta = targetPosition - (Vector2)transform.position;
		targetDelta.x /= camera.aspect;

		
		float targetZoom = cameraConfig.baseZoom + targetDelta.magnitude*cameraConfig.velocityZoom + objectFramingZoom;

		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, cameraConfig.zoomLerpSpeed*Time.unscaledDeltaTime);


	}
}