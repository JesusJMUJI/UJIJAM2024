using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Environment
{
	protected override void OnEnabled(){
		cameraController.enabled = true;
		LoadEnemy();
		//TODO reset camera
	}
	protected override void OnDisabled(){
		cameraController.enabled = false;
		if(enemyCreature){
			Destroy(enemyCreature);
		}
		if(playerCreature){
			Destroy(playerCreature);
		}
	}
	[SerializeField] CameraController cameraController;
	// [SerializeField] Transform battleContainer;
	[SerializeField] EnemyTier[] enemyTiers;
	[SerializeField] Transform playerSpawn;
	[SerializeField] Transform enemySpawn;
	Creature enemyCreature;
	Creature playerCreature;
	public void SetPlayerCreature(Creature _playerCreature){
		playerCreature = _playerCreature;
		playerCreature.transform.position = playerSpawn.position;
		TrackCreature(playerCreature);
	}
	void TrackCreature(Creature creature){
		CreaturePart[] parts = creature.GetParts();
		foreach(CreaturePart part in parts){
			cameraController.trackedObjects.Add(part.transform);
		}
	}
	void LoadEnemy(){
		enemyCreature = Instantiate(GetEnemyCreature(), enemySpawn.position, Quaternion.identity);
		TrackCreature(enemyCreature);
	}
	Creature GetEnemyCreature(){
		int enemyTierIndex = GameManager.instance.cycleIndex;
		enemyTierIndex = Mathf.Min(enemyTiers.Length-1, enemyTierIndex);
		return enemyTiers[enemyTierIndex].GetRandom();
	}
	[System.Serializable]
	public class EnemyTier{
		[SerializeField] Creature[] enemyCreatures;
		public Creature GetRandom(){
			return enemyCreatures[Random.Range(0,enemyCreatures.Length)];
		}
	}
}
