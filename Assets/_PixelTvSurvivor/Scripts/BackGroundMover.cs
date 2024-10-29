using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    public Transform PlayerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerTransform)
            PlayerTransform = GameController.Instance.PlayerReference.transform;
        transform.position = PlayerTransform.position;
    }
}
