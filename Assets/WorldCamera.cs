using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCamera : MonoBehaviour
{
	static WorldCamera activeInstance;
	Camera camera;
	void Awake()
	{
		camera = GetComponent<Camera>();
	}
	public static Vector2 GetWorldMousePos(){
		return activeInstance.camera.ScreenToWorldPoint(Input.mousePosition);
	}
	public static Camera GetCamera(){
		return activeInstance.camera;
	}
	public void Disable()
	{
		camera.enabled = false;
	}

	public void Enable()
	{
		if(activeInstance != null){
			activeInstance.Disable();
		}
		activeInstance = this;
		camera.enabled = true;
	}

}
