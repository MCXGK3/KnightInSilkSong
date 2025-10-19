using GlobalEnums;
using UnityEngine;
namespace Knight;

public class HeroAnimationController : MonoBehaviour
{
	public tk2dSpriteAnimator animator;

	private HeroController heroCtrl;

	private HeroControllerStates cState;

	private PlayerData pd;

	[HideInInspector]
	public bool playLanding;

	private bool playRunToIdle;

	private bool playDashToIdle;

	private bool playBackDashToIdleEnd;

	public bool wasAttacking;

	private bool wasFacingRight;

	[HideInInspector]
	public bool setEntryAnim;

	private bool changedClipFromLastFrame;

	private bool attackComplete;

	public ActorStates actorState { get; private set; }

	public ActorStates prevActorState { get; private set; }

	public ActorStates stateBeforeControl { get; private set; }

	public bool controlEnabled { get; private set; }

	private void Awake()
	{
		animator = GetComponent<tk2dSpriteAnimator>();
		heroCtrl = GetComponent<HeroController>();
		cState = heroCtrl.cState;
	}

	private void Start()
	{
		pd = PlayerData.instance;
		ResetAll();
		actorState = heroCtrl.hero_state;
		if (controlEnabled)
		{
			if (heroCtrl.hero_state == ActorStates.airborne)
			{
				PlayFromFrame("Airborne", 7);
			}
			else
			{
				PlayIdle();
			}
		}
		else
		{
			animator.Stop();
		}
	}

	private void Update()
	{
		if (controlEnabled)
		{
			UpdateAnimation();
		}
		else if (cState.facingRight)
		{
			wasFacingRight = true;
		}
		else
		{
			wasFacingRight = false;
		}
		if (pd.betaEnd)
		{
			animator.Play("Run");
		}
	}

	private void ResetAll()
	{
		playLanding = false;
		playRunToIdle = false;
		playDashToIdle = false;
		wasFacingRight = false;
		controlEnabled = true;
	}

	private void ResetPlays()
	{
		playLanding = false;
		playRunToIdle = false;
		playDashToIdle = false;
	}

	public void UpdateState(ActorStates newState)
	{
		if (controlEnabled && newState != actorState)
		{
			if (actorState == ActorStates.airborne && newState == ActorStates.idle && !playLanding)
			{
				playLanding = true;
			}
			if (actorState == ActorStates.running && newState == ActorStates.idle && !playRunToIdle && !cState.inWalkZone && !cState.attacking)
			{
				playRunToIdle = true;
			}
			prevActorState = actorState;
			actorState = newState;
		}
	}

	public void PlayClip(string clipName)
	{
		if (controlEnabled)
		{
			if (clipName == "Exit Door To Idle")
			{
				animator.AnimationCompleted = AnimationCompleteDelegate;
			}
			Play(clipName);
		}
	}

