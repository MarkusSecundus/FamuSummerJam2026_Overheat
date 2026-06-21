using MarkusSecundus.Utils.Primitives;
using MarkusSecundus.Utils.Randomness;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZombieSpawner : MonoBehaviour
{
	[SerializeField] ZombieSpawnPoint[] SpawnPoints;
	[SerializeField] List<float> TimeBetweenSpawns;
	[SerializeField] ZombieController[] ZombiePrefabs;
	[SerializeField] int MaxZombieCount = 8;
	[SerializeField] Interval<float> ZombieSpeeds;

	[SerializeField] Transform _spawnParent;


	System.Random rand = new();

	Shuffler<ZombieController> _zombieChoice;
	private void Start()
	{
		_zombieChoice = new Shuffler<ZombieController>(rand, ZombiePrefabs, 1);

		StartCoroutine(RunSpawnLoop());
		Camera.main.useOcclusionCulling = false;
	}

	IEnumerator RunSpawnLoop()
	{
		while (true)
		{
			SpawnSomething();
			float toWait = TimeBetweenSpawns.Choice(rand);
			yield return new WaitForSeconds(toWait);
		}
	}

	void SpawnSomething()
	{
		if(_spawnParent.childCount >= MaxZombieCount)
		{
			return;
		}
		var zombiePrefab = _zombieChoice.Next();
		var spawnPoint = SpawnPoints.Choice(rand);

		var spawned = Instantiate(zombiePrefab, spawnPoint.transform.position, zombiePrefab.transform.rotation, _spawnParent);
		spawned.MovementDirection = spawnPoint.Direction;
		spawned.Randomize(rand);
		spawned.gameObject.SetActive(true);

	}
}
