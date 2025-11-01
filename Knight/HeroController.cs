using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using HutongGames.PlayMaker;
using UnityEngine;
namespace Knight;

public class HeroController : MonoBehaviour
{
	public delegate void HeroInPosition(bool forceDirect);

	public delegate void TakeDamageEvent();

	public delegate void HeroDeathEvent();

	private bool verboseMode;


	public float RUN_SPEED;

	public float RUN_SPEED_CH;

	public float RUN_SPEED_CH_COMBO;

	public float WALK_SPEED;

	public float UNDERWATER_SPEED;

	public float JUMP_SPEED;

	public float JUMP_SPEED_UNDERWATER;

	public float MIN_JUMP_SPEED;

	public int JUMP_STEPS;

	public int JUMP_STEPS_MIN;

	public int JUMP_TIME;

	public int DOUBLE_JUMP_STEPS;

	public int WJLOCK_STEPS_SHORT;

	public int WJLOCK_STEPS_LONG;

	public float WJ_KICKOFF_SPEED;

	public int WALL_STICKY_STEPS;

	public float DASH_SPEED;

	public float DASH_SPEED_SHARP;

	public float DASH_TIME;

	public int DASH_QUEUE_STEPS;

	public float BACK_DASH_SPEED;

	public float BACK_DASH_TIME;

	public float SHADOW_DASH_SPEED;

	public float SHADOW_DASH_TIME;

	public float SHADOW_DASH_COOLDOWN;

	public float SUPER_DASH_SPEED;

	public float DASH_COOLDOWN;

	public float DASH_COOLDOWN_CH;

	public float BACKDASH_COOLDOWN;

	public float WALLSLIDE_SPEED;

	public float WALLSLIDE_DECEL;

	public float NAIL_CHARGE_TIME_DEFAULT;

	public float NAIL_CHARGE_TIME_CHARM;

	public float CYCLONE_HORIZONTAL_SPEED;

	public float SWIM_ACCEL;

	public float SWIM_MAX_SPEED;

	public float TIME_TO_ENTER_SCENE_BOT;

	public float TIME_TO_ENTER_SCENE_HOR;

	public float SPEED_TO_ENTER_SCENE_HOR;

	public float SPEED_TO_ENTER_SCENE_UP;

	public float SPEED_TO_ENTER_SCENE_DOWN;

	public float DEFAULT_GRAVITY;

	public float UNDERWATER_GRAVITY;

	public float ATTACK_DURATION;

	public float ATTACK_DURATION_CH;

	public float ALT_ATTACK_RESET;

	public float ATTACK_RECOVERY_TIME;

	public float ATTACK_COOLDOWN_TIME;

	public float ATTACK_COOLDOWN_TIME_CH;

	public float BOUNCE_TIME;

	public float BOUNCE_SHROOM_TIME;

	public float BOUNCE_VELOCITY;

	public float SHROOM_BOUNCE_VELOCITY;

	public float RECOIL_HOR_TIME;

	public float RECOIL_HOR_VELOCITY;

	public float RECOIL_HOR_VELOCITY_LONG;

	public float RECOIL_HOR_STEPS;

	public float RECOIL_DOWN_VELOCITY;

	public float RUN_PUFF_TIME;

	public float BIG_FALL_TIME;

	public float HARD_LANDING_TIME;

	public float DOWN_DASH_TIME;

	public float MAX_FALL_VELOCITY;

	public float MAX_FALL_VELOCITY_UNDERWATER;

	public float RECOIL_DURATION;

	public float RECOIL_DURATION_STAL;

	public float RECOIL_VELOCITY;

	public float DAMAGE_FREEZE_DOWN;

	public float DAMAGE_FREEZE_WAIT;

	public float DAMAGE_FREEZE_UP;

	public float INVUL_TIME;

	public float INVUL_TIME_STAL;

	public float INVUL_TIME_PARRY;

	public float INVUL_TIME_QUAKE;

	public float INVUL_TIME_CYCLONE;

	public float CAST_TIME;

	public float CAST_RECOIL_TIME;

	public float CAST_RECOIL_VELOCITY;

	public float WALLSLIDE_CLIP_DELAY;

	public int GRUB_SOUL_MP;

	public int GRUB_SOUL_MP_COMBO;

	private int JUMP_QUEUE_STEPS = 2;

	private int JUMP_RELEASE_QUEUE_STEPS = 2;

	private int DOUBLE_JUMP_QUEUE_STEPS = 10;

	private int ATTACK_QUEUE_STEPS = 5;

	private float DELAY_BEFORE_ENTER = 0.1f;

	private float LOOK_DELAY = 0.85f;

	private float LOOK_ANIM_DELAY = 0.25f;

	private float DEATH_WAIT = 2.85f;

	private float HAZARD_DEATH_CHECK_TIME = 3f;

	private float FLOATING_CHECK_TIME = 0.18f;

	private float NAIL_TERRAIN_CHECK_TIME = 0.12f;

	private float BUMP_VELOCITY = 4f;

	private float BUMP_VELOCITY_DASH = 5f;

	private int LANDING_BUFFER_STEPS = 5;

	private int LEDGE_BUFFER_STEPS = 2;

	private int HEAD_BUMP_STEPS = 3;

	private float MANTIS_CHARM_SCALE = 1.35f;

	private float FIND_GROUND_POINT_DISTANCE = 10f;

	private float FIND_GROUND_POINT_DISTANCE_EXT = 50f;

	public ActorStates hero_state;

	public ActorStates prev_hero_state;

	public HeroTransitionState transitionState;

	public DamageMode damageMode;

	public float move_input;

	public float vertical_input;

	public float controller_deadzone = 0.2f;

	public Vector2 current_velocity;

	private bool isGameplayScene;

	public bool isEnteringFirstLevel;

	public Vector2 slashOffset;

	public Vector2 upSlashOffset;

	public Vector2 downwardSlashOffset;

	public Vector2 spell1Offset;

	private int jump_steps;

	private int jumped_steps;

	private int doubleJump_steps;

	private float dash_timer;

	private float back_dash_timer;

	private float shadow_dash_timer;

	private float attack_time;

	private float attack_cooldown;

	private Vector2 transition_vel;

	private float altAttackTime;

	private float lookDelayTimer;

	private float bounceTimer;

	private float recoilHorizontalTimer;

	private float runPuffTimer;

	private float hardLandingTimer;

	private float dashLandingTimer;

	private float recoilTimer;

	private int recoilSteps;

	private int landingBufferSteps;

	private int dashQueueSteps;

	private bool dashQueuing;

	private float shadowDashTimer;

	private float dashCooldownTimer;

	private float nailChargeTimer;

	private int wallLockSteps;

	private float wallslideClipTimer;

	private float hardLandFailSafeTimer;

	private float hazardDeathTimer;

	private float floatingBufferTimer;

	private float attackDuration;

	public float parryInvulnTimer;

	[Space(6f)]
	[Header("Slash Prefabs")]
	public GameObject slashPrefab;

	public GameObject slashAltPrefab;

	public GameObject upSlashPrefab;

	public GameObject downSlashPrefab;

	public GameObject wallSlashPrefab;

	public NailSlash normalSlash;

	public NailSlash alternateSlash;

	public NailSlash upSlash;

	public NailSlash downSlash;

	public NailSlash wallSlash;

	public PlayMakerFSM normalSlashFsm;

	public PlayMakerFSM alternateSlashFsm;

	public PlayMakerFSM upSlashFsm;

	public PlayMakerFSM downSlashFsm;

	public PlayMakerFSM wallSlashFsm;

	[Space(6f)]
	[Header("Effect Prefabs")]
	public GameObject nailTerrainImpactEffectPrefab;

	public GameObject spell1Prefab;

	public GameObject takeHitPrefab;

	public GameObject takeHitDoublePrefab;

	public GameObject softLandingEffectPrefab;

	public GameObject hardLandingEffectPrefab;

	public GameObject runEffectPrefab;

	public GameObject backDashPrefab;

	public GameObject jumpEffectPrefab;

	public GameObject jumpTrailPrefab;

	public GameObject fallEffectPrefab;

	public ParticleSystem wallslideDustPrefab;

	public GameObject artChargeEffect;

	public GameObject artChargedEffect;

	public GameObject artChargedFlash;

	public tk2dSpriteAnimator artChargedEffectAnim;

	public GameObject shadowdashBurstPrefab;

	public GameObject shadowdashDownBurstPrefab;

	public GameObject dashParticlesPrefab;

	public GameObject shadowdashParticlesPrefab;

	public GameObject shadowRingPrefab;

	public GameObject shadowRechargePrefab;

	public GameObject dJumpWingsPrefab;

	public GameObject dJumpFlashPrefab;

	public ParticleSystem dJumpFeathers;

	public GameObject wallPuffPrefab;

	public GameObject sharpShadowPrefab;

	public GameObject grubberFlyBeamPrefabL;

	public GameObject grubberFlyBeamPrefabR;

	public GameObject grubberFlyBeamPrefabU;

	public GameObject grubberFlyBeamPrefabD;

	public GameObject grubberFlyBeamPrefabL_fury;

	public GameObject grubberFlyBeamPrefabR_fury;

	public GameObject grubberFlyBeamPrefabU_fury;

	public GameObject grubberFlyBeamPrefabD_fury;

	public GameObject carefreeShield;

	[Space(6f)]
	[Header("Hero Death")]
	public GameObject corpsePrefab;

	public GameObject spikeDeathPrefab;

	public GameObject acidDeathPrefab;

	public GameObject lavaDeathPrefab;

	public GameObject heroDeathPrefab;

	[Space(6f)]
	[Header("Hero Other")]
	public GameObject cutscenePrefab;

	private GameManager gm;

	private Rigidbody2D rb2d;

	private Collider2D col2d;

	private MeshRenderer renderer;

	private new Transform transform;

	private HeroAnimationController animCtrl;

	public HeroControllerStates cState;

	public PlayerData playerData;

	private HeroAudioController audioCtrl;

	private AudioSource audioSource;

	[HideInInspector]
	public UIManager ui;

	private InputHandler inputHandler;

	public PlayMakerFSM damageEffectFSM;

	private ParticleSystem dashParticleSystem;

	private InvulnerablePulse invPulse;

	private SpriteFlash spriteFlash;

	public AudioSource footStepsRunAudioSource;

	public AudioSource footStepsWalkAudioSource;

	private float prevGravityScale;

	private Vector2 recoilVector;

	private Vector2 lastInputState;

	public GatePosition gatePosition;

	private bool runMsgSent;

	private bool hardLanded;

	private bool fallRumble;

	public bool acceptingInput;

	private bool fallTrailGenerated;

	private bool drainMP;

	private float drainMP_timer;

	private float drainMP_time;

	private float MP_drained;

	private float drainMP_seconds;

	private float focusMP_amount;

	private float dashBumpCorrection;

	public bool controlReqlinquished;

	public bool enterWithoutInput;

	public bool lookingUpAnim;

	public bool lookingDownAnim;

	public bool carefreeShieldEquipped;

	private int hitsSinceShielded;

	private EndBeta endBeta;

	private int jumpQueueSteps;

	private bool jumpQueuing;

	private int doubleJumpQueueSteps;

	private bool doubleJumpQueuing;

	private int jumpReleaseQueueSteps;

	private bool jumpReleaseQueuing;

	private int attackQueueSteps;

	private bool attackQueuing;

	public bool touchingWallL;

	public bool touchingWallR;

	public bool wallSlidingL;

	public bool wallSlidingR;

	private bool airDashed;

	public bool dashingDown;

	public bool wieldingLantern;

	private bool startWithWallslide;

	private bool startWithJump;

	private bool startWithFullJump;

	private bool startWithDash;

	private bool startWithAttack;

	private bool nailArt_cyclone;

	private bool wallSlashing;

	private bool doubleJumped;

	public bool inAcid;

	private bool wallJumpedR;

	private bool wallJumpedL;

	public bool wallLocked;

	private float currentWalljumpSpeed;

	private float walljumpSpeedDecel;

	private int wallUnstickSteps;

	private bool recoilLarge;

	public float conveyorSpeed;

	public float conveyorSpeedV;

	private bool enteringVertically;

	private bool playingWallslideClip;

	private bool playedMantisClawClip;

	public bool exitedSuperDashing;

	public bool exitedQuake;

	private bool fallCheckFlagged;

	private int ledgeBufferSteps;

	private int headBumpSteps;

	private float nailChargeTime;

	public bool takeNoDamage;

	private bool joniBeam;

	public bool fadedSceneIn;

	private bool stopWalkingOut;

	private bool boundsChecking;

	private bool blockerFix;

	[SerializeField]
	private Vector2[] positionHistory;

	private bool tilemapTestActive;

	private Vector2 groundRayOriginC;

	private Vector2 groundRayOriginL;

	private Vector2 groundRayOriginR;

	private Coroutine takeDamageCoroutine;

	private Coroutine tilemapTestCoroutine;

	public AudioClip footstepsRunDust;

	public AudioClip footstepsRunGrass;

	public AudioClip footstepsRunBone;

	public AudioClip footstepsRunSpa;

	public AudioClip footstepsRunMetal;

	public AudioClip footstepsRunWater;

	public AudioClip footstepsWalkDust;

	public AudioClip footstepsWalkGrass;

	public AudioClip footstepsWalkBone;

	public AudioClip footstepsWalkSpa;

	public AudioClip footstepsWalkMetal;

	public AudioClip nailArtCharge;

	public AudioClip nailArtChargeComplete;

	public AudioClip blockerImpact;

	public AudioClip shadowDashClip;

	public AudioClip sharpShadowClip;

	public AudioClip doubleJumpClip;

	public AudioClip mantisClawClip;

	private GameObject slash;

	private NailSlash slashComponent;

	private PlayMakerFSM slashFsm;

	private GameObject runEffect;

	private GameObject backDash;

	private GameObject jumpEffect;

	private GameObject fallEffect;

	private GameObject dashEffect;

	private GameObject grubberFlyBeam;

	private GameObject hazardCorpe;

	public PlayMakerFSM vignetteFSM;

	public SpriteRenderer heroLight;

	public SpriteRenderer vignette;

	public PlayMakerFSM dashBurst;

	public PlayMakerFSM superDash;

	public PlayMakerFSM fsm_thornCounter;

	public PlayMakerFSM spellControl;

	public PlayMakerFSM fsm_fallTrail;

	public PlayMakerFSM fsm_orbitShield;

	public VibrationData softLandVibration;

	public VibrationData wallJumpVibration;

	public VibrationPlayer wallSlideVibrationPlayer;

	public VibrationData dashVibration;

	public VibrationData shadowDashVibration;

	public VibrationData doubleJumpVibration;

	public bool isHeroInPosition = true;

	private bool jumpReleaseQueueingEnabled;

	private static HeroController _instance;

	private const float PreventCastByDialogueEndDuration = 0.3f;

	private float preventCastByDialogueEndTimer;

	private Vector2 oldPos = Vector2.zero;

	public float fallTimer { get; private set; }

	// public GeoCounter geoCounter { get; private set; }

	public PlayMakerFSM proxyFSM { get; private set; }

	public TransitionPoint sceneEntryGate { get; private set; }

	public bool IsDreamReturning
	{
		get
		{
			PlayMakerFSM playMakerFSM = PlayMakerFSM.FindFsmOnGameObject(base.gameObject, "Dream Return");
			if ((bool)playMakerFSM)
			{
				FsmBool fsmBool = playMakerFSM.FsmVariables.FindFsmBool("Dream Returning");
				if (fsmBool != null)
				{
					return fsmBool.Value;
				}
			}
			return false;
		}
	}

	public static HeroController instance
	{
		get
		{
			HeroController silentInstance = SilentInstance;
			if (!silentInstance)
			{
				Debug.LogError("Couldn't find a Hero, make sure one exists in the scene.");
			}
			return silentInstance;
		}
	}

