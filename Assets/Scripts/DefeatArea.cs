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
	MachinegunController _machinegun;

	System.Random _rand = new();
	private void Start()
	{
		_ammo = FindAnyObjectByType<AmmoStatus>();
		_machinegun = FindAnyObjectByType<MachinegunController>();
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
			zombie.MissionSuccess();
			OnDamaged.Invoke(HP);
			if (HP <= 0)
			{
				OnDefeated.Invoke();
			}
		}
		else
		{
			zombie.MissionSuccess();

			var ammoToAdd = zombie.AmmoCounts.Choice(_rand);
			_ammo.AddAmmo(ammoToAdd);
			_machinegun.AddWaterTemperature(-zombie.HeatRemoval);
		}

	}
}
