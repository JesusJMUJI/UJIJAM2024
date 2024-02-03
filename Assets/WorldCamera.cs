using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCamera : MonoBehaviour
{
	static WorldCamera instance;
	Camera camera;
	void Awake()
	{
		camera = GetComponent<Camera>();
	}
	public static Vector2 GetWorldMousePos(){
		return instance.camera.ScreenToWorldPoint(Input.mousePosition);
	}
	public static Camera GetCamera(){
		return instance.camera;
	}
	public void Disable()
	{
		camera.enabled = false;
	}

	public void Enable()
	{
		if(instance != null){
			instance.Disable();
		}
		instance = this;
		camera.enabled = true;
	}

}
