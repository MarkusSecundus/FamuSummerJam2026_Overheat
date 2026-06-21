using MarkusSecundus.Utils.Randomness;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DefeatArea : MonoBehaviour
{
	[SerializeField] public int HP = 4;

	[SerializeField] UnityEvent<int> OnDamaged;
	[SerializeField] UnityEvent OnDefeated;

	AmmoStatus _ammo;

	System.Random _rand = new();
	private void Start()
	{
		_ammo = FindAnyObjectByType<AmmoStatus>();
		OnDamaged.Invoke(HP);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (HP <= 0) return;

		var zombie = other.GetComponentInParent<ZombieController>();
		if (!zombie) return;
		if (zombie.IsZombie)
		{
			HP -= zombie.Damage;
			zombie.DoDamage(zombie.HP); // kill the zombie
			OnDamaged.Invoke(HP);
			if (HP <= 0)
			{
				OnDefeated.Invoke();
			}
		}
		else
		{
			zombie.DoDamage(zombie.HP); // kill the soldier

			var ammoToAdd = zombie.AmmoCounts.Choice(_rand);
			_ammo.AddAmmo(ammoToAdd);
		}

	}
}
