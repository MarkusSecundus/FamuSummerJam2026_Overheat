using UnityEngine;

public class ZombieSpawnPoint : MonoBehaviour
{
	[SerializeField] Transform _direction;

	public Vector3 Direction => (_direction.position - transform.position).normalized;

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, _direction.position);
	}
}