	public static HeroController SilentInstance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<HeroController>();
				if ((bool)_instance && Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(_instance.gameObject);
				}
			}
			return _instance;
		}
	}

	public static HeroController UnsafeInstance => _instance;

	public event HeroInPosition heroInPosition;

	public event TakeDamageEvent OnTakenDamage;

	public event HeroDeathEvent OnDeath;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
		else if (this != _instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SetupGameRefs();
		SetupPools();
	}

	private void Start()
	{
		heroInPosition += delegate
		{
			isHeroInPosition = true;
		};
		playerData = PlayerData.instance;
		ui = UIManager.instance;
		// geoCounter = GameCameras.instance.geoCounter;
		if (superDash == null)
		{
			Debug.Log("SuperDash came up null, locating manually");
			superDash = FSMUtility.LocateFSM(base.gameObject, "Superdash");
		}
		if (fsm_thornCounter == null)
		{
			Debug.Log("Thorn Counter came up null, locating manually");
			fsm_thornCounter = FSMUtility.LocateFSM(transform.Find("Charm Effects").gameObject, "Thorn Counter");
		}
		if (dashBurst == null)
		{
			Debug.Log("DashBurst came up null, locating manually");
			dashBurst = FSMUtility.GetFSM(transform.Find("Effects").Find("Dash Burst").gameObject);
		}
		if (spellControl == null)
		{
			Debug.Log("SpellControl came up null, locating manually");
			spellControl = FSMUtility.LocateFSM(base.gameObject, "Spell Control");
		}
		if (playerData.equippedCharm_26)
		{
			nailChargeTime = NAIL_CHARGE_TIME_CHARM;
		}
		else
		{
			nailChargeTime = NAIL_CHARGE_TIME_DEFAULT;
		}
		if (gm.IsGameplayScene())
		{
			isGameplayScene = true;
			vignette.enabled = true;
			if (this.heroInPosition != null)
			{
				this.heroInPosition(forceDirect: false);
			}
			FinishedEnteringScene();
		}
		else
		{
			isGameplayScene = false;
			transform.SetPositionY(-2000f);
			vignette.enabled = false;
			AffectedByGravity(gravityApplies: false);
		}
		CharmUpdate();
		if ((bool)acidDeathPrefab)
		{
			ObjectPool.CreatePool(acidDeathPrefab, 1);
		}
		if ((bool)spikeDeathPrefab)
		{
			ObjectPool.CreatePool(spikeDeathPrefab, 1);
		}
	}

	public void SceneInit()
	{
		if (!(this == _instance))
		{
			return;
		}
		if (!gm)
		{
			gm = GameManager.instance;
		}
		if (gm.IsGameplayScene())
		{
			isGameplayScene = true;
			HeroBox.inactive = false;
		}
		else
		{
			isGameplayScene = false;
			acceptingInput = false;
			SetState(ActorStates.no_input);
			transform.SetPositionY(-2000f);
			vignette.enabled = false;
			AffectedByGravity(gravityApplies: false);
		}
		transform.SetPositionZ(0.004f);
		if (!blockerFix)
		{
			if (playerData.killedBlocker)
			{
				gm.SetPlayerDataInt("killsBlocker", 0);
			}
			blockerFix = true;
		}
		SetWalkZone(inWalkZone: false);
	}

	private void Update()
	{
		if (Time.frameCount % 10 == 0)
		{
			Update10();
		}
		current_velocity = rb2d.velocity;
		FallCheck();
		FailSafeChecks();
		if (hero_state == ActorStates.running && !cState.dashing && !cState.backDashing && !controlReqlinquished)
		{
			if (cState.inWalkZone)
			{
				audioCtrl.StopSound(HeroSounds.FOOTSTEPS_RUN);
				audioCtrl.PlaySound(HeroSounds.FOOTSTEPS_WALK);
			}
			else
			{
				audioCtrl.StopSound(HeroSounds.FOOTSTEPS_WALK);
				audioCtrl.PlaySound(HeroSounds.FOOTSTEPS_RUN);
			}
			if (runMsgSent && rb2d.velocity.x > -0.1f && rb2d.velocity.x < 0.1f)
			{
				runEffect.GetComponent<PlayMakerFSM>().SendEvent("RUN STOP");
				runEffect.transform.SetParent(null, worldPositionStays: true);
				runMsgSent = false;
			}
			if (!runMsgSent && (rb2d.velocity.x < -0.1f || rb2d.velocity.x > 0.1f))
			{
				runEffect = runEffectPrefab.Spawn();
				runEffect.transform.SetParent(base.gameObject.transform, worldPositionStays: false);
				runMsgSent = true;
			}
		}
		else
		{
			audioCtrl.StopSound(HeroSounds.FOOTSTEPS_RUN);
			audioCtrl.StopSound(HeroSounds.FOOTSTEPS_WALK);
			if (runMsgSent)
			{
				runEffect.GetComponent<PlayMakerFSM>().SendEvent("RUN STOP");
				runEffect.transform.SetParent(null, worldPositionStays: true);
				runMsgSent = false;
			}
		}
		if (hero_state == ActorStates.dash_landing)
		{
			dashLandingTimer += Time.deltaTime;
			if (dashLandingTimer > DOWN_DASH_TIME)
			{
				BackOnGround();
			}
		}
		if (hero_state == ActorStates.hard_landing)
		{
			hardLandingTimer += Time.deltaTime;
			if (hardLandingTimer > HARD_LANDING_TIME)
			{
				SetState(ActorStates.grounded);
				BackOnGround();
			}
		}
		else if (hero_state == ActorStates.no_input)
		{
			if (cState.recoiling)
			{
				if ((!playerData.equippedCharm_4 && recoilTimer < RECOIL_DURATION) || (playerData.equippedCharm_4 && recoilTimer < RECOIL_DURATION_STAL))
				{
					recoilTimer += Time.deltaTime;
				}
				else
				{
					CancelDamageRecoil();
					if ((prev_hero_state == ActorStates.idle || prev_hero_state == ActorStates.running) && !CheckTouchingGround())
					{
						cState.onGround = false;
						SetState(ActorStates.airborne);
					}
					else
					{
						SetState(ActorStates.previous);
					}
					fsm_thornCounter.SendEvent("THORN COUNTER");
				}
			}
		}
		else if (hero_state != ActorStates.no_input)
		{
			LookForInput();
			if (cState.recoiling)
			{
				cState.recoiling = false;
				AffectedByGravity(gravityApplies: true);
			}
			if (cState.attacking && !cState.dashing)
			{
				attack_time += Time.deltaTime;
				if (attack_time >= attackDuration)
				{
					ResetAttacks();
					animCtrl.StopAttack();
				}
			}
			if (cState.bouncing)
			{
				if (bounceTimer < BOUNCE_TIME)
				{
					bounceTimer += Time.deltaTime;
				}
				else
				{
					CancelBounce();
					rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
				}
			}
			if (cState.shroomBouncing && current_velocity.y <= 0f)
			{
				cState.shroomBouncing = false;
			}
			if (hero_state == ActorStates.idle)
			{
				if (!controlReqlinquished && !gm.isPaused)
				{
					if (inputHandler.inputActions.Up.IsPressed || inputHandler.inputActions.RsUp.IsPressed)
					{
						cState.lookingDown = false;
						cState.lookingDownAnim = false;
						if (lookDelayTimer >= LOOK_DELAY || (inputHandler.inputActions.RsUp.IsPressed && !cState.jumping && !cState.dashing))
						{
							cState.lookingUp = true;
						}
						else
						{
							lookDelayTimer += Time.deltaTime;
						}
						if (lookDelayTimer >= LOOK_ANIM_DELAY || inputHandler.inputActions.RsUp.IsPressed)
						{
							cState.lookingUpAnim = true;
						}
						else
						{
							cState.lookingUpAnim = false;
						}
					}
					else if (inputHandler.inputActions.Down.IsPressed || inputHandler.inputActions.RsDown.IsPressed)
					{
						cState.lookingUp = false;
						cState.lookingUpAnim = false;
						if (lookDelayTimer >= LOOK_DELAY || (inputHandler.inputActions.RsDown.IsPressed && !cState.jumping && !cState.dashing))
						{
							cState.lookingDown = true;
						}
						else
						{
							lookDelayTimer += Time.deltaTime;
						}
						if (lookDelayTimer >= LOOK_ANIM_DELAY || inputHandler.inputActions.RsDown.IsPressed)
						{
							cState.lookingDownAnim = true;
						}
						else
						{
							cState.lookingDownAnim = false;
						}
					}
					else
					{
						ResetLook();
					}
				}
				runPuffTimer = 0f;
			}
		}
		LookForQueueInput();
		if (drainMP)
		{
			drainMP_timer += Time.deltaTime;
			drainMP_seconds += Time.deltaTime;
			while (drainMP_timer >= drainMP_time)
			{
				MP_drained += 1f;
				drainMP_timer -= drainMP_time;
				TakeMP(1);
				gm.soulOrb_fsm.SendEvent("MP DRAIN");
				if (MP_drained == focusMP_amount)
				{
					MP_drained -= drainMP_time;
					proxyFSM.SendEvent("HeroCtrl-FocusCompleted");
				}
			}
		}
		if (cState.wallSliding)
		{
			if (airDashed)
			{
				airDashed = false;
			}
			if (doubleJumped)
			{
				doubleJumped = false;
			}
			if (cState.onGround)
			{
				FlipSprite();
				CancelWallsliding();
			}
			if (!cState.touchingWall)
			{
				FlipSprite();
				CancelWallsliding();
			}
			if (!CanWallSlide())
			{
				CancelWallsliding();
			}
			if (!playedMantisClawClip)
			{
				audioSource.PlayOneShot(mantisClawClip, 1f);
				playedMantisClawClip = true;
			}
			if (!playingWallslideClip)
			{
				if (wallslideClipTimer <= WALLSLIDE_CLIP_DELAY)
				{
					wallslideClipTimer += Time.deltaTime;
				}
				else
				{
					wallslideClipTimer = 0f;
					audioCtrl.PlaySound(HeroSounds.WALLSLIDE);
					playingWallslideClip = true;
				}
			}
		}
		else if (playedMantisClawClip)
		{
			playedMantisClawClip = false;
		}
		if (!cState.wallSliding && playingWallslideClip)
		{
			audioCtrl.StopSound(HeroSounds.WALLSLIDE);
			playingWallslideClip = false;
		}
		if (!cState.wallSliding && wallslideClipTimer > 0f)
		{
			wallslideClipTimer = 0f;
		}
		if (wallSlashing && !cState.wallSliding)
		{
			CancelAttack();
		}
		if (attack_cooldown > 0f)
		{
			attack_cooldown -= Time.deltaTime;
		}
		if (dashCooldownTimer > 0f)
		{
			dashCooldownTimer -= Time.deltaTime;
		}
		if (shadowDashTimer > 0f)
		{
			shadowDashTimer -= Time.deltaTime;
			if (shadowDashTimer <= 0f)
			{
				spriteFlash.FlashShadowRecharge();
			}
		}
		preventCastByDialogueEndTimer -= Time.deltaTime;
		if (!gm.isPaused)
		{
			if (inputHandler.inputActions.Attack.IsPressed && CanNailCharge())
			{
				cState.nailCharging = true;
				nailChargeTimer += Time.deltaTime;
			}
			else if (cState.nailCharging || nailChargeTimer != 0f)
			{
				artChargeEffect.SetActive(value: false);
				cState.nailCharging = false;
				audioCtrl.StopSound(HeroSounds.NAIL_ART_CHARGE);
			}
			if (cState.nailCharging && nailChargeTimer > 0.5f && !artChargeEffect.activeSelf && nailChargeTimer < nailChargeTime)
			{
				artChargeEffect.SetActive(value: true);
				audioCtrl.PlaySound(HeroSounds.NAIL_ART_CHARGE);
			}
			if (artChargeEffect.activeSelf && (!cState.nailCharging || nailChargeTimer > nailChargeTime))
			{
				artChargeEffect.SetActive(value: false);
				audioCtrl.StopSound(HeroSounds.NAIL_ART_CHARGE);
			}
			if (!artChargedEffect.activeSelf && nailChargeTimer >= nailChargeTime)
			{
				artChargedEffect.SetActive(value: true);
				artChargedFlash.SetActive(value: true);
				artChargedEffectAnim.PlayFromFrame(0);
				GameCameras.instance.cameraShakeFSM.SendEvent("EnemyKillShake");
				audioSource.PlayOneShot(nailArtChargeComplete, 1f);
				audioCtrl.PlaySound(HeroSounds.NAIL_ART_READY);
				cState.nailCharging = true;
			}
			if (artChargedEffect.activeSelf && (nailChargeTimer < nailChargeTime || !cState.nailCharging))
			{
				artChargedEffect.SetActive(value: false);
				audioCtrl.StopSound(HeroSounds.NAIL_ART_READY);
			}
		}
		if (gm.isPaused && !inputHandler.inputActions.Attack.IsPressed)
		{
			cState.nailCharging = false;
			nailChargeTimer = 0f;
		}
		if (cState.swimming && !CanSwim())
		{
			cState.swimming = false;
		}
		if (parryInvulnTimer > 0f)
		{
			parryInvulnTimer -= Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		if (cState.recoilingLeft || cState.recoilingRight)
		{
			if ((float)recoilSteps <= RECOIL_HOR_STEPS)
			{
				recoilSteps++;
			}
			else
			{
				CancelRecoilHorizontal();
			}
		}
		if (cState.dead)
		{
			rb2d.velocity = new Vector2(0f, 0f);
		}
		if ((hero_state == ActorStates.hard_landing && !cState.onConveyor) || hero_state == ActorStates.dash_landing)
		{
			ResetMotion();
		}
		else if (hero_state == ActorStates.no_input)
		{
			if (cState.transitioning)
			{
				if (transitionState == HeroTransitionState.EXITING_SCENE)
				{
					AffectedByGravity(gravityApplies: false);
					if (!stopWalkingOut)
					{
						rb2d.velocity = new Vector2(transition_vel.x, transition_vel.y + rb2d.velocity.y);
					}
				}
				else if (transitionState == HeroTransitionState.ENTERING_SCENE)
				{
					rb2d.velocity = transition_vel;
				}
				else if (transitionState == HeroTransitionState.DROPPING_DOWN)
				{
					rb2d.velocity = new Vector2(transition_vel.x, rb2d.velocity.y);
				}
			}
			else if (cState.recoiling)
			{
				AffectedByGravity(gravityApplies: false);
				rb2d.velocity = recoilVector;
			}
		}
		else if (hero_state != ActorStates.no_input)
		{
			if (hero_state == ActorStates.running)
			{
				if (move_input > 0f)
				{
					if (CheckForBump(CollisionSide.right))
					{
						rb2d.velocity = new Vector2(rb2d.velocity.x, BUMP_VELOCITY);
					}
				}
				else if (move_input < 0f && CheckForBump(CollisionSide.left))
				{
					rb2d.velocity = new Vector2(rb2d.velocity.x, BUMP_VELOCITY);
				}
			}
			if (!cState.backDashing && !cState.dashing)
			{
				Move(move_input);
				if ((!cState.attacking || !(attack_time < ATTACK_RECOVERY_TIME)) && !cState.wallSliding && !wallLocked)
				{
					if (move_input > 0f && !cState.facingRight)
					{
						FlipSprite();
						CancelAttack();
					}
					else if (move_input < 0f && cState.facingRight)
					{
						FlipSprite();
						CancelAttack();
					}
				}
				if (cState.recoilingLeft)
				{
					float num = ((!recoilLarge) ? RECOIL_HOR_VELOCITY : RECOIL_HOR_VELOCITY_LONG);
					if (rb2d.velocity.x > 0f - num)
					{
						rb2d.velocity = new Vector2(0f - num, rb2d.velocity.y);
					}
					else
					{
						rb2d.velocity = new Vector2(rb2d.velocity.x - num, rb2d.velocity.y);
					}
				}
				if (cState.recoilingRight)
				{
					float num2 = ((!recoilLarge) ? RECOIL_HOR_VELOCITY : RECOIL_HOR_VELOCITY_LONG);
					if (rb2d.velocity.x < num2)
					{
						rb2d.velocity = new Vector2(num2, rb2d.velocity.y);
					}
					else
					{
						rb2d.velocity = new Vector2(rb2d.velocity.x + num2, rb2d.velocity.y);
					}
				}
			}
			if ((cState.lookingUp || cState.lookingDown) && Mathf.Abs(move_input) > 0.6f)
			{
				ResetLook();
			}
			if (cState.jumping)
			{
				Jump();
			}
			if (cState.doubleJumping)
			{
				DoubleJump();
			}
			if (cState.dashing)
			{
				Dash();
			}
			if (cState.casting)
			{
				if (cState.castRecoiling)
				{
					if (cState.facingRight)
					{
						rb2d.velocity = new Vector2(0f - CAST_RECOIL_VELOCITY, 0f);
					}
					else
					{
						rb2d.velocity = new Vector2(CAST_RECOIL_VELOCITY, 0f);
					}
				}
				else
				{
					rb2d.velocity = Vector2.zero;
				}
			}
			if (cState.bouncing)
			{
				rb2d.velocity = new Vector2(rb2d.velocity.x, BOUNCE_VELOCITY);
			}
			_ = cState.shroomBouncing;
			if (wallLocked)
			{
				if (wallJumpedR)
				{
					rb2d.velocity = new Vector2(currentWalljumpSpeed, rb2d.velocity.y);
				}
				else if (wallJumpedL)
				{
					rb2d.velocity = new Vector2(0f - currentWalljumpSpeed, rb2d.velocity.y);
				}
				wallLockSteps++;
				if (wallLockSteps > WJLOCK_STEPS_LONG)
				{
					wallLocked = false;
				}
				currentWalljumpSpeed -= walljumpSpeedDecel;
			}
			if (cState.wallSliding)
			{
				if (wallSlidingL && inputHandler.inputActions.Right.IsPressed)
				{
					wallUnstickSteps++;
				}
				else if (wallSlidingR && inputHandler.inputActions.Left.IsPressed)
				{
					wallUnstickSteps++;
				}
				else
				{
					wallUnstickSteps = 0;
				}
				if (wallUnstickSteps >= WALL_STICKY_STEPS)
				{
					CancelWallsliding();
				}
				if (wallSlidingL)
				{
					if (!CheckStillTouchingWall(CollisionSide.left))
					{
						FlipSprite();
						CancelWallsliding();
					}
				}
				else if (wallSlidingR && !CheckStillTouchingWall(CollisionSide.right))
				{
					FlipSprite();
					CancelWallsliding();
				}
			}
		}
		if (rb2d.velocity.y < 0f - MAX_FALL_VELOCITY && !inAcid && !controlReqlinquished && !cState.shadowDashing && !cState.spellQuake)
		{
			rb2d.velocity = new Vector2(rb2d.velocity.x, 0f - MAX_FALL_VELOCITY);
		}
		if (jumpQueuing)
		{
			jumpQueueSteps++;
		}
		if (doubleJumpQueuing)
		{
			doubleJumpQueueSteps++;
		}
		if (dashQueuing)
		{
			dashQueueSteps++;
		}
		if (attackQueuing)
		{
			attackQueueSteps++;
		}
		if (cState.wallSliding && !cState.onConveyorV)
		{
			if (rb2d.velocity.y > WALLSLIDE_SPEED)
			{
				rb2d.velocity = new Vector3(rb2d.velocity.x, rb2d.velocity.y - WALLSLIDE_DECEL);
				if (rb2d.velocity.y < WALLSLIDE_SPEED)
				{
					rb2d.velocity = new Vector3(rb2d.velocity.x, WALLSLIDE_SPEED);
				}
			}
			if (rb2d.velocity.y < WALLSLIDE_SPEED)
			{
				rb2d.velocity = new Vector3(rb2d.velocity.x, rb2d.velocity.y + WALLSLIDE_DECEL);
				if (rb2d.velocity.y < WALLSLIDE_SPEED)
				{
					rb2d.velocity = new Vector3(rb2d.velocity.x, WALLSLIDE_SPEED);
				}
			}
		}
		if (nailArt_cyclone)
		{
			if (inputHandler.inputActions.Right.IsPressed && !inputHandler.inputActions.Left.IsPressed)
			{
				rb2d.velocity = new Vector3(CYCLONE_HORIZONTAL_SPEED, rb2d.velocity.y);
			}
			else if (inputHandler.inputActions.Left.IsPressed && !inputHandler.inputActions.Right.IsPressed)
			{
				rb2d.velocity = new Vector3(0f - CYCLONE_HORIZONTAL_SPEED, rb2d.velocity.y);
			}
			else
			{
				rb2d.velocity = new Vector3(0f, rb2d.velocity.y);
			}
		}
		if (cState.swimming)
		{
			rb2d.velocity = new Vector3(rb2d.velocity.x, rb2d.velocity.y + SWIM_ACCEL);
			if (rb2d.velocity.y > SWIM_MAX_SPEED)
			{
				rb2d.velocity = new Vector3(rb2d.velocity.x, SWIM_MAX_SPEED);
			}
		}
		if (cState.superDashOnWall && !cState.onConveyorV)
		{
			rb2d.velocity = new Vector3(0f, 0f);
		}
		if (cState.onConveyor && ((cState.onGround && !cState.superDashing) || hero_state == ActorStates.hard_landing))
		{
			if (cState.freezeCharge || hero_state == ActorStates.hard_landing || controlReqlinquished)
			{
				rb2d.velocity = new Vector3(0f, 0f);
			}
			rb2d.velocity = new Vector2(rb2d.velocity.x + conveyorSpeed, rb2d.velocity.y);
		}
		if (cState.inConveyorZone)
		{
			if (cState.freezeCharge || hero_state == ActorStates.hard_landing)
			{
				rb2d.velocity = new Vector3(0f, 0f);
			}
			rb2d.velocity = new Vector2(rb2d.velocity.x + conveyorSpeed, rb2d.velocity.y);
			superDash.SendEvent("SLOPE CANCEL");
		}
		if (cState.slidingLeft && rb2d.velocity.x > -5f)
		{
			rb2d.velocity = new Vector2(-5f, rb2d.velocity.y);
		}
		if (landingBufferSteps > 0)
		{
			landingBufferSteps--;
		}
		if (ledgeBufferSteps > 0)
		{
			ledgeBufferSteps--;
		}
		if (headBumpSteps > 0)
		{
			headBumpSteps--;
		}
		if (jumpReleaseQueueSteps > 0)
		{
			jumpReleaseQueueSteps--;
		}
		positionHistory[1] = positionHistory[0];
		positionHistory[0] = transform.position;
		cState.wasOnGround = cState.onGround;
	}

	private void Update10()
	{
		if (isGameplayScene)
		{
			OutOfBoundsCheck();
		}
		float scaleX = transform.GetScaleX();
		if (scaleX < -1f)
		{
			transform.SetScaleX(-1f);
		}
		if (scaleX > 1f)
		{
			transform.SetScaleX(1f);
		}
		if (transform.position.z != 0.004f)
		{
			transform.SetPositionZ(0.004f);
		}
	}

	private void OnLevelUnload()
	{
		if (transform.parent != null)
		{
			SetHeroParent(null);
		}
	}

	private void OnDisable()
	{
		if (gm != null)
		{
			gm.UnloadingLevel -= OnLevelUnload;
		}
	}

	private void Move(float move_direction)
	{
		if (cState.onGround)
		{
			SetState(ActorStates.grounded);
		}
		if (acceptingInput && !cState.wallSliding)
		{
			if (cState.inWalkZone)
			{
				rb2d.velocity = new Vector2(move_direction * WALK_SPEED, rb2d.velocity.y);
			}
			else if (inAcid)
			{
				rb2d.velocity = new Vector2(move_direction * UNDERWATER_SPEED, rb2d.velocity.y);
			}
			else if (playerData.equippedCharm_37 && cState.onGround && playerData.equippedCharm_31)
			{
				rb2d.velocity = new Vector2(move_direction * RUN_SPEED_CH_COMBO, rb2d.velocity.y);
			}
			else if (playerData.equippedCharm_37 && cState.onGround)
			{
				rb2d.velocity = new Vector2(move_direction * RUN_SPEED_CH, rb2d.velocity.y);
			}
			else
			{
				rb2d.velocity = new Vector2(move_direction * RUN_SPEED, rb2d.velocity.y);
			}
		}
	}

	private void Jump()
	{
		if (jump_steps <= JUMP_STEPS)
		{
			if (inAcid)
			{
				rb2d.velocity = new Vector2(rb2d.velocity.x, JUMP_SPEED_UNDERWATER);
			}
			else
			{
				rb2d.velocity = new Vector2(rb2d.velocity.x, JUMP_SPEED);
			}
			jump_steps++;
			jumped_steps++;
			ledgeBufferSteps = 0;
		}
		else
		{
			CancelJump();
		}
	}

	private void DoubleJump()
	{
		if (doubleJump_steps <= DOUBLE_JUMP_STEPS)
		{
			if (doubleJump_steps > 3)
			{
				rb2d.velocity = new Vector2(rb2d.velocity.x, JUMP_SPEED * 1.1f);
			}
			doubleJump_steps++;
		}
		else
		{
			CancelDoubleJump();
		}
		if (cState.onGround)
		{
			CancelDoubleJump();
		}
	}

	private void Attack(AttackDirection attackDir)
	{
		if (Time.timeSinceLevelLoad - altAttackTime > ALT_ATTACK_RESET)
		{
			cState.altAttack = false;
		}
		cState.attacking = true;
		if (playerData.equippedCharm_32)
		{
			attackDuration = ATTACK_DURATION_CH;
		}
		else
		{
			attackDuration = ATTACK_DURATION;
		}
		if (cState.wallSliding)
		{
			wallSlashing = true;
			slashComponent = wallSlash;
			slashFsm = wallSlashFsm;
			if (playerData.equippedCharm_35)
			{
				if ((playerData.health == playerData.CurrentMaxHealth && !playerData.equippedCharm_27) || (joniBeam && playerData.equippedCharm_27))
				{
					if (transform.localScale.x > 0f)
					{
						grubberFlyBeam = grubberFlyBeamPrefabR.Spawn(transform.position);
					}
					else
					{
						grubberFlyBeam = grubberFlyBeamPrefabL.Spawn(transform.position);
					}
					if (playerData.equippedCharm_13)
					{
						grubberFlyBeam.transform.SetScaleY(MANTIS_CHARM_SCALE);
					}
					else
					{
						grubberFlyBeam.transform.SetScaleY(1f);
					}
				}
				if (playerData.health == 1 && playerData.equippedCharm_6 && playerData.healthBlue < 1)
				{
					if (transform.localScale.x > 0f)
					{
						grubberFlyBeam = grubberFlyBeamPrefabR_fury.Spawn(transform.position);
					}
					else
					{
						grubberFlyBeam = grubberFlyBeamPrefabL_fury.Spawn(transform.position);
					}
					if (playerData.equippedCharm_13)
					{
						grubberFlyBeam.transform.SetScaleY(MANTIS_CHARM_SCALE);
					}
					else
					{
						grubberFlyBeam.transform.SetScaleY(1f);
					}
				}
			}
		}
		else
		{
			wallSlashing = false;
			switch (attackDir)
			{
				case AttackDirection.normal:
					if (!cState.altAttack)
					{
						slashComponent = normalSlash;
						slashFsm = normalSlashFsm;
						cState.altAttack = true;
					}
					else
					{
						slashComponent = alternateSlash;
						slashFsm = alternateSlashFsm;
						cState.altAttack = false;
					}
					if (!playerData.equippedCharm_35)
					{
						break;
					}
					if ((playerData.health >= playerData.CurrentMaxHealth && !playerData.equippedCharm_27) || (joniBeam && playerData.equippedCharm_27))
					{
						if (transform.localScale.x < 0f)
						{
							grubberFlyBeam = grubberFlyBeamPrefabR.Spawn(transform.position);
						}
						else
						{
							grubberFlyBeam = grubberFlyBeamPrefabL.Spawn(transform.position);
						}
						if (playerData.equippedCharm_13)
						{
							grubberFlyBeam.transform.SetScaleY(MANTIS_CHARM_SCALE);
						}
						else
						{
							grubberFlyBeam.transform.SetScaleY(1f);
						}
					}
					if (playerData.health == 1 && playerData.equippedCharm_6 && playerData.healthBlue < 1)
					{
						if (transform.localScale.x < 0f)
						{
							grubberFlyBeam = grubberFlyBeamPrefabR_fury.Spawn(transform.position);
						}
						else
						{
							grubberFlyBeam = grubberFlyBeamPrefabL_fury.Spawn(transform.position);
						}
						if (playerData.equippedCharm_13)
						{
							grubberFlyBeam.transform.SetScaleY(MANTIS_CHARM_SCALE);
						}
						else
						{
							grubberFlyBeam.transform.SetScaleY(1f);
						}
					}
					break;
				case AttackDirection.upward:
					slashComponent = upSlash;
					slashFsm = upSlashFsm;
					cState.upAttacking = true;
					if (!playerData.equippedCharm_35)
					{
						break;
					}
					if ((playerData.health >= playerData.CurrentMaxHealth && !playerData.equippedCharm_27) || (joniBeam && playerData.equippedCharm_27))
					{
						grubberFlyBeam = grubberFlyBeamPrefabU.Spawn(transform.position);
						grubberFlyBeam.transform.SetScaleY(transform.localScale.x);
						grubberFlyBeam.transform.localEulerAngles = new Vector3(0f, 0f, 270f);
						if (playerData.equippedCharm_13)
						{
							grubberFlyBeam.transform.SetScaleY(grubberFlyBeam.transform.localScale.y * MANTIS_CHARM_SCALE);
						}
					}
					if (playerData.health == 1 && playerData.equippedCharm_6 && playerData.healthBlue < 1)
					{
						grubberFlyBeam = grubberFlyBeamPrefabU_fury.Spawn(transform.position);
						grubberFlyBeam.transform.SetScaleY(transform.localScale.x);
						grubberFlyBeam.transform.localEulerAngles = new Vector3(0f, 0f, 270f);
						if (playerData.equippedCharm_13)
						{
							grubberFlyBeam.transform.SetScaleY(grubberFlyBeam.transform.localScale.y * MANTIS_CHARM_SCALE);
						}
					}
					break;
				case AttackDirection.downward:
					slashComponent = downSlash;
					slashFsm = downSlashFsm;
					cState.downAttacking = true;
					if (!playerData.equippedCharm_35)
					{
						break;
					}
					if ((playerData.health >= playerData.CurrentMaxHealth && !playerData.equippedCharm_27) || (joniBeam && playerData.equippedCharm_27))
					{
						grubberFlyBeam = grubberFlyBeamPrefabD.Spawn(transform.position);
						grubberFlyBeam.transform.SetScaleY(transform.localScale.x);
						grubberFlyBeam.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
						if (playerData.equippedCharm_13)
						{
							grubberFlyBeam.transform.SetScaleY(grubberFlyBeam.transform.localScale.y * MANTIS_CHARM_SCALE);
						}
					}
					if (playerData.health == 1 && playerData.equippedCharm_6 && playerData.healthBlue < 1)
					{
						grubberFlyBeam = grubberFlyBeamPrefabD_fury.Spawn(transform.position);
						grubberFlyBeam.transform.SetScaleY(transform.localScale.x);
						grubberFlyBeam.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
						if (playerData.equippedCharm_13)
						{
							grubberFlyBeam.transform.SetScaleY(grubberFlyBeam.transform.localScale.y * MANTIS_CHARM_SCALE);
						}
					}
					break;
			}
		}
		if (cState.wallSliding)
		{
			if (cState.facingRight)
			{
				slashFsm.FsmVariables.GetFsmFloat("direction").Value = 180f;
			}
			else
			{
				slashFsm.FsmVariables.GetFsmFloat("direction").Value = 0f;
			}
		}
		else if (attackDir == AttackDirection.normal && cState.facingRight)
		{
			slashFsm.FsmVariables.GetFsmFloat("direction").Value = 0f;
		}
		else if (attackDir == AttackDirection.normal && !cState.facingRight)
		{
			slashFsm.FsmVariables.GetFsmFloat("direction").Value = 180f;
		}
		else
		{
			switch (attackDir)
			{
				case AttackDirection.upward:
					slashFsm.FsmVariables.GetFsmFloat("direction").Value = 90f;
					break;
				case AttackDirection.downward:
					slashFsm.FsmVariables.GetFsmFloat("direction").Value = 270f;
					break;
			}
		}
		altAttackTime = Time.timeSinceLevelLoad;
		slashComponent.StartSlash();
		if (playerData.equippedCharm_38)
		{
			fsm_orbitShield.SendEvent("SLASH");
		}
	}

	private void Dash()
	{
		AffectedByGravity(gravityApplies: false);
		ResetHardLandingTimer();
		if (dash_timer > DASH_TIME)
		{
			FinishedDashing();
			return;
		}
		float num = ((!playerData.equippedCharm_16 || !cState.shadowDashing) ? DASH_SPEED : DASH_SPEED_SHARP);
		if (dashingDown)
		{
			rb2d.velocity = new Vector2(0f, 0f - num);
		}
		else if (cState.facingRight)
		{
			if (CheckForBump(CollisionSide.right))
			{
				rb2d.velocity = new Vector2(num, cState.onGround ? BUMP_VELOCITY : BUMP_VELOCITY_DASH);
			}
			else
			{
				rb2d.velocity = new Vector2(num, 0f);
			}
		}
		else if (CheckForBump(CollisionSide.left))
		{
			rb2d.velocity = new Vector2(0f - num, cState.onGround ? BUMP_VELOCITY : BUMP_VELOCITY_DASH);
		}
		else
		{
			rb2d.velocity = new Vector2(0f - num, 0f);
		}
		dash_timer += Time.deltaTime;
	}

	private void BackDash()
	{
	}

	private void ShadowDash()
	{
	}

	private void SuperDash()
	{
	}

	public void FaceRight()
	{
		cState.facingRight = true;
		Vector3 localScale = transform.localScale;
		localScale.x = -1f;
		transform.localScale = localScale;
	}

	public void FaceLeft()
	{
		cState.facingRight = false;
		Vector3 localScale = transform.localScale;
		localScale.x = 1f;
		transform.localScale = localScale;
	}

	public void StartMPDrain(float time)
	{
		drainMP = true;
		drainMP_timer = 0f;
		MP_drained = 0f;
		drainMP_time = time;
		focusMP_amount = playerData.GetInt("focusMP_amount");
	}

	public void StopMPDrain()
	{
		drainMP = false;
	}

	public void SetBackOnGround()
	{
		cState.onGround = true;
	}

	public void SetStartWithWallslide()
	{
		startWithWallslide = true;
	}

	public void SetStartWithJump()
	{
		startWithJump = true;
	}

	public void SetStartWithFullJump()
	{
		startWithFullJump = true;
	}

	public void SetStartWithDash()
	{
		startWithDash = true;
	}

	public void SetStartWithAttack()
	{
		startWithAttack = true;
	}

	public void SetSuperDashExit()
	{
		exitedSuperDashing = true;
	}

	public void SetQuakeExit()
	{
		exitedQuake = true;
	}

	public void SetTakeNoDamage()
	{
		takeNoDamage = true;
	}

	public void EndTakeNoDamage()
	{
		takeNoDamage = false;
	}

	public void SetHeroParent(Transform newParent)
	{
		transform.parent = newParent;
		if (newParent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public void IsSwimming()
	{
		cState.swimming = true;
	}

	public void NotSwimming()
	{
		cState.swimming = false;
	}

	public void EnableRenderer()
	{
		renderer.enabled = true;
	}

	public void ResetAirMoves()
	{
		doubleJumped = false;
		airDashed = false;
	}

	public void SetConveyorSpeed(float speed)
	{
		conveyorSpeed = speed;
	}

	public void SetConveyorSpeedV(float speed)
	{
		conveyorSpeedV = speed;
	}

	public void EnterWithoutInput(bool flag)
	{
		enterWithoutInput = flag;
	}

	public void SetDarkness(int darkness)
	{
		if (darkness > 0 && playerData.hasLantern)
		{
			wieldingLantern = true;
		}
		else
		{
			wieldingLantern = false;
		}
	}

	public void CancelHeroJump()
	{
		if (cState.jumping)
		{
			CancelJump();
			CancelDoubleJump();
			if (rb2d.velocity.y > 0f)
			{
				rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
			}
		}
	}

	public void CharmUpdate()
	{
		if (playerData.equippedCharm_26)
		{
			nailChargeTime = NAIL_CHARGE_TIME_CHARM;
		}
		else
		{
			nailChargeTime = NAIL_CHARGE_TIME_DEFAULT;
		}
		if (playerData.equippedCharm_23 && !playerData.brokenCharm_23)
		{
			playerData.maxHealth = playerData.maxHealthBase + 2;
			MaxHealth();
		}
		else
		{
			playerData.maxHealth = playerData.maxHealthBase;
			MaxHealth();
		}
		if (playerData.equippedCharm_27)
		{
			playerData.joniHealthBlue = (int)((float)playerData.maxHealth * 1.4f);
			playerData.maxHealth = 1;
			MaxHealth();
			joniBeam = true;
		}
		else
		{
			playerData.joniHealthBlue = 0;
		}
		if (playerData.equippedCharm_40 && playerData.grimmChildLevel == 5)
		{
			carefreeShieldEquipped = true;
		}
		else
		{
			carefreeShieldEquipped = false;
		}
		playerData.UpdateBlueHealth();
	}

	public void checkEnvironment()
	{
		if (playerData.environmentType == 0)
		{
			footStepsRunAudioSource.clip = footstepsRunDust;
			footStepsWalkAudioSource.clip = footstepsWalkDust;
		}
		else if (playerData.environmentType == 1)
		{
			footStepsRunAudioSource.clip = footstepsRunGrass;
			footStepsWalkAudioSource.clip = footstepsWalkGrass;
		}
		else if (playerData.environmentType == 2)
		{
			footStepsRunAudioSource.clip = footstepsRunBone;
			footStepsWalkAudioSource.clip = footstepsWalkBone;
		}
		else if (playerData.environmentType == 3)
		{
			footStepsRunAudioSource.clip = footstepsRunSpa;
			footStepsWalkAudioSource.clip = footstepsWalkSpa;
		}
		else if (playerData.environmentType == 4)
		{
			footStepsRunAudioSource.clip = footstepsRunMetal;
			footStepsWalkAudioSource.clip = footstepsWalkMetal;
		}
		else if (playerData.environmentType == 6)
		{
			footStepsRunAudioSource.clip = footstepsRunWater;
			footStepsWalkAudioSource.clip = footstepsRunWater;
		}
		else if (playerData.environmentType == 7)
		{
			footStepsRunAudioSource.clip = footstepsRunGrass;
			footStepsWalkAudioSource.clip = footstepsWalkGrass;
		}
	}

	public void SetBenchRespawn(string spawnMarker, string sceneName, int spawnType, bool facingRight)
	{
		playerData.SetBenchRespawn(spawnMarker, sceneName, spawnType, facingRight);
	}

	public void SetHazardRespawn(Vector3 position, bool facingRight)
	{
		playerData.SetHazardRespawn(position, facingRight);
	}

	// public void AddGeo(int amount)
	// {
	// 	playerData.AddGeo(amount);
	// 	geoCounter.AddGeo(amount);
	// }

	// public void ToZero()
	// {
	// 	geoCounter.ToZero();
	// }

	// public void AddGeoQuietly(int amount)
	// {
	// 	playerData.AddGeo(amount);
	// }

	// public void AddGeoToCounter(int amount)
	// {
	// 	geoCounter.AddGeo(amount);
	// }

	// public void TakeGeo(int amount)
	// {
	// 	playerData.TakeGeo(amount);
	// 	geoCounter.TakeGeo(amount);
	// }

	// public void UpdateGeo()
	// {
	// 	geoCounter.UpdateGeo();
	// }

	public bool CanInput()
	{
		return acceptingInput;
	}

	public bool CanTalk()
	{
		bool result = false;
		if (CanInput() && hero_state != ActorStates.no_input && !controlReqlinquished && cState.onGround && !cState.attacking && !cState.dashing)
		{
			result = true;
		}
		return result;
	}

	public void FlipSprite()
	{
		cState.facingRight = !cState.facingRight;
		Vector3 localScale = transform.localScale;
		localScale.x *= -1f;
		transform.localScale = localScale;
	}

	public void NailParry()
	{
		parryInvulnTimer = INVUL_TIME_PARRY;
	}

	public void NailParryRecover()
	{
		attackDuration = 0f;
		attack_cooldown = 0f;
		CancelAttack();
	}

	public void QuakeInvuln()
	{
		parryInvulnTimer = INVUL_TIME_QUAKE;
	}

	public void CancelParryInvuln()
	{
		parryInvulnTimer = 0f;
	}

	public void CycloneInvuln()
	{
		parryInvulnTimer = INVUL_TIME_CYCLONE;
	}

	public void SetWieldingLantern(bool set)
	{
		wieldingLantern = set;
	}

	public void TakeDamage(GameObject go, CollisionSide damageSide, int damageAmount, int hazardType)
	{
		bool spawnDamageEffect = true;
		if (damageAmount <= 0)
		{
			return;
		}
		if (BossSceneController.IsBossScene)
		{
			switch (BossSceneController.Instance.BossLevel)
			{
				case 1:
					damageAmount *= 2;
					break;
				case 2:
					damageAmount = 9999;
					break;
			}
		}
		if (CanTakeDamage())
		{
			if ((damageMode == DamageMode.HAZARD_ONLY && hazardType == 1) || (cState.shadowDashing && hazardType == 1) || (parryInvulnTimer > 0f && hazardType == 1))
			{
				return;
			}
			VibrationManager.GetMixer()?.StopAllEmissionsWithTag("heroAction");
			bool flag = false;
			if (carefreeShieldEquipped && hazardType == 1)
			{
				if (hitsSinceShielded > 7)
				{
					hitsSinceShielded = 7;
				}
				switch (hitsSinceShielded)
				{
					case 1:
						if ((float)UnityEngine.Random.Range(1, 100) <= 10f)
						{
							flag = true;
						}
						break;
					case 2:
						if ((float)UnityEngine.Random.Range(1, 100) <= 20f)
						{
							flag = true;
						}
						break;
					case 3:
						if ((float)UnityEngine.Random.Range(1, 100) <= 30f)
						{
							flag = true;
						}
						break;
					case 4:
						if ((float)UnityEngine.Random.Range(1, 100) <= 50f)
						{
							flag = true;
						}
						break;
					case 5:
						if ((float)UnityEngine.Random.Range(1, 100) <= 70f)
						{
							flag = true;
						}
						break;
					case 6:
						if ((float)UnityEngine.Random.Range(1, 100) <= 80f)
						{
							flag = true;
						}
						break;
					case 7:
						if ((float)UnityEngine.Random.Range(1, 100) <= 90f)
						{
							flag = true;
						}
						break;
					default:
						flag = false;
						break;
				}
				if (flag)
				{
					hitsSinceShielded = 0;
					carefreeShield.SetActive(value: true);
					damageAmount = 0;
					spawnDamageEffect = false;
				}
				else
				{
					hitsSinceShielded++;
				}
			}
			if (playerData.equippedCharm_5 && playerData.blockerHits > 0 && hazardType == 1 && cState.focusing && !flag)
			{
				proxyFSM.SendEvent("HeroCtrl-TookBlockerHit");
				audioSource.PlayOneShot(blockerImpact, 1f);
				spawnDamageEffect = false;
				damageAmount = 0;
			}
			else
			{
				proxyFSM.SendEvent("HeroCtrl-HeroDamaged");
			}
			CancelAttack();
			if (cState.wallSliding)
			{
				cState.wallSliding = false;
				wallSlideVibrationPlayer.Stop();
			}
			if (cState.touchingWall)
			{
				cState.touchingWall = false;
			}
			if (cState.recoilingLeft || cState.recoilingRight)
			{
				CancelRecoilHorizontal();
			}
			if (cState.bouncing)
			{
				CancelBounce();
				rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
			}
			if (cState.shroomBouncing)
			{
				CancelBounce();
				rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
			}
			if (!flag)
			{
				audioCtrl.PlaySound(HeroSounds.TAKE_HIT);
			}
			if (!takeNoDamage && !playerData.invinciTest)
			{
				if (playerData.overcharmed)
				{
					playerData.TakeHealth(damageAmount * 2);
				}
				else
				{
					playerData.TakeHealth(damageAmount);
				}
			}
			if (playerData.equippedCharm_3 && damageAmount > 0)
			{
				if (playerData.equippedCharm_35)
				{
					AddMPCharge(GRUB_SOUL_MP_COMBO);
				}
				else
				{
					AddMPCharge(GRUB_SOUL_MP);
				}
			}
			if (joniBeam && damageAmount > 0)
			{
				joniBeam = false;
			}
			if (cState.nailCharging || nailChargeTimer != 0f)
			{
				cState.nailCharging = false;
				nailChargeTimer = 0f;
			}
			if (damageAmount > 0 && this.OnTakenDamage != null)
			{
				this.OnTakenDamage();
			}
			if (playerData.health == 0)
			{
				StartCoroutine(Die());
				return;
			}
			switch (hazardType)
			{
				case 2:
					StartCoroutine(DieFromHazard(HazardType.SPIKES, (go != null) ? go.transform.rotation.z : 0f));
					break;
				case 3:
					StartCoroutine(DieFromHazard(HazardType.ACID, 0f));
					break;
				case 4:
					Debug.Log("Lava death");
					break;
				case 5:
					StartCoroutine(DieFromHazard(HazardType.PIT, 0f));
					break;
				default:
					StartCoroutine(StartRecoil(damageSide, spawnDamageEffect, damageAmount));
					break;
			}
		}
		else
		{
			if (!cState.invulnerable || cState.hazardDeath || playerData.isInvincible)
			{
				return;
			}
			switch (hazardType)
			{
				case 2:
					if (!takeNoDamage)
					{
						playerData.TakeHealth(damageAmount);
					}
					proxyFSM.SendEvent("HeroCtrl-HeroDamaged");
					if (playerData.health == 0)
					{
						StartCoroutine(Die());
						break;
					}
					audioCtrl.PlaySound(HeroSounds.TAKE_HIT);
					StartCoroutine(DieFromHazard(HazardType.SPIKES, (go != null) ? go.transform.rotation.z : 0f));
					break;
				case 3:
					playerData.TakeHealth(damageAmount);
					proxyFSM.SendEvent("HeroCtrl-HeroDamaged");
					if (playerData.health == 0)
					{
						StartCoroutine(Die());
					}
					else
					{
						StartCoroutine(DieFromHazard(HazardType.ACID, 0f));
					}
					break;
				case 4:
					Debug.Log("Lava damage");
					break;
			}
		}
	}

	public string GetEntryGateName()
	{
		if (sceneEntryGate != null)
		{
			return sceneEntryGate.name;
		}
		return "";
	}

	public void AddMPCharge(int amount)
	{
		int mPReserve = playerData.MPReserve;
		playerData.AddMPCharge(amount);
		GameCameras.instance.soulOrbFSM.SendEvent("MP GAIN");
		if (playerData.MPReserve != mPReserve && (bool)gm && (bool)gm.soulVessel_fsm)
		{
			gm.soulVessel_fsm.SendEvent("MP RESERVE UP");
		}
	}

	public void SoulGain()
	{
		int num;
		if (playerData.MPCharge < playerData.maxMP)
		{
			num = 11;
			if (playerData.equippedCharm_20)
			{
				num += 3;
			}
			if (playerData.equippedCharm_21)
			{
				num += 8;
			}
		}
		else
		{
			num = 6;
			if (playerData.equippedCharm_20)
			{
				num += 2;
			}
			if (playerData.equippedCharm_21)
			{
				num += 6;
			}
		}
		int mPReserve = playerData.MPReserve;
		playerData.AddMPCharge(num);
		GameCameras.instance.soulOrbFSM.SendEvent("MP GAIN");
		if (playerData.MPReserve != mPReserve)
		{
			gm.soulVessel_fsm.SendEvent("MP RESERVE UP");
		}
	}

	public void AddMPChargeSpa(int amount)
	{
		TryAddMPChargeSpa(amount);
	}

	public bool TryAddMPChargeSpa(int amount)
	{
		int mPReserve = playerData.MPReserve;
		bool result = playerData.AddMPCharge(amount);
		gm.soulOrb_fsm.SendEvent("MP GAIN SPA");
		if (playerData.MPReserve != mPReserve)
		{
			gm.soulVessel_fsm.SendEvent("MP RESERVE UP");
		}
		return result;
	}

	public void SetMPCharge(int amount)
	{
		playerData.MPCharge = amount;
		GameCameras.instance.soulOrbFSM.SendEvent("MP SET");
	}

	public void TakeMP(int amount)
	{
		if (playerData.MPCharge > 0)
		{
			playerData.TakeMP(amount);
			if (amount > 1)
			{
				GameCameras.instance.soulOrbFSM.SendEvent("MP LOSE");
			}
		}
	}

	public void TakeMPQuick(int amount)
	{
		if (playerData.MPCharge > 0)
		{
			playerData.TakeMP(amount);
			if (amount > 1)
			{
				GameCameras.instance.soulOrbFSM.SendEvent("MP DRAIN");
			}
		}
	}

	public void TakeReserveMP(int amount)
	{
		playerData.TakeReserveMP(amount);
		gm.soulVessel_fsm.SendEvent("MP RESERVE DOWN");
	}

	public void AddHealth(int amount)
	{
		playerData.AddHealth(amount);
		proxyFSM.SendEvent("HeroCtrl-Healed");
	}

	public void TakeHealth(int amount)
	{
		playerData.TakeHealth(amount);
		proxyFSM.SendEvent("HeroCtrl-HeroDamaged");
	}

	public void MaxHealth()
	{
		proxyFSM.SendEvent("HeroCtrl-MaxHealth");
		playerData.MaxHealth();
	}

	public void MaxHealthKeepBlue()
	{
		int healthBlue = playerData.healthBlue;
		playerData.MaxHealth();
		playerData.healthBlue = healthBlue;
		proxyFSM.SendEvent("HeroCtrl-Healed");
	}

	public void AddToMaxHealth(int amount)
	{
		playerData.AddToMaxHealth(amount);
		gm.AwardAchievement("PROTECTED");
		if (playerData.maxHealthBase == playerData.maxHealthCap)
		{
			gm.AwardAchievement("MASKED");
		}
	}

	public void ClearMP()
	{
		playerData.ClearMP();
	}

	public void ClearMPSendEvents()
	{
		ClearMP();
		GameManager.instance.soulOrb_fsm.SendEvent("MP LOSE");
		GameManager.instance.soulVessel_fsm.SendEvent("MP RESERVE DOWN");
	}

	public void AddToMaxMPReserve(int amount)
	{
		playerData.AddToMaxMPReserve(amount);
		gm.AwardAchievement("SOULFUL");
		if (playerData.MPReserveMax == playerData.MPReserveCap)
		{
			gm.AwardAchievement("WORLDSOUL");
		}
	}

	public void Bounce()
	{
		if (!cState.bouncing && !cState.shroomBouncing && !controlReqlinquished)
		{
			doubleJumped = false;
			airDashed = false;
			cState.bouncing = true;
		}
	}

	public void BounceHigh()
	{
		if (!cState.bouncing && !controlReqlinquished)
		{
			doubleJumped = false;
			airDashed = false;
			cState.bouncing = true;
			bounceTimer = -0.03f;
			rb2d.velocity = new Vector2(rb2d.velocity.x, BOUNCE_VELOCITY);
		}
	}

	public void ShroomBounce()
	{
		doubleJumped = false;
		airDashed = false;
		cState.bouncing = false;
		cState.shroomBouncing = true;
		rb2d.velocity = new Vector2(rb2d.velocity.x, SHROOM_BOUNCE_VELOCITY);
	}

	public void RecoilLeft()
	{
		if (!cState.recoilingLeft && !cState.recoilingRight && !playerData.equippedCharm_14 && !controlReqlinquished)
		{
			CancelDash();
			recoilSteps = 0;
			cState.recoilingLeft = true;
			cState.recoilingRight = false;
			recoilLarge = false;
			rb2d.velocity = new Vector2(0f - RECOIL_HOR_VELOCITY, rb2d.velocity.y);
		}
	}

	public void RecoilRight()
	{
		if (!cState.recoilingLeft && !cState.recoilingRight && !playerData.equippedCharm_14 && !controlReqlinquished)
		{
			CancelDash();
			recoilSteps = 0;
			cState.recoilingRight = true;
			cState.recoilingLeft = false;
			recoilLarge = false;
			rb2d.velocity = new Vector2(RECOIL_HOR_VELOCITY, rb2d.velocity.y);
		}
	}

	public void RecoilRightLong()
	{
		if (!cState.recoilingLeft && !cState.recoilingRight && !controlReqlinquished)
		{
			CancelDash();
			ResetAttacks();
			recoilSteps = 0;
			cState.recoilingRight = true;
			cState.recoilingLeft = false;
			recoilLarge = true;
			rb2d.velocity = new Vector2(RECOIL_HOR_VELOCITY_LONG, rb2d.velocity.y);
		}
	}

	public void RecoilLeftLong()
	{
		if (!cState.recoilingLeft && !cState.recoilingRight && !controlReqlinquished)
		{
			CancelDash();
			ResetAttacks();
			recoilSteps = 0;
			cState.recoilingRight = false;
			cState.recoilingLeft = true;
			recoilLarge = true;
			rb2d.velocity = new Vector2(0f - RECOIL_HOR_VELOCITY_LONG, rb2d.velocity.y);
		}
	}

	public void RecoilDown()
	{
		CancelJump();
		if (rb2d.velocity.y > RECOIL_DOWN_VELOCITY && !controlReqlinquished)
		{
			rb2d.velocity = new Vector2(rb2d.velocity.x, RECOIL_DOWN_VELOCITY);
		}
	}

	public void ForceHardLanding()
	{
		if (!cState.onGround)
		{
			cState.willHardLand = true;
		}
	}

	public void EnterSceneDreamGate()
	{
		IgnoreInputWithoutReset();
		ResetMotion();
		airDashed = false;
		doubleJumped = false;
		ResetHardLandingTimer();
		ResetAttacksDash();
		AffectedByGravity(gravityApplies: false);
		sceneEntryGate = null;
		SetState(ActorStates.no_input);
		transitionState = HeroTransitionState.WAITING_TO_ENTER_LEVEL;
		vignetteFSM.SendEvent("RESET");
		if (this.heroInPosition != null)
		{
			this.heroInPosition(forceDirect: false);
		}
		FinishedEnteringScene();
	}

	public IEnumerator EnterScene(TransitionPoint enterGate, float delayBeforeEnter)
	{
		IgnoreInputWithoutReset();
		ResetMotion();
		airDashed = false;
		doubleJumped = false;
		ResetHardLandingTimer();
		ResetAttacksDash();
		AffectedByGravity(gravityApplies: false);
		sceneEntryGate = enterGate;
		SetState(ActorStates.no_input);
		transitionState = HeroTransitionState.WAITING_TO_ENTER_LEVEL;
		vignetteFSM.SendEvent("RESET");
		if (!cState.transitioning)
		{
			cState.transitioning = true;
		}
		gatePosition = enterGate.GetGatePosition();
		if (gatePosition == GatePosition.top)
		{
			cState.onGround = false;
			enteringVertically = true;
			exitedSuperDashing = false;
			renderer.enabled = false;
			float x = enterGate.transform.position.x + enterGate.entryOffset.x;
			float y = enterGate.transform.position.y + enterGate.entryOffset.y;
			transform.SetPosition2D(x, y);
			if (this.heroInPosition != null)
			{
				this.heroInPosition(forceDirect: false);
			}
			yield return new WaitForSeconds(0.165f);
			if (!enterGate.customFade)
			{
				gm.FadeSceneIn();
			}
			if (delayBeforeEnter > 0f)
			{
				yield return new WaitForSeconds(delayBeforeEnter);
			}
			if (enterGate.entryDelay > 0f)
			{
				yield return new WaitForSeconds(enterGate.entryDelay);
			}
			// yield return new WaitForSeconds(0.4f);
			renderer.enabled = true;
			if (exitedQuake)
			{
				IgnoreInput();
				proxyFSM.SendEvent("HeroCtrl-EnterQuake");
				yield return new WaitForSeconds(0.25f);
				FinishedEnteringScene();
				yield break;
			}
			rb2d.velocity = new Vector2(0f, SPEED_TO_ENTER_SCENE_DOWN);
			transitionState = HeroTransitionState.ENTERING_SCENE;
			transitionState = HeroTransitionState.DROPPING_DOWN;
			AffectedByGravity(gravityApplies: true);
			if (enterGate.hardLandOnExit)
			{
				cState.willHardLand = true;
			}
			yield return new WaitForSeconds(0.33f);
			transitionState = HeroTransitionState.ENTERING_SCENE;
			if (transitionState != HeroTransitionState.WAITING_TO_TRANSITION)
			{
				FinishedEnteringScene();
			}
		}
		else if (gatePosition == GatePosition.bottom)
		{
			cState.onGround = false;
			enteringVertically = true;
			exitedSuperDashing = false;
			if (enterGate.alwaysEnterRight)
			{
				FaceRight();
			}
			if (enterGate.alwaysEnterLeft)
			{
				FaceLeft();
			}
			float x2 = enterGate.transform.position.x + enterGate.entryOffset.x;
			float y2 = enterGate.transform.position.y + enterGate.entryOffset.y + 3f;
			transform.SetPosition2D(x2, y2);
			if (this.heroInPosition != null)
			{
				this.heroInPosition(forceDirect: false);
			}
			yield return new WaitForSeconds(0.165f);
			if (delayBeforeEnter > 0f)
			{
				yield return new WaitForSeconds(delayBeforeEnter);
			}
			if (enterGate.entryDelay > 0f)
			{
				yield return new WaitForSeconds(enterGate.entryDelay);
			}
			// yield return new WaitForSeconds(0.4f);
			if (!enterGate.customFade)
			{
				gm.FadeSceneIn();
			}
			if (cState.facingRight)
			{
				transition_vel = new Vector2(SPEED_TO_ENTER_SCENE_HOR, SPEED_TO_ENTER_SCENE_UP);
			}
			else
			{
				transition_vel = new Vector2(0f - SPEED_TO_ENTER_SCENE_HOR, SPEED_TO_ENTER_SCENE_UP);
			}
			transitionState = HeroTransitionState.ENTERING_SCENE;
			transform.SetPosition2D(x2, y2);
			yield return new WaitForSeconds(TIME_TO_ENTER_SCENE_BOT);
			transition_vel = new Vector2(rb2d.velocity.x, 0f);
			AffectedByGravity(gravityApplies: true);
			transitionState = HeroTransitionState.DROPPING_DOWN;
		}
		else if (gatePosition == GatePosition.left)
		{
			cState.onGround = true;
			enteringVertically = false;
			SetState(ActorStates.no_input);
			float num = enterGate.transform.position.x + enterGate.entryOffset.x;
			float y3 = FindGroundPointY(num + 2f, enterGate.transform.position.y);
			transform.SetPosition2D(num, y3);
			if (this.heroInPosition != null)
			{
				this.heroInPosition(forceDirect: true);
			}
			FaceRight();
			yield return new WaitForSeconds(0.165f);
			if (!enterGate.customFade)
			{
				gm.FadeSceneIn();
			}
			if (delayBeforeEnter > 0f)
			{
				yield return new WaitForSeconds(delayBeforeEnter);
			}
			if (enterGate.entryDelay > 0f)
			{
				yield return new WaitForSeconds(enterGate.entryDelay);
			}
			// yield return new WaitForSeconds(0.4f);
			if (exitedSuperDashing)
			{
				IgnoreInput();
				proxyFSM.SendEvent("HeroCtrl-EnterSuperDash");
				yield return new WaitForSeconds(0.25f);
				FinishedEnteringScene();
			}
			else
			{
				transition_vel = new Vector2(RUN_SPEED, 0f);
				transitionState = HeroTransitionState.ENTERING_SCENE;
				yield return new WaitForSeconds(0.33f);
				FinishedEnteringScene(setHazardMarker: true, preventRunBob: true);
			}
		}
		else if (gatePosition == GatePosition.right)
		{
			cState.onGround = true;
			enteringVertically = false;
			SetState(ActorStates.no_input);
			float num2 = enterGate.transform.position.x + enterGate.entryOffset.x;
			float y4 = FindGroundPointY(num2 - 2f, enterGate.transform.position.y);
			transform.SetPosition2D(num2, y4);
			if (this.heroInPosition != null)
			{
				this.heroInPosition(forceDirect: true);
			}
			FaceLeft();
			yield return new WaitForSeconds(0.165f);
			if (!enterGate.customFade)
			{
				gm.FadeSceneIn();
			}
			if (delayBeforeEnter > 0f)
			{
				yield return new WaitForSeconds(delayBeforeEnter);
			}
			if (enterGate.entryDelay > 0f)
			{
				yield return new WaitForSeconds(enterGate.entryDelay);
			}
			// yield return new WaitForSeconds(0.4f);
			if (exitedSuperDashing)
			{
				IgnoreInput();
				proxyFSM.SendEvent("HeroCtrl-EnterSuperDash");
				yield return new WaitForSeconds(0.25f);
				FinishedEnteringScene();
			}
			else
			{
				transition_vel = new Vector2(0f - RUN_SPEED, 0f);
				transitionState = HeroTransitionState.ENTERING_SCENE;
				yield return new WaitForSeconds(0.33f);
				FinishedEnteringScene(setHazardMarker: true, preventRunBob: true);
			}
		}
		else
		{
			if (gatePosition != GatePosition.door)
			{
				yield break;
			}
			if (enterGate.alwaysEnterRight)
			{
				FaceRight();
			}
			if (enterGate.alwaysEnterLeft)
			{
				FaceLeft();
			}
			cState.onGround = true;
			enteringVertically = false;
			SetState(ActorStates.idle);
			SetState(ActorStates.no_input);
			exitedSuperDashing = false;
			animCtrl.PlayClip("Idle");
			transform.SetPosition2D(FindGroundPoint(enterGate.transform.position));
			if (this.heroInPosition != null)
			{
				this.heroInPosition(forceDirect: false);
			}
			yield return new WaitForEndOfFrame();
			if (delayBeforeEnter > 0f)
			{
				yield return new WaitForSeconds(delayBeforeEnter);
			}
			if (enterGate.entryDelay > 0f)
			{
				yield return new WaitForSeconds(enterGate.entryDelay);
			}
			// yield return new WaitForSeconds(0.4f);
			if (!enterGate.customFade)
			{
				gm.FadeSceneIn();
			}
			_ = Time.realtimeSinceStartup;
			if (enterGate.dontWalkOutOfDoor)
			{
				yield return new WaitForSeconds(0.33f);
			}
			else
			{
				float clipDuration = animCtrl.GetClipDuration("Exit Door To Idle");
				animCtrl.PlayClip("Exit Door To Idle");
				if (clipDuration > 0f)
				{
					yield return new WaitForSeconds(clipDuration);
				}
				else
				{
					yield return new WaitForSeconds(0.33f);
				}
			}
			FinishedEnteringScene();
		}
	}

	public void LeaveScene(GatePosition? gate = null)
	{
		isHeroInPosition = false;
		IgnoreInputWithoutReset();
		ResetHardLandingTimer();
		SetState(ActorStates.no_input);
		SetDamageMode(DamageMode.NO_DAMAGE);
		transitionState = HeroTransitionState.EXITING_SCENE;
		CancelFallEffects();
		tilemapTestActive = false;
		SetHeroParent(null);
		StopTilemapTest();
		if (gate.HasValue)
		{
			switch (gate.Value)
			{
				case GatePosition.top:
					transition_vel = new Vector2(0f, MIN_JUMP_SPEED);
					cState.onGround = false;
					break;
				case GatePosition.right:
					transition_vel = new Vector2(RUN_SPEED, 0f);
					break;
				case GatePosition.bottom:
					transition_vel = Vector2.zero;
					cState.onGround = false;
					break;
				case GatePosition.left:
					transition_vel = new Vector2(0f - RUN_SPEED, 0f);
					break;
			}
		}
		cState.transitioning = true;
	}

	public IEnumerator BetaLeave(EndBeta betaEndTrigger)
	{
		if (!playerData.betaEnd)
		{
			endBeta = betaEndTrigger;
			IgnoreInput();
			playerData.disablePause = true;
			SetState(ActorStates.no_input);
			ResetInput();
			tilemapTestActive = false;
			yield return new WaitForSeconds(0.66f);
			GameObject.Find("Beta Ender").GetComponent<SimpleSpriteFade>().FadeIn();
			ResetMotion();
			yield return new WaitForSeconds(1.25f);
			playerData.betaEnd = true;
		}
	}

	public IEnumerator BetaReturn()
	{
		rb2d.velocity = new Vector2(RUN_SPEED, 0f);
		if (!cState.facingRight)
		{
			FlipSprite();
		}
		GameObject.Find("Beta Ender").GetComponent<SimpleSpriteFade>().FadeOut();
		animCtrl.PlayClip("Run");
		yield return new WaitForSeconds(1.4f);
		SetState(ActorStates.grounded);
		SetStartingMotionState();
		AcceptInput();
		playerData.betaEnd = false;
		playerData.disablePause = false;
		tilemapTestActive = true;
		if (endBeta != null)
		{
			endBeta.Reactivate();
		}
	}

	public IEnumerator Respawn()
	{
		playerData = PlayerData.instance;
		playerData.disablePause = true;
		base.gameObject.layer = 9;
		renderer.enabled = true;
		rb2d.isKinematic = false;
		cState.dead = false;
		cState.onGround = true;
		cState.hazardDeath = false;
		cState.recoiling = false;
		enteringVertically = false;
		airDashed = false;
		doubleJumped = false;
		CharmUpdate();
		MaxHealth();
		ClearMP();
		ResetMotion();
		ResetHardLandingTimer();
		ResetAttacks();
		ResetInput();
		CharmUpdate();
		Transform spawnPoint = LocateSpawnPoint();
		if (spawnPoint != null)
		{
			transform.SetPosition2D(FindGroundPoint(spawnPoint.transform.position));
			PlayMakerFSM component = spawnPoint.GetComponent<PlayMakerFSM>();
			if (component != null)
			{
				FSMUtility.GetVector3(component, "Adjust Vector");
			}
			else if (verboseMode)
			{
				Debug.Log("Could not find Bench Control FSM on respawn point. Ignoring Adjustment offset.");
			}
		}
		else
		{
			Debug.LogError("Couldn't find the respawn point named " + playerData.respawnMarkerName + " within objects tagged with RespawnPoint");
		}
		if (verboseMode)
		{
			Debug.Log("HC Respawn Type: " + playerData.respawnType);
		}
		GameCameras.instance.cameraFadeFSM.SendEvent("RESPAWN");
		if (playerData.respawnType == 1)
		{
			AffectedByGravity(gravityApplies: false);
			PlayMakerFSM benchFSM = FSMUtility.LocateFSM(spawnPoint.gameObject, "Bench Control");
			if (benchFSM == null)
			{
				Debug.LogError("HeroCtrl: Could not find Bench Control FSM on this spawn point, respawn type is set to Bench");
				yield break;
			}
			benchFSM.FsmVariables.GetFsmBool("RespawnResting").Value = true;
			yield return new WaitForEndOfFrame();
			if (this.heroInPosition != null)
			{
				this.heroInPosition(forceDirect: false);
			}
			proxyFSM.SendEvent("HeroCtrl-Respawned");
			FinishedEnteringScene();
			benchFSM.SendEvent("RESPAWN");
		}
		else
		{
			yield return new WaitForEndOfFrame();
			IgnoreInput();
			RespawnMarker component2 = spawnPoint.GetComponent<RespawnMarker>();
			if ((bool)component2)
			{
				if (component2.respawnFacingRight)
				{
					FaceRight();
				}
				else
				{
					FaceLeft();
				}
			}
			else
			{
				Debug.LogError("Spawn point does not contain a RespawnMarker");
			}
			if (this.heroInPosition != null)
			{
				this.heroInPosition(forceDirect: false);
			}
			if (gm.GetSceneNameString() != "GG_Atrium")
			{
				float clipDuration = animCtrl.GetClipDuration("Wake Up Ground");
				animCtrl.PlayClip("Wake Up Ground");
				StopAnimationControl();
				controlReqlinquished = true;
				yield return new WaitForSeconds(clipDuration);
				StartAnimationControl();
				controlReqlinquished = false;
			}
			proxyFSM.SendEvent("HeroCtrl-Respawned");
			FinishedEnteringScene();
		}
		playerData.disablePause = false;
		playerData.isInvincible = false;
	}

	public IEnumerator HazardRespawn()
	{
		cState.hazardDeath = false;
		cState.onGround = true;
		cState.hazardRespawning = true;
		ResetMotion();
		ResetHardLandingTimer();
		ResetAttacks();
		ResetInput();
		cState.recoiling = false;
		enteringVertically = false;
		airDashed = false;
		doubleJumped = false;
		transform.SetPosition2D(FindGroundPoint(playerData.hazardRespawnLocation, useExtended: true));
		base.gameObject.layer = 9;
		renderer.enabled = true;
		yield return new WaitForEndOfFrame();
		if (playerData.hazardRespawnFacingRight)
		{
			FaceRight();
		}
		else
		{
			FaceLeft();
		}
		if (this.heroInPosition != null)
		{
			this.heroInPosition(forceDirect: false);
		}
		StartCoroutine(Invulnerable(INVUL_TIME * 2f));
		GameCameras.instance.cameraFadeFSM.SendEvent("RESPAWN");
		float clipDuration = animCtrl.GetClipDuration("Hazard Respawn");
		animCtrl.PlayClip("Hazard Respawn");
		yield return new WaitForSeconds(clipDuration);
		cState.hazardRespawning = false;
		rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;
		FinishedEnteringScene(setHazardMarker: false);
	}

	public void StartCyclone()
	{
		nailArt_cyclone = true;
	}

	public void EndCyclone()
	{
		nailArt_cyclone = false;
	}

	public bool GetState(string stateName)
	{
		return cState.GetState(stateName);
	}

	public bool GetCState(string stateName)
	{
		return cState.GetState(stateName);
	}

	public void SetCState(string stateName, bool value)
	{
		cState.SetState(stateName, value);
	}

	public void ResetHardLandingTimer()
	{
		cState.willHardLand = false;
		hardLandingTimer = 0f;
		fallTimer = 0f;
		hardLanded = false;
	}

	public void CancelSuperDash()
	{
		superDash.SendEvent("SLOPE CANCEL");
	}

	public void RelinquishControlNotVelocity()
	{
		if (!controlReqlinquished)
		{
			prev_hero_state = ActorStates.idle;
			ResetInput();
			ResetMotionNotVelocity();
			SetState(ActorStates.no_input);
			IgnoreInput();
			controlReqlinquished = true;
			ResetLook();
			ResetAttacks();
			touchingWallL = false;
			touchingWallR = false;
		}
	}

	public void RelinquishControl()
	{
		if (!controlReqlinquished && !cState.dead)
		{
			ResetInput();
			ResetMotion();
			IgnoreInput();
			controlReqlinquished = true;
			ResetLook();
			ResetAttacks();
			touchingWallL = false;
			touchingWallR = false;
		}
	}

	public void RegainControl()
	{
		enteringVertically = false;
		doubleJumpQueuing = false;
		AcceptInput();
		hero_state = ActorStates.idle;
		if (!controlReqlinquished || cState.dead)
		{
			return;
		}
		AffectedByGravity(gravityApplies: true);
		SetStartingMotionState();
		controlReqlinquished = false;
		if (startWithWallslide)
		{
			wallSlideVibrationPlayer.Play();
			cState.wallSliding = true;
			cState.willHardLand = false;
			cState.touchingWall = true;
			airDashed = false;
			wallslideDustPrefab.enableEmission = true;
			startWithWallslide = false;
			if (transform.localScale.x < 0f)
			{
				wallSlidingR = true;
				touchingWallR = true;
			}
			else
			{
				wallSlidingL = true;
				touchingWallL = true;
			}
		}
		else if (startWithJump)
		{
			HeroJumpNoEffect();
			doubleJumpQueuing = false;
			startWithJump = false;
		}
		else if (startWithFullJump)
		{
			HeroJump();
			doubleJumpQueuing = false;
			startWithFullJump = false;
		}
		else if (startWithDash)
		{
			HeroDash();
			doubleJumpQueuing = false;
			startWithDash = false;
		}
		else if (startWithAttack)
		{
			DoAttack();
			doubleJumpQueuing = false;
			startWithAttack = false;
		}
		else
		{
			cState.touchingWall = false;
			touchingWallL = false;
			touchingWallR = false;
		}
	}

	public void PreventCastByDialogueEnd()
	{
		preventCastByDialogueEndTimer = 0.3f;
	}

	public bool CanCast()
	{
		if (!gm.isPaused && !cState.dashing && hero_state != ActorStates.no_input && !cState.backDashing && (!cState.attacking || !(attack_time < ATTACK_RECOVERY_TIME)) && !cState.recoiling && !cState.recoilFrozen && !cState.transitioning && !cState.hazardDeath && !cState.hazardRespawning && CanInput() && preventCastByDialogueEndTimer <= 0f)
		{
			return true;
		}
		return false;
	}

	public bool CanFocus()
	{
		if (!gm.isPaused && hero_state != ActorStates.no_input && !cState.dashing && !cState.backDashing && (!cState.attacking || !(attack_time < ATTACK_RECOVERY_TIME)) && !cState.recoiling && cState.onGround && !cState.transitioning && !cState.recoilFrozen && !cState.hazardDeath && !cState.hazardRespawning && CanInput())
		{
			return true;
		}
		return false;
	}

	public bool CanNailArt()
	{
		if (!cState.transitioning && hero_state != ActorStates.no_input && !cState.attacking && !cState.hazardDeath && !cState.hazardRespawning && nailChargeTimer >= nailChargeTime)
		{
			nailChargeTimer = 0f;
			return true;
		}
		nailChargeTimer = 0f;
		return false;
	}

	public bool CanQuickMap()
	{
		if (!gm.isPaused && !controlReqlinquished && hero_state != ActorStates.no_input && !cState.onConveyor && !cState.dashing && !cState.backDashing && (!cState.attacking || !(attack_time < ATTACK_RECOVERY_TIME)) && !cState.recoiling && !cState.transitioning && !cState.hazardDeath && !cState.hazardRespawning && !cState.recoilFrozen && cState.onGround && CanInput())
		{
			return true;
		}
		return false;
	}

	public bool CanInspect()
	{
		if (!gm.isPaused && !cState.dashing && hero_state != ActorStates.no_input && !cState.backDashing && (!cState.attacking || !(attack_time < ATTACK_RECOVERY_TIME)) && !cState.recoiling && !cState.transitioning && !cState.hazardDeath && !cState.hazardRespawning && !cState.recoilFrozen && cState.onGround && CanInput())
		{
			return true;
		}
		return false;
	}

	public bool CanBackDash()
	{
		if (!gm.isPaused && !cState.dashing && hero_state != ActorStates.no_input && !cState.backDashing && (!cState.attacking || !(attack_time < ATTACK_RECOVERY_TIME)) && !cState.preventBackDash && !cState.backDashCooldown && !controlReqlinquished && !cState.recoilFrozen && !cState.recoiling && !cState.transitioning && cState.onGround && playerData.canBackDash)
		{
			return true;
		}
		return false;
	}

	public bool CanSuperDash()
	{
		if (!gm.isPaused && hero_state != ActorStates.no_input && !cState.dashing && !cState.hazardDeath && !cState.hazardRespawning && !cState.backDashing && (!cState.attacking || !(attack_time < ATTACK_RECOVERY_TIME)) && !cState.slidingLeft && !cState.slidingRight && !controlReqlinquished && !cState.recoilFrozen && !cState.recoiling && !cState.transitioning && playerData.hasSuperDash && (cState.onGround || cState.wallSliding))
		{
			return true;
		}
		return false;
	}

	public bool CanDreamNail()
	{
		if (!gm.isPaused && hero_state != ActorStates.no_input && !cState.dashing && !cState.backDashing && (!cState.attacking || !(attack_time < ATTACK_RECOVERY_TIME)) && !controlReqlinquished && !cState.hazardDeath && rb2d.velocity.y > -0.1f && !cState.hazardRespawning && !cState.recoilFrozen && !cState.recoiling && !cState.transitioning && playerData.hasDreamNail && cState.onGround)
		{
			return true;
		}
		return false;
	}

	public bool CanDreamGate()
	{
		if (!gm.isPaused && hero_state != ActorStates.no_input && !cState.dashing && !cState.backDashing && (!cState.attacking || !(attack_time < ATTACK_RECOVERY_TIME)) && !controlReqlinquished && !cState.hazardDeath && !cState.hazardRespawning && !cState.recoilFrozen && !cState.recoiling && !cState.transitioning && playerData.hasDreamGate && cState.onGround)
		{
			return true;
		}
		return false;
	}

	public bool CanInteract()
	{
		if (CanInput() && hero_state != ActorStates.no_input && !gm.isPaused && !cState.dashing && !cState.backDashing && !cState.attacking && !controlReqlinquished && !cState.hazardDeath && !cState.hazardRespawning && !cState.recoilFrozen && !cState.recoiling && !cState.transitioning && cState.onGround)
		{
			return true;
		}
		return false;
	}

	public bool CanOpenInventory()
	{
		if ((!gm.isPaused && hero_state != ActorStates.airborne && !controlReqlinquished && !cState.recoiling && !cState.transitioning && !cState.hazardDeath && !cState.hazardRespawning && cState.onGround && !playerData.disablePause && !cState.dashing && CanInput()) || playerData.atBench)
		{
			return true;
		}
		return false;
	}

	public void SetDamageMode(int invincibilityType)
	{
		switch (invincibilityType)
		{
			case 0:
				damageMode = DamageMode.FULL_DAMAGE;
				break;
			case 1:
				damageMode = DamageMode.HAZARD_ONLY;
				break;
			case 2:
				damageMode = DamageMode.NO_DAMAGE;
				break;
		}
	}

	public void SetDamageModeFSM(int invincibilityType)
	{
		switch (invincibilityType)
		{
			case 0:
				damageMode = DamageMode.FULL_DAMAGE;
				break;
			case 1:
				damageMode = DamageMode.HAZARD_ONLY;
				break;
			case 2:
				damageMode = DamageMode.NO_DAMAGE;
				break;
		}
	}

	public void ResetQuakeDamage()
	{
		if (damageMode == DamageMode.HAZARD_ONLY)
		{
			damageMode = DamageMode.FULL_DAMAGE;
		}
	}

	public void SetDamageMode(DamageMode newDamageMode)
	{
		damageMode = newDamageMode;
		if (newDamageMode == DamageMode.NO_DAMAGE)
		{
			playerData.isInvincible = true;
		}
		else
		{
			playerData.isInvincible = false;
		}
	}

	public void StopAnimationControl()
	{
		animCtrl.StopControl();
	}

	public void StartAnimationControl()
	{
		animCtrl.StartControl();
	}

	public void IgnoreInput()
	{
		if (acceptingInput)
		{
			acceptingInput = false;
			ResetInput();
		}
	}

	public void IgnoreInputWithoutReset()
	{
		if (acceptingInput)
		{
			acceptingInput = false;
		}
	}

	public void AcceptInput()
	{
		acceptingInput = true;
	}

	public void Pause()
	{
		PauseInput();
		PauseAudio();
		JumpReleased();
		cState.isPaused = true;
	}

	public void UnPause()
	{
		cState.isPaused = false;
		UnPauseAudio();
		UnPauseInput();
	}

	public void NearBench(bool isNearBench)
	{
		cState.nearBench = isNearBench;
	}

	public void SetWalkZone(bool inWalkZone)
	{
		cState.inWalkZone = inWalkZone;
	}

	public void ResetState()
	{
		cState.Reset();
	}

	public void StopPlayingAudio()
	{
		audioCtrl.StopAllSounds();
	}

	public void PauseAudio()
	{
		audioCtrl.PauseAllSounds();
	}

	public void UnPauseAudio()
	{
		audioCtrl.UnPauseAllSounds();
	}

	private void PauseInput()
	{
		if (acceptingInput)
		{
			acceptingInput = false;
		}
		lastInputState = new Vector2(move_input, vertical_input);
	}

	private void UnPauseInput()
	{
		if (!controlReqlinquished)
		{
			_ = lastInputState;
			if (inputHandler.inputActions.Right.IsPressed)
			{
				move_input = lastInputState.x;
			}
			else if (inputHandler.inputActions.Left.IsPressed)
			{
				move_input = lastInputState.x;
			}
			else
			{
				rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
				move_input = 0f;
			}
			vertical_input = lastInputState.y;
			acceptingInput = true;
		}
	}

	public void SpawnSoftLandingPrefab()
	{
		softLandingEffectPrefab.Spawn(transform.position);
	}

	public void AffectedByGravity(bool gravityApplies)
	{
		_ = rb2d.gravityScale;
		if (rb2d.gravityScale > Mathf.Epsilon && !gravityApplies)
		{
			prevGravityScale = rb2d.gravityScale;
			rb2d.gravityScale = 0f;
		}
		else if (rb2d.gravityScale <= Mathf.Epsilon && gravityApplies)
		{
			rb2d.gravityScale = prevGravityScale;
			prevGravityScale = 0f;
		}
	}

	private void LookForInput()
	{
		if (!acceptingInput || gm.isPaused || !isGameplayScene)
		{
			return;
		}
		move_input = inputHandler.inputActions.MoveVector.Vector.x;
		vertical_input = inputHandler.inputActions.MoveVector.Vector.y;
		FilterInput();
		if (playerData.hasWalljump && CanWallSlide() && !cState.attacking)
		{
			if (touchingWallL && inputHandler.inputActions.Left.IsPressed && !cState.wallSliding)
			{
				airDashed = false;
				doubleJumped = false;
				wallSlideVibrationPlayer.Play();
				cState.wallSliding = true;
				cState.willHardLand = false;
				wallslideDustPrefab.enableEmission = true;
				wallSlidingL = true;
				wallSlidingR = false;
				FaceLeft();
				CancelFallEffects();
			}
			if (touchingWallR && inputHandler.inputActions.Right.IsPressed && !cState.wallSliding)
			{
				airDashed = false;
				doubleJumped = false;
				wallSlideVibrationPlayer.Play();
				cState.wallSliding = true;
				cState.willHardLand = false;
				wallslideDustPrefab.enableEmission = true;
				wallSlidingL = false;
				wallSlidingR = true;
				FaceRight();
				CancelFallEffects();
			}
		}
		if (cState.wallSliding && inputHandler.inputActions.Down.WasPressed)
		{
			CancelWallsliding();
			FlipSprite();
		}
		if (wallLocked && wallJumpedL && inputHandler.inputActions.Right.IsPressed && wallLockSteps >= WJLOCK_STEPS_SHORT)
		{
			wallLocked = false;
		}
		if (wallLocked && wallJumpedR && inputHandler.inputActions.Left.IsPressed && wallLockSteps >= WJLOCK_STEPS_SHORT)
		{
			wallLocked = false;
		}
		if (inputHandler.inputActions.Jump.WasReleased && jumpReleaseQueueingEnabled)
		{
			jumpReleaseQueueSteps = JUMP_RELEASE_QUEUE_STEPS;
			jumpReleaseQueuing = true;
		}
		if (!inputHandler.inputActions.Jump.IsPressed)
		{
			JumpReleased();
		}
		if (!inputHandler.inputActions.Dash.IsPressed)
		{
			if (cState.preventDash && !cState.dashCooldown)
			{
				cState.preventDash = false;
			}
			dashQueuing = false;
		}
		if (!inputHandler.inputActions.Attack.IsPressed)
		{
			attackQueuing = false;
		}
	}

	private void LookForQueueInput()
	{
		if (!acceptingInput || gm.isPaused || !isGameplayScene)
		{
			return;
		}
		if (inputHandler.inputActions.Jump.WasPressed)
		{
			if (CanWallJump())
			{
				DoWallJump();
			}
			else if (CanJump())
			{
				HeroJump();
			}
			else if (CanDoubleJump())
			{
				DoDoubleJump();
			}
			else if (CanInfiniteAirJump())
			{
				CancelJump();
				audioCtrl.PlaySound(HeroSounds.JUMP);
				ResetLook();
				cState.jumping = true;
			}
			else
			{
				jumpQueueSteps = 0;
				jumpQueuing = true;
				doubleJumpQueueSteps = 0;
				doubleJumpQueuing = true;
			}
		}
		if (inputHandler.inputActions.Dash.WasPressed)
		{
			if (CanDash())
			{
				HeroDash();
			}
			else
			{
				dashQueueSteps = 0;
				dashQueuing = true;
			}
		}
		if (inputHandler.inputActions.Attack.WasPressed)
		{
			if (CanAttack())
			{
				DoAttack();
			}
			else
			{
				attackQueueSteps = 0;
				attackQueuing = true;
			}
		}
		if (inputHandler.inputActions.Jump.IsPressed)
		{
			if (jumpQueueSteps <= JUMP_QUEUE_STEPS && CanJump() && jumpQueuing)
			{
				HeroJump();
			}
			else if (doubleJumpQueueSteps <= DOUBLE_JUMP_QUEUE_STEPS && CanDoubleJump() && doubleJumpQueuing)
			{
				if (cState.onGround)
				{
					HeroJump();
				}
				else
				{
					DoDoubleJump();
				}
			}
			if (CanSwim())
			{
				if (hero_state != ActorStates.airborne)
				{
					SetState(ActorStates.airborne);
				}
				cState.swimming = true;
			}
		}
		if (inputHandler.inputActions.Dash.IsPressed && dashQueueSteps <= DASH_QUEUE_STEPS && CanDash() && dashQueuing)
		{
			HeroDash();
		}
		if (inputHandler.inputActions.Attack.IsPressed && attackQueueSteps <= ATTACK_QUEUE_STEPS && CanAttack() && attackQueuing)
		{
			DoAttack();
		}
	}

	private void HeroJump()
	{
		jumpEffectPrefab.Spawn(transform.position);
		audioCtrl.PlaySound(HeroSounds.JUMP);
		ResetLook();
		cState.recoiling = false;
		cState.jumping = true;
		jumpQueueSteps = 0;
		jumped_steps = 0;
		doubleJumpQueuing = false;
	}

	private void HeroJumpNoEffect()
	{
		ResetLook();
		jump_steps = 5;
		cState.jumping = true;
		jumpQueueSteps = 0;
		jumped_steps = 0;
		jump_steps = 5;
	}

	private void DoWallJump()
	{
		wallPuffPrefab.SetActive(value: true);
		audioCtrl.PlaySound(HeroSounds.WALLJUMP);
		VibrationManager.PlayVibrationClipOneShot(wallJumpVibration);
		if (touchingWallL)
		{
			FaceRight();
			wallJumpedR = true;
			wallJumpedL = false;
		}
		else if (touchingWallR)
		{
			FaceLeft();
			wallJumpedR = false;
			wallJumpedL = true;
		}
		CancelWallsliding();
		cState.touchingWall = false;
		touchingWallL = false;
		touchingWallR = false;
		airDashed = false;
		doubleJumped = false;
		currentWalljumpSpeed = WJ_KICKOFF_SPEED;
		walljumpSpeedDecel = (WJ_KICKOFF_SPEED - RUN_SPEED) / (float)WJLOCK_STEPS_LONG;
		dashBurst.SendEvent("CANCEL");
		cState.jumping = true;
		wallLockSteps = 0;
		wallLocked = true;
		jumpQueueSteps = 0;
		jumped_steps = 0;
	}

	private void DoDoubleJump()
	{
		dJumpWingsPrefab.SetActive(value: true);
		dJumpFlashPrefab.SetActive(value: true);
		dJumpFeathers.Play();
		VibrationManager.PlayVibrationClipOneShot(doubleJumpVibration);
		audioSource.PlayOneShot(doubleJumpClip, 1f);
		ResetLook();
		cState.jumping = false;
		cState.doubleJumping = true;
		doubleJump_steps = 0;
		doubleJumped = true;
	}

	private void DoHardLanding()
	{
		AffectedByGravity(gravityApplies: true);
		ResetInput();
		SetState(ActorStates.hard_landing);
		CancelAttack();
		hardLanded = true;
		audioCtrl.PlaySound(HeroSounds.HARD_LANDING);
		hardLandingEffectPrefab.Spawn(transform.position);
	}

	private void DoAttack()
	{
		ResetLook();
		cState.recoiling = false;
		if (playerData.equippedCharm_32)
		{
			attack_cooldown = ATTACK_COOLDOWN_TIME_CH;
		}
		else
		{
			attack_cooldown = ATTACK_COOLDOWN_TIME;
		}
		if (vertical_input > Mathf.Epsilon)
		{
			Attack(AttackDirection.upward);
			StartCoroutine(CheckForTerrainThunk(AttackDirection.upward));
		}
		else if (vertical_input < 0f - Mathf.Epsilon)
		{
			if (hero_state != ActorStates.idle && hero_state != ActorStates.running)
			{
				Attack(AttackDirection.downward);
				StartCoroutine(CheckForTerrainThunk(AttackDirection.downward));
			}
			else
			{
				Attack(AttackDirection.normal);
				StartCoroutine(CheckForTerrainThunk(AttackDirection.normal));
			}
		}
		else
		{
			Attack(AttackDirection.normal);
			StartCoroutine(CheckForTerrainThunk(AttackDirection.normal));
		}
	}

	private void HeroDash()
	{
		if (!cState.onGround && !inAcid)
		{
			airDashed = true;
		}
		ResetAttacksDash();
		CancelBounce();
		audioCtrl.StopSound(HeroSounds.FOOTSTEPS_RUN);
		audioCtrl.StopSound(HeroSounds.FOOTSTEPS_WALK);
		audioCtrl.PlaySound(HeroSounds.DASH);
		ResetLook();
		cState.recoiling = false;
		if (cState.wallSliding)
		{
			FlipSprite();
		}
		else if (inputHandler.inputActions.Right.IsPressed)
		{
			FaceRight();
		}
		else if (inputHandler.inputActions.Left.IsPressed)
		{
			FaceLeft();
		}
		cState.dashing = true;
		dashQueueSteps = 0;
		HeroActions inputActions = inputHandler.inputActions;
		if (inputActions.Down.IsPressed && !cState.onGround && playerData.equippedCharm_31 && !inputActions.Left.IsPressed && !inputActions.Right.IsPressed)
		{
			dashBurst.transform.localPosition = new Vector3(-0.07f, 3.74f, 0.01f);
			dashBurst.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
			dashingDown = true;
		}
		else
		{
			dashBurst.transform.localPosition = new Vector3(4.11f, -0.55f, 0.001f);
			dashBurst.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			dashingDown = false;
		}
		if (playerData.equippedCharm_31)
		{
			dashCooldownTimer = DASH_COOLDOWN_CH;
		}
		else
		{
			dashCooldownTimer = DASH_COOLDOWN;
		}
		if (playerData.hasShadowDash && shadowDashTimer <= 0f)
		{
			shadowDashTimer = SHADOW_DASH_COOLDOWN;
			cState.shadowDashing = true;
			if (playerData.equippedCharm_16)
			{
				audioSource.PlayOneShot(sharpShadowClip, 1f);
				sharpShadowPrefab.SetActive(value: true);
			}
			else
			{
				audioSource.PlayOneShot(shadowDashClip, 1f);
			}
		}
		if (cState.shadowDashing)
		{
			if (dashingDown)
			{
				dashEffect = shadowdashDownBurstPrefab.Spawn(new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z + 0.00101f));
				dashEffect.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
			}
			else if (transform.localScale.x > 0f)
			{
				dashEffect = shadowdashBurstPrefab.Spawn(new Vector3(transform.position.x + 5.21f, transform.position.y - 0.58f, transform.position.z + 0.00101f));
				dashEffect.transform.localScale = new Vector3(1.919591f, dashEffect.transform.localScale.y, dashEffect.transform.localScale.z);
			}
			else
			{
				dashEffect = shadowdashBurstPrefab.Spawn(new Vector3(transform.position.x - 5.21f, transform.position.y - 0.58f, transform.position.z + 0.00101f));
				dashEffect.transform.localScale = new Vector3(-1.919591f, dashEffect.transform.localScale.y, dashEffect.transform.localScale.z);
			}
			shadowRechargePrefab.SetActive(value: true);
			FSMUtility.LocateFSM(shadowRechargePrefab, "Recharge Effect").SendEvent("RESET");
			shadowdashParticlesPrefab.GetComponent<ParticleSystem>().enableEmission = true;
			VibrationManager.PlayVibrationClipOneShot(shadowDashVibration);
			shadowRingPrefab.Spawn(transform.position);
		}
		else
		{
			dashBurst.SendEvent("PLAY");
			dashParticlesPrefab.GetComponent<ParticleSystem>().enableEmission = true;
			VibrationManager.PlayVibrationClipOneShot(dashVibration);
		}
		if (cState.onGround && !cState.shadowDashing)
		{
			dashEffect = backDashPrefab.Spawn(transform.position);
			dashEffect.transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
		}
	}

	private void StartFallRumble()
	{
		fallRumble = true;
		audioCtrl.PlaySound(HeroSounds.FALLING);
		GameCameras.instance.cameraShakeFSM.Fsm.Variables.FindFsmBool("RumblingFall").Value = true;
	}

	private void SetState(ActorStates newState)
	{
		switch (newState)
		{
			case ActorStates.grounded:
				newState = ((!(Mathf.Abs(move_input) > Mathf.Epsilon)) ? ActorStates.idle : ActorStates.running);
				break;
			case ActorStates.previous:
				newState = prev_hero_state;
				break;
		}
		if (newState != hero_state)
		{
			prev_hero_state = hero_state;
			hero_state = newState;
			animCtrl.UpdateState(newState);
		}
	}

	private void FinishedEnteringScene(bool setHazardMarker = true, bool preventRunBob = false)
	{
		if (isEnteringFirstLevel)
		{
			isEnteringFirstLevel = false;
		}
		else
		{
			playerData.disablePause = false;
		}
		cState.transitioning = false;
		transitionState = HeroTransitionState.WAITING_TO_TRANSITION;
		stopWalkingOut = false;
		if (exitedSuperDashing || exitedQuake)
		{
			controlReqlinquished = true;
			IgnoreInput();
		}
		else
		{
			SetStartingMotionState(preventRunBob);
			AffectedByGravity(gravityApplies: true);
		}
		if (setHazardMarker)
		{
			if (gm.startedOnThisScene || sceneEntryGate == null)
			{
				playerData.SetHazardRespawn(transform.position, cState.facingRight);
			}
			else if (!sceneEntryGate.nonHazardGate)
			{
				playerData.SetHazardRespawn(sceneEntryGate.respawnMarker);
			}
		}
		if (exitedQuake)
		{
			SetDamageMode(DamageMode.HAZARD_ONLY);
		}
		else
		{
			SetDamageMode(DamageMode.FULL_DAMAGE);
		}
		if (enterWithoutInput || exitedSuperDashing || exitedQuake)
		{
			enterWithoutInput = false;
		}
		else
		{
			AcceptInput();
		}
		gm.FinishedEnteringScene();
		if (exitedSuperDashing)
		{
			exitedSuperDashing = false;
		}
		if (exitedQuake)
		{
			exitedQuake = false;
		}
		positionHistory[0] = transform.position;
		positionHistory[1] = transform.position;
		tilemapTestActive = true;
	}

	private IEnumerator Die()
	{
		if (this.OnDeath != null)
		{
			this.OnDeath();
		}
		if (cState.dead)
		{
			yield break;
		}
		playerData.disablePause = true;
		boundsChecking = false;
		StopTilemapTest();
		cState.onConveyor = false;
		cState.onConveyorV = false;
		rb2d.velocity = new Vector2(0f, 0f);
		CancelRecoilHorizontal();
		string currentMapZone = gm.GetCurrentMapZone();
		if (currentMapZone == "DREAM_WORLD" || currentMapZone == "GODS_GLORY")
		{
			RelinquishControl();
			StopAnimationControl();
			AffectedByGravity(gravityApplies: false);
			playerData.isInvincible = true;
			ResetHardLandingTimer();
			renderer.enabled = false;
			heroDeathPrefab.SetActive(value: true);
			yield break;
		}
		if (playerData.permadeathMode == 1)
		{
			playerData.permadeathMode = 2;
		}
		AffectedByGravity(gravityApplies: false);
		HeroBox.inactive = true;
		rb2d.isKinematic = true;
		SetState(ActorStates.no_input);
		cState.dead = true;
		ResetMotion();
		ResetHardLandingTimer();
		renderer.enabled = false;
		base.gameObject.layer = 2;
		heroDeathPrefab.SetActive(value: true);
		yield return null;
		StartCoroutine(gm.PlayerDead(DEATH_WAIT));
	}

	private IEnumerator DieFromHazard(HazardType hazardType, float angle)
	{
		if (!cState.hazardDeath)
		{
			playerData.disablePause = true;
			SetHeroParent(null);
			StopTilemapTest();
			SetState(ActorStates.no_input);
			cState.hazardDeath = true;
			ResetMotion();
			ResetHardLandingTimer();
			AffectedByGravity(gravityApplies: false);
			renderer.enabled = false;
			base.gameObject.layer = 2;
			switch (hazardType)
			{
				case HazardType.SPIKES:
					{
						GameObject obj2 = spikeDeathPrefab.Spawn();
						obj2.transform.position = transform.position;
						FSMUtility.SetFloat(obj2.GetComponent<PlayMakerFSM>(), "Spike Direction", angle * 57.29578f);
						break;
					}
				case HazardType.ACID:
					{
						GameObject obj = acidDeathPrefab.Spawn();
						obj.transform.position = transform.position;
						obj.transform.localScale = transform.localScale;
						break;
					}
			}
			yield return null;
			StartCoroutine(gm.PlayerDeadFromHazard(0f));
		}
	}

	public IEnumerator StartRecoil(CollisionSide impactSide, bool spawnDamageEffect, int damageAmount)
	{
		if (cState.recoiling)
		{
			yield break;
		}
		playerData.disablePause = true;
		ResetMotion();
		AffectedByGravity(gravityApplies: false);
		switch (impactSide)
		{
			case CollisionSide.left:
				recoilVector = new Vector2(RECOIL_VELOCITY, RECOIL_VELOCITY * 0.5f);
				if (cState.facingRight)
				{
					FlipSprite();
				}
				break;
			case CollisionSide.right:
				recoilVector = new Vector2(0f - RECOIL_VELOCITY, RECOIL_VELOCITY * 0.5f);
				if (!cState.facingRight)
				{
					FlipSprite();
				}
				break;
			default:
				recoilVector = Vector2.zero;
				break;
		}
		SetState(ActorStates.no_input);
		cState.recoilFrozen = true;
		if (spawnDamageEffect)
		{
			damageEffectFSM.SendEvent("DAMAGE");
			if (damageAmount > 1)
			{
				UnityEngine.Object.Instantiate(takeHitDoublePrefab, transform.position, transform.rotation);
			}
		}
		if (playerData.equippedCharm_4)
		{
			StartCoroutine(Invulnerable(INVUL_TIME_STAL));
		}
		else
		{
			StartCoroutine(Invulnerable(INVUL_TIME));
		}
		yield return takeDamageCoroutine = StartCoroutine(gm.FreezeMoment(DAMAGE_FREEZE_DOWN, DAMAGE_FREEZE_WAIT, DAMAGE_FREEZE_UP, 0.0001f));
		cState.recoilFrozen = false;
		cState.recoiling = true;
		playerData.disablePause = false;
	}

	private IEnumerator Invulnerable(float duration)
	{
		cState.invulnerable = true;
		yield return new WaitForSeconds(DAMAGE_FREEZE_DOWN);
		invPulse.StartInvulnerablePulse();
		yield return new WaitForSeconds(duration);
		invPulse.StopInvulnerablePulse();
		cState.invulnerable = false;
		cState.recoiling = false;
	}

	private IEnumerator FirstFadeIn()
	{
		yield return new WaitForSeconds(0.25f);
		gm.FadeSceneIn();
		fadedSceneIn = true;
	}

	private void FallCheck()
	{
		if (rb2d.velocity.y <= -1E-06f)
		{
			if (CheckTouchingGround())
			{
				return;
			}
			cState.falling = true;
			cState.onGround = false;
			cState.wallJumping = false;
			proxyFSM.SendEvent("HeroCtrl-LeftGround");
			if (hero_state != ActorStates.no_input)
			{
				SetState(ActorStates.airborne);
			}
			if (cState.wallSliding)
			{
				fallTimer = 0f;
			}
			else
			{
				fallTimer += Time.deltaTime;
			}
			if (fallTimer > BIG_FALL_TIME)
			{
				if (!cState.willHardLand)
				{
					cState.willHardLand = true;
				}
				if (!fallRumble)
				{
					StartFallRumble();
				}
			}
			if (fallCheckFlagged)
			{
				fallCheckFlagged = false;
			}
		}
		else
		{
			cState.falling = false;
			fallTimer = 0f;
			if (transitionState != HeroTransitionState.ENTERING_SCENE)
			{
				cState.willHardLand = false;
			}
			if (fallCheckFlagged)
			{
				fallCheckFlagged = false;
			}
			if (fallRumble)
			{
				CancelFallEffects();
			}
		}
	}

	private void OutOfBoundsCheck()
	{
		if (isGameplayScene)
		{
			Vector2 vector = transform.position;
			if ((vector.y < -60f || vector.y > gm.sceneHeight + 60f || vector.x < -60f || vector.x > gm.sceneWidth + 60f) && !cState.dead)
			{
				_ = boundsChecking;
			}
		}
	}

	private void ConfirmOutOfBounds()
	{
		if (!boundsChecking)
		{
			return;
		}
		Debug.Log("Confirming out of bounds");
		Vector2 vector = transform.position;
		if (vector.y < -60f || vector.y > gm.sceneHeight + 60f || vector.x < -60f || vector.x > gm.sceneWidth + 60f)
		{
			if (!cState.dead)
			{
				rb2d.velocity = Vector2.zero;
				Debug.LogFormat("Pos: {0} Transition State: {1}", transform.position, transitionState);
			}
		}
		else
		{
			boundsChecking = false;
		}
	}

	private void FailSafeChecks()
	{
		if (hero_state == ActorStates.hard_landing)
		{
			hardLandFailSafeTimer += Time.deltaTime;
			if (hardLandFailSafeTimer > HARD_LANDING_TIME + 0.3f)
			{
				SetState(ActorStates.grounded);
				BackOnGround();
				hardLandFailSafeTimer = 0f;
			}
		}
		else
		{
			hardLandFailSafeTimer = 0f;
		}
		if (cState.hazardDeath)
		{
			hazardDeathTimer += Time.deltaTime;
			if (hazardDeathTimer > HAZARD_DEATH_CHECK_TIME && hero_state != ActorStates.no_input)
			{
				ResetMotion();
				AffectedByGravity(gravityApplies: false);
				SetState(ActorStates.no_input);
				hazardDeathTimer = 0f;
			}
		}
		else
		{
			hazardDeathTimer = 0f;
		}
		if (rb2d.velocity.y != 0f || cState.onGround || cState.falling || cState.jumping || cState.dashing || hero_state == ActorStates.hard_landing || hero_state == ActorStates.no_input)
		{
			return;
		}
		if (CheckTouchingGround())
		{
			floatingBufferTimer += Time.deltaTime;
			if (floatingBufferTimer > FLOATING_CHECK_TIME)
			{
				if (cState.recoiling)
				{
					CancelDamageRecoil();
				}
				BackOnGround();
				floatingBufferTimer = 0f;
			}
		}
		else
		{
			floatingBufferTimer = 0f;
		}
	}

	public Transform LocateSpawnPoint()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("RespawnPoint");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name == playerData.respawnMarkerName)
			{
				return array[i].transform;
			}
		}
		return null;
	}

	private void CancelJump()
	{
		cState.jumping = false;
		jumpReleaseQueuing = false;
		jump_steps = 0;
	}

	private void CancelDoubleJump()
	{
		cState.doubleJumping = false;
		doubleJump_steps = 0;
	}

	private void CancelDash()
	{
		if (cState.shadowDashing)
		{
			cState.shadowDashing = false;
		}
		cState.dashing = false;
		dash_timer = 0f;
		AffectedByGravity(gravityApplies: true);
		sharpShadowPrefab.SetActive(value: false);
		if (dashParticlesPrefab.GetComponent<ParticleSystem>().enableEmission)
		{
			dashParticlesPrefab.GetComponent<ParticleSystem>().enableEmission = false;
		}
		if (shadowdashParticlesPrefab.GetComponent<ParticleSystem>().enableEmission)
		{
			shadowdashParticlesPrefab.GetComponent<ParticleSystem>().enableEmission = false;
		}
	}

	private void CancelWallsliding()
	{
		wallslideDustPrefab.enableEmission = false;
		wallSlideVibrationPlayer.Stop();
		cState.wallSliding = false;
		wallSlidingL = false;
		wallSlidingR = false;
		touchingWallL = false;
		touchingWallR = false;
	}

	private void CancelBackDash()
	{
		cState.backDashing = false;
		back_dash_timer = 0f;
	}

	private void CancelDownAttack()
	{
		if (cState.downAttacking)
		{
			slashComponent.CancelAttack();
			ResetAttacks();
		}
	}

	private void CancelAttack()
	{
		if (cState.attacking)
		{
			slashComponent.CancelAttack();
			ResetAttacks();
		}
	}

	private void CancelBounce()
	{
		cState.bouncing = false;
		cState.shroomBouncing = false;
		bounceTimer = 0f;
	}

	private void CancelRecoilHorizontal()
	{
		cState.recoilingLeft = false;
		cState.recoilingRight = false;
		recoilSteps = 0;
	}

	private void CancelDamageRecoil()
	{
		cState.recoiling = false;
		recoilTimer = 0f;
		ResetMotion();
		AffectedByGravity(gravityApplies: true);
		SetDamageMode(DamageMode.FULL_DAMAGE);
	}

	private void CancelFallEffects()
	{
		fallRumble = false;
		audioCtrl.StopSound(HeroSounds.FALLING);
		GameCameras.instance.cameraShakeFSM.Fsm.Variables.FindFsmBool("RumblingFall").Value = false;
	}

	private void ResetAttacks()
	{
		cState.nailCharging = false;
		nailChargeTimer = 0f;
		cState.attacking = false;
		cState.upAttacking = false;
		cState.downAttacking = false;
		attack_time = 0f;
	}

	private void ResetAttacksDash()
	{
		cState.attacking = false;
		cState.upAttacking = false;
		cState.downAttacking = false;
		attack_time = 0f;
	}

	private void ResetMotion()
	{
		CancelJump();
		CancelDoubleJump();
		CancelDash();
		CancelBackDash();
		CancelBounce();
		CancelRecoilHorizontal();
		CancelWallsliding();
		rb2d.velocity = Vector2.zero;
		transition_vel = Vector2.zero;
		wallLocked = false;
		nailChargeTimer = 0f;
	}

	private void ResetMotionNotVelocity()
	{
		CancelJump();
		CancelDoubleJump();
		CancelDash();
		CancelBackDash();
		CancelBounce();
		CancelRecoilHorizontal();
		CancelWallsliding();
		transition_vel = Vector2.zero;
		wallLocked = false;
	}

	private void ResetLook()
	{
		cState.lookingUp = false;
		cState.lookingDown = false;
		cState.lookingUpAnim = false;
		cState.lookingDownAnim = false;
		lookDelayTimer = 0f;
	}

	private void ResetInput()
	{
		move_input = 0f;
		vertical_input = 0f;
	}

	private void BackOnGround()
	{
		if (landingBufferSteps <= 0)
		{
			landingBufferSteps = LANDING_BUFFER_STEPS;
			if (!cState.onGround && !hardLanded && !cState.superDashing)
			{
				softLandingEffectPrefab.Spawn(transform.position);
				VibrationManager.PlayVibrationClipOneShot(softLandVibration);
			}
		}
		cState.falling = false;
		fallTimer = 0f;
		dashLandingTimer = 0f;
		cState.willHardLand = false;
		hardLandingTimer = 0f;
		hardLanded = false;
		jump_steps = 0;
		if (cState.doubleJumping)
		{
			HeroJump();
		}
		SetState(ActorStates.grounded);
		cState.onGround = true;
		airDashed = false;
		doubleJumped = false;
		if (dJumpWingsPrefab.activeSelf)
		{
			dJumpWingsPrefab.SetActive(value: false);
		}
	}

	private void JumpReleased()
	{
		if (rb2d.velocity.y > 0f && jumped_steps >= JUMP_STEPS_MIN && !inAcid && !cState.shroomBouncing)
		{
			if (jumpReleaseQueueingEnabled)
			{
				if (jumpReleaseQueuing && jumpReleaseQueueSteps <= 0)
				{
					rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
					CancelJump();
				}
			}
			else
			{
				rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
				CancelJump();
			}
		}
		jumpQueuing = false;
		doubleJumpQueuing = false;
		if (cState.swimming)
		{
			cState.swimming = false;
		}
	}

	private void FinishedDashing()
	{
		CancelDash();
		AffectedByGravity(gravityApplies: true);
		animCtrl.FinishedDash();
		proxyFSM.SendEvent("HeroCtrl-DashEnd");
		if (cState.touchingWall && !cState.onGround && (playerData.hasWalljump & (touchingWallL || touchingWallR)))
		{
			wallslideDustPrefab.enableEmission = true;
			wallSlideVibrationPlayer.Play();
			cState.wallSliding = true;
			cState.willHardLand = false;
			if (touchingWallL)
			{
				wallSlidingL = true;
			}
			if (touchingWallR)
			{
				wallSlidingR = true;
			}
			if (dashingDown)
			{
				FlipSprite();
			}
		}
	}

	private void SetStartingMotionState()
	{
		SetStartingMotionState(preventRunDip: false);
	}

	private void SetStartingMotionState(bool preventRunDip)
	{
		move_input = ((acceptingInput || preventRunDip) ? inputHandler.inputActions.MoveVector.X : 0f);
		cState.touchingWall = false;
		if (CheckTouchingGround())
		{
			cState.onGround = true;
			SetState(ActorStates.grounded);
			ResetAirMoves();
			if (enteringVertically)
			{
				SpawnSoftLandingPrefab();
				animCtrl.playLanding = true;
				enteringVertically = false;
			}
		}
		else
		{
			cState.onGround = false;
			SetState(ActorStates.airborne);
		}
		animCtrl.UpdateState(hero_state);
	}

	[Obsolete("This was used specifically for underwater swimming in acid but is no longer in use.")]
	private void EnterAcid()
	{
		rb2d.gravityScale = UNDERWATER_GRAVITY;
		inAcid = true;
		cState.inAcid = true;
	}

	[Obsolete("This was used specifically for underwater swimming in acid but is no longer in use.")]
	private void ExitAcid()
	{
		rb2d.gravityScale = DEFAULT_GRAVITY;
		inAcid = false;
		cState.inAcid = false;
		airDashed = false;
		doubleJumped = false;
		if (inputHandler.inputActions.Jump.IsPressed)
		{
			HeroJump();
		}
	}

	private void TileMapTest()
	{
		if (!tilemapTestActive || cState.jumping)
		{
			return;
		}
		Vector2 vector = transform.position;
		Vector2 direction = new Vector2(positionHistory[0].x - vector.x, positionHistory[0].y - vector.y);
		float magnitude = direction.magnitude;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, direction, magnitude, 256);
		if (raycastHit2D.collider != null)
		{
			Debug.LogFormat("TERRAIN INGRESS {0} at {1} Jumping: {2}", gm.GetSceneNameString(), vector, cState.jumping);
			ResetMotion();
			rb2d.velocity = Vector2.zero;
			if (cState.dashing)
			{
				FinishedDashing();
				transform.SetPosition2D(positionHistory[1]);
			}
			if (cState.superDashing)
			{
				transform.SetPosition2D(raycastHit2D.point);
				superDash.SendEvent("HIT WALL");
			}
			if (cState.spellQuake)
			{
				spellControl.SendEvent("Hero Landed");
				transform.SetPosition2D(positionHistory[1]);
			}
			tilemapTestActive = false;
			tilemapTestCoroutine = StartCoroutine(TilemapTestPause());
		}
	}

	private IEnumerator TilemapTestPause()
	{
		yield return new WaitForSeconds(0.1f);
		tilemapTestActive = true;
	}

	private void StopTilemapTest()
	{
		if (tilemapTestCoroutine != null)
		{
			StopCoroutine(tilemapTestCoroutine);
			tilemapTestActive = false;
		}
	}

	public IEnumerator CheckForTerrainThunk(AttackDirection attackDir)
	{
		bool terrainHit = false;
		float thunkTimer = NAIL_TERRAIN_CHECK_TIME;
		while (thunkTimer > 0f)
		{
			if (!terrainHit)
			{
				float num = 0.25f;
				float num2 = ((attackDir != AttackDirection.normal) ? 1.5f : 2f);
				float num3 = 1f;
				if (playerData.equippedCharm_18)
				{
					num3 += 0.2f;
				}
				if (playerData.equippedCharm_13)
				{
					num3 += 0.3f;
				}
				num2 *= num3;
				Vector2 size = new Vector2(0.45f, 0.45f);
				Vector2 origin = new Vector2(col2d.bounds.center.x, col2d.bounds.center.y + num);
				Vector2 origin2 = new Vector2(col2d.bounds.center.x, col2d.bounds.max.y);
				Vector2 origin3 = new Vector2(col2d.bounds.center.x, col2d.bounds.min.y);
				int layerMask = 33554688;
				RaycastHit2D raycastHit2D = default(RaycastHit2D);
				switch (attackDir)
				{
					case AttackDirection.normal:
						raycastHit2D = (((!cState.facingRight || cState.wallSliding) && (cState.facingRight || !cState.wallSliding)) ? Physics2D.BoxCast(origin, size, 0f, Vector2.left, num2, layerMask) : Physics2D.BoxCast(origin, size, 0f, Vector2.right, num2, layerMask));
						break;
					case AttackDirection.upward:
						raycastHit2D = Physics2D.BoxCast(origin2, size, 0f, Vector2.up, num2, layerMask);
						break;
					case AttackDirection.downward:
						raycastHit2D = Physics2D.BoxCast(origin3, size, 0f, Vector2.down, num2, layerMask);
						break;
				}
				if (raycastHit2D.collider != null && !raycastHit2D.collider.isTrigger)
				{
					NonThunker component = raycastHit2D.collider.gameObject.GetComponent<NonThunker>();
					if (!(component != null) || !component.active)
					{
						terrainHit = true;
						nailTerrainImpactEffectPrefab.Spawn(raycastHit2D.point, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)));
						switch (attackDir)
						{
							case AttackDirection.normal:
								if (cState.facingRight)
								{
									RecoilLeft();
								}
								else
								{
									RecoilRight();
								}
								break;
							case AttackDirection.upward:
								RecoilDown();
								break;
						}
					}
				}
				thunkTimer -= Time.deltaTime;
			}
			yield return null;
		}
	}

	private bool CheckStillTouchingWall(CollisionSide side, bool checkTop = false)
	{
		Vector2 origin = new Vector2(col2d.bounds.min.x, col2d.bounds.max.y);
		Vector2 origin2 = new Vector2(col2d.bounds.min.x, col2d.bounds.center.y);
		Vector2 origin3 = new Vector2(col2d.bounds.min.x, col2d.bounds.min.y);
		Vector2 origin4 = new Vector2(col2d.bounds.max.x, col2d.bounds.max.y);
		Vector2 origin5 = new Vector2(col2d.bounds.max.x, col2d.bounds.center.y);
		Vector2 origin6 = new Vector2(col2d.bounds.max.x, col2d.bounds.min.y);
		float distance = 0.1f;
		RaycastHit2D raycastHit2D = default(RaycastHit2D);
		RaycastHit2D raycastHit2D2 = default(RaycastHit2D);
		RaycastHit2D raycastHit2D3 = default(RaycastHit2D);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		switch (side)
		{
			case CollisionSide.left:
				if (checkTop)
				{
					raycastHit2D = Physics2D.Raycast(origin, Vector2.left, distance, 256);
				}
				raycastHit2D2 = Physics2D.Raycast(origin2, Vector2.left, distance, 256);
				raycastHit2D3 = Physics2D.Raycast(origin3, Vector2.left, distance, 256);
				break;
			case CollisionSide.right:
				if (checkTop)
				{
					raycastHit2D = Physics2D.Raycast(origin4, Vector2.right, distance, 256);
				}
				raycastHit2D2 = Physics2D.Raycast(origin5, Vector2.right, distance, 256);
				raycastHit2D3 = Physics2D.Raycast(origin6, Vector2.right, distance, 256);
				break;
			default:
				Debug.LogError("Invalid CollisionSide specified.");
				return false;
		}
		if (raycastHit2D2.collider != null)
		{
			flag2 = true;
			if (raycastHit2D2.collider.isTrigger)
			{
				flag2 = false;
			}
			if (raycastHit2D2.collider.GetComponent<SteepSlope>() != null)
			{
				flag2 = false;
			}
			if (raycastHit2D2.collider.GetComponent<NonSlider>() != null)
			{
				flag2 = false;
			}
			if (flag2)
			{
				return true;
			}
		}
		if (raycastHit2D3.collider != null)
		{
			flag3 = true;
			if (raycastHit2D3.collider.isTrigger)
			{
				flag3 = false;
			}
			if (raycastHit2D3.collider.GetComponent<SteepSlope>() != null)
			{
				flag3 = false;
			}
			if (raycastHit2D3.collider.GetComponent<NonSlider>() != null)
			{
				flag3 = false;
			}
			if (flag3)
			{
				return true;
			}
		}
		if (checkTop && raycastHit2D.collider != null)
		{
			flag = true;
			if (raycastHit2D.collider.isTrigger)
			{
				flag = false;
			}
			if (raycastHit2D.collider.GetComponent<SteepSlope>() != null)
			{
				flag = false;
			}
			if (raycastHit2D.collider.GetComponent<NonSlider>() != null)
			{
				flag = false;
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckForBump(CollisionSide side)
	{
		float num = 0.025f;
		float num2 = 0.2f;
		Vector2 vector = new Vector2(col2d.bounds.min.x + num2, col2d.bounds.min.y + 0.2f);
		Vector2 vector2 = new Vector2(col2d.bounds.min.x + num2, col2d.bounds.min.y - num);
		Vector2 vector3 = new Vector2(col2d.bounds.max.x - num2, col2d.bounds.min.y + 0.2f);
		Vector2 vector4 = new Vector2(col2d.bounds.max.x - num2, col2d.bounds.min.y - num);
		float num3 = 0.32f + num2;
		RaycastHit2D raycastHit2D = default(RaycastHit2D);
		RaycastHit2D raycastHit2D2 = default(RaycastHit2D);
		switch (side)
		{
			case CollisionSide.left:
				Debug.DrawLine(vector2, vector2 + Vector2.left * num3, Color.cyan, 0.15f);
				Debug.DrawLine(vector, vector + Vector2.left * num3, Color.cyan, 0.15f);
				raycastHit2D2 = Physics2D.Raycast(vector2, Vector2.left, num3, 256);
				raycastHit2D = Physics2D.Raycast(vector, Vector2.left, num3, 256);
				break;
			case CollisionSide.right:
				Debug.DrawLine(vector4, vector4 + Vector2.right * num3, Color.cyan, 0.15f);
				Debug.DrawLine(vector3, vector3 + Vector2.right * num3, Color.cyan, 0.15f);
				raycastHit2D2 = Physics2D.Raycast(vector4, Vector2.right, num3, 256);
				raycastHit2D = Physics2D.Raycast(vector3, Vector2.right, num3, 256);
				break;
			default:
				Debug.LogError("Invalid CollisionSide specified.");
				break;
		}
		if (raycastHit2D2.collider != null && raycastHit2D.collider == null)
		{
			Vector2 vector5 = raycastHit2D2.point + new Vector2((side == CollisionSide.right) ? 0.1f : (-0.1f), 1f);
			RaycastHit2D raycastHit2D3 = Physics2D.Raycast(vector5, Vector2.down, 1.5f, 256);
			Vector2 vector6 = raycastHit2D2.point + new Vector2((side == CollisionSide.right) ? (-0.1f) : 0.1f, 1f);
			RaycastHit2D raycastHit2D4 = Physics2D.Raycast(vector6, Vector2.down, 1.5f, 256);
			if (raycastHit2D3.collider != null)
			{
				Debug.DrawLine(vector5, raycastHit2D3.point, Color.cyan, 0.15f);
				if (!(raycastHit2D4.collider != null))
				{
					return true;
				}
				Debug.DrawLine(vector6, raycastHit2D4.point, Color.cyan, 0.15f);
				float num4 = raycastHit2D3.point.y - raycastHit2D4.point.y;
				if (num4 > 0f)
				{
					Debug.Log("Bump Height: " + num4);
					return true;
				}
			}
		}
		return false;
	}

	public bool CheckNearRoof()
	{
		Vector2 origin = col2d.bounds.max;
		Vector2 origin2 = new Vector2(col2d.bounds.min.x, col2d.bounds.max.y);
		new Vector2(col2d.bounds.center.x, col2d.bounds.max.y);
		Vector2 origin3 = new Vector2(col2d.bounds.center.x + col2d.bounds.size.x / 4f, col2d.bounds.max.y);
		Vector2 origin4 = new Vector2(col2d.bounds.center.x - col2d.bounds.size.x / 4f, col2d.bounds.max.y);
		Vector2 direction = new Vector2(-0.5f, 1f);
		Vector2 direction2 = new Vector2(0.5f, 1f);
		Vector2 up = Vector2.up;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(origin2, direction, 2f, 256);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(origin, direction2, 2f, 256);
		RaycastHit2D raycastHit2D3 = Physics2D.Raycast(origin3, up, 1f, 256);
		RaycastHit2D raycastHit2D4 = Physics2D.Raycast(origin4, up, 1f, 256);
		if (raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null || raycastHit2D4.collider != null)
		{
			return true;
		}
		return false;
	}

	public bool CheckTouchingGround()
	{
		Vector2 vector = new Vector2(col2d.bounds.min.x, col2d.bounds.center.y);
		Vector2 vector2 = col2d.bounds.center;
		Vector2 vector3 = new Vector2(col2d.bounds.max.x, col2d.bounds.center.y);
		float distance = col2d.bounds.extents.y + 0.16f;
		Debug.DrawRay(vector, Vector2.down, Color.yellow);
		Debug.DrawRay(vector2, Vector2.down, Color.yellow);
		Debug.DrawRay(vector3, Vector2.down, Color.yellow);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, Vector2.down, distance, 256);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(vector2, Vector2.down, distance, 256);
		RaycastHit2D raycastHit2D3 = Physics2D.Raycast(vector3, Vector2.down, distance, 256);
		if (raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null)
		{
			return true;
		}
		return false;
	}

	private List<CollisionSide> CheckTouching(PhysLayers layer)
	{
		List<CollisionSide> list = new List<CollisionSide>(4);
		Vector3 center = col2d.bounds.center;
		float distance = col2d.bounds.extents.x + 0.16f;
		float distance2 = col2d.bounds.extents.y + 0.16f;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(center, Vector2.up, distance2, 1 << (int)layer);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(center, Vector2.right, distance, 1 << (int)layer);
		RaycastHit2D raycastHit2D3 = Physics2D.Raycast(center, Vector2.down, distance2, 1 << (int)layer);
		RaycastHit2D raycastHit2D4 = Physics2D.Raycast(center, Vector2.left, distance, 1 << (int)layer);
		if (raycastHit2D.collider != null)
		{
			list.Add(CollisionSide.top);
		}
		if (raycastHit2D2.collider != null)
		{
			list.Add(CollisionSide.right);
		}
		if (raycastHit2D3.collider != null)
		{
			list.Add(CollisionSide.bottom);
		}
		if (raycastHit2D4.collider != null)
		{
			list.Add(CollisionSide.left);
		}
		return list;
	}

	private List<CollisionSide> CheckTouchingAdvanced(PhysLayers layer)
	{
		List<CollisionSide> list = new List<CollisionSide>();
		Vector2 origin = new Vector2(col2d.bounds.min.x, col2d.bounds.max.y);
		Vector2 origin2 = new Vector2(col2d.bounds.center.x, col2d.bounds.max.y);
		Vector2 origin3 = new Vector2(col2d.bounds.max.x, col2d.bounds.max.y);
		Vector2 origin4 = new Vector2(col2d.bounds.min.x, col2d.bounds.center.y);
		Vector2 origin5 = new Vector2(col2d.bounds.max.x, col2d.bounds.center.y);
		Vector2 origin6 = new Vector2(col2d.bounds.min.x, col2d.bounds.min.y);
		Vector2 origin7 = new Vector2(col2d.bounds.center.x, col2d.bounds.min.y);
		Vector2 origin8 = new Vector2(col2d.bounds.max.x, col2d.bounds.min.y);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, Vector2.up, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(origin2, Vector2.up, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D3 = Physics2D.Raycast(origin3, Vector2.up, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D4 = Physics2D.Raycast(origin3, Vector2.right, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D5 = Physics2D.Raycast(origin5, Vector2.right, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D6 = Physics2D.Raycast(origin8, Vector2.right, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D7 = Physics2D.Raycast(origin8, Vector2.down, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D8 = Physics2D.Raycast(origin7, Vector2.down, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D9 = Physics2D.Raycast(origin6, Vector2.down, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D10 = Physics2D.Raycast(origin6, Vector2.left, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D11 = Physics2D.Raycast(origin4, Vector2.left, 0.16f, 1 << (int)layer);
		RaycastHit2D raycastHit2D12 = Physics2D.Raycast(origin, Vector2.left, 0.16f, 1 << (int)layer);
		if (raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null)
		{
			list.Add(CollisionSide.top);
		}
		if (raycastHit2D4.collider != null || raycastHit2D5.collider != null || raycastHit2D6.collider != null)
		{
			list.Add(CollisionSide.right);
		}
		if (raycastHit2D7.collider != null || raycastHit2D8.collider != null || raycastHit2D9.collider != null)
		{
			list.Add(CollisionSide.bottom);
		}
		if (raycastHit2D10.collider != null || raycastHit2D11.collider != null || raycastHit2D12.collider != null)
		{
			list.Add(CollisionSide.left);
		}
		return list;
	}

	private CollisionSide FindCollisionDirection(Collision2D collision)
	{
		Vector2 normal = collision.GetSafeContact().Normal;
		float x = normal.x;
		float y = normal.y;
		if (y >= 0.5f)
		{
			return CollisionSide.bottom;
		}
		if (y <= -0.5f)
		{
			return CollisionSide.top;
		}
		if (x < 0f)
		{
			return CollisionSide.right;
		}
		if (x > 0f)
		{
			return CollisionSide.left;
		}
		Debug.LogError("ERROR: unable to determine direction of collision - contact points at (" + normal.x + "," + normal.y + ")");
		return CollisionSide.bottom;
	}

	private bool CanJump()
	{
		if (hero_state != ActorStates.no_input && hero_state != ActorStates.hard_landing && hero_state != ActorStates.dash_landing && !cState.wallSliding && !cState.dashing && !cState.backDashing && !cState.jumping && !cState.bouncing && !cState.shroomBouncing)
		{
			if (cState.onGround)
			{
				return true;
			}
			if (ledgeBufferSteps > 0 && !cState.dead && !cState.hazardDeath && !controlReqlinquished && headBumpSteps <= 0 && !CheckNearRoof())
			{
				ledgeBufferSteps = 0;
				return true;
			}
			return false;
		}
		return false;
	}

	private bool CanDoubleJump()
	{
		if (playerData.hasDoubleJump && !controlReqlinquished && !doubleJumped && !inAcid && hero_state != ActorStates.no_input && hero_state != ActorStates.hard_landing && hero_state != ActorStates.dash_landing && !cState.dashing && !cState.wallSliding && !cState.backDashing && !cState.attacking && !cState.bouncing && !cState.shroomBouncing && !cState.onGround)
		{
			return true;
		}
		return false;
	}

	private bool CanInfiniteAirJump()
	{
		if (playerData.infiniteAirJump && hero_state != ActorStates.hard_landing && !cState.onGround)
		{
			return true;
		}
		return false;
	}

	private bool CanSwim()
	{
		if (hero_state != ActorStates.no_input && hero_state != ActorStates.hard_landing && hero_state != ActorStates.dash_landing && !cState.attacking && !cState.dashing && !cState.jumping && !cState.bouncing && !cState.shroomBouncing && !cState.onGround && inAcid)
		{
			return true;
		}
		return false;
	}

	private bool CanDash()
	{
		if (hero_state != ActorStates.no_input && hero_state != ActorStates.hard_landing && hero_state != ActorStates.dash_landing && dashCooldownTimer <= 0f && !cState.dashing && !cState.backDashing && (!cState.attacking || !(attack_time < ATTACK_RECOVERY_TIME)) && !cState.preventDash && (cState.onGround || !airDashed || cState.wallSliding) && !cState.hazardDeath && playerData.canDash)
		{
			return true;
		}
		return false;
	}

	private bool CanAttack()
	{
		if (attack_cooldown <= 0f && !cState.attacking && !cState.dashing && !cState.dead && !cState.hazardDeath && !cState.hazardRespawning && !controlReqlinquished && hero_state != ActorStates.no_input && hero_state != ActorStates.hard_landing && hero_state != ActorStates.dash_landing)
		{
			return true;
		}
		return false;
	}

	private bool CanNailCharge()
	{
		if (!cState.attacking && !controlReqlinquished && !cState.recoiling && !cState.recoilingLeft && !cState.recoilingRight && playerData.hasNailArt)
		{
			return true;
		}
		return false;
	}

	private bool CanWallSlide()
	{
		if (cState.wallSliding && gm.isPaused)
		{
			return true;
		}
		if (!cState.touchingNonSlider && !inAcid && !cState.dashing && playerData.hasWalljump && !cState.onGround && !cState.recoiling && !gm.isPaused && !controlReqlinquished && !cState.transitioning && (cState.falling || cState.wallSliding) && !cState.doubleJumping && CanInput())
		{
			return true;
		}
		return false;
	}

	private bool CanTakeDamage()
	{
		if (damageMode != DamageMode.NO_DAMAGE && transitionState == HeroTransitionState.WAITING_TO_TRANSITION && !cState.invulnerable && !cState.recoiling && !playerData.isInvincible && !cState.dead && !cState.hazardDeath && !BossSceneController.IsTransitioning)
		{
			return true;
		}
		return false;
	}

	private bool CanWallJump()
	{
		if (playerData.hasWalljump)
		{
			if (cState.touchingNonSlider)
			{
				return false;
			}
			if (cState.wallSliding)
			{
				return true;
			}
			if (cState.touchingWall && !cState.onGround)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	private bool ShouldHardLand(Collision2D collision)
	{
		if (!collision.gameObject.GetComponent<NoHardLanding>() && cState.willHardLand && !inAcid && hero_state != ActorStates.hard_landing)
		{
			return true;
		}
		return false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (cState.superDashing && (CheckStillTouchingWall(CollisionSide.left) || CheckStillTouchingWall(CollisionSide.right)))
		{
			superDash.SendEvent("HIT WALL");
		}
		if ((collision.gameObject.layer == 8 || collision.gameObject.CompareTag("HeroWalkable")) && CheckTouchingGround())
		{
			proxyFSM.SendEvent("HeroCtrl-Landed");
		}
		if (hero_state != ActorStates.no_input)
		{
			CollisionSide collisionSide = FindCollisionDirection(collision);
			if (collision.gameObject.layer != 8 && !collision.gameObject.CompareTag("HeroWalkable"))
			{
				return;
			}
			fallTrailGenerated = false;
			if (collisionSide == CollisionSide.top)
			{
				headBumpSteps = HEAD_BUMP_STEPS;
				if (cState.jumping)
				{
					CancelJump();
					CancelDoubleJump();
				}
				if (cState.bouncing)
				{
					CancelBounce();
					rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
				}
				if (cState.shroomBouncing)
				{
					CancelBounce();
					rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
				}
			}
			if (collisionSide == CollisionSide.bottom)
			{
				if (cState.attacking)
				{
					CancelDownAttack();
				}
				if (ShouldHardLand(collision))
				{
					DoHardLanding();
				}
				else if (collision.gameObject.GetComponent<SteepSlope>() == null && hero_state != ActorStates.hard_landing)
				{
					BackOnGround();
				}
				if (cState.dashing && dashingDown)
				{
					AffectedByGravity(gravityApplies: true);
					SetState(ActorStates.dash_landing);
					hardLanded = true;
				}
			}
		}
		else if (hero_state == ActorStates.no_input && transitionState == HeroTransitionState.DROPPING_DOWN && (gatePosition == GatePosition.bottom || gatePosition == GatePosition.top))
		{
			FinishedEnteringScene();
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (cState.superDashing && (CheckStillTouchingWall(CollisionSide.left) || CheckStillTouchingWall(CollisionSide.right)))
		{
			superDash.SendEvent("HIT WALL");
		}
		if (hero_state == ActorStates.no_input || collision.gameObject.layer != 8)
		{
			return;
		}
		if (collision.gameObject.GetComponent<NonSlider>() == null)
		{
			cState.touchingNonSlider = false;
			if (CheckStillTouchingWall(CollisionSide.left))
			{
				cState.touchingWall = true;
				touchingWallL = true;
				touchingWallR = false;
			}
			else if (CheckStillTouchingWall(CollisionSide.right))
			{
				cState.touchingWall = true;
				touchingWallL = false;
				touchingWallR = true;
			}
			else
			{
				cState.touchingWall = false;
				touchingWallL = false;
				touchingWallR = false;
			}
			if (CheckTouchingGround())
			{
				if (ShouldHardLand(collision))
				{
					DoHardLanding();
				}
				else if (hero_state != ActorStates.hard_landing && hero_state != ActorStates.dash_landing && cState.falling)
				{
					BackOnGround();
				}
			}
			else if (cState.jumping || cState.falling)
			{
				cState.onGround = false;
				proxyFSM.SendEvent("HeroCtrl-LeftGround");
				SetState(ActorStates.airborne);
			}
		}
		else
		{
			cState.touchingNonSlider = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (cState.recoilingLeft || cState.recoilingRight)
		{
			cState.touchingWall = false;
			touchingWallL = false;
			touchingWallR = false;
			cState.touchingNonSlider = false;
		}
		if (touchingWallL && !CheckStillTouchingWall(CollisionSide.left))
		{
			cState.touchingWall = false;
			touchingWallL = false;
		}
		if (touchingWallR && !CheckStillTouchingWall(CollisionSide.right))
		{
			cState.touchingWall = false;
			touchingWallR = false;
		}
		if (hero_state == ActorStates.no_input || cState.recoiling || collision.gameObject.layer != 8 || CheckTouchingGround())
		{
			return;
		}
		if (!cState.jumping && !fallTrailGenerated && cState.onGround)
		{
			if (playerData.environmentType != 6)
			{
				fsm_fallTrail.SendEvent("PLAY");
			}
			fallTrailGenerated = true;
		}
		cState.onGround = false;
		proxyFSM.SendEvent("HeroCtrl-LeftGround");
		SetState(ActorStates.airborne);
		if (cState.wasOnGround)
		{
			ledgeBufferSteps = LEDGE_BUFFER_STEPS;
		}
	}

	private void SetupGameRefs()
	{
		if (cState == null)
		{
			cState = new HeroControllerStates();
		}
		gm = GameManager.instance;
		animCtrl = GetComponent<HeroAnimationController>();
		rb2d = GetComponent<Rigidbody2D>();
		col2d = GetComponent<Collider2D>();
		transform = GetComponent<Transform>();
		renderer = GetComponent<MeshRenderer>();
		audioCtrl = GetComponent<HeroAudioController>();
		inputHandler = gm.GetComponent<InputHandler>();
		proxyFSM = FSMUtility.LocateFSM(base.gameObject, "ProxyFSM");
		audioSource = GetComponent<AudioSource>();
		if (!footStepsRunAudioSource)
		{
			footStepsRunAudioSource = transform.Find("Sounds/FootstepsRun").GetComponent<AudioSource>();
		}
		if (!footStepsWalkAudioSource)
		{
			footStepsWalkAudioSource = transform.Find("Sounds/FootstepsWalk").GetComponent<AudioSource>();
		}
		invPulse = GetComponent<InvulnerablePulse>();
		spriteFlash = GetComponent<SpriteFlash>();
		gm.UnloadingLevel += OnLevelUnload;
		prevGravityScale = DEFAULT_GRAVITY;
		transition_vel = Vector2.zero;
		current_velocity = Vector2.zero;
		acceptingInput = true;
		positionHistory = new Vector2[2];
		if (!artChargedEffectAnim)
		{
			artChargedEffectAnim = transform.Find("Effects").Find("NA Charged").GetComponent<tk2dSpriteAnimator>();
		}
	}

	private void SetupPools()
	{
	}

	private void FilterInput()
	{
		if (move_input > 0.3f)
		{
			move_input = 1f;
		}
		else if (move_input < -0.3f)
		{
			move_input = -1f;
		}
		else
		{
			move_input = 0f;
		}
		if (vertical_input > 0.5f)
		{
			vertical_input = 1f;
		}
		else if (vertical_input < -0.5f)
		{
			vertical_input = -1f;
		}
		else
		{
			vertical_input = 0f;
		}
	}

	public Vector3 FindGroundPoint(Vector2 startPoint, bool useExtended = false)
	{
		float num = FIND_GROUND_POINT_DISTANCE;
		if (useExtended)
		{
			num = FIND_GROUND_POINT_DISTANCE_EXT;
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(startPoint, Vector2.down, num, 256);
		if (raycastHit2D.collider == null)
		{
			Debug.LogErrorFormat("FindGroundPoint: Could not find ground point below {0}, check reference position is not too high (more than {1} tiles).", startPoint.ToString(), num);
		}
		return new Vector3(raycastHit2D.point.x, raycastHit2D.point.y + col2d.bounds.extents.y - col2d.offset.y + 0.01f, transform.position.z);
	}

	private float FindGroundPointY(float x, float y, bool useExtended = false)
	{
		float num = FIND_GROUND_POINT_DISTANCE;
		if (useExtended)
		{
			num = FIND_GROUND_POINT_DISTANCE_EXT;
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(new Vector2(x, y), Vector2.down, num, 256);
		if (raycastHit2D.collider == null)
		{
			Debug.LogErrorFormat("FindGroundPoint: Could not find ground point below ({0},{1}), check reference position is not too high (more than {2} tiles).", x, y, num);
		}
		return raycastHit2D.point.y + col2d.bounds.extents.y - col2d.offset.y + 0.01f;
	}
}
