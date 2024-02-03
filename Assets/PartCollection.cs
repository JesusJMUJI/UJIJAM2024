using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCollection : MonoBehaviour
{
	[SerializeField] public Part[] parts;

	[System.Serializable]
	public class Part
	{
		public PartAsset asset;
		public Transform element;
	}
}