	private void UpdateAnimation()
	{
		changedClipFromLastFrame = false;
		if (playLanding)
		{
			Play("Land");
			animator.AnimationCompleted = AnimationCompleteDelegate;
			playLanding = false;
		}
		if (playRunToIdle)
		{
			Play("Run To Idle");
			animator.AnimationCompleted = AnimationCompleteDelegate;
			playRunToIdle = false;
		}
		if (playBackDashToIdleEnd)
		{
			Play("Backdash Land 2");
			animator.AnimationCompleted = AnimationCompleteDelegate;
			playBackDashToIdleEnd = false;
		}
		if (playDashToIdle)
		{
			Play("Dash To Idle");
			animator.AnimationCompleted = AnimationCompleteDelegate;
			playDashToIdle = false;
		}
		if (actorState == ActorStates.no_input)
		{
			if (cState.recoilFrozen)
			{
				Play("Stun");
			}
			else if (cState.recoiling)
			{
				Play("Recoil");
			}
			else if (cState.transitioning)
			{
				if (cState.onGround)
				{
					if (heroCtrl.transitionState == HeroTransitionState.EXITING_SCENE)
					{
						if (!animator.IsPlaying("Run"))
						{
							if (!pd.equippedCharm_37)
							{
								Play("Run");
							}
							else
							{
								Play("Sprint");
							}
						}
					}
					else if (heroCtrl.transitionState == HeroTransitionState.ENTERING_SCENE)
					{
						if (!pd.equippedCharm_37)
						{
							if (!animator.IsPlaying("Run"))
							{
								PlayFromFrame("Run", 3);
							}
						}
						else
						{
							Play("Sprint");
						}
					}
				}
				else if (heroCtrl.transitionState == HeroTransitionState.EXITING_SCENE)
				{
					if (!animator.IsPlaying("Airborne"))
					{
						PlayFromFrame("Airborne", 7);
					}
				}
				else if (heroCtrl.transitionState == HeroTransitionState.WAITING_TO_ENTER_LEVEL)
				{
					if (!animator.IsPlaying("Airborne"))
					{
						PlayFromFrame("Airborne", 7);
					}
				}
				else if (heroCtrl.transitionState == HeroTransitionState.ENTERING_SCENE && !setEntryAnim)
				{
					if (heroCtrl.gatePosition == GatePosition.top)
					{
						PlayFromFrame("Airborne", 7);
					}
					else if (heroCtrl.gatePosition == GatePosition.bottom)
					{
						PlayFromFrame("Airborne", 3);
					}
					setEntryAnim = true;
				}
			}
		}
		else if (setEntryAnim)
		{
			setEntryAnim = false;
		}
		else if (cState.dashing)
		{
			if (heroCtrl.dashingDown)
			{
				if (cState.shadowDashing)
				{
					if (pd.equippedCharm_16)
					{
						Play("Shadow Dash Down Sharp");
					}
					else
					{
						Play("Shadow Dash Down");
					}
				}
				else
				{
					Play("Dash Down");
				}
			}
			else if (cState.shadowDashing)
			{
				if (pd.equippedCharm_16)
				{
					Play("Shadow Dash Sharp");
				}
				else
				{
					Play("Shadow Dash");
				}
			}
			else
			{
				Play("Dash");
			}
		}
		else if (cState.backDashing)
		{
			Play("Back Dash");
		}
		else if (cState.attacking)
		{
			if (cState.upAttacking)
			{
				Play("UpSlash");
			}
			else if (cState.downAttacking)
			{
				Play("DownSlash");
			}
			else if (cState.wallSliding)
			{
				Play("Wall Slash");
			}
			else if (!cState.altAttack)
			{
				Play("Slash");
			}
			else
			{
				Play("SlashAlt");
			}
		}
		else if (cState.casting)
		{
			Play("Fireball");
		}
		else if (cState.wallSliding)
		{
			Play("Wall Slide");
		}
		else if (actorState == ActorStates.idle)
		{
			if (cState.lookingUpAnim && !animator.IsPlaying("LookUp"))
			{
				Play("LookUp");
			}
			else if (CanPlayLookDown())
			{
				Play("LookDown");
			}
			else if (!cState.lookingUpAnim && !cState.lookingDownAnim && CanPlayIdle())
			{
				PlayIdle();
			}
		}
		else if (actorState == ActorStates.running)
		{
			if (!animator.IsPlaying("Turn"))
			{
				if (cState.inWalkZone)
				{
					if (!animator.IsPlaying("Walk"))
					{
						Play("Walk");
					}
				}
				else
				{
					PlayRun();
				}
			}
		}
		else if (actorState == ActorStates.airborne)
		{
			if (cState.swimming)
			{
				Play("Swim");
			}
			else if (heroCtrl.wallLocked)
			{
				Play("Walljump");
			}
			else if (cState.doubleJumping)
			{
				Play("Double Jump");
			}
			else if (cState.jumping)
			{
				if (!animator.IsPlaying("Airborne"))
				{
					PlayFromFrame("Airborne", 0);
				}
			}
			else if (cState.falling)
			{
				if (!animator.IsPlaying("Airborne"))
				{
					PlayFromFrame("Airborne", 5);
				}
			}
			else if (!animator.IsPlaying("Airborne"))
			{
				PlayFromFrame("Airborne", 3);
			}
		}
		else if (actorState == ActorStates.dash_landing)
		{
			Play("Dash Down Land");
		}
		else if (actorState == ActorStates.hard_landing)
		{
			Play("HardLand");
		}
		if (cState.facingRight)
		{
			if (!wasFacingRight && cState.onGround && canPlayTurn())
			{
				Play("Turn");
			}
			wasFacingRight = true;
		}
		else
		{
			if (wasFacingRight && cState.onGround && canPlayTurn())
			{
				Play("Turn");
			}
			wasFacingRight = false;
		}
		if (cState.attacking)
		{
			wasAttacking = true;
		}
		else
		{
			wasAttacking = false;
		}
		ResetPlays();
	}

