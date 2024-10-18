using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupTextHandler : MonoBehaviour
{
    public string TextContent;
    public Color TextColor;
    public AudioClip playThis;

    public AnimationCurve VisibilityCurve;
    public TMP_Text TextElementReference;
    public AudioSource myASref;

    private float timeLeft = 3f;

    // Start is called before the first frame update
    void Start()
    {
        TextElementReference.text = TextContent;
        TextElementReference.color = TextColor;
        if (playThis != null)
        {
            myASref.clip = playThis;
            myASref.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;
        TextElementReference.color = Color.Lerp(TextColor, Color.clear, VisibilityCurve.Evaluate(timeLeft));
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) Destroy(gameObject);
    }
}
