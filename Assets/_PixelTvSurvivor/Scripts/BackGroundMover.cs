using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    public Transform PlayerTransform;
    public Material Material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerTransform)
            PlayerTransform = GameController.Instance.PlayerReference.transform;
        transform.position = PlayerTransform.position;
        Material.SetFloat("_TimeT",Time.timeSinceLevelLoad);
    }
}
