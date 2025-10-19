using System.Collections;
using GlobalEnums;
using UnityEngine;
namespace Knight;

public class HeroAudioController : MonoBehaviour
{
	private HeroController heroCtrl;

	[Header("Sound Effects")]
	public AudioSource softLanding;

	public AudioSource hardLanding;

	public AudioSource jump;

	public AudioSource takeHit;

	public AudioSource backDash;

	public AudioSource dash;

	public AudioSource footStepsRun;

	public AudioSource footStepsWalk;

	public AudioSource wallslide;

	public AudioSource nailArtCharge;

	public AudioSource nailArtReady;

	public AudioSource falling;

	public AudioSource walljump;

	private Coroutine fallingCo;

	private void Awake()
	{
		heroCtrl = GetComponent<HeroController>();
	}

	public void PlaySound(HeroSounds soundEffect)
	{
		if (heroCtrl.cState.isPaused)
		{
			return;
		}
		switch (soundEffect)
		{
		case HeroSounds.SOFT_LANDING:
			RandomizePitch(jump, 0.9f, 1.1f);
			softLanding.Play();
			break;
		case HeroSounds.HARD_LANDING:
			hardLanding.Play();
			break;
		case HeroSounds.JUMP:
			RandomizePitch(jump, 0.9f, 1.1f);
			jump.Play();
			break;
		case HeroSounds.WALLJUMP:
			RandomizePitch(walljump, 0.9f, 1.1f);
			walljump.Play();
			break;
		case HeroSounds.TAKE_HIT:
			takeHit.Play();
			break;
		case HeroSounds.DASH:
			dash.Play();
			break;
		case HeroSounds.WALLSLIDE:
			wallslide.Play();
			break;
		case HeroSounds.BACKDASH:
			backDash.Play();
			break;
		case HeroSounds.FOOTSTEPS_RUN:
			if (!footStepsRun.isPlaying && !softLanding.isPlaying)
			{
				footStepsRun.Play();
			}
			break;
		case HeroSounds.FOOTSTEPS_WALK:
			if (!footStepsWalk.isPlaying && !softLanding.isPlaying)
			{
				footStepsWalk.Play();
			}
			break;
		case HeroSounds.NAIL_ART_CHARGE:
			nailArtCharge.Play();
			break;
		case HeroSounds.NAIL_ART_READY:
			nailArtReady.Play();
			break;
		case HeroSounds.FALLING:
			fallingCo = StartCoroutine(FadeInVolume(falling, 0.7f));
			falling.Play();
			break;
		}
	}

	public void StopSound(HeroSounds soundEffect)
	{
		switch (soundEffect)
		{
		case HeroSounds.FOOTSTEPS_RUN:
			footStepsRun.Stop();
			break;
		case HeroSounds.FOOTSTEPS_WALK:
			footStepsWalk.Stop();
			break;
		case HeroSounds.WALLSLIDE:
			wallslide.Stop();
			break;
		case HeroSounds.NAIL_ART_CHARGE:
			nailArtCharge.Stop();
			break;
		case HeroSounds.NAIL_ART_READY:
			nailArtReady.Stop();
			break;
		case HeroSounds.FALLING:
			falling.Stop();
			if (fallingCo != null)
			{
				StopCoroutine(fallingCo);
			}
			break;
		}
	}

	public void StopAllSounds()
	{
		softLanding.Stop();
		hardLanding.Stop();
		jump.Stop();
		takeHit.Stop();
		backDash.Stop();
		dash.Stop();
		footStepsRun.Stop();
		footStepsWalk.Stop();
		wallslide.Stop();
		nailArtCharge.Stop();
		nailArtReady.Stop();
		falling.Stop();
	}

	public void PauseAllSounds()
	{
		softLanding.Pause();
		hardLanding.Pause();
		jump.Pause();
		takeHit.Pause();
		backDash.Pause();
		dash.Pause();
		footStepsRun.Pause();
		footStepsWalk.Pause();
		wallslide.Pause();
		nailArtCharge.Pause();
		nailArtReady.Pause();
		falling.Pause();
	}

	public void UnPauseAllSounds()
	{
		softLanding.UnPause();
		hardLanding.UnPause();
		jump.UnPause();
		takeHit.UnPause();
		backDash.UnPause();
		dash.UnPause();
		footStepsRun.UnPause();
		footStepsWalk.UnPause();
		wallslide.UnPause();
		nailArtCharge.UnPause();
		nailArtReady.UnPause();
		falling.UnPause();
	}

	private void RandomizePitch(AudioSource src, float minPitch, float maxPitch)
	{
		float pitch = Random.Range(minPitch, maxPitch);
		src.pitch = pitch;
	}

	private void ResetPitch(AudioSource src)
	{
		src.pitch = 1f;
	}

	private IEnumerator FadeInVolume(AudioSource src, float duration)
	{
		float elapsedTime = 0f;
		src.volume = 0f;
		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / duration;
			src.volume = Mathf.Lerp(0f, 1f, t);
			yield return null;
		}
	}
}
