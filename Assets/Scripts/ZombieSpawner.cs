using MarkusSecundus.Utils.Primitives;
using MarkusSecundus.Utils.Randomness;
using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
	[SerializeField] ZombieSpawnPoint[] SpawnPoints;
	[SerializeField] Interval<float> TimeBetweenSpawns;
	[SerializeField] ZombieController[] ZombiePrefabs;
	[SerializeField] int MaxZombieCount = 8;
	[SerializeField] Interval<float> ZombieSpeeds;

	[SerializeField] Transform _spawnParent;

	System.Random rand = new();
	private void Start()
	{

		StartCoroutine(RunSpawnLoop());
		Camera.main.useOcclusionCulling = false;
	}

	IEnumerator RunSpawnLoop()
	{
		while (true)
		{
			SpawnSomething();
			float toWait = rand.Next(TimeBetweenSpawns);
			yield return new WaitForSeconds(toWait);
		}
	}

	void SpawnSomething()
	{
		if(_spawnParent.childCount >= MaxZombieCount)
		{
			return;
		}
		var zombiePrefab = ZombiePrefabs.Choice(rand);
		var spawnPoint = SpawnPoints.Choice(rand);

		var spawned = Instantiate(zombiePrefab, spawnPoint.transform.position, zombiePrefab.transform.rotation, _spawnParent);
		spawned.MovementDirection = spawnPoint.Direction;
		spawned.Randomize(rand);
		spawned.gameObject.SetActive(true);

	}
}
