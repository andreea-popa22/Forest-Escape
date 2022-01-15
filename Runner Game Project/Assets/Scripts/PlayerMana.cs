using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] float maxMana = 100f;
    private const float minMana = 0f;
    [SerializeField] float manaPoints = 0f;
    [NonSerialized] public GameObject manaBar;
    private float fullManaLevel = 4f;
    private SpriteRenderer spriteBar;
    private GameObject manaSprite;
    private Vector2 screenBounds;
    private PlayerController pc; 

    // Start is called before the first frame update
    void Start()
    {
        manaBar = GameObject.Find("ManaBar");
        spriteBar = GameObject.Find("Mana Bar Sprite").GetComponent<SpriteRenderer>();
        pc = this.gameObject.GetComponent<PlayerController>();

        SetMana(100f);
        //screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        if (pc.hasInvincibility)
        {
            manaPoints -= Time.deltaTime * 25;
            TransformManaBar();
            if (manaPoints < 0)
            {
                pc.hasInvincibility = false;
                Debug.Log("Player invincible no more!");
            }
        }
    }

    public float GetMaxMana()
    {
        return maxMana;
    }

    public float GetMana()
    {
        return manaPoints;
    }

    public void SetMana(float mana)
    {
        if (mana > maxMana) { mana = maxMana; }
        if (mana < minMana) { mana = minMana; }
        manaPoints = mana;
        TransformManaBar();
    }

    public void AddMana(float mana)
    {
        manaPoints += mana;
        

        TransformManaBar();
    }

    public void TransformManaBar()
    {
        Vector3 spriteBarScale = spriteBar.transform.localScale;

        spriteBarScale.x = (manaPoints / maxMana) * fullManaLevel;

        spriteBar.transform.localScale = new Vector3(spriteBarScale.x, spriteBarScale.y, spriteBarScale.z);
    }
}
