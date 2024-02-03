using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
	[SerializeField] protected WorldCamera camera;
	public void Enable(){
		camera.Enable();
		OnEnabled();
	}
	protected virtual void OnEnabled(){

	}
	public void Disable(){
		camera.Disable();
		OnDisabled();
	}
	protected virtual void OnDisabled(){

	}
}