using UnityEngine;

internal class KeepHornet : MonoBehaviour
{
    internal GameObject Hornet => HeroController.instance.gameObject;
    private Vector3 offset;

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
    }
    private void Update()
    {
        Hornet.transform.position = base.transform.position;
    }
    private void FixedUpdate()
    {

    }
}