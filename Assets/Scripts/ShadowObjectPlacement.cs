using UnityEngine;

public class ShadowObjectPlacement : MonoBehaviour
{
    public Transform placementPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShadowObject"))
        {
            other.transform.position = placementPoint.position;
        }
    }
}