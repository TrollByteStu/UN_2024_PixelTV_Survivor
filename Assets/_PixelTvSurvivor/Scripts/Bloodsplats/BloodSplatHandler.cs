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
    private float Lifetime;

    // Start is called before the first frame update
    public void Start()
    {
        // random graphics
        randomSprite = Random.Range(0, BloodSprites.Length);
        mySpriteRenderer.sprite = BloodSprites[randomSprite];
        myAnimator.runtimeAnimatorController = AnimController[randomSprite];

        // random size and 50% flip
        randomSize = Random.Range(0.75f, 1.25f);
        if ( Random.value > 0.5f ) transform.localScale = new Vector3(-randomSize, randomSize, 1);
            else new Vector3(randomSize, randomSize, 1);

        Lifetime = Time.time + 10f;
        // the more shouting kids, the lower the volume
        myAudioSource.volume = 1 / Mathf.Log(GameController.Instance.currentBloodSplats + 1, 3);
        //Debug.Log("Volume " + myAudioSource.volume.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > Lifetime) GameController.Instance.BloodPool_Release(gameObject);
    }

    public void colorRange(Color colorA, Color colorB)
    { // choose randomly between the two colors
        mySpriteRenderer.color = Color.Lerp(colorA, colorB, Random.value);
    }

    public void audioRange(AudioClip[] Sounds)
    {
        if (Sounds.Length == 0 || Random.value > 0.05f) return;
        myAudioSource.clip = Sounds[Random.Range(0, Sounds.Length)];
        myAudioSource.pitch = Random.Range(0.75f, 1.25f);
        myAudioSource.Play();
    }
}
