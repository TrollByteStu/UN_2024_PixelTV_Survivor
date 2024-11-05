using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public LootItemScriptable ItemType;

    private SpriteRenderer SpriteRenderer;
    private GameObject Player;

    public bool DoneDropAnimation;
    public bool MoveToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameController.Instance.PlayerReference.gameObject;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.sprite = ItemType.ItemSprite;
        SpriteRenderer.color = ItemType.spriteColor;
        transform.localScale = new Vector3(ItemType.spriteScale, ItemType.spriteScale,1);
        Destroy(gameObject, 20);
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, (Vector3.Distance(transform.position, Player.transform.position) + 5) * Time.deltaTime);
            if (Vector3.Distance(transform.position, Player.transform.position) < 0.1f)
            {
                PlayerController PlayerRef = GameController.Instance.PlayerReference;

                // actually upgrade stats
                if (ItemType.Stats.HealthIncrease > 0) PlayerRef.Stats.Health += ItemType.Stats.HealthIncrease;
                if (PlayerRef.Stats.Health > PlayerRef.Stats.MaxHealth) PlayerRef.Stats.Health = PlayerRef.Stats.MaxHealth;
                if (ItemType.Stats.MaxHealthIncrease > 0) PlayerRef.Stats.MaxHealth += ItemType.Stats.MaxHealthIncrease;
                if (ItemType.Stats.HealthModifierIncrease > 0) PlayerRef.Stats.HealthModifier += ItemType.Stats.HealthModifierIncrease;
                if (ItemType.Stats.RecoveryIncrease > 0) PlayerRef.Stats.Recovery += ItemType.Stats.RecoveryIncrease;
                if (ItemType.Stats.ArmorIncrease > 0) PlayerRef.Stats.Armor += ItemType.Stats.ArmorIncrease;
                if (ItemType.Stats.MoveSpeedIncrease > 0) PlayerRef.Stats.MoveSpeed += ItemType.Stats.MoveSpeedIncrease;
                if (ItemType.Stats.MoveSpeedModifierIncrease > 0) PlayerRef.Stats.MoveSpeedModifier += ItemType.Stats.MoveSpeedModifierIncrease;
                if (ItemType.Stats.DamageModifierIncrease > 0) PlayerRef.Stats.DamageModifier += ItemType.Stats.DamageModifierIncrease;
                if (ItemType.Stats.CooldownModifierIncrease > 0) PlayerRef.Stats.CooldownModifier += ItemType.Stats.CooldownModifierIncrease;
                if (ItemType.Stats.XpModifierIncrease > 0) PlayerRef.Stats.XpModifier += ItemType.Stats.XpModifierIncrease;

                // actually add/upgrade weapons
                if (ItemType.givenWeapon.Weapon != null) PlayerRef.AddWeapon(ItemType.givenWeapon);

                // popup text
                var popup = Instantiate(GameController.Instance.PopupTextPrefab, transform.position, Quaternion.identity).GetComponent<PopupTextHandler>();
                popup.TextContent = ItemType.PopupText;
                popup.TextColor = ItemType.PopupColor;
                popup.playThis = ItemType.pickupAudio;

                // no more object flying after player
                Destroy(gameObject);
            }
        }
    }

    public void Pickup()
    {
        DoneDropAnimation = true;
        MoveToPlayer = true;
        transform.tag = "PickedUp";
        Destroy(GetComponent<BoxCollider2D>());
    }
}
