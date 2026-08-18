using UnityEngine;

public class CodigoDelJuego : MonoBehaviour
{
    public float velocidad = 5f;

    // AUDIO PROCEDURAL HI-HAT
    public AudioSource AudioHiHat;

    public int VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo = 44100;
    public float Duracion = 0.3f;
    public float FrecuenciaCorte = 3000f;

    void Start()
    {
        GenerarHiHat();
    }

    void Update()   
    {
        // MOVIMIENTO DEL PERSONAJE
        float movimientoX = Input.GetAxis("Horizontal");
        float movimientoZ = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(movimientoX, 0f, movimientoZ);

        transform.Translate(movimiento * velocidad * Time.deltaTime);

        // GENERAR HI-HAT
        if (Input.GetKeyDown(KeyCode.Space))
        {
           GenerarHiHat();
        }
    }

    void GenerarHiHat()
    {
        Debug.Log("SE GENERÓ HI-HAT");
        int NumeroDeMuestras = Mathf.RoundToInt(VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo * Duracion);

        float[] VectorDeLaSeñal = new float[NumeroDeMuestras];

        System.Random ObjetoRandomMio = new System.Random();

        for (int muestra = 0; muestra < NumeroDeMuestras; muestra++)
        {
            // Ruido
            float NumeroRuido =
                (float)(ObjetoRandomMio.NextDouble() * 2.0 - 1.0);

            // Envolvente
            float Envolvente =
                Mathf.Exp(-20f * muestra / VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo);

            // Varias componentes rápidas para darle carácter metálico
            float Componente1 = Mathf.Sin(2f * Mathf.PI * 5000f * muestra / VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo);
            float Componente2 = Mathf.Sin(2f * Mathf.PI * 7000f * muestra / VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo);
            float Componente3 = Mathf.Sin(2f * Mathf.PI * 9000f * muestra / VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo);

            // Mezcla de ruido + componentes metálicas
            float Senal =
                (NumeroRuido * 0.6f) +
                (Componente1 * 0.15f) +
                (Componente2 * 0.15f) +
                (Componente3 * 0.10f);

            // Aplicar envolvente
            VectorDeLaSeñal[muestra] = Senal * Envolvente;
        }

        AudioClip ClipHiHat = AudioClip.Create(
            "HiHatProcedural",
            NumeroDeMuestras,
            1,
            VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo,
            false
        );

        ClipHiHat.SetData(VectorDeLaSeñal, 0);

        AudioHiHat.clip = ClipHiHat;
        AudioHiHat.Play();
    }
}