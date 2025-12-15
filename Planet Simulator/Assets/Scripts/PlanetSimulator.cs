using UnityEngine;

public class PlanetSimulator : MonoBehaviour
{
    //coment for something
    PlanetaryBody[] planets;
    public int timeBeforeReverse;
    public bool testReversal;

    void Awake()
    {
        planets = FindObjectsByType<PlanetaryBody>(FindObjectsSortMode.None);
        Time.fixedDeltaTime = UniversalConstant.physicsTimeStep;
    }
  
    private void FixedUpdate()
    {
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
        if (Time.time >= timeBeforeReverse * 2 && testReversal)
        {
            Time.timeScale = 0;
        }
    }
}
