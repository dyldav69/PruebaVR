
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

    [Header("Configuracion de la señal")]
    public string nombreSenal = "Señal A";
    public float[] valoresTabla = { 1f, 2f, 3f, 4f, 5f };

    [Header("Rotacion")]
    public float sensibilidad = 150f;
    public float anguloMaximo = 180f;

    [Header("Visualizacion")]
    public TextMeshPro textoValor;

    [Header("Diagnostico")]
    [SerializeField] private float anguloActual;
    [SerializeField] private float valorActual;
    [SerializeField] private bool isGrabbed;

    private XRGrabInteractable grabInteractable;
    private float anguloInicial;
    private float anguloAcumulado;

    public float ValorActual => valorActual;

    public bool AjusteCompleto =>
        valoresTabla != null &&
        valoresTabla.Length > 0 &&
        anguloAcumulado >= anguloMaximo - 0.1f;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.trackPosition = false;
            grabInteractable.trackRotation = false;
        }

        anguloInicial = transform.localEulerAngles.y;
        anguloAcumulado = 0f;

        ActualizarValor();
    }

    private void OnEnable()
    {
        if (joystickGiro.action != null)
            joystickGiro.action.Enable();
    }

    private void OnDisable()
    {
        if (joystickGiro.action != null)
            joystickGiro.action.Disable();
    }

    private void Update()
    {
        isGrabbed = grabInteractable != null &&
                    grabInteractable.isSelected;

        if (isGrabbed && joystickGiro.action != null)
        {
            float giro =
                joystickGiro.action.ReadValue<Vector2>().x;

            anguloAcumulado = Mathf.Clamp(
                anguloAcumulado +
                giro * velocidadJoystick * Time.deltaTime,
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

    private void ActualizarValor()
    {
        if (valoresTabla == null || valoresTabla.Length == 0)
            return;

        float progreso = Mathf.Clamp01(
            anguloActual / Mathf.Max(anguloMaximo, 1f)
        );

        float posicionTabla =
            progreso * (valoresTabla.Length - 1);

        int inferior = Mathf.FloorToInt(posicionTabla);
        int superior = Mathf.Min(
            inferior + 1,
            valoresTabla.Length - 1
        );

        valorActual = Mathf.Lerp(
            valoresTabla[inferior],
            valoresTabla[superior],
            posicionTabla - inferior
        );

        if (textoValor != null)
            textoValor.text = valorActual.ToString("F2");
    }
}
