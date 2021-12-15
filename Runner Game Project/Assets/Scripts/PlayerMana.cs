using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] float manaPoints = 0f;
    public GameObject manaBar;
    private float fullManaLevel = 4f;
    private SpriteRenderer spriteBar;
    private GameObject manaSprite;
    private Vector2 screenBounds;

    

    // Start is called before the first frame update
    void Start()
    {
        manaBar = GameObject.Find("ManaBar");
        spriteBar = GameObject.Find("Mana Bar Sprite").GetComponent<SpriteRenderer>();

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMana(float mana)
    {
        manaPoints += mana;
        if (manaPoints > 100f)
        {
            manaPoints = 100f;
        }

        TransformManaBar();
        CollisionHandler ch = gameObject.GetComponent<CollisionHandler>();
    }

    public void TransformManaBar()
    {
        Vector3 spriteBarScale = spriteBar.transform.localScale;

        spriteBarScale.x = (manaPoints / 100) * fullManaLevel;

        spriteBar.transform.localScale = new Vector3(spriteBarScale.x, spriteBarScale.y, spriteBarScale.z);
    }
}
