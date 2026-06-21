using MarkusSecundus.Utils.Behaviors.GUI;
using UnityEngine;

public class DisplayEndStats : MonoBehaviour
{
    [SerializeField] TMProFormatter text;
    void Start()
    {
        var e = EndStatsContainer.Instance;
        text.SetText(e.AmmoUsed, e.ZombiesKilled, e.FriendliesKilled);
    }
}
