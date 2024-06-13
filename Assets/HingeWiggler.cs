using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeWiggler : MonoBehaviour
{
	[SerializeField] float slowdownAngle = 5;
	[SerializeField] HingeJoint2D joint;
	[SerializeField] NoiseSampler sampler;
	[SerializeField] float seed;
	[SerializeField] PartAsset asset;
	bool isAttacking;
	float attackDir;
	float motorForce = 180;
	public void SetSampler(NoiseSampler _sampler){
		sampler = _sampler;
	}
	public void SetHinge(HingeJoint2D _joint){
		joint = _joint;
	}
	public void SetSeed(float _seed){
		seed = _seed;
	}
	public void SetAsset(PartAsset _asset){
		asset = _asset;
	}
	public void ToggleAttack(bool _isAttacking){
		isAttacking = _isAttacking;
		attackDir = 1-pastFase;
		if(Mathf.Abs(attackDir - 0.5f) < 0.1f){
			attackDir = Random.Range(0,1);
		}
	}
	void Start(){
		joint.useMotor = true;
	}

	float pastFase;
	void Update()
	{
		if(joint == null){return;}
		if(joint.attachedRigidbody == null){
			Destroy(joint);
			Destroy(this);
			return;
		}

		float fase = sampler.SampleAt((Vector2)transform.position * (isAttacking ? 2 : 1) + Vector2.down*seed*12.445f);
		pastFase = fase;
		fase = isAttacking ? attackDir : fase;
		
		float targetAngle = Mathf.Lerp(asset.angleRange.x, asset.angleRange.y, fase);
		float deltaAngle = Mathf.DeltaAngle(joint.jointAngle,-targetAngle);
		float angleFactor = Mathf.Clamp(deltaAngle/slowdownAngle,-1,1);
		JointMotor2D motor = joint.motor;
		motor.maxMotorTorque = motorForce * (isAttacking ? asset.attackMotorFactor : 1);
		float speed = asset.speed * (isAttacking ? asset.attackSpeedFactor : 1);
		motor.motorSpeed = Mathf.LerpUnclamped(0, speed,angleFactor);
		joint.motor = motor;
	}
}
