using DG.Tweening;
using MarkusSecundus.Utils.Behaviors.Cosmetics;
using MarkusSecundus.Utils.Behaviors.GUI;
using MarkusSecundus.Utils.Extensions;
using UnityEngine;

public class AmmoStatus : MonoBehaviour
{
	[SerializeField] TMProFormatter AmmoCountDisplay;
	[SerializeField] TMProFormatter AmmoAddedDisplay;
	[SerializeField] int MaxAmmoCount = 100;

	[field: SerializeField] public int AmmoCount { get; private set; } = 50;

	private void Start()
	{
		_ammoAddedEffect = AmmoAddedDisplay.GetComponent<FadeEffect>();
		UpdateAmmoUI();
	}

	FadeEffect _ammoAddedEffect;

	public void AddAmmo(int AmmountToAdd)
	{
		AmmoCount += AmmountToAdd;
		if (AmmoCount > MaxAmmoCount) AmmoCount = MaxAmmoCount;
		UpdateAmmoUI();

		AmmoAddedDisplay.SetTextWithIntArgument(AmmountToAdd);
		_ammoAddedEffect.Stop();
		_ammoAddedEffect.FadeIn(() => _ammoAddedEffect.InvokeWithDelay(() => _ammoAddedEffect.FadeOut(), 2f));
		
	}
	public void ConsumeAmmo(int ammount)
	{
		AmmoCount -= ammount;
		if (AmmoCount < 0) AmmoCount = 0;
		EndStatsContainer.Instance.AmmoUsed += ammount;
		UpdateAmmoUI();
	}

	void UpdateAmmoUI()
	{
		AmmoCountDisplay.SetTextWithIntArgument(AmmoCount);
	}
}
