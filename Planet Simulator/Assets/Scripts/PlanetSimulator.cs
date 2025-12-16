using UnityEngine;

public class PlanetSimulator : MonoBehaviour
{
    PlanetaryBody[] planets;
    public int timeBeforeReverse;
    public bool testReversal;
    public float setTimeScale = 1;

    void Awake()
    {
        planets = FindObjectsByType<PlanetaryBody>(FindObjectsSortMode.None);
        Time.fixedDeltaTime = UniversalConstant.physicsTimeStep;
    }
  
    private void FixedUpdate()
    {
        bool isTimeStopped = false;
        //Updates the time scale
        if (!isTimeStopped)
        {
            Time.timeScale = setTimeScale;
        }

        //Does the simulation of planets
        for (int i = 0; i < planets.Length; i++)
        {
            if (Time.time >= timeBeforeReverse && testReversal)
            {
                planets[i].UpdateVelocity(planets, UniversalConstant.physicsTimeStep, true);
            }
            else
            {
                planets[i].UpdateVelocity(planets, UniversalConstant.physicsTimeStep, false);
            }
        }

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].UpdatePosition(UniversalConstant.physicsTimeStep);
        }

        //Extra for testing the simulation
        if (Time.time >= timeBeforeReverse * 2 && testReversal)
        {
            Time.timeScale = 0;
            isTimeStopped = true;
            setTimeScale = 0;
        }
    }
}
