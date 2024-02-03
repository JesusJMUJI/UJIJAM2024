using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
	protected bool active = false;
	[SerializeField] protected WorldCamera camera;
	public void Enable(){
		camera.Enable();
		active = true;
		OnEnabled();
	}
	protected virtual void OnEnabled(){

	}
	public void Disable(){
		camera.Disable();
		active = false;
		OnDisabled();
	}
	protected virtual void OnDisabled(){

	}
}