using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
	public void Reset(Vector2 localPosition, float zoom){
		targetPosition = localPosition;
		transform.localPosition = new Vector3(localPosition.x,localPosition.y,-10);
		targetSize = zoom;
		camera.orthographicSize = zoom;
	}
	Camera camera;
	[SerializeField] float scrollSpeed;
	[SerializeField] float scrollLerpSpeed;
	[SerializeField] Vector2 sizeClamp;
	[SerializeField] float panLerpSpeed;
	[SerializeField] float panSpeed;
	[SerializeField] Vector2 HorizontalClamp;
	[SerializeField] Vector2 VerticalClamp;

	float targetSize;
	// Start is called before the first frame update
	void Awake()
	{
		camera = gameObject.GetComponent<Camera>();
		targetSize = camera.orthographicSize;
		lastCursorPosition = camera.ScreenToWorldPoint(Input.mousePosition);
		targetPosition = transform.localPosition;
	}
	Vector2 lastCursorPosition;
	Vector3 targetPosition;
	// Update is called once per frame
	void LateUpdate()
	{

		if(!CreatureEditor.isEditing){
			targetSize -= Input.mouseScrollDelta.y*scrollSpeed*targetSize;
		}
		
		
		targetSize = Mathf.Clamp(targetSize,sizeClamp.x,sizeClamp.y);
		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetSize, scrollLerpSpeed*Time.deltaTime);
		Vector2 CursorPosition = camera.ScreenToWorldPoint(Input.mousePosition);
		if(Input.GetMouseButtonDown(1)){
			lastCursorPosition = CursorPosition;
		}
		else if(Input.GetMouseButton(1)){
			
			Vector2 cursorDelta = CursorPosition - lastCursorPosition;
			targetPosition -= new Vector3(cursorDelta.x, cursorDelta.y, 0)*panSpeed;
			targetPosition.x = Mathf.Clamp(targetPosition.x, HorizontalClamp.x, HorizontalClamp.y);
			targetPosition.y = Mathf.Clamp(targetPosition.y, VerticalClamp.x, VerticalClamp.y);
			lastCursorPosition = CursorPosition;
			
		}
		targetPosition.z = transform.localPosition.z;
		transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, panLerpSpeed*Time.deltaTime);	
		
		
	}
}
// Vector2 cursorPosition = Input.mousePosition;
// cursorPosition.x /= Screen.width;
// cursorPosition.y /= Screen.height;
// cursorPosition -= new Vector2(0.5f,0.5f);
// Debug.Log(cursorPosition);