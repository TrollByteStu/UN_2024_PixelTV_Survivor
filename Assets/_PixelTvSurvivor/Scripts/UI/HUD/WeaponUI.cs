using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public int ID;
    public TMP_Text Text;
    public Image WeaponImage;
    private PlayerController Player;
    private Material Mat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameController.Instance.PlayerReference;
        Mat = WeaponImage.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
            return;
        if (ID >= Player.WeaponsArray.Length)
        {
            //SpriteRenderer.enabled = false;
            Text.enabled = false;
            return;
        }

        Text.enabled = true;
        //SpriteRenderer.enabled = true;
        //Mat.SetTexture("_Texture", Player.WeaponsArray[ID].Weapon.WeaponImage);
        WeaponImage.sprite = Player.WeaponsArray[ID].Weapon.WeaponImage;
        Text.text = (Player.WeaponsArray[ID].Level + 1).ToString();

    }
}
