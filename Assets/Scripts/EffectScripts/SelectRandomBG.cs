﻿using UnityEngine;
using System.Collections;

public class SelectRandomBG : MonoBehaviour {

	void Start () {
		transform.GetChild(Random.Range(0, transform.childCount)).gameObject.SetActive(true);
	}
}
