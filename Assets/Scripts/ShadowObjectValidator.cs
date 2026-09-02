using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // XRIT v3

public class ShadowObjectValidator : MonoBehaviour
{
    [Header("Rotación Objetivo (X, Y, Z)")]
    public Vector3 targetRotation = new Vector3(5.609f, 25.782f, 40.861f);

    [Header("Configuración de Validación")]
    [Tooltip("Recomendado 20-30 para VR a mano alzada")]
    public float toleranceAngle = 25f;
    [Tooltip("Evalúa respecto al mundo (true) o respecto al padre (false)")]
    public bool useWorldRotation = true;

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

        // 1. Verificar si el usuario está sosteniendo el objeto en VR
        isGrabbed = grabInteractable != null && grabInteractable.isSelected;

        // 2. Obtener rotación actual y calcular la diferencia en grados 3D
        Quaternion currentRot = useWorldRotation ? transform.rotation : transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(targetRotation);

        currentAngleDifference = Quaternion.Angle(currentRot, targetRot);
        isAligned = currentAngleDifference <= toleranceAngle;

        // 3. Imprimir diagnóstico en la Consola mientras se sostiene
        if (isGrabbed)
        {
            Debug.Log($"[VR] Diferencia: {currentAngleDifference:F1}° / Tolerancia: {toleranceAngle}°");
        }

        // 4. Solo validar si está agarrado y dentro de la tolerancia
        if (isAligned && isGrabbed)
        {
            LockInPlace();
        }
    }

    private void LockInPlace()
    {
        completed = true;

        // Aplicar el ajuste fino
        if (useWorldRotation)
            transform.rotation = Quaternion.Euler(targetRotation);
        else
            transform.localRotation = Quaternion.Euler(targetRotation);

        // Soltar y congelar
        if (grabInteractable != null)
            grabInteractable.enabled = false;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        Debug.Log($"<color=green>¡BLOQUEADO CON ÉXITO!</color> Diferencia final: {currentAngleDifference:F1}°");
    }
    [Header("Transición de Nivel")]
    public GameObject puertaONivel2; // Arrastra aquí la puerta o pared que cubre la Zona 2

    private void LockInPlace()
    {
        completed = true;

        // ... (Tu código de snapping y congelar el objeto) ...

        // Abrir o activar el paso al Nivel 2
        if (puertaONivel2 != null)
        {
            puertaONivel2.SetActive(false); // O puedes animarla/moverla
            Debug.Log("<color=cyan>¡Nivel 1 completado! Acceso al Nivel 2 abierto.</color>");
        }
    }
}