using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public int cycleIndex = 0;
	public static GameManager instance;
	void Awake(){
		if(instance != null){
			Debug.LogError("Multiple GameManagers active!");
		}
		instance = this;
	}
	[SerializeField] CollectionPicker collectionPicker;
	[SerializeField] CreatureEditor editor;
	[SerializeField] BattleManager battleManager;
	public void SwitchToEditor(PartPreview[] selectedParts, Vector2 relativePosition){
		collectionPicker.Disable();
		editor.AssignCollection(selectedParts, relativePosition);
		editor.Enable();
	}
	public void SwitchToCollectionPicker(){
		cycleIndex++;
		battleManager.Disable();
		collectionPicker.Enable();

	}
	public void SwitchToBattle(Creature playerCreature){
		collectionPicker.Disable();
		battleManager.SetPlayerCreature(playerCreature);
		battleManager.Enable();
	}
	void Start(){
		SwitchToCollectionPicker();
	}
}