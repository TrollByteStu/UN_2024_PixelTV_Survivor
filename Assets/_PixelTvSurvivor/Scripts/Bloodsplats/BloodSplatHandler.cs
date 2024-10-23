using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatHandler : MonoBehaviour
{
    [Header("Internal references")]
    public SpriteRenderer mySpriteRenderer;
    public Animator myAnimator;
    public AudioSource myAudioSource;

    [Header("Bloodsplat Spites and Animation")]
    public Sprite[] BloodSprites;
    public RuntimeAnimatorController[] AnimController;

    private int randomSprite;

    // Start is called before the first frame update
    void Start()
    {
        randomSprite = Random.Range(0, BloodSprites.Length);
        mySpriteRenderer.sprite = BloodSprites[randomSprite];
        myAnimator.runtimeAnimatorController = AnimController[randomSprite];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
