using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    public int ID;
    public TextMeshPro Text;
    private PlayerController Player;
    private Material Mat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameController.Instance.PlayerReference;
        Mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
            return;
        if (ID > Player.WeaponsArray.Length)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        Mat.SetTexture("_Texture", Player.WeaponsArray[ID].Weapon.WeaponImage);
        Text.text = Player.WeaponsArray[ID].Level.ToString();

    }
}
