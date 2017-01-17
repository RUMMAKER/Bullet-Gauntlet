using UnityEngine;
using System.Collections;

public static class ShipFactory {
	private static Vector3 playerSpawn = new Vector3(0,-2,0);
	public static GameObject MakePlayerShip(int x){
		return GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player/Ship_"+x), playerSpawn, Quaternion.identity) as GameObject;
	}
}
