using MarkusSecundus.Utils.Behaviors.Cosmetics;
using MarkusSecundus.Utils.Behaviors.GUI;
using MarkusSecundus.Utils.Primitives;
using MarkusSecundus.Utils.Time;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MachinegunController : MonoBehaviour
{
    [SerializeField] Transform Rotatable;
    [SerializeField] TMProFormatter ammoCountLabel;
    [SerializeField] WaterTankController waterBoilingProgressBar;

    [SerializeField] int MaxAmmoCount = 100;
    [SerializeField] int CurrentAmmoCount = 50;
    [SerializeField] float WaterTemperatureIncreasePerShot = 0.03f;
    [SerializeField] float WaterTemperatureDecreasePerSecond = 0.1f;
    [SerializeField] float CurrentWaterTemeperature = 0f;

    void Start()
    {
        UpdateAmmoCountUI();
        UpdateWaterTemepratureUI();
	}

    void Update()
    {
        var mousePos = Mouse.current.position.ReadValue();
        var mouseRay = Camera.main.ScreenPointToRay(mousePos);
        Debug.DrawRay(mouseRay.origin, mouseRay.direction);
        if (Physics.Raycast(mouseRay, out var hitInfo, float.MaxValue, (1<<6)))
        {
            var rotation = Quaternion.LookRotation(hitInfo.point - Rotatable.position, Vector3.up);
            Rotatable.rotation = rotation;
        }
        var mouseVal = Mouse.current.leftButton.ReadValue();
        if(mouseVal != 0f) DoShoot(hitInfo);

        UpdateWaterTemeprature();

	}


    [SerializeField] float _timeBetweenShots_seconds = 0.4f;
    [SerializeField] int _shootParticlesCount = 5;
    EventTimestamp _shootTimestamp = EventTimestamp.Make();
    void DoShoot(in RaycastHit hitInfo)
    {
        if (CurrentAmmoCount <= 0) return;
        if (!_shootTimestamp.TryConsume(_timeBetweenShots_seconds)) return;

        GetComponentInChildren<VisualEffectsHelper>().EmitParticles(_shootParticlesCount);

        CurrentWaterTemeperature = (CurrentWaterTemeperature + WaterTemperatureIncreasePerShot).Clamp01();
        --CurrentAmmoCount;
        UpdateAmmoCountUI();

        //Debug.Log($"{_shootTimestamp.LastTimestamp_seconds}, Shooting!", this);
    }

    void UpdateWaterTemeprature()
    {
        CurrentWaterTemeperature = (CurrentWaterTemeperature - WaterTemperatureDecreasePerSecond * Time.deltaTime).Clamp01();

        UpdateWaterTemepratureUI();
    }

    void UpdateWaterTemepratureUI()
	{
		waterBoilingProgressBar.SetValue(Mathf.Lerp(waterBoilingProgressBar.Value, CurrentWaterTemeperature, Time.deltaTime));
	}

    void UpdateAmmoCountUI()
    {
        this.ammoCountLabel.SetText(CurrentAmmoCount);
    }
}
