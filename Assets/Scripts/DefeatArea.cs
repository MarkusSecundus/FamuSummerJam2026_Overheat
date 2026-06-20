using UnityEngine;
using UnityEngine.Events;

public class DefeatArea : MonoBehaviour
{
	[SerializeField] public int HP = 4;

	[SerializeField] UnityEvent<int> OnDamaged;
	[SerializeField] UnityEvent OnDefeated;

	private void Start()
	{
		OnDamaged.Invoke(HP);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (HP <= 0) return;

		var zombie = other.GetComponentInParent<ZombieController>();
		if (!zombie) return;

		HP -= zombie.Damage;
		zombie.DoDamage(zombie.HP); // kill the zombie
		OnDamaged.Invoke(HP);
		if(HP <= 0)
		{
			OnDefeated.Invoke();
		}
	}
}
