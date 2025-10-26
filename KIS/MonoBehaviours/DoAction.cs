public class DoAction : MonoBehaviour
{
    public Action<DoAction> action;
    public void DoActionNow()
    {
        action?.Invoke(this);
    }
}