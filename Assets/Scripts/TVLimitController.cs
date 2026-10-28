
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

    private bool resultadoVisible = false;

    private void Start()
    {
        if (pantallaResultado != null)
            pantallaResultado.SetActive(false);

        MostrarEstatica();
    }

    private void Update()
    {
        if (antenaA == null || antenaB == null)
            return;

        float valorA = antenaA.ValorActual;
        float valorB = antenaB.ValorActual;

        // Evitar mostrar un resultado al iniciar,
        // cuando ambas antenas todavía están en su valor inicial.
        bool ambasAvanzaron =
            AvanzoAntena(antenaA, valorA) &&
            AvanzoAntena(antenaB, valorB);

        // Si los valores no coinciden, mostrar estática.
        if (!ambasAvanzaron ||
            Mathf.Abs(valorA - valorB) > tolerancia)
        {
            MostrarEstatica();
            return;
        }

        // Si coinciden, mostrar el resultado matemático.
        float limite = (valorA + valorB) / 2f;

        if (textoResultado != null)
        {
            textoResultado.text =
                "LIMITE EXISTE\n\nL = " +
                limite.ToString("F2");
        }

        MostrarResultado();
    }

    private bool AvanzoAntena(
        AntennaSignalController antena,
        float valor)
    {
        if (antena.valoresTabla == null ||
            antena.valoresTabla.Length == 0)
            return false;

        float valorInicial = antena.valoresTabla[0];

        return valor > valorInicial + tolerancia;
    }

    private void MostrarEstatica()
    {
        if (pantallaResultado != null &&
            pantallaResultado.activeSelf)
        {
            pantallaResultado.SetActive(false);
        }

        if (pantallaEstatica != null &&
            !pantallaEstatica.activeSelf)
        {
            pantallaEstatica.SetActive(true);
        }

        if (videoEstatica != null &&
            pantallaEstatica != null &&
            pantallaEstatica.activeInHierarchy &&
            !videoEstatica.isPlaying)
        {
            videoEstatica.Play();
        }

        resultadoVisible = false;
    }

    private void MostrarResultado()
    {
        if (resultadoVisible)
            return;

        if (videoEstatica != null)
            videoEstatica.Stop();

        if (pantallaEstatica != null)
            pantallaEstatica.SetActive(false);

        if (pantallaResultado != null)
            pantallaResultado.SetActive(true);

        resultadoVisible = true;
    }
}
