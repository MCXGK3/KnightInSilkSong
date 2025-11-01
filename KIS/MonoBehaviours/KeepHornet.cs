using UnityEngine;
using UnityEngine.UIElements;

internal class KeepHornet : MonoBehaviour
{
    internal GameObject Hornet => HeroController.instance.gameObject;
    private Vector3 offset;
    private Vector2 boxSize;
    private Vector2 boxOffset;

    private void Awake()
    {

    }
    private void Start()
    {
        base.transform.position = Hornet.transform.position;
    }
    private void OnEnable()
    {
        base.transform.position = Hornet.transform.position;
        var box = Hornet.GetComponent<BoxCollider2D>();
        offset = box.offset;
        boxSize = box.size;
        box.size = base.GetComponent<BoxCollider2D>().size;
        box.offset = base.GetComponent<BoxCollider2D>().offset;
        Hornet.GetComponent<HeroWaterController>().enabled = false;
        List<string> slashes = ["Slash", "AltSlash", "DownSlash", "UpSlash", "WallSlash"];
        foreach (var slash in slashes)
        {
            base.gameObject.FindGameObjectInChildren("Attacks").FindGameObjectInChildren(slash).FindGameObjectInChildren("Clash Tink").tag = "Nail Attack";
        }

    }
    private void Update()
    {
        Hornet.transform.position = base.transform.position;
    }
    private void FixedUpdate()
    {

    }
    private void OnDisable()
    {
        var box = Hornet.GetComponent<BoxCollider2D>();
        box.offset = offset;
        box.size = boxSize;
        Hornet.GetComponent<HeroWaterController>().enabled = true;
    }
}