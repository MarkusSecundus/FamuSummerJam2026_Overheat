using UnityEngine;

public class EndStatsContainer : MonoBehaviour
{
    public int AmmoUsed = 0;
    public int FriendliesKilled = 0;
    public int ZombiesKilled = 0;


	public static EndStatsContainer Instance { get; private set; }
	private void Awake()
	{
		if (Instance && Instance != this)
		{
			Destroy(this.gameObject);
			(Instance.AmmoUsed, Instance.FriendliesKilled, Instance.ZombiesKilled) = (0,0,0);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}
	}
}
