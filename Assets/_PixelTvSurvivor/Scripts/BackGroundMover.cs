using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    public Transform PlayerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerTransform = GameController.Instance.PlayerReference.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerTransform.position;
    }
}
