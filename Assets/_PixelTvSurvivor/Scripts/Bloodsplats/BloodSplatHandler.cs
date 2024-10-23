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

    // the internal ones
    private int randomSprite;
    private float randomSize;

    // Start is called before the first frame update
    void Start()
    {
        // random graphics
        randomSprite = Random.Range(0, BloodSprites.Length);
        mySpriteRenderer.sprite = BloodSprites[randomSprite];
        myAnimator.runtimeAnimatorController = AnimController[randomSprite];

        // random size and 50% flip
        randomSize = Random.Range(0.75f, 1.25f);
        if ( Random.value > 0.5f ) transform.localScale = new Vector3(-randomSize, randomSize, 1);
            else new Vector3(randomSize, randomSize, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void colorRange(Color colorA, Color colorB)
    { // choose randomly between the two colors
        mySpriteRenderer.color = Color.Lerp(colorA, colorB, Random.value);
    }

    public void audioRange(AudioClip[] Sounds)
    {
        if (Sounds.Length == 0 || Random.value > 0.2f) return;
        myAudioSource.clip = Sounds[Random.Range(0, Sounds.Length)];
        myAudioSource.Play();
    }
}
