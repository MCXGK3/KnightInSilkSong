using KIS;

[RequireComponent(typeof(SpriteRenderer))]
internal class CheckKnight : MonoBehaviour
{
    SpriteRenderer render;
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (KnightInSilksong.IsKnight)
        {
            if (render != null)
            {
                render.enabled = true;
            }
        }
        else
        {
            if (render != null)
            {
                render.enabled = false;
            }
        }
    }

}