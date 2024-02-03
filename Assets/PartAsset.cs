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
}
