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
	[SerializeField] CollectionPicker collectionPicker;
	[SerializeField] CreatureEditor editor;
	public void SwitchToEditor(PartCollection collection){
		collectionPicker.Disable();
		editor.Enable();
		editor.AssignCollection(collection);
	}
	public void SwitchToCollectionPicker(){
		collectionPicker.Enable();
	}
	void Start(){
		SwitchToCollectionPicker();
	}
}