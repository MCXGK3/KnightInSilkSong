using GlobalEnums;
using UnityEngine;
namespace Knight;

public class HeroBox : MonoBehaviour
{
	public static bool inactive;

	private HeroController heroCtrl;

	private GameObject damagingObject;

	private bool isHitBuffered;

	private int damageDealt;

	private int hazardType;

	private CollisionSide collisionSide;

	private void Start()
	{
		heroCtrl = HeroController.instance;
	}

	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		if (!inactive)
		{
			CheckForDamage(otherCollider);
		}
	}

	private void OnTriggerStay2D(Collider2D otherCollider)
	{
		if (!inactive)
		{
			CheckForDamage(otherCollider);
		}
	}

	private void CheckForDamage(Collider2D otherCollider)
	{
		if (FSMUtility.ContainsFSM(otherCollider.gameObject, "damages_hero"))
		{
			PlayMakerFSM fsm = FSMUtility.LocateFSM(otherCollider.gameObject, "damages_hero");
			int damageAmount = FSMUtility.GetInt(fsm, "damageDealt");
			int num = FSMUtility.GetInt(fsm, "hazardType");
			if (otherCollider.transform.position.x > base.transform.position.x)
			{
				heroCtrl.TakeDamage(otherCollider.gameObject, CollisionSide.right, damageAmount, num);
			}
			else
			{
				heroCtrl.TakeDamage(otherCollider.gameObject, CollisionSide.left, damageAmount, num);
			}
			return;
		}
		DamageHero component = otherCollider.gameObject.GetComponent<DamageHero>();
		if (component != null && (!heroCtrl.cState.shadowDashing /*|| !component.shadowDashHazard*/))
		{
			damageDealt = component.damageDealt;
			hazardType = (int)component.hazardType;
			damagingObject = otherCollider.gameObject;
			collisionSide = ((!(damagingObject.transform.position.x > base.transform.position.x)) ? CollisionSide.left : CollisionSide.right);
			if (!IsHitTypeBuffered(hazardType))
			{
				ApplyBufferedHit();
			}
			else
			{
				isHitBuffered = true;
			}
		}
	}

	private static bool IsHitTypeBuffered(int hazardType)
	{
		return hazardType == 0;
	}

	private void LateUpdate()
	{
		if (isHitBuffered)
		{
			ApplyBufferedHit();
		}
	}

	private void ApplyBufferedHit()
	{
		heroCtrl.TakeDamage(damagingObject, collisionSide, damageDealt, hazardType);
		isHitBuffered = false;
	}
}
