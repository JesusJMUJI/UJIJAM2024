using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraLerper : MonoBehaviour
{
	float defaultSize;
	Vector3 defaultPosition;
	public void Reset(){
		camera.orthographicSize = defaultSize;
		transform.position = defaultPosition;
		target = null;
	}
	void Awake(){
		defaultSize = camera.orthographicSize;
		defaultPosition = transform.position;
	}

	Transform target = null;
	float targetSize;
	float lerpStartTime;
	float lerpEndTime;
	Vector3 startPos;
	float startSize;
	[SerializeField] Camera camera;
	[SerializeField] AnimationCurve lerpCurve;
	public void ZoomTowards(Transform _target, float _targetSize, float duration){
		target = _target;
		targetSize = _targetSize;
		lerpStartTime = Time.time;
		lerpEndTime = Time.time + duration;
		startPos = transform.position;
		startSize = WorldCamera.GetCamera().orthographicSize;
	}

	// Update is called once per frame
	void Update()
	{
		if (target != null)
		{
			float t = 1 - Mathf.Clamp01((lerpEndTime-Time.time)/(lerpEndTime-lerpStartTime));
			t = lerpCurve.Evaluate(t);
			camera.orthographicSize = Mathf.LerpUnclamped(startSize, targetSize, t);
			transform.position = Vector3.LerpUnclamped(startPos, new Vector3(target.position.x, target.position.y, -10), t);
		}
	}
	
}
