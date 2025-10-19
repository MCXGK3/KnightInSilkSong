using System;
using System.Reflection;
using UnityEngine;
namespace Knight;

[Serializable]
public class HeroControllerStates
{
	public bool facingRight;

	public bool onGround;

	public bool jumping;

	public bool wallJumping;

	public bool doubleJumping;

	public bool nailCharging;

	public bool shadowDashing;

	public bool swimming;

	public bool falling;

	public bool dashing;

	public bool superDashing;

	public bool superDashOnWall;

	public bool backDashing;

	public bool touchingWall;

	public bool wallSliding;

	public bool transitioning;

	public bool attacking;

	public bool lookingUp;

	public bool lookingDown;

	public bool lookingUpAnim;

	public bool lookingDownAnim;

	public bool altAttack;

	public bool upAttacking;

	public bool downAttacking;

	public bool bouncing;

	public bool shroomBouncing;

	public bool recoilingRight;

	public bool recoilingLeft;

	public bool dead;

	public bool hazardDeath;

	public bool hazardRespawning;

	public bool willHardLand;

	public bool recoilFrozen;

	public bool recoiling;

	public bool invulnerable;

	public bool casting;

	public bool castRecoiling;

	public bool preventDash;

	public bool preventBackDash;

	public bool dashCooldown;

	public bool backDashCooldown;

	public bool nearBench;

	public bool inWalkZone;

	public bool isPaused;

	public bool onConveyor;

	public bool onConveyorV;

	public bool inConveyorZone;

	public bool spellQuake;

	public bool freezeCharge;

	public bool focusing;

	public bool inAcid;

	public bool slidingLeft;

	public bool slidingRight;

	public bool touchingNonSlider;

	public bool wasOnGround;

	public HeroControllerStates()
	{
		facingRight = false;
		Reset();
	}

	public bool GetState(string stateName)
	{
		FieldInfo field = GetType().GetField(stateName);
		if (field != null)
		{
			return (bool)field.GetValue(HeroController.instance.cState);
		}
		Debug.LogError("HeroControllerStates: Could not find bool named" + stateName + "in cState");
		return false;
	}

	public void SetState(string stateName, bool value)
	{
		FieldInfo field = GetType().GetField(stateName);
		if (field != null)
		{
			try
			{
				field.SetValue(HeroController.instance.cState, value);
				return;
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to set cState: " + ex);
				return;
			}
		}
		Debug.LogError("HeroControllerStates: Could not find bool named" + stateName + "in cState");
	}

	public void Reset()
	{
		onGround = false;
		jumping = false;
		falling = false;
		dashing = false;
		backDashing = false;
		touchingWall = false;
		wallSliding = false;
		transitioning = false;
		attacking = false;
		lookingUp = false;
		lookingDown = false;
		altAttack = false;
		upAttacking = false;
		downAttacking = false;
		bouncing = false;
		dead = false;
		hazardDeath = false;
		willHardLand = false;
		recoiling = false;
		recoilFrozen = false;
		invulnerable = false;
		casting = false;
		castRecoiling = false;
		preventDash = false;
		preventBackDash = false;
		dashCooldown = false;
		backDashCooldown = false;
	}
}
