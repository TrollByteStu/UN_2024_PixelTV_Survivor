using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    public int ID;
    public TextMeshPro Text;
    private SpriteRenderer SpriteRenderer;
    private PlayerController Player;
    private Material Mat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameController.Instance.PlayerReference;
        Mat = GetComponent<SpriteRenderer>().material;
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
            return;
        if (ID >= Player.WeaponsArray.Length)
        {
            SpriteRenderer.enabled = false;
            Text.enabled = false;
            return;
        }

        Text.enabled = true;
        SpriteRenderer.enabled = true;
        Mat.SetTexture("_Texture", Player.WeaponsArray[ID].Weapon.WeaponImage);
        Text.text = (Player.WeaponsArray[ID].Level + 1).ToString();

    }
}
