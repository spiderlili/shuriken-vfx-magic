using UnityEngine;
using System.Collections;

public class ShipMover : MonoBehaviour
{
    public float speed = 100;
    public float waypointRadius = 500;

    Vector3 waypointPosition = new Vector3();
    Quaternion newRot;

    void Start()
    {
        waypointPosition = Random.onUnitSphere * waypointRadius * 2;
        InvokeRepeating("NewWaypoint", 5, 5);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        float smooth = (speed * 2) / Vector3.Distance(transform.position, waypointPosition);

        newRot = Quaternion.LookRotation(waypointPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * smooth);
    }

    void NewWaypoint()
    {
        while (true)
        {
            waypointPosition = Random.onUnitSphere * waypointRadius * 2;

            if (Vector3.Distance(transform.position, waypointPosition) > waypointRadius) return;
        }
    }
}
