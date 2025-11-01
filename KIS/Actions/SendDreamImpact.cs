using System.Collections;
using HutongGames.PlayMaker;
using KIS.Utils;

public class SendDreamImpact : FsmStateAction
{
    public FsmOwnerDefault target = new()
    {
        ownerOption = OwnerDefaultOption.SpecifyGameObject
    };
    NeedolinTextOwner needolinTextOwner;
    public DreamHelper dreamHelper = Knight.HeroController.instance.GetComponent<DreamHelper>();
    public override void Reset()
    {
        base.Reset();
        target = new FsmOwnerDefault();
    }
    public override void OnEnter()
    {
        base.OnEnter();

        target.ownerOption = OwnerDefaultOption.SpecifyGameObject;
        target.gameObject = fsm.GetVariable<FsmGameObject>("Collider");
        GameObject safe = target.GetSafe(this);
        ("ONENTER " + safe).LogInfo();
        if (safe != null)
        {
            var hm = safe.GetComponent<HealthManager>();
            if (hm != null)
            {
                safe.LogInfo();
                if (!dreamHelper.Exist(hm))
                {
                    DoDreamImpact(safe);
                    dreamHelper.Add(hm);
                }
            }
        }
        Finish();

    }
    public void DoDreamImpact(GameObject safe)
    {
        int amount = Knight.PlayerData.instance.equippedCharm_30 ? 66 : 33;
        Knight.HeroController.instance.AddMPCharge(amount);
        Recoil recoil = safe.GetComponent<Recoil>();
        if (recoil != null)
        {
            bool flag = Knight.HeroController.instance.transform.localScale.x <= 0f;
            recoil.RecoilByDirection((!flag) ? 2 : 0, 2f);
        }
        SpriteFlash spriteFlash = safe.GetComponent<SpriteFlash>();
        if (spriteFlash != null)
        {
            spriteFlash.flashDreamImpact();
        }
        needolinTextOwner = safe.GetComponent<NeedolinTextOwner>();
        if (needolinTextOwner != null)
        {
            needolinTextOwner.OnAddText.AddListener(RemoveText);
            needolinTextOwner.AddNeedolinText();
        }
    }

    private void RemoveText()
    {
        needolinTextOwner.RemoveNeedolinText();
        needolinTextOwner.OnAddText.RemoveListener(RemoveText);
    }

    public IEnumerator DelayText(NeedolinTextOwner needolinTextOwner)
    {
        if (needolinTextOwner == null)
        {
            yield break;
        }
        needolinTextOwner.AddNeedolinText();
        yield return new WaitForSeconds(0.2f);
        NeedolinMsgBox._instance.ClearAllText();
        yield break;
    }
}