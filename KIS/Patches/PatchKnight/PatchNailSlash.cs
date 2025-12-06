using KIS;
using Knight;
using HutongGames.PlayMaker;
using UnityEngine.SceneManagement;

[HarmonyPatch(typeof(Knight.NailSlash), "OnTriggerEnter2D")]
public class Patch_NailSlash_OnTriggerEnter2D : GeneralPatch
{
    public static bool Prefix(Knight.PlayerData __instance, Collider2D otherCollider)
    {
        if (!PlayerData.instance.hasHarpoonDash)
            return true;

        if (otherCollider.gameObject.tag == "Harpoon Ring")
        {
            GameObject ring = otherCollider.gameObject;
            Console.WriteLine(ring.name);

            PlayMakerFSM rideFSM = null;
            foreach (PlayMakerFSM fsm in ring.GetComponents<PlayMakerFSM>())
            {
                if (fsm.FsmName == "ride")
                    rideFSM = fsm;
                if (fsm.FsmName == "Rail Control")
                    rideFSM = fsm;
            }


            if (rideFSM != null)
            {
                rideFSM.SendEvent("SELF RING GRAB");
                rideFSM.SendEvent("CONNECT");

                Rigidbody2D rb2d = Knight.HeroController.instance.GetComponent<Rigidbody2D>();
                rb2d.position = new Vector2(rb2d.position.x, rb2d.position.y + 0.3f);
            }

            if (SceneManager.GetActiveScene().name.ToLower() == "under_18" && ring.name == "Harpoon Ring Citadel")
            {
                PlayMakerFSM pullFSM = null;

                GameObject newRing = GameObject.Find("Harpoon Ring Pull Switch");
                foreach (PlayMakerFSM fsm in newRing.GetComponents<PlayMakerFSM>())
                {
                    if (fsm.FsmName == "Pull Control")
                    {
                        pullFSM = fsm;
                    }
                }
                pullFSM.SendEvent("CONNECT");
            }

            Transform p1 = ring.transform.parent;

            if (p1 != null)
            {
                Transform p2 = p1.transform.parent;
                if (p1 != null)
                {
                    GameObject gate = p2.gameObject;
                    if (gate.GetComponent<HarpoonRingSlideLock>() != null)
                    {
                        // now we know it is a gate
                        openHarpoonGate(gate);
                    }
                }
            }
        }
        return true;
    }

    private static void openHarpoonGate(GameObject gate)
    {
        HarpoonRingSlideLock lockC = gate.GetComponent<HarpoonRingSlideLock>();
        lockC.HeroOnRing();
    }
}
