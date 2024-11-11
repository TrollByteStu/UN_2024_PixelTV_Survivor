using UnityEngine;

public class KillParent : MonoBehaviour
{
    
    public void OnFinish()
    {
        Destroy(transform.parent.gameObject);
    }

}
