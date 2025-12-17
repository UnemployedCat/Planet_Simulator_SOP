using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    public float turn;

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, turn);
    }
}
