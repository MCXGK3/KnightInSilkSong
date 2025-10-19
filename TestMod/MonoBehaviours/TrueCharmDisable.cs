internal class TrueCharmDisable : MonoBehaviour
{
    private void OnDisable()
    {
        if (base.gameObject.activeSelf)
        {
            Patch_InventoryItemSelectableDirectional_Awake.ToggleCharm();
        }
    }
}