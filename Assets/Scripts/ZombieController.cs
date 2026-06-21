using DG.Tweening;
using MarkusSecundus.Utils.Behaviors.Automatization;
using MarkusSecundus.Utils.Behaviors.Cosmetics;
using MarkusSecundus.Utils.Primitives;
using MarkusSecundus.Utils.Randomness;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour, IRandomizer
{
	public Vector3 MovementDirection = Vector2.right;
	public float MovementSpeed = 1f;

	[SerializeField] Interval<float> MovementSpeedRange;
	public int Damage => 1;

	public int HP = 5;
	public bool IsZombie = true;
	public List<int> AmmoCounts;
	public float HeatRemoval = 0f;
	public bool IsDead => HP <= 0;
	public void Start()
	{
		DoWalkingAnimation(false);

	}

	private void Update()
	{
		transform.position += MovementDirection.normalized * MovementSpeed * Time.deltaTime;
	}

	[System.Serializable]
	public struct AnimationConfig
	{
		public Transform RotationRoot;
		public Quaternion LeftRotation;
		public Quaternion RightRotation;
		public float LeftRotationSegment_seconds;
		public float RightRotationSegment_seconds;
		public Ease Ease;
		public float DeathScaleDuration_seconds;

		public (Quaternion, float) Get(bool isLeft)=> isLeft ? (LeftRotation, LeftRotationSegment_seconds) : (RightRotation, RightRotationSegment_seconds);
	}
	[SerializeField] AnimationConfig Animation;

	void DoWalkingAnimation(bool isLeft)
	{
		var (rotation, duration) = Animation.Get(isLeft);
		var root = Animation.RotationRoot;
		root.DOLocalRotateQuaternion(rotation, duration).OnComplete(() => DoWalkingAnimation(!isLeft)).SetLink(this.gameObject).SetEase(Animation.Ease);
	}

	[SerializeField] VisualEffectsHelper DamagedEffect;
	
	public void DoDamage(int dmg)
	{
		if (IsDead) return;

		HP -= dmg;
		if (IsDead)
		{
			DamagedEffect.Blink();
			transform.DOScale(0f, Animation.DeathScaleDuration_seconds).OnComplete(() =>
			{
				Destroy(this.gameObject);
			});
		}
		else
		{
			DamagedEffect.Blink();
		}
	}

	public void Randomize(System.Random random)
	{
		this.MovementSpeed = random.Next(MovementSpeedRange);
	}
}
