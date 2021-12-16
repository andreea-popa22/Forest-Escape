using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject prefabObstacle;
    public GameObject prefabManaItem;
    public int ObstaclesDistance = 8;

    void Start()
    {
        GameObject obstacles = GameObject.Find("Obstacles");
        GameObject items = GameObject.Find("Pickables");

        for (int i=1; i<10; i++)
        {
            GameObject child = Instantiate(prefabObstacle, obstacles.transform);
            child.transform.position += new Vector3(Lane.NextRandomLane(), 0, i * ObstaclesDistance);
        }

        //for (int i=0; i<10; i++)
        //{
        //    Instantiate(prefabManaItem,
        //        new Vector3(Lane.array.GetRandom(), 1, i * 3),
        //        Quaternion.Euler(new Vector3(45, 45, 45)),
        //        items.transform);
        //}

        //Instantiate(o)
    }
}