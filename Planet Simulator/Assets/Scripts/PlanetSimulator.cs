using UnityEngine;

public class PlanetSimulator : MonoBehaviour
{

    PlanetaryBody[] planets;

    void Awake()
    {
        planets = FindObjectsByType<PlanetaryBody>(FindObjectsSortMode.None);
        Time.fixedDeltaTime = UniversalConstant.physicsTimeStep;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].UpdateVelocity(planets, UniversalConstant.physicsTimeStep);
        }

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].UpdatePosition(UniversalConstant.physicsTimeStep);
        }
    }
}
