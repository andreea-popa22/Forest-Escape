using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] float manaPoints = 100f;
    public GameObject manaBar;
    private float fullManaLevel;
    private SpriteRenderer sprite;
    private GameObject manaSprite;
    private Vector2 screenBounds;

    

    // Start is called before the first frame update
    void Start()
    {
        manaBar = GameObject.Find("ManaBar");
        fullManaLevel = manaBar.transform.localScale.x;

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        sprite = GameObject.Find("Mana Bar Sprite").GetComponent<SpriteRenderer>();
    }

    public void LoseMana(float damage)
    {
        manaPoints -= damage;
        TransformManaBar();
        CollisionHandler ch = gameObject.GetComponent<CollisionHandler>();
        Debug.Log(manaPoints);
        if (manaPoints <= 0)
        {
            Debug.Log("You lost!");
            gameObject.GetComponent<PlayerController>().enabled = false;
            ch.Invoke("ReloadLevel", 1f);
        }
    }

    public void TransformManaBar()
    {
        Vector3 manaBarScale = manaBar.transform.localScale;
        float manaLevel = manaBarScale.x;
        manaLevel -= (0.25f * fullManaLevel);
        manaBar.transform.localScale = new Vector3(manaLevel, manaBarScale.y, manaBarScale.z);
    }
}
