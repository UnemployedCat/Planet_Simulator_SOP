using UnityEngine;

public class PlanetaryBody : MonoBehaviour
{
    public float mass;
    public float radius;
    public Vector3 startingVelocity;
    Vector3 currentVelocity;

    void Awake()
    {
        currentVelocity = startingVelocity;
    }

    public void UpdateVelocity(PlanetaryBody[] allPlanetaryBodies, float timeStep)
    {
        foreach (var otherbody in allPlanetaryBodies)
        {
            if(otherbody != this)
            {
                float squareDistance = (otherbody.GetComponent<Rigidbody>().position - this.GetComponent<Rigidbody>().position).sqrMagnitude;
                Vector3 forceDirection = (otherbody.GetComponent<Rigidbody>().position - this.GetComponent<Rigidbody>().position).normalized;
                Vector3 force = forceDirection * UniversalConstant.gravitationalConstant * mass * otherbody.mass / squareDistance;
                Vector3 acceleratiion = force / mass;
                currentVelocity += acceleratiion * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep)
    {
        this.GetComponent<Rigidbody>().position += currentVelocity * timeStep;
    }
}
