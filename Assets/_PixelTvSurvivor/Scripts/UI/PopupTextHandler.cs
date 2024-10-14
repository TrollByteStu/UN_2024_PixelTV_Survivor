using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupTextHandler : MonoBehaviour
{
    public string TextContent;
    public Color TextColor;

    public AnimationCurve VisibilityCurve;
    public TMP_Text TextElementReference;

    private float timeLeft = 2f;

    // Start is called before the first frame update
    void Start()
    {
        TextElementReference.text = TextContent;
        TextElementReference.color = TextColor;
    }

    // Update is called once per frame
    void Update()
    {

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) Destroy(gameObject);
    }
}
