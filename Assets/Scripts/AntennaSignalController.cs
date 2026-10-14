using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;
using UnityEngine.InputSystem;

public class AntennaSignalController : MonoBehaviour
{
    [Header("Control con joystick")]
    public InputActionProperty joystickGiro;
    public float velocidadJoystick = 90f;

    [Header("Configuración de la señal")]
    public string nombreSenal = "Señal A";
    public float[] valoresTabla = { 1f, 2f, 3f, 4f, 5f };

    [Header("Rotación")]
    public float sensibilidad = 150f; // Se mantiene por compatibilidad, aunque usaremos velocidadJoystick para el stick
    public float anguloMaximo = 180f;

    [Header("Visualización")]
    public TextMeshPro textoValor;

    [Header("Diagnóstico (solo lectura)")]
    [SerializeField] private float anguloActual;
    [SerializeField] private float valorActual;
    [SerializeField] private bool isGrabbed;

    private XRGrabInteractable grabInteractable;
    private float anguloInicial;
    private float anguloAcumulado;
    private float ultimaPosicionMano;
    private Transform manoActual;

    public float ValorActual => valorActual;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.trackPosition = false;
            grabInteractable.trackRotation = false;

            grabInteractable.selectEntered.AddListener(AlAgarrar);
            grabInteractable.selectExited.AddListener(AlSoltar);
        }

        anguloInicial = transform.localEulerAngles.y;
        anguloAcumulado = 0f;

        ActualizarValor();
    }

    private void OnEnable()
    {
        // Activamos la acción del Input System para que empiece a recibir datos del joystick
        if (joystickGiro.action != null)
            joystickGiro.action.Enable();
    }

    private void OnDisable()
    {
        // Desactivamos la acción cuando el objeto se desactiva
        if (joystickGiro.action != null)
            joystickGiro.action.Disable();
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(AlAgarrar);
            grabInteractable.selectExited.RemoveListener(AlSoltar);
        }
    }

    private void AlAgarrar(SelectEnterEventArgs args)
    {
        manoActual = args.interactorObject.transform;
        ultimaPosicionMano = ObtenerAnguloMano();
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        manoActual = null;
    }

    private void Update()
    {
        isGrabbed = grabInteractable != null && grabInteractable.isSelected;

        if (isGrabbed)
        {
            float giro = 0f;

            // Leemos el valor horizontal del joystick (Vector2)
            if (joystickGiro.action != null)
            {
                Vector2 valorInput = joystickGiro.action.ReadValue<Vector2>();
                giro = valorInput.x; // Eje horizontal (-1 a 1)
            }

            anguloAcumulado = Mathf.Clamp(
                anguloAcumulado + giro * velocidadJoystick * Time.deltaTime,
                0f,
                anguloMaximo
            );

            transform.localRotation = Quaternion.Euler(
                transform.localEulerAngles.x,
                anguloInicial + anguloAcumulado,
                transform.localEulerAngles.z
            );
        }

        anguloActual = anguloAcumulado;
        ActualizarValor();
    }

    private float ObtenerAnguloMano()
    {
        Vector3 direccion = manoActual.position - transform.position;
        direccion.y = 0f;

        if (direccion.sqrMagnitude < 0.0001f)
            return ultimaPosicionMano;

        return Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg;
    }

    private void ActualizarValor()
    {
        if (valoresTabla == null || valoresTabla.Length == 0)
            return;

        float progreso = Mathf.Clamp01(
            anguloActual / Mathf.Max(anguloMaximo, 1f));

        float posicionTabla = progreso * (valoresTabla.Length - 1);
        int inferior = Mathf.FloorToInt(posicionTabla);
        int superior = Mathf.Min(inferior + 1, valoresTabla.Length - 1);

        valorActual = Mathf.Lerp(
            valoresTabla[inferior],
            valoresTabla[superior],
            posicionTabla - inferior);

        if (textoValor != null)
            textoValor.text = valorActual.ToString("F2");
    }
}