using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePart : MonoBehaviour
{
	public GameObject hitEffect;
	public GameObject deathEffect;
	static float speedUpTime;
	static bool slowedDown;
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
		if(col.relativeVelocity.magnitude > 15){
			Instantiate(hitEffect, transform.position,Quaternion.Euler(0,0,Random.Range(-45,45)));
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
			SlowMotion.LerpSlowDown(0.1f, 0.05f);
		}
		speedUpTime = Time.realtimeSinceStartup + 0.1f;
	}
	void Update(){
		if(!slowedDown){return;}
		if(Time.realtimeSinceStartup > speedUpTime){
			SlowMotion.LerpSpeedUp(0.2f);
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
				Instantiate(deathEffect, transform.position,Quaternion.Euler(0,0,Random.Range(-45,45)));
			}
		}
	}
}
