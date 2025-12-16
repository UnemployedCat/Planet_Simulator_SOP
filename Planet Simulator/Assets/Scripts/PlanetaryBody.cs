using UnityEngine;

public class PlanetaryBody : MonoBehaviour
{
    public float mass;
    public float radius;
    public Vector3 startingVelocity;
    public Vector3 currentVelocity;
    public float maxVelocity;
    public float maxDistance;
    public float minDistance;
    bool isReversed;

    void Awake()
    {
        currentVelocity = startingVelocity;
    }

    //Updates the various extra data
    private void FixedUpdate()
    {
        if (currentVelocity.magnitude >= maxVelocity)
        {
            maxVelocity = currentVelocity.magnitude;
        }
        if(transform.position.magnitude >= maxDistance)
        {
            maxDistance = transform.position.magnitude;
        }
        if(transform.position.magnitude <= minDistance)
        {
            minDistance = transform.position.magnitude;
        }
    }

    public void UpdateVelocity(PlanetaryBody[] allPlanetaryBodies, float timeStep, bool reverse)
    {
        //Makes it so the sun never moves
        if (name == "Sun")
        {
            currentVelocity = Vector3.zero;
        }
        else
        {
            foreach (var otherbody in allPlanetaryBodies)
            {
                if (otherbody != this)
                {
                    //Does Newtons gravity equation
                    float squareDistance = (otherbody.transform.position - this.transform.position).sqrMagnitude;
                    Vector3 forceDirection = (otherbody.transform.position - this.transform.position).normalized;
                    Vector3 force = forceDirection * UniversalConstant.gravitationalConstant * mass * otherbody.mass / squareDistance;
                    Vector3 acceleratiion = force / mass;
                    currentVelocity += acceleratiion * timeStep;
                }
            }
        }
        //Extra for testing the simulation
        if (reverse == true && !isReversed)
        {
            currentVelocity = currentVelocity * -1;
            isReversed = true;
        }
    }

    public void UpdatePosition(float timeStep)
    {
        this.transform.position += currentVelocity * timeStep;
    }
}
