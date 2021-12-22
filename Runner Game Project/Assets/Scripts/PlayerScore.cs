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
    Vector3 startPosition;
    public int itemsPicked = 0;
    float totalDistance = 0;
    public Text scoreText;
    private CollisionHandler collisionHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        collisionHandler = gameObject.GetComponent<CollisionHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        totalDistance = Vector3.Distance(transform.position, startPosition);
        //oldPosition = transform.position;
        if (!collisionHandler.isTransitioning)
        {
            calculateScore(itemsPicked, totalDistance);
        }
        else
        {
            if (transform.position.z >= collisionHandler.crashPosition.z + 1)
            {
                collisionHandler.isTransitioning = false;
            }
        }
    }

    void calculateScore(int items, float dist)
    {
        score = totalDistance * distanceCoef + items * itemValue;
        scoreText.GetComponent<Text>().text = "Score: " + (int) score;
    }
}
