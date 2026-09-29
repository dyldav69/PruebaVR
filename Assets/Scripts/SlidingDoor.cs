using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Configuración del Movimiento")]
    [Tooltip("Altura (en metros) que se elevará la puerta al abrirse")]
    public float openHeight = 3.5f;

    [Tooltip("Velocidad a la que sube y baja la puerta")]
    public float speed = 3.0f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        // Posición actual de la puerta como estado 'Cerrada'
        closedPosition = transform.position;
        // Posición 'Abierta' calculada sumando la altura en el eje Y
        openPosition = closedPosition + new Vector3(0, openHeight, 0);
        
        targetPosition = closedPosition;
    }

    private void Update()
    {
        // Desplaza la puerta de manera fluida hacia la posición objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            // El jugador se acerca: Abrir puerta
            targetPosition = openPosition;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            // El jugador se aleja/pasa la puerta: Cerrar puerta
            targetPosition = closedPosition;
        }
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player") || other.GetComponentInParent<Unity.XR.CoreUtils.XROrigin>() != null;
    }
}