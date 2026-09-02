using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ShadowObjectValidator : MonoBehaviour
{
    [Header("Rotación Objetivo (X, Y, Z)")]
    public Vector3 targetRotation = new Vector3(5.609f, 25.782f, 40.861f);

    [Header("Configuración de Validación")]
    public float toleranceAngle = 25f;
    public bool useWorldRotation = true;

    [Header("Transición de Nivel")]
    [Tooltip("Arrastra aquí la puerta, pared o muro que bloquea el paso al Nivel 2")]
    public GameObject puertaONivel2;

    [Header("Diagnóstico en tiempo real (Solo Lectura)")]
    [SerializeField] private float currentAngleDifference;
    [SerializeField] private bool isGrabbed;
    [SerializeField] private bool isAligned;

    private bool completed = false;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Update()
    {
        if (completed) return;

        isGrabbed = grabInteractable != null && grabInteractable.isSelected;

        Quaternion currentRot = useWorldRotation ? transform.rotation : transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(targetRotation);

        currentAngleDifference = Quaternion.Angle(currentRot, targetRot);
        isAligned = currentAngleDifference <= toleranceAngle;

        if (isAligned && isGrabbed)
        {
            LockInPlace();
        }
    }

    private void LockInPlace()
    {
        completed = true;

        // 1. Ajuste visual exacto
        if (useWorldRotation)
            transform.rotation = Quaternion.Euler(targetRotation);
        else
            transform.localRotation = Quaternion.Euler(targetRotation);

        // 2. Desactivar agarre y físicas
        if (grabInteractable != null)
            grabInteractable.enabled = false;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        // 3. DESBLOQUEAR EL ACCESO AL NIVEL 2
        if (puertaONivel2 != null)
        {
            puertaONivel2.SetActive(false); // Hace desaparecer la pared/puerta
            Debug.Log("<color=cyan>¡Paso al Nivel 2 desbloqueado!</color>");
        }

        Debug.Log($"<color=green>¡COMPLETADO!</color> Objeto bloqueado en su posición.");
    }
}