	private bool CanPlayIdle()
	{
		if (!animator.IsPlaying("Land") && !animator.IsPlaying("Run To Idle") && !animator.IsPlaying("Dash To Idle") && !animator.IsPlaying("Backdash Land") && !animator.IsPlaying("Backdash Land 2") && !animator.IsPlaying("LookUpEnd") && !animator.IsPlaying("LookDownEnd") && !animator.IsPlaying("Exit Door To Idle") && !animator.IsPlaying("Wake Up Ground") && !animator.IsPlaying("Hazard Respawn"))
		{
			return true;
		}
		return false;
	}

	private bool CanPlayLookDown()
	{
		if (cState.lookingDownAnim && !animator.IsPlaying("Lookup"))
		{
			return true;
		}
		return false;
	}

	private bool canPlayTurn()
	{
		if (!animator.IsPlaying("Wake Up Ground") && !animator.IsPlaying("Hazard Respawn"))
		{
			return true;
		}
		return false;
	}

	private void AnimationCompleteDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		if (clip.name == "Land")
		{
			PlayIdle();
		}
		if (clip.name == "Run To Idle")
		{
			PlayIdle();
		}
		if (clip.name == "Backdash To Idle")
		{
			PlayIdle();
		}
		if (clip.name == "Dash To Idle")
		{
			PlayIdle();
		}
		if (clip.name == "Exit Door To Idle")
		{
			PlayIdle();
		}
	}

	public void PlayIdle()
	{
		if (pd.health == 1 && pd.healthBlue < 1)
		{
			if (pd.equippedCharm_6)
			{
				animator.Play("Idle");
			}
			else
			{
				animator.Play("Idle Hurt");
			}
		}
		else if (animator.IsPlaying("LookUp"))
		{
			animator.Play("LookUpEnd");
		}
		else if (animator.IsPlaying("LookDown"))
		{
			animator.Play("LookDownEnd");
		}
		else if (heroCtrl.wieldingLantern)
		{
			animator.Play("Lantern Idle");
		}
		else
		{
			animator.Play("Idle");
		}
	}

	private void PlayRun()
	{
		if (heroCtrl.wieldingLantern)
		{
			animator.Play("Lantern Run");
		}
		else if (pd.equippedCharm_37)
		{
			Play("Sprint");
		}
		else if (wasAttacking)
		{
			animator.PlayFromFrame("Run", 3);
		}
		else
		{
			animator.Play("Run");
		}
	}

	private void Play(string clipName)
	{
		if (clipName != animator.CurrentClip.name)
		{
			changedClipFromLastFrame = true;
		}
		animator.Play(clipName);
	}

	private void PlayFromFrame(string clipName, int frame)
	{
		if (clipName != animator.CurrentClip.name)
		{
			changedClipFromLastFrame = true;
		}
		animator.PlayFromFrame(clipName, frame);
	}

	public void StopControl()
	{
		if (controlEnabled)
		{
			controlEnabled = false;
			stateBeforeControl = actorState;
		}
	}

	public void StartControl()
	{
		actorState = heroCtrl.hero_state;
		controlEnabled = true;
		PlayIdle();
	}

	public void StartControlWithoutSettingState()
	{
		controlEnabled = true;
		if (stateBeforeControl == ActorStates.running && actorState == ActorStates.running)
		{
			actorState = ActorStates.idle;
		}
	}

	public void FinishedDash()
	{
		playDashToIdle = true;
	}

	public void StopAttack()
	{
		if (animator.IsPlaying("UpSlash") || animator.IsPlaying("DownSlash"))
		{
			animator.Stop();
		}
	}

	public float GetCurrentClipDuration()
	{
		return (float)animator.CurrentClip.frames.Length / animator.CurrentClip.fps;
	}

	public float GetClipDuration(string clipName)
	{
		if (animator == null)
		{
			animator = GetComponent<tk2dSpriteAnimator>();
		}
		tk2dSpriteAnimationClip clipByName = animator.GetClipByName(clipName);
		if (clipByName == null)
		{
			Debug.LogError("HeroAnim: Could not find animation clip with the name " + clipName);
			return -1f;
		}
		return (float)clipByName.frames.Length / clipByName.fps;
	}
}
