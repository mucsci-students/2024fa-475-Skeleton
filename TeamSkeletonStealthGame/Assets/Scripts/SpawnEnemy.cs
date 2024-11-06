using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
	public GameObject enemyPrefab;
    public GameObject corpsePrefab;
	public float spawnCycle = .5f;

	float elapsedTime;
	bool spawnAlive = true;

	void Start()
	{
		//manager = GetComponent<GameManager>();
	}

	void Update()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime > spawnCycle)
		{
			GameObject instance;
			if (spawnAlive)
				instance = Instantiate(enemyPrefab) as GameObject;
			else
				instance = Instantiate(corpsePrefab) as GameObject;

			Vector3 position = instance.transform.position;
			instance.transform.position = position;

			//Collidable col = temp.GetComponent<Collidable>();
			//col.manager = manager;

			elapsedTime = 0;
			spawnAlive = !spawnAlive;
		}
	}

}