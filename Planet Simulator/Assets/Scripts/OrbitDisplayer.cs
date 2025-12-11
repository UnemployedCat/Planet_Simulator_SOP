using UnityEngine;

[ExecuteInEditMode]
public class OrbitDisplayer : MonoBehaviour
{
    public int numberOfSteps = 10000;
    public float timeStep = 0.1f;
    public bool usePhysicsTimeStep;
    public bool relativToBody;
    public PlanetaryBody centerBody;
    public int lineWidth;
    public bool useThickLines;

    private void Update()
    {
        if (!Application.isPlaying)
        {
            DrawOrbit();
        }
    }

    void DrawOrbit()
    {
        PlanetaryBody[] planetaryBodies = FindObjectsByType<PlanetaryBody>(FindObjectsSortMode.None);
        var virtualPlanetaryBody = new VirtualPlanetaryBodies[planetaryBodies.Length];
        var drawPoints = new Vector3[planetaryBodies.Length][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyStartingPosition = Vector3.zero;

        //Make the virtual bodies of every planetary body in the simulator
        for (int i = 0; i < planetaryBodies.Length; i++)
        {
            virtualPlanetaryBody[i] = new VirtualPlanetaryBodies(planetaryBodies[i]);
            drawPoints[i] = new Vector3[numberOfSteps];

            if (planetaryBodies[i] == centerBody && relativToBody)
            {
                referenceFrameIndex = i;
                referenceBodyStartingPosition = planetaryBodies[i].transform.position;
            }
        }

        //Simulating the path for planetary bodies
        for (int steps = 0; steps < numberOfSteps; steps++)
        {
            Vector3 referenceBodyPosition = (relativToBody) ? virtualPlanetaryBody[referenceFrameIndex].position : Vector3.zero;

            //Updates the velocity of every body
            for (int i = 0;i < planetaryBodies.Length; i++)
            {
                virtualPlanetaryBody[i].velocity += CalulateNewAcceleration(i, virtualPlanetaryBody) * timeStep;
            }

            //Updates the position of every body
            for (int i = 0; i < planetaryBodies.Length; i++)
            {
                Vector3 newPosition = virtualPlanetaryBody[i].position + virtualPlanetaryBody[i].velocity * timeStep;
                virtualPlanetaryBody[i].position = newPosition;
                if (relativToBody)
                {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyStartingPosition;
                    newPosition -= referenceFrameOffset;
                }
                if (relativToBody && i == referenceFrameIndex)
                {
                    newPosition = referenceBodyStartingPosition;
                }

                drawPoints[i][steps] = newPosition;
            }
        }

        //Drawing the paths
        for (int bodyIndex = 0; bodyIndex < virtualPlanetaryBody.Length; bodyIndex++)
        {
            var pathColour = planetaryBodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>().sharedMaterial.color;

            if (useThickLines)
            {
                var lineRenderer = planetaryBodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                lineRenderer.enabled = true;
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions(drawPoints[bodyIndex]);
                lineRenderer.startColor = pathColour;
                lineRenderer.endColor = pathColour;
                lineRenderer.widthMultiplier = lineWidth;
            }
            else
            {
                for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
                {
                    Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour);
                }

                var lineRenderer = planetaryBodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                if (lineRenderer)
                {
                    lineRenderer.enabled = false;
                }
            }
        }
    }

    Vector3 CalulateNewAcceleration(int i, VirtualPlanetaryBodies[] virtualPlanetaryBodies)
    {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < virtualPlanetaryBodies.Length; j ++)
        {
            if(j == i)
            {
                continue;
            }
            Vector3 forceDirection = (virtualPlanetaryBodies[j].position - virtualPlanetaryBodies[i].position).normalized;
            float squareDistance = (virtualPlanetaryBodies[j].position - virtualPlanetaryBodies[i].position).sqrMagnitude;
            acceleration += forceDirection * UniversalConstant.gravitationalConstant * virtualPlanetaryBodies[j].mass / squareDistance;
        }

        return acceleration;
    }
}

class VirtualPlanetaryBodies
{
    public Vector3 velocity;
    public Vector3 position;
    public float mass;

    public VirtualPlanetaryBodies(PlanetaryBody body)
    {
        velocity = body.startingVelocity;
        position = body.transform.position;
        mass = body.mass;
    }
}