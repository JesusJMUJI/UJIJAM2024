using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Environment
{

	const float realPhysicsTime = 0.02f;
    const float realTime = 1;

    void SetTimeScale(float newTimeScale){
        float delta = Time.timeScale/newTimeScale;
        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = newTimeScale * (realPhysicsTime/realTime);
    }
	protected override void OnEnabled(){
		cameraController.enabled = true;
		LoadEnemy();
		//TODO reset camera
	}
	protected override void OnDisabled(){
		cameraController.enabled = false;
		if(enemyCreature){
			Destroy(enemyCreature.gameObject);
		}
		if(playerCreature){
			Destroy(playerCreature.gameObject);
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
	void Update(){
		if(!active){return;}
		if(enemyCreature == null){
			return;
		}
		if(playerCreature == null){
			return;
		}
		enemyCreature.UpdateParts();
		playerCreature.UpdateParts();
		enemyCreature.PullTowards(playerCreature.GetCenter());

		playerCreature.PullTowards(enemyCreature.GetCenter());
		if(enemyCreature.GetParts().Length == 0){
			GameManager.instance.SwitchToCollectionPicker();
			SetTimeScale(1);
		}
		else if(playerCreature.GetParts().Length == 0){
			//Game Over
			SetTimeScale(1);
		}
	}
	[System.Serializable]
	public class EnemyTier{
		[SerializeField] Creature[] enemyCreatures;
		public Creature GetRandom(){
			return enemyCreatures[Random.Range(0,enemyCreatures.Length)];
		}
	}
}
