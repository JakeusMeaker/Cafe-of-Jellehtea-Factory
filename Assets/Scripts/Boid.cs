using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Vector3 velocity;

    public float speed = 5;
    public float maxTurnSpeed = 90;
    public float maxAcceleration = 5;
    public Transform goal;

    public float separationDistance = 2;
    public float separationStrength = 1;

    public float alingnmentRadius = 3;
    public float alignmentStrength = 2;

    public float cohesionRadius = 3;
    public float cohesionStrength = 3;

    public bool isFleeing = false;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Random.onUnitSphere * speed;
    }

    // Update is called once per frame
    void Update()
    {
        List<Boid> otherBoids = new List<Boid>(FindObjectsOfType<Boid>());
        otherBoids.Remove(this);

        Vector3 targetVelocity = velocity;

        //Target Tracking
        Vector3 goalVector = goal.position - transform.position;

        //set to -goalvector to make them flee
        if (!isFleeing)
            targetVelocity = goalVector.normalized * speed;
        else
            targetVelocity = -goalVector.normalized * speed;

        //Separation
        foreach(Boid other in otherBoids)
        {
            Vector3 difference = transform.position - other.transform.position;
            if(difference.magnitude < separationDistance)
            {
                targetVelocity += separationStrength * difference.normalized;
            }
        }

        //Allignment
        Vector3 total = Vector3.zero;
        int count = 0;
        foreach(Boid other in otherBoids)
        {
            if(Vector3.Distance(transform.position, other.transform.position) < alingnmentRadius)
            {
                total += other.velocity;
                count++;
            }
        }

        if (count > 0)
        {
            Vector3 averageVelocity = total / count;
            averageVelocity.Normalize();
            targetVelocity += averageVelocity * alignmentStrength;
        }

        //Cohesion
        total = Vector3.zero;
        count = 0;
        foreach (Boid other in otherBoids)
        {
            if (Vector3.Distance(transform.position, other.transform.position) < cohesionRadius)
            {
                total += other.transform.position;
                count++;
            }
        }

        if (count > 0)
        {
            Vector3 averagePosition = total / count;
            targetVelocity += (averagePosition - transform.position).normalized * cohesionStrength;
        }

        //set speed
        targetVelocity = targetVelocity.normalized * speed;

        //Steering
        velocity = Vector3.RotateTowards(velocity, targetVelocity, maxTurnSpeed * Mathf.Deg2Rad * Time.deltaTime, maxAcceleration * Time.deltaTime);
        velocity = targetVelocity;

        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, velocity);
    }
}
