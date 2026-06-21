using DG.Tweening;
using MarkusSecundus.Utils.Behaviors.Cosmetics;
using MarkusSecundus.Utils.Behaviors.GUI;
using MarkusSecundus.Utils.Extensions;
using MarkusSecundus.Utils.Graphics;
using MarkusSecundus.Utils.Primitives;
using MarkusSecundus.Utils.Time;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MachinegunController : MonoBehaviour
{
    [SerializeField] Transform Rotatable;
    [SerializeField] WaterTankControl waterBoilingProgressBar;

    AmmoStatus _ammoStatus;
    //[SerializeField] int MaxAmmoCount = 100;
    int CurrentAmmoCount => _ammoStatus.AmmoCount;
    [SerializeField] float WaterTemperatureIncreasePerShot = 0.03f;
    [SerializeField] float WaterTemperatureDecreasePerSecond = 0.1f;
    [SerializeField] float CurrentWaterTemeperature = 0f;
    [SerializeField] float WaterTooHotThreshold = 0.9f;
    [SerializeField] Renderer _gunMesh;
    bool IsWaterTooHot => CurrentWaterTemeperature >= WaterTooHotThreshold;
    [SerializeField] Interval<Vector2> _shootArea;

    [SerializeField] UnityEvent OnShot;
    [SerializeField] UnityEvent OnShotOutOfAmmo;
    [SerializeField] UnityEvent OnShotTooHeated;

	void Start()
    {
        _ammoStatus = FindAnyObjectByType<AmmoStatus>();
        UpdateWaterTemepratureUI();
	}

    Quaternion _desiredRotation = Quaternion.identity;
    void Update()
	{
		//Debug.Log($"rect: {_shootArea.rect}, wrect: {_shootArea.GetRect()}", this);
		var mousePos = Mouse.current.position.ReadValue();
        var mouseRay = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(mouseRay, out var hitInfo, float.MaxValue, (1<<6) | (1<<7)))
        {
            if(hitInfo.collider.gameObject.layer == 6)
			{
				Cursor.SetCursor(CursorInfo.Crosshair, CursorInfo.Crosshair.GetSize() * 0.5f, CursorMode.Auto);
				//Debug.Log($"DesiredLocation!", this);
				_desiredRotation = Quaternion.LookRotation(hitInfo.point - Rotatable.position, Vector3.up);
				Debug.DrawRay(mouseRay.origin, mouseRay.direction);
                //DrawHelpers.DrawWireSphere(hitInfo.point, 0.1f, Debug.DrawLine);
				//Rotatable.rotation = _desiredRotation;

				var mouseVal = Mouse.current.leftButton.ReadValue();
				if (mouseVal != 0f) DoShoot(hitInfo);
			}
            else
			{
				Cursor.SetCursor(CursorInfo.Normal, Vector2.zero, CursorMode.Auto);
                var area = _shootArea.Transform(t => new Vector2(t.x * Camera.main.pixelWidth, t.y * Camera.main.pixelHeight));
                
				var newMousePos = mousePos.Clamp(area);
				var newMouseRay = Camera.main.ScreenPointToRay(newMousePos);
				if (Physics.Raycast(newMouseRay, out var newHitInfo, float.MaxValue, (1<< 6)))
				{
					//Debug.Log($"area: {area}| {mousePos} => {newMousePos}", this);
					_desiredRotation = Quaternion.LookRotation(newHitInfo.point - Rotatable.position, Vector3.up);
					Debug.DrawRay(newMouseRay.origin, newMouseRay.direction);
				}
            }
        }

		Rotatable.rotation = Quaternion.Lerp(Rotatable.rotation, _desiredRotation, (Mathf.Sqrt(Time.deltaTime) * 2f).Clamp01());
		//Rotatable.rotation = Quaternion.Lerp(Rotatable.rotation, _desiredRotation, Time.deltaTime);
		UpdateWaterTemperature();

	}

    [System.Serializable]
    public struct ShotParticleInfo
	{
		[SerializeField] public ParticleSystem Particles;
		[SerializeField] public int Count;
        [SerializeField] public float DistanceAlongNormal;
        [SerializeField] public GameObject ShootSprite;
	}
    [SerializeField] ShotParticleInfo ShotParticles;

    [SerializeField] float _timeBetweenShots_seconds = 0.4f;
    [SerializeField] int _shootParticlesCount = 5;
    EventTimestamp _shootTimestamp = EventTimestamp.Make();

    [SerializeField] Vector3 _knockbackIntensity = Vector3.zero;
    [SerializeField] float _knockbackIn_seconds = 0.08f;
    [SerializeField] float _knockbackOut_seconds = 0.1f;
    void DoShoot(in RaycastHit hitInfo)
	{

		if (!_shootTimestamp.TryConsume(_timeBetweenShots_seconds)) return;

		if (CurrentAmmoCount <= 0)
        {
            OnShotOutOfAmmo.Invoke();
			return;
		}
		if (IsWaterTooHot)
        {
            OnShotTooHeated.Invoke();
            _shootTimestamp.Postpone(1.5f);
            return;
        }


		GetComponentInChildren<VisualEffectsHelper>().EmitParticles(_shootParticlesCount);

        var particlePoint = hitInfo.point + hitInfo.normal * ShotParticles.DistanceAlongNormal;
		ShotParticles.Particles.Emit(new ParticleSystem.EmitParams { position = particlePoint }, ShotParticles.Count);
        var shotSprite = GameObject.Instantiate(ShotParticles.ShootSprite, particlePoint, Quaternion.identity, this.transform);
        shotSprite.SetActive(true);

        var zombie = hitInfo.collider.GetComponentInParent<ZombieController>();
        if (zombie)
        {
            zombie.DoDamage(1);
        }
        var knockback_child = Rotatable.transform.GetChild(0);
        knockback_child.DOLocalMove(_knockbackIntensity, _knockbackIn_seconds).OnComplete(() => knockback_child.DOLocalMove(Vector3.zero, _knockbackOut_seconds));

		AddWaterTemperature(WaterTemperatureIncreasePerShot);
        _ammoStatus.ConsumeAmmo(1);

        OnShot.Invoke();
        //Debug.Log($"{_shootTimestamp.LastTimestamp_seconds}, Shooting!", this);
    }
    public void AddWaterTemperature(float increase)
	{
		CurrentWaterTemeperature = (CurrentWaterTemeperature + increase).Clamp01();
	}

    [System.Serializable] struct CursorConfig
	{
		[SerializeField] public Texture2D Normal;
		[SerializeField] public Texture2D Crosshair;
    }
    [SerializeField] CursorConfig CursorInfo;

    void UpdateWaterTemperature()
    {
        CurrentWaterTemeperature = (CurrentWaterTemeperature - WaterTemperatureDecreasePerSecond * Time.deltaTime).Clamp01();

        UpdateWaterTemepratureUI();
    }

    [SerializeField] Color hotColor;
    [SerializeField] AnimationCurve hotCurve;
    void UpdateWaterTemepratureUI()
	{
		waterBoilingProgressBar.SetValue(Mathf.Lerp(waterBoilingProgressBar.Value, CurrentWaterTemeperature, Time.deltaTime));
        _gunMesh.material.color = Color.Lerp(Color.white, hotColor, hotCurve.Evaluate(waterBoilingProgressBar.Value));
	}

}
