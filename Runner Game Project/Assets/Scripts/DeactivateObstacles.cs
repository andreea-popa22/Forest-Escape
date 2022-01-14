using System.Collections;
using UnityEngine;

public class DeactivateObstacles : MonoBehaviour
{
    private GameObject player;
    private GameObject generatedObtacles;
    private GameObject manualObstacles;
    public int distanceToDeactivate = 10;

    private float timePassed = 0;

    void Start()
    {
        player = GameObject.Find("Player");
        generatedObtacles = GameObject.Find("Obstacles");
        manualObstacles = GameObject.Find("Obstacles-Manual");
    }

    void Update()
    {
        // ruleaza doar de 4 ori pe secunda
        timePassed += Time.deltaTime;
        if (timePassed > 0.25)
        {
            timePassed -= 0.25f;

            float playerZ = player.transform.position.z;
            // dezactiveaza obstacolele daca sunt in spatele player-ului
            foreach (var trans in generatedObtacles.transform.GetComponentsInChildren<Transform>())
                if (trans != generatedObtacles.transform)
                    if ((playerZ - trans.position.z) > distanceToDeactivate)
                        trans.gameObject.SetActive(false);

            foreach (var trans in manualObstacles.transform.GetComponentsInChildren<Transform>())
                if (trans != manualObstacles.transform)
                    if ((playerZ - trans.position.z) > distanceToDeactivate)
                        trans.gameObject.SetActive(false);
        }
    }
}