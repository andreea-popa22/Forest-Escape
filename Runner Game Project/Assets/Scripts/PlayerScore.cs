using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private float score = 0f;
    [SerializeField] private float distanceCoef = 0.01f;
    [SerializeField] private int itemValue = 5;
    [SerializeField] Vector3 startPosition;
    [SerializeField] public int itemsPicked = 0;
    [SerializeField] float totalDistance = 0;
    [NonSerialized] private float maxDist = 0f;
    [NonSerialized] public Text scoreText;
    [NonSerialized] private CollisionHandler collisionHandler;
    [NonSerialized] public int prevScore;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        collisionHandler = gameObject.GetComponent<CollisionHandler>();
        score = 0;
        prevScore = PlayerPrefs.GetInt(PlayerPrefsKeys.prevSceneScore, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        totalDistance = Vector3.Distance(transform.position, startPosition);
        if (maxDist < totalDistance)
            maxDist = totalDistance;
        CalculateScore(itemsPicked, maxDist);
    }

    void CalculateScore(int items, float dist)
    {
        score = dist * distanceCoef + items * itemValue + prevScore;
        scoreText.GetComponent<Text>().text = "Score: " + (int) score;
    }

    public int SaveScoreToPrefs()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.prevSceneScore, (int) score);
        PlayerPrefs.Save();
        return (int) score;
    }
}
