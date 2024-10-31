using UnityEngine;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour
{
    public UI_Slotmachine myMachine;

    public RectTransform Mover;
    public Image Image1;
    public Image Image2;
    public Image Image3;
    public Image Image4;

    private bool isRolling = false;
    private float mover = 0f;
    private float offset = 0f;

    public void Setup( UI_Slotmachine master, float newOffset)
    {
        myMachine = master;
        offset = newOffset;
    }

    public void StartRolling()
    {
        isRolling = true;
        mover = 0f;
    }
    public void StopRolling()
    {
        isRolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRolling) return;
        mover -= Time.deltaTime*250f;
        Mover.localPosition = new Vector2(0f,mover+offset);
        while (mover<-35f)
        { // moveup, reshuffle
            mover += 35f;
        }
    }
}
