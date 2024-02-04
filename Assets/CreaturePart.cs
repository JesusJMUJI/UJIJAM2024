using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePart : MonoBehaviour
{
    const float realPhysicsTime = 0.02f;
    const float realTime = 1;
	static float speedUpTime;
	static bool slowedDown;

    static void SetTimeScale(float newTimeScale){
        float delta = Time.timeScale/newTimeScale;
        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = newTimeScale * (realPhysicsTime/realTime);
    }
	Rigidbody2D rb;
	public Rigidbody2D GetRigidbody(){
		return rb;
	}
	void Awake(){
		rb = GetComponent<Rigidbody2D>();
	}
	[SerializeField] float health;
	[SerializeField] PartAsset asset;
	public void AssignAsset(PartAsset _asset){
		asset = _asset;
		health = asset.health;
	}
	[SerializeField] Creature creature;
	public void AssignCreature(Creature _creature){
		creature = _creature;
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.layer == gameObject.layer){
			return;
		}
		CreaturePart otherPart = col.gameObject.GetComponentInParent<CreaturePart>();
		if (!otherPart)
		{
			return;
		}
		otherPart.Damage(asset.damage);
		Vector2 delta = transform.position - col.gameObject.transform.position;

		creature.ApplyImpulse(delta, transform.position);

		if(!slowedDown){
			slowedDown = true;
			SetTimeScale(0.1f);
		}
		speedUpTime = Time.realtimeSinceStartup + 0.2f;
	}
	void Update(){
		if(!slowedDown){return;}
		if(Time.realtimeSinceStartup > speedUpTime){
			SetTimeScale(1);
			slowedDown = false;
		}
	}
	public void Damage(float damage)
	{
		if (health > 0)
		{
			health -= damage;
			if (health <= 0)
			{
				Destroy(gameObject);
			}
		}
	}
}
