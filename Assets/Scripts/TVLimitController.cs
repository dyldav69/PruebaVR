
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class TVLimitController : MonoBehaviour
{
    [Header("Antenas")]
    public AntennaSignalController antenaA;
    public AntennaSignalController antenaB;

    [Header("Televisor")]
    public VideoPlayer videoEstatica;
    public GameObject pantallaEstatica;
    public GameObject pantallaResultado;
    public TextMeshPro textoResultado;

    [Header("Tolerancia")]
    public float tolerancia = 0.1f;

    private bool evaluado = false;

    private void Start()
    {
        if (pantallaResultado != null)
            pantallaResultado.SetActive(false);
    }

    private void Update()
    {
        if (evaluado)
            return;

        if (antenaA == null || antenaB == null)
            return;

        if (!antenaA.AjusteCompleto ||
            !antenaB.AjusteCompleto)
            return;

        evaluado = true;

        float valorA = antenaA.ValorActual;
        float valorB = antenaB.ValorActual;

        if (Mathf.Abs(valorA - valorB) <= tolerancia)
        {
            float limite = (valorA + valorB) / 2f;

            if (textoResultado != null)
            {
                textoResultado.text =
                    "LIMITE EXISTE\n\nL = " +
                    limite.ToString("F2");
            }

            Debug.Log("Limite existente: " + limite);
        }
        else
        {
            if (textoResultado != null)
            {
                textoResultado.text =
                    "SEÑAL PERDIDA\n\nA = " +
                    valorA.ToString("F2") +
                    "\nB = " + valorB.ToString("F2");
            }

            Debug.Log("El limite bilateral no existe.");
        }

        // Detener la estatica y ocultar su pantalla.
        if (videoEstatica != null)
            videoEstatica.Stop();

        if (pantallaEstatica != null)
            pantallaEstatica.SetActive(false);

        // Mostrar el resultado en el televisor.
        if (pantallaResultado != null)
            pantallaResultado.SetActive(true);
    }
}
