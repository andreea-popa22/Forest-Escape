using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesController : MonoBehaviour
{
    public Transform player; //pozitie player
    [SerializeField] float maxDistanceDropper = 25f; //distanta de unde cade dropperul
    private Rigidbody rb;
    [SerializeField] float maxDistanceSpinner = 30f; //distanta de unde se roteste spinnerul
    [SerializeField] private float maxDistanceMovingObj = 30;

    public float moverSpeed = 7;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <
            maxDistanceDropper && gameObject.tag.Equals(Prefabs.Dropper)) //cand playerul se apropie va cadea dropperul
        {
            rb.useGravity = true;
        }
    }

    private void FixedUpdate() //sa se roteasca spinnerul
    {
        if ((Vector3.Distance(player.position, transform.position) < maxDistanceSpinner) &&
            gameObject.tag.Equals(Prefabs.Spinner))
        {
            //cand playerul se apropie se roteste spinnerul
            transform.Rotate(0, 1f, 0);
        }

        if ((Vector3.Distance(player.position, transform.position) < maxDistanceMovingObj) &&
            gameObject.tag.Equals(Prefabs.MovingObstacle))
        {
            //cand playerul se apropie se roteste spinnerul
            transform.Translate(0, 0, -moverSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) //ca sa cada dropperul
    {
        if (other.gameObject.tag == "Friendly")
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}