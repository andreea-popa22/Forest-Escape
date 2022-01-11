using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerrainGenerator : MonoBehaviour
{
    public int ObstaclesDistance = 8;
    public static Pools Pool { get; private set; }

    void Start()
    {
        Pool = new Pools();

        for (int i = 1; i < 10; i++)
        {
            GameObject child = Pool.GetObject(Prefabs.Random());
            child.transform.position += new Vector3(Lane.NextRandomLane(), 0, i * ObstaclesDistance);
        }
    }
}