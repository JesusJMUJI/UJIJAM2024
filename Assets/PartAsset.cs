using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Part", menuName = "ScriptableObjects/CreaturePart", order = 1)]
public class PartAsset : ScriptableObject
{
	[SerializeField] public ConnectablePart editorObject;
	[SerializeField] public Transform previewObject;
	[SerializeField] public float health;
	[SerializeField] public float damage;
	[SerializeField] public float mass = 1;
	[SerializeField] public float speed = 200;
	[System.NonSerialized] public float attackSpeedFactor = 3;
	[System.NonSerialized] public float attackMotorFactor = 15f;
	[SerializeField] public Vector2 angleRange;
}
