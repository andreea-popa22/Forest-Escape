using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// string-ul asta ar trebui sa fie la fel ca tag-ul obiectului, altfel nu merge
public static class Prefabs
{
    public static string Obstacle = "Obstacle";
    public static string ManaItem = "Pickable";
    public static string Dropper = "Dropper";
    public static string MovingObstacle = "Moving Obstacle";
    public static string Spinner = "Spinner";

    public static string Random() => UnityEngine.Random.Range(0, 5) switch
    {
        0 => Obstacle,
        1 => ManaItem,
        2 => Dropper,
        3 => MovingObstacle,
        4 => Spinner,
        _ => Spinner
    };
}

public class Pools
{
    public class SinglePool
    {
        public List<GameObject> list = new List<GameObject>();
        public GameObject prefab;
        public GameObject parent;
    }

    private Dictionary<string, SinglePool> pools = new Dictionary<string, SinglePool>();

    public Pools()
    {
        AddPool(Prefabs.Obstacle);
        AddPool(Prefabs.ManaItem);
        AddPool(Prefabs.MovingObstacle);
        AddPool(Prefabs.Spinner);
        AddPool(Prefabs.Dropper);
    }

    /** 
     * returns an Inactive object if found
     * if not found, allocate a new one
     */
    public GameObject GetObject(string prefab)
    {
        var pool = pools[prefab];
        foreach (var ob in pool.list)
            if (!ob.activeSelf)
                return ob;
        // allocate new instance
        pool.list.Add(GameObject.Instantiate<GameObject>(pool.prefab, pool.parent.transform));
        return pool.list.Last();
    }

    public void SetInactiveAll()
    {
        foreach(var pair in pools)
            foreach (var ob in pair.Value.list)
                ob.SetActive(false);
    }

    private void AddPool(string name)
    {
        var start = GameObject.Find("Obstacles");
        pools.Add(name, new SinglePool() {
            prefab = Resources.Load<GameObject>("Prefabs/" + name),
            parent = start
        });
    }
}
