using System.Collections;
using UnityEngine;

public class DeactivateObstacles : MonoBehaviour
{
    private GameObject player;
    private GameObject generatedObtacles;
    private GameObject manualObstacles;
    public int distanceToDeactivate = 10;
    private int distancecToActivate = 100;

    private float timePassed = 0;

    void Start()
    {
        player = GameObject.Find("Player");
        generatedObtacles = GameObject.Find("Obstacles");
        manualObstacles = GameObject.Find("Obstacles-Manual");
        //foreach (var trans in generatedObtacles.transform.GetComponentsInChildren<Transform>())
        //    trans.gameObject.SetActive(false);
        //foreach (var trans in manualObstacles.transform.GetComponentsInChildren<Transform>())
        //    trans.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        // ruleaza doar de 4 ori pe secunda
        timePassed += Time.fixedDeltaTime;
        if (timePassed > 0.5)
        {
            timePassed -= 0.5f;

            float playerZ = player.transform.position.z;
            // dezactiveaza obstacolele daca sunt in spatele player-ului
            foreach (var trans in generatedObtacles.transform.GetComponentsInChildren<Transform>(includeInactive: false))
            {
                if (trans != generatedObtacles.transform)
                {
                    if ((playerZ - trans.position.z) > distanceToDeactivate)
                        trans.gameObject.SetActive(false);
                    //if (trans.position.z > playerZ && trans.position.z - playerZ < distancecToActivate)
                    //    trans.gameObject.SetActive(true);
                }
            }
            foreach (var trans in manualObstacles.transform.GetComponentsInChildren<Transform>(includeInactive: false))
            {
                if (trans != manualObstacles.transform) 
                { 
                    if ((playerZ - trans.position.z) > distanceToDeactivate)
                        trans.gameObject.SetActive(false);
                    //if (trans.position.z > playerZ && trans.position.z - playerZ < distancecToActivate)
                    //    trans.gameObject.SetActive(true);
                }
            }
        }
    }
}