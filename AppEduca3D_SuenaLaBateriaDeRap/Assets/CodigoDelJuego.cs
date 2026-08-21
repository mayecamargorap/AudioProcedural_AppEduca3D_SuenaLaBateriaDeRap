using UnityEngine;

public class CodigoDelJuego : MonoBehaviour
{
    public float velocidad = 3f;

    // AUDIO PROCEDURAL HI-HAT
    public AudioSource AudioHiHat;

    public int VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo = 44100;
    //La frecuencia de muestreo es cuántas muestras genera o procesa el computador 
    // por segundo, es decir la velocidad con que el computador lee o genera la señal.
    //Ejemplo:
    //Si tenemos una señal de 44.000 muestras entonces N=44.000 muestras
    //Si tenemos que procesa 1.000 muestras por segundo entonces FS= 1.000 muestrassegundo
    //Lo que nos lleva a deducir que la duración de la señal será:
    // VariableDuracionEnSegundos = N / Fs
    // VariableDuracionEnSegundos = 44.000 / 1.000
    // VariableDuracionEnSegundos = 44 segundos
    // Por lo tanto, la señal tardará 44 segundos en reproducirse.
    //A mayor frecuencia de muestreo podremos representar la onda auditiva que queremos más 
    // fielmente osea mas precisa, porque vamos a tener más puntos que describen la onda por segundo

    public float VariableDuracionEnSegundos = 0.3f; // en segundos
    public float FrecuenciaCorte = 3000f;

    void Start()
    {
        //GenerarHiHat();
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
        int VariableNumeroDeMuestras = Mathf.RoundToInt(VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo * VariableDuracionEnSegundos);
        //numero de muestras = frecuencia de muestreo * duración en segundos
        //numero de muestras = 44.000 * 0.3
        //numero de muestras = 13.200 
        // N = 13.200 muestras
        // VariableNumeroDeMuestras N representa la longitud de la señal en muestras,
        // es decir, cuántos puntos de datos tiene la señal.

        float[] VectorDeLaSeñal = new float[VariableNumeroDeMuestras];
        //creamos un vector de la señal de tipo float con una longitud igual al número de muestras
        //en nuestro caso 13.200 muestras, es decir, 13.200 puntos de datos que representan la señal de audio.

        System.Random ObjetoRandomMio = new System.Random();

        // Coeficiente del filtro pasa-altos
        float RC =  1f / (2f * Mathf.PI * FrecuenciaCorte);

        float DeltaTiempo = 1f / VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo;

        float Alpha = RC / (RC + DeltaTiempo);

        float EntradaAnterior = 0f;
        float SalidaAnterior = 0f;


        for (int NumeroDeMuestra = 0; NumeroDeMuestra < VariableNumeroDeMuestras; NumeroDeMuestra++)
        {
            // Ruido
            float NumeroRuido =
                (float)(ObjetoRandomMio.NextDouble() * 2.0 - 1.0);

            // Envolvente
            // La envolvente controla cómo cambia el volumen/amplitud del ruido a lo largo del tiempo.
            // Al inicio → amplitud o volumen cercana a 1 → sonido fuerte;
            // conforme avanzan las muestras → la amplitud o volumen disminuye;
            // al final → amplitud o volumen  se acerca a 0 → silencio.

            // Entonces por medio del volumen convertimos el ruido continuo en un golpe corto , 
            // fuerfe y que disminuye rapidamente hacia cero.
            //Y el 20f controla qué tan rápido ocurre esa caída: mayor valor → caída más rápida; menor valor → caída más lent   
            
            // La envolvente se calcula usando una función exponencial asi
            //Envolvente = e^(-20*1/44100) = 0.9995
            //Envolvente = e^(-20*2/44100) = 0.9990
            //Envolvente = e^(-20*2000/44100) = 0.4034
            //Envolvente = e^(-20*10000/44100) =  0.1093
            //Envolvente = e^(-20*13200/44100) = 0.00251
            float Envolvente = Mathf.Exp(-50f * NumeroDeMuestra / VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo);

            // Varias componentes rápidas para darle carácter metálico
            // COMPONENTES AGUDAS
            float Componente1 = Mathf.Sin(2f * Mathf.PI * 5000f * NumeroDeMuestra / VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo);
            float Componente2 = Mathf.Sin(2f * Mathf.PI * 7000f * NumeroDeMuestra / VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo);
            float Componente3 = Mathf.Sin(2f * Mathf.PI * 9000f * NumeroDeMuestra / VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo);

            // Mezcla de ruido + componentes metálicas
            float Senal =
                (NumeroRuido * 0.6f) +
                (Componente1 * 0.15f)+ 
                (Componente2 * 0.15f) +
                (Componente3 * 0.10f);

            // FILTRO PASA-ALTOS
            float SalidaActual =
                Alpha *
                (SalidaAnterior +
                Senal -
                EntradaAnterior);

            EntradaAnterior = Senal;
            SalidaAnterior = SalidaActual;

            // Aplicar envolvente
            //VectorDeLaSeñal[NumeroDeMuestra] = NumeroRuido*Envolvente;

            //VectorDeLaSeñal[NumeroDeMuestra] = Senal * Envolvente;
            VectorDeLaSeñal[NumeroDeMuestra] =
                SalidaActual * Envolvente;

        }

        //Esa línea crea en memoria un AudioClip vacío donde después vas a guardar 
        // las muestras que generaste proceduralmente.
        //"HiHatProcedural"
        //→ nombre del AudioClip.

        //VariableNumeroDeMuestras
        //→ cantidad total de muestras que tendrá el audio.
        //Ejemplo: 13.230 muestras.

        //1
        //→ número de canales de audio.
        //1 = mono.
        //2 = estéreo.

        //VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo
        //→ frecuencia de muestreo.
        //Ejemplo: 44.100 muestras/segundo.

        //false
        //→ el clip NO se crea como un clip de streaming.
        // es decir no se genera mientras se va reproduciendo, sino que se genera completo antes de reproducirse.
        //Es decir, el hi-hat de 0.3 s tiene sus 13.230 muestras disponibles antes de reproducirse.
        AudioClip ClipHiHat = AudioClip.Create(
            "HiHatProcedural",
            VariableNumeroDeMuestras,
            1,
            VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo,
            false
        );

        //Le pasamos nuestra señal al vector de datos del AudioClip que acabamos de crear, 
        // para que el AudioClip tenga la señal que generamos.
        ClipHiHat.SetData(VectorDeLaSeñal, 0);

        AudioHiHat.clip = ClipHiHat;
        //Le decimos al Audio Source: “El audio que vas a reproducir es este ClipHiHat que acabamos de generar”.
        AudioHiHat.Play();
        //Le decimos al Audio Source “El audio que vas a reproducir es este ClipHiHat que acabamos de generar”.
    }
}