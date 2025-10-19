using System;
using GlobalEnums;
using UnityEngine;
namespace Knight;

public class NailSlash : MonoBehaviour
{
	public string animName;

	public Vector3 scale;

	private HeroController heroCtrl;

	private PlayMakerFSM slashFsm;

	private tk2dSpriteAnimator anim;

	private MeshRenderer mesh;

	private float slashAngle;

	private bool struck;

	private bool longnail;

	private bool mantis;

	private bool fury;

	private bool slashing;

	private int stepCounter;

	private PolygonCollider2D poly;

	private int polyCounter;

	private bool animCompleted;

	private AudioSource audio;

	private PolygonCollider2D clashTinkPoly;

	private void Awake()
	{
		try
		{
			heroCtrl = base.transform.root.GetComponent<HeroController>();
		}
		catch (NullReferenceException ex)
		{
			Debug.LogError("NailSlash: could not find HeroController on parent: " + base.transform.root.name + " " + ex);
		}
		audio = GetComponent<AudioSource>();
		anim = GetComponent<tk2dSpriteAnimator>();
		slashFsm = GetComponent<PlayMakerFSM>();
		poly = GetComponent<PolygonCollider2D>();
		mesh = GetComponent<MeshRenderer>();
		clashTinkPoly = base.transform.Find("Clash Tink").GetComponent<PolygonCollider2D>();
		poly.enabled = false;
		mesh.enabled = false;
	}

	public void StartSlash()
	{
		audio.Play();
		slashAngle = slashFsm.FsmVariables.FindFsmFloat("direction").Value;
		if (mantis && longnail)
		{
			base.transform.localScale = new Vector3(scale.x * 1.4f, scale.y * 1.4f, scale.z);
			anim.Play(animName + " M");
		}
		else if (mantis)
		{
			base.transform.localScale = new Vector3(scale.x * 1.25f, scale.y * 1.25f, scale.z);
			anim.Play(animName + " M");
		}
		else if (longnail)
		{
			base.transform.localScale = new Vector3(scale.x * 1.15f, scale.y * 1.15f, scale.z);
			anim.Play(animName);
		}
		else
		{
			base.transform.localScale = scale;
			anim.Play(animName);
		}
		if (fury)
		{
			anim.Play(animName + " F");
		}
		anim.PlayFromFrame(0);
		stepCounter = 0;
		polyCounter = 0;
		poly.enabled = false;
		clashTinkPoly.enabled = false;
		animCompleted = false;
		anim.AnimationCompleted = Disable;
		slashing = true;
		mesh.enabled = true;
	}

	private void FixedUpdate()
	{
		if (slashing)
		{
			if (stepCounter == 1)
			{
				poly.enabled = true;
				clashTinkPoly.enabled = true;
			}
			if (stepCounter >= 5 && (float)polyCounter > 0f)
			{
				poly.enabled = false;
				clashTinkPoly.enabled = false;
			}
			if (animCompleted && polyCounter > 1)
			{
				CancelAttack();
			}
			if (poly.enabled)
			{
				polyCounter++;
			}
			stepCounter++;
		}
	}

	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		if (!(otherCollider != null))
		{
			return;
		}
		if (slashAngle == 0f)
		{
			int layer = otherCollider.gameObject.layer;
			if (layer == 11 && (otherCollider.gameObject.GetComponent<NonBouncer>() == null || !otherCollider.gameObject.GetComponent<NonBouncer>().active))
			{
				if (otherCollider.gameObject.GetComponent<BounceShroom>() != null)
				{
					heroCtrl.RecoilLeftLong();
					Bounce(otherCollider, useEffects: false);
				}
				else
				{
					heroCtrl.RecoilLeft();
				}
			}
			if (layer == 19 && otherCollider.gameObject.GetComponent<BounceShroom>() != null)
			{
				heroCtrl.RecoilLeftLong();
				Bounce(otherCollider, useEffects: false);
			}
		}
		else if (slashAngle == 180f)
		{
			int layer2 = otherCollider.gameObject.layer;
			if (layer2 == 11 && (otherCollider.gameObject.GetComponent<NonBouncer>() == null || !otherCollider.gameObject.GetComponent<NonBouncer>().active))
			{
				if (otherCollider.gameObject.GetComponent<BounceShroom>() != null)
				{
					heroCtrl.RecoilRightLong();
					Bounce(otherCollider, useEffects: false);
				}
				else
				{
					heroCtrl.RecoilRight();
				}
			}
			if (layer2 == 19 && otherCollider.gameObject.GetComponent<BounceShroom>() != null)
			{
				heroCtrl.RecoilRightLong();
				Bounce(otherCollider, useEffects: false);
			}
		}
		else if (slashAngle == 90f)
		{
			int layer3 = otherCollider.gameObject.layer;
			if (layer3 == 11 && (otherCollider.gameObject.GetComponent<NonBouncer>() == null || !otherCollider.gameObject.GetComponent<NonBouncer>().active))
			{
				if (otherCollider.gameObject.GetComponent<BounceShroom>() != null)
				{
					heroCtrl.RecoilDown();
					Bounce(otherCollider, useEffects: false);
				}
				else
				{
					heroCtrl.RecoilDown();
				}
			}
			if (layer3 == 19 && otherCollider.gameObject.GetComponent<BounceShroom>() != null)
			{
				heroCtrl.RecoilDown();
				Bounce(otherCollider, useEffects: false);
			}
		}
		else
		{
			if (slashAngle != 270f)
			{
				return;
			}
			PhysLayers layer4 = (PhysLayers)otherCollider.gameObject.layer;
			if ((layer4 == PhysLayers.ENEMIES || layer4 == PhysLayers.INTERACTIVE_OBJECT || layer4 == PhysLayers.HERO_ATTACK) && (otherCollider.gameObject.GetComponent<NonBouncer>() == null || !otherCollider.gameObject.GetComponent<NonBouncer>().active))
			{
				if (otherCollider.gameObject.GetComponent<BigBouncer>() != null)
				{
					heroCtrl.BounceHigh();
				}
				else if (otherCollider.gameObject.GetComponent<BounceShroom>() != null)
				{
					heroCtrl.ShroomBounce();
					Bounce(otherCollider, useEffects: true);
				}
				else
				{
					heroCtrl.Bounce();
				}
			}
		}
	}

	private void Bounce(Collider2D otherCollider, bool useEffects)
	{
		PlayMakerFSM playMakerFSM = FSMUtility.LocateFSM(otherCollider.gameObject, "Bounce Shroom");
		if ((bool)playMakerFSM)
		{
			playMakerFSM.SendEvent("BOUNCE UPWARD");
			return;
		}
		BounceShroom component = otherCollider.GetComponent<BounceShroom>();
		if ((bool)component)
		{
			component.BounceLarge(useEffects);
		}
	}

	private void OnTriggerStay2D(Collider2D otherCollider)
	{
		OnTriggerEnter2D(otherCollider);
	}

	private void Disable(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		animCompleted = true;
	}

	public void SetLongnail(bool set)
	{
		longnail = set;
	}

	public void SetMantis(bool set)
	{
		mantis = set;
	}

	public void SetFury(bool set)
	{
		fury = set;
	}

	public void CancelAttack()
	{
		slashing = false;
		poly.enabled = false;
		clashTinkPoly.enabled = false;
		mesh.enabled = false;
	}
}
