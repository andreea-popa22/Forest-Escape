using System.Collections;
using UnityEngine;

public class SetupObstacles : MonoBehaviour
{
    void Start()
    {
        var obs = GameObject.Find("Obstacles-Manual");
        foreach (var ob in obs.GetComponentsInChildren<ObstaclesController>())
        {
            if (ob.gameObject.tag == "Dropper")
            {
                ob.maxDistanceDropper = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name switch
                {
                    "TotalScene" => 10,
                    "TotalScene1" => 15,
                    "TotalScene2" => 20,
                    _ => 10
                };
                //ob.maxDistanceDropper = 7 + GameObject.Find("Player").GetComponent<PlayerController>().playerSpeed;
            }
        }
    }
}