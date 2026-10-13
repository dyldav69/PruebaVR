using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Malla de la Puerta")]
    [Tooltip("Arrastra aquí el objeto hijo de la puerta que se moverá hacia arriba")]
    public Transform doorMesh;

    [Header("Configuración del Movimiento")]
    [Tooltip("Altura (en metros) que se elevará la puerta al abrirse")]
    public float openHeight = 3.5f;

    [Tooltip("Velocidad de movimiento")]
    public float speed = 4.0f;

    private Vector3 closedLocalPosition;
    private Vector3 openLocalPosition;
    private Vector3 targetLocalPosition;

    private void Start()
    {
        // Si no se asigna en el Inspector, toma el propio objeto
        if (doorMesh == null) doorMesh = transform;

        // Guarda la posición local inicial de la puerta
        closedLocalPosition = doorMesh.localPosition;
        openLocalPosition = closedLocalPosition + new Vector3(0, openHeight, 0);
        targetLocalPosition = closedLocalPosition;
    }

    private void Update()
    {
        if (doorMesh != null)
        {
            // Mueve la malla de forma fluida a nivel local
            doorMesh.localPosition = Vector3.MoveTowards(doorMesh.localPosition, targetLocalPosition, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            targetLocalPosition = openLocalPosition; // Elevar
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            targetLocalPosition = closedLocalPosition; // Bajar
        }
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player") || other.GetComponentInParent<Unity.XR.CoreUtils.XROrigin>() != null;
    }
}