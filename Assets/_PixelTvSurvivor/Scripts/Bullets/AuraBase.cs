using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class AuraBase : MonoBehaviour
{
    private float Spawntime;
    private Material Material;
    private bool Invers;
    private float Duration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Material.SetFloat("_Time1", math.pow(-1, Convert.ToInt16(Invers)) * Time.timeSinceLevelLoad );
        Material.SetFloat("_FadeOut", 0.5f -(Time.timeSinceLevelLoad - Spawntime) / Duration);
    }

    public void Setup(Texture texture , bool invers,Vector3 scale,float duration)
    {
        Spawntime = Time.timeSinceLevelLoad;
        Material = GetComponent<SpriteRenderer>().material;
        Material.SetTexture("_Texture2D", texture);
        Invers = invers;
        transform.localScale = scale;
        Duration = duration;
        Material.SetFloat("_FadeOut", 0.5f);
    }

    public void AuraReset(Vector3 scale)
    {
        transform.localScale = scale;
        Spawntime = Time.timeSinceLevelLoad;
        Material.SetFloat("_FadeOut", 0.5f);
    }
}
