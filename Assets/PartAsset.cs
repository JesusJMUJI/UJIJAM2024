using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Part", menuName = "ScriptableObjects/CreaturePart", order = 1)]
public class PartAsset : ScriptableObject
{
	[SerializeField] ConnectablePart partObject;
	[SerializeField] float health;
	[SerializeField] float damage;
}
