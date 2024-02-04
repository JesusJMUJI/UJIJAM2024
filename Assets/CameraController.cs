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

		[SerializeField] public Vector2 HorizontalClamp;
		[SerializeField] public Vector2 VerticalClamp;
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
		Vector2 newPosition = Vector2.Lerp(transform.position, targetPosition, cameraConfig.lerpSpeed*Time.unscaledDeltaTime);

		//Computing frame velocity
		Vector2 targetDelta = targetPosition - newPosition;
		targetDelta.x /= camera.aspect;

		
		float targetZoom = cameraConfig.baseZoom + targetDelta.magnitude*cameraConfig.velocityZoom + objectFramingZoom;

		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, cameraConfig.zoomLerpSpeed*Time.unscaledDeltaTime);

		camera.orthographicSize = Mathf.Min(camera.orthographicSize, (cameraConfig.VerticalClamp.y - cameraConfig.VerticalClamp.x) /2);

		Vector2 localTarget = transform.parent.InverseTransformPoint(newPosition);
		localTarget.x = Mathf.Clamp(localTarget.x, cameraConfig.HorizontalClamp.x + camera.orthographicSize*camera.aspect, cameraConfig.HorizontalClamp.y - camera.orthographicSize*camera.aspect);
		localTarget.y = Mathf.Clamp(localTarget.y, cameraConfig.VerticalClamp.x + camera.orthographicSize, cameraConfig.VerticalClamp.y - camera.orthographicSize);
		Vector3 clampedPos = transform.parent.TransformPoint(localTarget);
		clampedPos.z = transform.position.z;
		transform.position = clampedPos;

	}
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.parent.position + new Vector3(cameraConfig.HorizontalClamp.x, cameraConfig.VerticalClamp.x,0), 1);
        Gizmos.DrawSphere(transform.parent.position + new Vector3(cameraConfig.HorizontalClamp.x, cameraConfig.VerticalClamp.y,0), 1);
        Gizmos.DrawSphere(transform.parent.position + new Vector3(cameraConfig.HorizontalClamp.y, cameraConfig.VerticalClamp.y,0), 1);
        Gizmos.DrawSphere(transform.parent.position + new Vector3(cameraConfig.HorizontalClamp.y, cameraConfig.VerticalClamp.x,0), 1);
	}
}