using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	void Awake(){
		if(instance != null){
			Debug.LogError("Multiple GameManagers active!");
		}
		instance = this;
	}
	[SerializeField] PartCollection collection;
	[SerializeField] CreatureEditor editor;
	public void SwitchToEditor(){
		editor.Enable();
		editor.AssignCollection(collection);
	}
	void Start(){
		SwitchToEditor();
	}
}