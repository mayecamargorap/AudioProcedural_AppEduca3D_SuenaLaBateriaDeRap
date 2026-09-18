//                         ╔═══════════════════════════════════════════════════════════════╗
//                         ║                 CODIGO APLICACION EDUCATIVA                   ║
//                         ║                   SUENA LA BATERIA DEL RAP                    ║
//                         ║     Estudiante: Mayerly Camargo 1202327                       ║
//                         ║     correo: est.diana.camargo3@unimilitar.edu.co              ║
//                         ║     Docente : Gabriel Rodriguez                               ║
//                         ║     Asignatura : Audio Procedural 2026-2                      ║
//                         ╚═══════════════════════════════════════════════════════════════╝

// nomenclatura de variables:
// prefijo Va_→ variable 
// prefijo Co_ → constante
// prefijo Ve_ → vector
// prefijo Fu_ → función
// prefijo Ob_ → objeto
// prefijo Re_ → referencia
// prefijo Ar_ → arreglo

using UnityEngine;

public class CodigoDelJuego : MonoBehaviour
{
    
#region 1 Variables
//                         ╔═══════════════════════════════════════════════════════════════╗
//                         ║                            VARIABLES                          ║
//                         ╚═══════════════════════════════════════════════════════════════╝

    // ------------------------------------------ Variables del personaje ------------------------------------------
    [Header("Variables del Personaje")]
    public float Va_VelocidadDelPersonaje = 3f;

    // ------------------------------------------ Variables del audio HiHat ------------------------------------------
    [Header("Variables del HiHat")]
    //Variable referencia tipo audio source que apuntara al audio source del instrumento HiHat
    //Lo que le hagamos a esta variable referencia mediante codigo se vera reflejado en el audioSource del HiHat
    //en el hierachy 
    public AudioSource Re_AudioSourceHiHat;
    const int Co_FrecuenciaDeMuestreo = 44100; //en muesttras por segundo
    //La frecuencia de muestreo es cuántas muestras genera el computador 
    // por segundo, es decir la Va_VelocidadDelPersonaje con que el computador genera la señal.
    //Ejemplo:
    //Si tenemos una señal de 44.100 muestras entonces N=44.100 muestras
    //Si tenemos que procesa 1.000 muestras por segundo entonces FS= 1.000 muestrassegundo
    //Lo que nos lleva a deducir que la duración de la señal será:
    // Co_DuracionDeAudioEnSegundos = N / Fs
    // Co_DuracionDeAudioEnSegundos = 44.100 / 1.000
    // Co_DuracionDeAudioEnSegundos = 44.1 segundos
    // Por lo tanto, la señal tardará 44.1 segundos en reproducirse.
    //A mayor frecuencia de muestreo podremos representar la onda auditiva que queremos más 
    // fielmente osea mas precisa, porque vamos a tener más puntos que describen la onda por segundo
    const float Co_DuracionDeAudioEnSegundos = 0.3f; // en segundos
    const float Co_FrecuenciaCorteEnHz = 3000f;
    //Es la frecuencia a partir de la cual se "corta" la señal y solo se dejan pasar las frecuencias superiores a ese valor.

        // ------------------------------------------ Variables del audio Bombo ------------------------------------------
    [Header("Variables del Bombo")]
    //Variable referencia tipo audio source que apuntara al audio source del instrumento HiHat
    //Lo que le hagamos a esta variable referencia mediante codigo se vera reflejado en el audioSource del HiHat
    //en el hierachy 
    public AudioSource Re_AudioSourceBombo;
    public float Va_FrecuenciaInicial = 150f; //frecuencia inicial del tono fundamental senosoidal del bombo
    public float Va_VelocidadDeCaida = 10f;   //Velocidad de caida del tono fundamental senosoidal del bombo
    public float Va_FrecuenciaFinal = 75f;
    public float Va_FrecuenciaDelGolpe = 2000f;
    public float Va_NivelDelGolpe = 0.25f;
    public float Va_VelocidadDeCaidaDelGolpe = 150f;
    //public float Va_NivelDeRuidoDeGolpe = 0.500f; // el ruido tendrá poca intensidad, solo 15% de la señal 
    //public float Va_VelocidadDeCaidaDelGolpe = 150f; //hace que el ruido desaparezca rápidamente.

    // ------------------------------------------ Variables del audio Caja ------------------------------------------
    [Header("Variables de la Caja")]
    public AudioSource Re_AudioSourceCaja;
    public int Co_NumeroDeMuestras = Mathf.RoundToInt(Co_FrecuenciaDeMuestreo * Co_DuracionDeAudioEnSegundos);
    
#endregion

#region 2 Metodos
//                         ╔═══════════════════════════════════════════════════════════════╗
//                         ║                             METODOS                           ║
//                         ╚═══════════════════════════════════════════════════════════════╝


    #region 2.1 Fu_Start()
    // ------------------------------------------ Metodo Start ------------------------------------------
    void Start()
    {
        //Fu_GenerarHiHat();
    }
    #endregion // endregion de Start

    #region 2.2 Fu_Update()
    void Update()   
    {
        // MOVIMIENTO DEL PERSONAJE
        float Va_MovimientoX = Input.GetAxis("Horizontal");
        float Va_MovimientoZ = Input.GetAxis("Vertical");

        Vector3 Ve_Movimiento = new Vector3(-Va_MovimientoX, 0f, -Va_MovimientoZ);

        transform.Translate(Ve_Movimiento * Va_VelocidadDelPersonaje * Time.deltaTime);

        // Si la tecla G esta presionada, reproduzca sonido de hihat
        if (Input.GetKeyDown(KeyCode.G))
        {
           Fu_GenerarHiHat();
        }
        // Si la tecla H esta presionada, reproduzca sonido de Bombo   
        if (Input.GetKeyDown(KeyCode.H))
        {
           Fu_GenerarBombo();
        }
        // Si la tecla J esta presionada, reproduzca sonido de la caja   
        if (Input.GetKeyDown(KeyCode.J))
        {
           Fu_GenerarCaja();
        }

    }
    #endregion // endregion de Update

    #region 2.3 Fu_GenerarHIHAT()
    void Fu_GenerarHiHat()
    {
        Debug.Log("SE GENERÓ HI-HAT");

        int Co_NumeroDeMuestras = Mathf.RoundToInt(Co_FrecuenciaDeMuestreo * Co_DuracionDeAudioEnSegundos);
        //numero de muestras = frecuencia de muestreo * duración en segundos
        //numero de muestras = 44.100 * 0.3
        //numero de muestras = 13.230 
        // N = 13.230 muestras
        // VariableNumeroDeMuestras N representa la longitud de la señal en muestras,
        // es decir, cuántos puntos de datos tiene la señal.

        float[] Ar_Senal = new float[Co_NumeroDeMuestras];
        //creamos un vector de la señal de tipo float con una longitud igual al número de muestras
        //en nuestro caso 
        // float[] Ar_Senal = new float[13230];
        // 13.230 muestras, es decir, 13.230 puntos de datos que representan la señal de audio.

        System.Random Ob_RandomMio = new System.Random();

         #region 2.3.1 VariabFiltroPasaAlt
        // FILTRO PASA ALTOS

        // 1. Calculamos Va_FrecuenciaDeCorteDHzASegundos
        float Va_FrecuenciaDeCorteDeHzASegundos =  1f / (2f * Mathf.PI * Co_FrecuenciaCorteEnHz ); 
        // Hacemos la conversion de la frecuencia de corte de 3000 Hz a un valor de tiempo  en segundos
        // En nuestro caso
        // VariableFrecuenciaDeCorteDeHzASegundos= 1 / (2 * π * 3000) = 0.00005305 segundos

        // Esto no filtra el sonido, ni la señal solo es un calculo de conversion de Hz a segundos
        // Es simplemente un número intermedio calculado que necesitamos para poder construir el valor de alpha 
        // para el filtro

        // 2. Calculamos Va_TiempoEntreUnaMuestraYLaSiguiente
        float Va_TiempoEntreUnaMuestraYLaSiguiente = 1f / Co_FrecuenciaDeMuestreo;
        // En nuestro caso:
        // VariableTiempoEntreUnaMuestraYLaSiguiente = 1 / 44100 = 0.00002267 segundos

        // 3. Calculamos Va_Alpha
        float Va_Alpha = Va_FrecuenciaDeCorteDeHzASegundos / (Va_FrecuenciaDeCorteDeHzASegundos + Va_TiempoEntreUnaMuestraYLaSiguiente);
        // En nuestro caso:
        // VariableAlpha = 0.00005305 segundos / (0.00005305 segundos + 0.00002267 segundos);
        // VariableAlpha = 0.00005305 segundos / 0.00007572 segundos;
        // VariableAlpha = 0.7009 

        float Va_RuidoMasSenosAnterior = 0f; //para usar en el filtro pasa altos
        float Va_FiltradoAnterior = 0f;           //para usar en el filtro pasa altos

        #endregion // endregion de Filtro PasaAlto

        #region 2.3.2 Llenar Señal FOR 
        for (int Muestra = 0; Muestra < Co_NumeroDeMuestras; Muestra++)
        {
            
            // Ruido
            #region 2.3.2.1.Ruido
            // En nuestro caso: 
            // seria un numero Random entre -1 y 1 Por ejemplo en cada ciclo for generaria uno de estos valores:
            float VaRuido = (float)(Ob_RandomMio.NextDouble() * 2.0 - 1.0);
            // VaRuido [1]     = -1
             //...
            // VaRuido[2000]  = -0.7
             //...
            // VaRuido[10000] = 1
             //...
            // VaRuido [13230] = 0.5
            // hasta completar los 13.230  ciclos del for que representan el # de muestras de la señal de audio.
            #endregion // endregion de Ruido


            #region 2.3.2.3 Senos Agudos
            // Varias componentes SINUSOIDALES para darle carácter metálico
            // En nuestro caso: 
            // float SenalSeno1 = Mathf.Sin(2f * Mathf.PI * 5000f * Muestra / ConstanteFrecuenciaDeMuestreoFsenMuestrasPorSegundo);
            
            // SenalSeno1= Mathf.Sin (2*π * 5000 * 1 / 44100) = 0.6536
            // SenalSeno2= Mathf.Sin (2*π * 7000 * 1 / 44100) = 0.8400
            // SenalSeno3= Mathf.Sin (2*π * 9000 * 1 / 44100) = 0.9586

            // SenalSeno1= Mathf.Sin (2*π * 5000 * 2000 / 44100) = -0.9989
            // SenalSeno2= Mathf.Sin (2*π * 7000 * 2000 / 44100) = 0.2467
            // SenalSeno3= Mathf.Sin (2*π * 9000 * 2000 / 44100) = 0.8551

            // SenalSeno1= Mathf.Sin (2*π * 5000 * 10000 / 44100) = -0.9733
            // SenalSeno2= Mathf.Sin (2*π * 7000 * 10000 / 44100) = 0.9479
            // SenalSeno3= Mathf.Sin (2*π * 9000 * 10000 / 44100) = -0.9144

            // SenalSeno1= Mathf.Sin (2*π * 5000 * 13230 / 44100) = 0.9999
            // SenalSeno2= Mathf.Sin (2*π * 7000 * 13230 / 44100) = -0.9999
            // SenalSeno3= Mathf.Sin (2*π * 9000 *  13230/ 44100) = 0.9999 

            float Va_SenalSeno1 = Mathf.Sin(2f * Mathf.PI * 5000f * Muestra / Co_FrecuenciaDeMuestreo);
            float Va_SenalSeno2 = Mathf.Sin(2f * Mathf.PI * 7000f * Muestra / Co_FrecuenciaDeMuestreo);
            float Va_SenalSeno3 = Mathf.Sin(2f * Mathf.PI * 9000f * Muestra / Co_FrecuenciaDeMuestreo);
            #endregion // endregion de Senos Agudos


            #region 2.3.2.4 Ruido +Senos 
            // Mezcla de ruido + componentes metálicas
            float Va_RuidoMasSenosActual = 
                (VaRuido * 0.6f) + // el ruido será el 60% de la señal
                (Va_SenalSeno1 * 0.15f)+          // el primer seno será el 15% de la señal
                (Va_SenalSeno2 * 0.15f) +         // el segundo seno será el 15% de la señal
                (Va_SenalSeno3 * 0.10f);          // el tercer seno será el 10% de la señal
                                                  // para un total de 100% de la señal
            #endregion // endregion de Senos Agudos


            #region 2.3.2.5 Filtro Pasa Altos 
            // Para aplicar el filtro pasa altos, necesitamos el valor de la muestra de la SeñalRuidoMasSenos, 
            // A toda la señal le vamos a aplicar el filtro pasa altos, pero lo haremos muestra a muestra con el for
            // en cada ciclo del for le aplicaremos el filtro a una muestra hasta recorrer todo el for 
            // y filtrar todas las muestras de la señal.
            // De manera que aqui le estamos aplicando el filtro muestra a muestra
            // El propósito de aplicar este filtro es eliminar o quitar todas las frecuencias inferiores a 3000 Hz 
            // Osea quitar frecuencias graves 

            // 4. Calculamos Va_MuestraFiltrada
            float Va_FiltradoActual = Va_Alpha * (Va_FiltradoAnterior +  Va_RuidoMasSenosActual -  Va_RuidoMasSenosAnterior);

            // 5. Guardamos valores actuales  para que en el otro ciclo sean los valores anteriores 
            // Se actualizan estos dos valores para que en la siguiente iteración del for 
            // tengamos los valores correctos de la muestra anterior.
            Va_RuidoMasSenosAnterior = Va_RuidoMasSenosActual; 
            // Guardamos el valor de la muestra actual de la señal Va_SenalRuidoMasSenos como 
            // Va_SenalRuidoMasSenosAnterior, para que en la siguiente iteración sea el valor anterior
            // y podamos compararla con la nueva muestra que llegue.

            Va_FiltradoAnterior = Va_FiltradoActual; 
            // hacemos la Va_FiltradoAnterior igual a Va_MuestraFiltrada osea
            // hacemos Va_FiltradoAnterior igual al ultimo valor de la señal filtrada que calculamos en la línea anterior
            #endregion // endregion de Filtro Pasa Altos 

            // En nuestro caso:
            // vamos a suponer que nuestra Va_SenalRuidoMasSenos es [-1, -0.7, 1.2, 0.5 ] solo para explicar 
            // porque realmente no son 4 muestras sino 13.230 muestras
            // y asumiremos que Va_Alpha = 0.7009

            //Muestra 1:
            //          Va_RuidoMasSenosActual          = -1
            //          Va_RuidoMasSenosAnterior  =  0
            //          Va_FiltradoAnterior            =  0
            //          Va_FiltradoActual               =  Va_Alpha *( Va_FiltradoAnterior+Va_SenalRuidoMasSenos - Va_SenalRuidoMasSenosAnterior)
            //                                         = -0.7009

            //Muestra 2:
            //          Va_RuidoMasSenosActual          = -0.7
            //          Va_RuidoMasSenosAnterior  = -1
            //          Va_FiltradoAnterior            = -0.7009
            //          Va_FiltradoActual               =  Va_Alpha *( Va_FiltradoAnterior+Va_SenalRuidoMasSenos - Va_SenalRuidoMasSenosAnterior)
            //                                         = -0.2810
            
            //Muestra 3:
            //          Va_RuidoMasSenosActual          = 1.2
            //          Va_RuidoMasSenosAnterior  = -0.7   
            //          Va_FiltradoAnterior            = -0.2810
            //          Va_FiltradoActual               =  Va_Alpha *( Va_FiltradoAnterior+Va_SenalRuidoMasSenos - Va_SenalRuidoMasSenosAnterior)
            //                                         = 1.1348

            //Muestra 4:
            //          Va_RuidoMasSenosActual          = 0.5
            //          Va_RuidoMasSenosAnterior  = 1.2   
            //          Va_FiltradoAnterior            = 1.1348
            //          Va_FiltradoActual               =  Va_Alpha *( Va_FiltradoAnterior+Va_SenalRuidoMasSenos - Va_SenalRuidoMasSenosAnterior)
            //                                         = 0.3047
            
            //De manera que teniamos 
            //Va_SenalRuidoMasSenos es [-1, -0.7, 1.2, 0.5 ] y terminamos con 
            //Va_SeñalFiltrada [-0.70, -0.28, 1.13, 0.30]

            #region 2.3.2.6EnvolvExponAmpli
            float Va_EnvolventeDeLaMuestra = Mathf.Exp(-40f * Muestra / Co_FrecuenciaDeMuestreo);
            // Va_EnvolventeEnLaMuestra
            // La envolvente controla cómo cambia el volumen/amplitud del ruido a lo largo del tiempo.
            // Al inicio → a  envolvente que controla la amplitud o volumen cercana a 1 → sonido fuerte;
            // conforme avanzan las muestras → a  envolvente que controla la la amplitud o volumen disminuye;
            // al final → a  envolvente que controla la amplitud o volumen  se acerca a 0 → silencio.

            // Entonces por medio del volumen convertimos el ruido continuo en un golpe corto , 
            // fuerfe y que disminuye rapidamente hacia cero.
            //Y el 40f controla qué tan rápido ocurre esa caída: mayor valor → caída más rápida; menor valor → caída más lenta   
            
            // La envolvente se calcula usando una función exponencial asi
            // En nuestro caso: 
            //En cada ciclo for generaria uno de estos valores:
            //Va_EnvolventeEnLaMuestraActual = e^(-40*1/44100)     = 0.9988668
            //Va_EnvolventeEnLaMuestraActual = e^(-40*2000/44100)  = 0.1035631
            //Va_EnvolventeEnLaMuestraActual = e^(-40*10000/44100) = 0.0000119
            //Va_EnvolventeEnLaMuestraActual = e^(-40*13200/44100) = 0.0000003

            // Aplicar ENVOLVENTE 
            Ar_Senal[Muestra] =  Va_FiltradoActual * Va_EnvolventeDeLaMuestra;
            #endregion // endregion 2.3.2.6 EnvolvExponAmpli
        }


        #region 2.4.3.CrearAudioReprodu
            AudioClip ClipHiHat = AudioClip.Create( "HiHatProcedural", Co_NumeroDeMuestras,  1,  Co_FrecuenciaDeMuestreo, false );
                    // Esta línea crea en memoria un AudioClip VACIO donde después vas a guardar 
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

                    //Co_FrecuenciaDeMuestreoFsenMuestrasPorSegundo        //→ frecuencia de muestreo.
                    //Ejemplo: 44.100 muestras/segundo.

                    //false
                    //→ el clip NO se crea como un clip de streaming.
                    // es decir no se genera mientras se va reproduciendo, sino que se genera completo antes de reproducirse.
                    //Es decir, el hi-hat de 0.3 s tiene sus 13.230 muestras disponibles antes de reproducirse.

                    ClipHiHat.SetData(Ar_Senal, 0);
                    //Le pasamos nuestra señal al vector de datos del AudioClip que acabamos de crear, 
                    // para que el AudioClip tenga la señal que generamos.

                    Re_AudioSourceHiHat.clip = ClipHiHat;
                    //Le decimos al Audio Source: “El audio que vas a reproducir es este ClipHiHat que acabamos de generar”.

                    Re_AudioSourceHiHat.Play();
                    //Le decimos al Audio Source “El audio que vas a reproducir es este ClipHiHat que acabamos de generar”.

        #endregion //endregion 2.4.3CrearAudio yReproducir
      
        #endregion // endregion de llenar señal
    }

    #endregion // endregion de GenerarHiHat


    #region 2.4 Fu_GenerarBOMBO()

    void Fu_GenerarBombo()
    {
        Debug.Log("SE GENERÓ BOMBO");

        // 1. Calcular número de muestras
        int Co_NumeroDeMuestras = Mathf.RoundToInt( Co_FrecuenciaDeMuestreo * Co_DuracionDeAudioEnSegundos  );

        // 2. Crear vector donde guardaremos la señal
        float[] Ar_Senal = new float[Co_NumeroDeMuestras];

        //System.Random Ob_RandomMio = new System.Random(); //para generar el ruido del golpe aleatorio 
        
        // 3. Recorrer todas las muestras
        float Va_Fase = 0f;
        for (int Muestra = 0; Muestra < Co_NumeroDeMuestras; Muestra++)
        {
            #region 2.4.1Seno con pitch drop   
            // 4. Generar seno variando su frecuencia de 150 a 75 Hz
            float Va_Tiempo = (float)Muestra / Co_FrecuenciaDeMuestreo;
            float Va_Frecuencia = Va_FrecuenciaFinal + (Va_FrecuenciaInicial - Va_FrecuenciaFinal) * Mathf.Exp(-Va_VelocidadDeCaida * Va_Tiempo);
            Va_Fase +=  2f * Mathf.PI * Va_Frecuencia / Co_FrecuenciaDeMuestreo;
            float Va_SenalSeno = Mathf.Sin(Va_Fase);
            #endregion // endregion de 2.4.1.Seno con f variable 

            #region 2.4.2 EnvolvExponAmpli
            //inicia con alto volumen baja super rapido (exponencial) a cero (silencio)
            float Va_Envolvente = Mathf.Exp( -35f * Muestra / Co_FrecuenciaDeMuestreo );
            #endregion //endregion 2.4.2 EnvolvExponAmpli

            #region 2.4.2 Señal Seno Como Golpe de ataque + envolvente de golpe 
            //inicia con alto volumen baja super rapido (exponencial) a cero (silencio)
            float Va_SenalDelGolpe = Mathf.Sin( 2f * Mathf.PI * Va_FrecuenciaDelGolpe * Va_Tiempo );
            float Va_EnvolventeDelGolpe = Mathf.Exp( -Va_VelocidadDeCaidaDelGolpe * Va_Tiempo);
            Va_SenalDelGolpe *=  Va_EnvolventeDelGolpe * Va_NivelDelGolpe;
            #endregion //endregion 2.4.2 Señal Seno Como Golpe de ataque + envolvente de golpe 

            // 5. Guardar la muestra
            //Ar_Senal[Muestra] = Va_SenalSeno * Va_Envolvente+ Va_RuidoDelGolpe;
            Ar_Senal[Muestra] = (Va_SenalSeno * Va_Envolvente) + Va_SenalDelGolpe;
        }

        #region 2.4.3.CrearAudioReprodu
            // 6. Crear AudioClip
            AudioClip ClipBombo = AudioClip.Create( "BomboProcedural", Co_NumeroDeMuestras, 1, Co_FrecuenciaDeMuestreo, false);
            // 1   número de canales de audio.   -  1 = mono. - 2 = estéreo.
            //false              
            //→ el clip NO se crea como un clip de streaming.
            // es decir no se genera mientras se va reproduciendo, sino que se genera completo antes de reproducirse.
            //Es decir, el bombo de 0.3 s tiene sus 13.230 muestras disponibles antes de reproducirse.

            // 7. Pasar las muestras al AudioClip
            ClipBombo.SetData(Ar_Senal, 0);

            // 8. Asignar el clip al AudioSource del bombo
            Re_AudioSourceBombo.clip = ClipBombo;

            // 9. Reproducir
            Re_AudioSourceBombo.Play();
        #endregion //endregion 2.4.3CrearAudio yReproducir

    }

    #endregion //endregion de GenerarBombo

   
    #region 2.5 Fu_GenerarCAJA()
 
    // Las 5 componentes se mezclan para construir la Wavetable:
    void Fu_GenerarCaja()
    {
        Debug.Log("SE GENERÓ CAJA");

        int Co_TamanoDeWavetable = 1024; // Como sabemos muy bien que los computadores trabajan muy bien con potencias de 2 (128, 256, 512, 1024, 2048) 
        // se decidió darle tamaño de 1024 para que su procesamiento sea fácil, el audio que generaremos con la Wavetable tendrá 1024 muestras. 
        // Reutilizando esa Wavetable construiremos el audio completo que tendrá 44100 muestras/segundo × 0.3 segundos = 13230 muestras, aunque 
        // se podría generar toda con Wavetable, al hacer asi, le quitamos peso al computador.

        System.Random Ob_RandomMio = new System.Random();//Creamos el generador aleatorio antes del for.
        //Creamos el arreglo que tendrá dentro la señal antes del for.

        float[] Ar_Senal = new float[Co_NumeroDeMuestras];// Creamos el arreglo que tendrá dentro la señal.
        float[] Ar_WavetableMixDe5Senos = new float[Co_TamanoDeWavetable]; // Arreglo que guardará la wavetable.
        //se llena toda la "tabla" en el for, osea despues del for la wavetable de 1024 muestras
        // ya esta llena y luego se reutiliza para generar la señal completa de 13230 muestras.
        //--> Reutilizado, ciclico, por eso se genera con el numero de muestras de la Co_TamanoDeWavetable, para ahorrar procesamiento
        float[] Ar_RuidoBlanco = new float[Co_NumeroDeMuestras]; // arreflo donde se guardaran tantos ruidos como posiciones tenga la 
        //tabla o wavetable, //--> Sin reutilizar, único, ocurre una sola vez, por eso se genera con el total de muestras
        // y no con el solo tamaño de la wavetable
        //--> Reutilizado, ciclico, por eso se genera con el numero de muestras de la Co_TamanoDeWavetable, para ahorrar procesamiento
        float[] Ar_Envolvente = new float[Co_NumeroDeMuestras];//--> Sin reutilizar, único, ocurre una sola vez, por eso se genera con el total de muestras
        // y no con el solo tamaño de la wavetable
        float[] Ar_Ataque = new float[Co_NumeroDeMuestras];  //--> Sin reutilizar, único, ocurre una sola vez, por eso se genera con el total de muestras
        // y no con el solo tamaño de la wavetable
       
        int Va_IndiceWavetable = 0; //muestra 0 → posición 0 de la Wavetable
        //muestra 1 → posición 1...
        // muestra 1023 → posición 1023 de la wavetable
        // muestra 1024 → vuelve a posición 0 de la wavetable
        // muestra 1025 → posición 1
        // etc.
        // Así la Wavetable se reutiliza para llenar los 0.3 segundos que es la duracion de todo el audio.

        // For para construir las 1024 muestras de la  Ar_WAVETABLE MIX DE 5SENOS 
        // Como esta caracteristica del sonido en la caja, es repetitivo  se genera con solo 1024 muestras,
        // que mas adelante se reutilizaran para llenar ciclicamente las 13230 muestras en total
        for (int Muestra = 0; Muestra < Co_TamanoDeWavetable; Muestra++)
        {
            #region 2.5.1 Wavetable5Senos                 
                // 1. Generamos para cada muestras sus 5 componentes sinusoidales
                float Va_SenalSeno1 = Mathf.Sin(2f * Mathf.PI * 120f * Muestra / Co_FrecuenciaDeMuestreo); //120 es mas grave que 180
                float Va_SenalSeno2 = Mathf.Sin(2f * Mathf.PI * 240f * Muestra / Co_FrecuenciaDeMuestreo); //240 es mas grave que 360
                float Va_SenalSeno3 = Mathf.Sin(2f * Mathf.PI * 2000f * Muestra / Co_FrecuenciaDeMuestreo);
                float Va_SenalSeno4 = Mathf.Sin(2f * Mathf.PI * 5000f * Muestra / Co_FrecuenciaDeMuestreo);
                float Va_SenalSeno5 = Mathf.Sin(2f * Mathf.PI * 10000f * Muestra / Co_FrecuenciaDeMuestreo);

                // 2. Mezcla de las 5 componentes para construir la Wavetable , cada una con estos porcentajes o pesos.
                Ar_WavetableMixDe5Senos[Muestra]=(Va_SenalSeno1*0.50f)+(Va_SenalSeno2*0.50f)+(Va_SenalSeno3*0.01f)+(Va_SenalSeno4*0.01f)+(Va_SenalSeno5*0.01f);
            #endregion // endregion de 2.5.1 Wavetable5Senos        
        }


        // For para construir 13230 muestras del RUIDO BLANCO 
        // Como esta caracteristica del sonido en la caja, NO es repetitivo se generan las 13200 muestras en total
        // Para que sea único sin ser ciclico.
        for (int Muestra = 0; Muestra < Co_NumeroDeMuestras; Muestra++)
        {
            #region 2.5.2 RuidoBlanco  
                // 3.  Generamos el ruido blanco dentro del for para que en cada muestra el ruido sea diferente 
                Ar_RuidoBlanco[Muestra] = (float)( Ob_RandomMio.NextDouble() * 2.0 - 1.0);
                //en cada ciclo del for para cada una de las 1024 muestras de la wavetable.                
            #endregion // endregion 2.5.2 RuidoBlanco       
        }


        // For para construir 13230 muestras del ATAQUE 
        // Como esta caracteristica del sonido en la caja, NO es repetitivo se generan las 13200 muestras en total
        // Para que sea único sin ser ciclico.
        for (int Muestra = 0; Muestra < Co_NumeroDeMuestras; Muestra++)
        {
            float Va_SenalAtaque = Mathf.Sin(2f * Mathf.PI * 2000f * Muestra / Co_FrecuenciaDeMuestreo );  
            //2000f frecuencia del ataque, agudo          
            float Va_EnvolventeAtaque = Mathf.Exp(-240f * Muestra / Co_FrecuenciaDeMuestreo);
            //60f → ataque largo, 120f → ataque corto, 180f → ataque mas corto, (duración)
            Ar_Ataque[Muestra] = Va_SenalAtaque * Va_EnvolventeAtaque;
        }


        // For para construir 13230 muestras de la envolvente exponencial de amplitud
        // Como esta caracteristica del sonido en la caja, NO es repetitivo se generan las 13200 muestras en total
        // Para que sea único sin ser ciclico.
        for (int Muestra = 0; Muestra < Co_NumeroDeMuestras; Muestra++)
        {
            Ar_Envolvente[Muestra] = Mathf.Exp(-8f * Muestra / Co_FrecuenciaDeMuestreo);
        }


        //Reutilizamos los 1024 senos (wavetable basica)  --> Reutilizada ciclicamente 
        //               + 1024 ruidos                    ---> Sin reutilizar, único, ocurre una sola vez.  
        //               + 13230 muestras de envolventes  --> Sin reutilizar, único, ocurre una sola vez. 
        //               + 13230 muestras de ataque       --> Sin reutilizar, único, ocurre una sola vez.
        //                 para llenar las 13230 muestras de la señal completa de 0.3 segundos.
        for (int Muestra = 0; Muestra < Co_NumeroDeMuestras; Muestra++)
        {
            Va_IndiceWavetable = Muestra % Co_TamanoDeWavetable;
            //Ar_Senal[Muestra] = Ar_Wavetable5Senos[Va_IndiceWavetable];// si queremos escuchar solo las 13.200 muestras del mix de los 5 senos
            //Ar_Senal[Muestra] = Ar_Wavetable5Senos[Va_IndiceWavetable]*0.60f + Ar_RuidoBlanco[Va_IndiceWavetable]*0.40f; // si queremos 
            //escuchar los 13.200 mix de senos + 13.200 ruidos 
            //Ar_Senal[Muestra] = (
                                    //(
                                        //  Ar_WavetableMixDe5Senos[Va_IndiceWavetable]*0.60f  //reutlizando 1024 muestrs - ciclico
                                        //+ Ar_RuidoBlanco[Va_IndiceWavetable]*0.40f      //SIN reutlizar, no se usan 1024 muestras sino las 13200 - único
                                        //+ Ar_Ataque[Muestra]*0.20f                      //SIN reutlizar, no se usan 1024 muestras sino las 13200 - único
                                    //)
                                //); // si queremos escuchar los 13.200 mix de senos + 13.200 ruidos + ataque
            Ar_Senal[Muestra] = (
                        (
                                Ar_WavetableMixDe5Senos[Va_IndiceWavetable]*0.60f  //reutlizando 1024 muestras - ciclico
                            + Ar_RuidoBlanco[Va_IndiceWavetable]*0.20f      //SIN reutlizar, no se usan 1024 muestras sino las 13200 - único
                            + Ar_Ataque[Muestra]*0.20f                      //SIN reutlizar, no se usan 1024 muestras sino las 13200 - único
                        )
                        * Ar_Envolvente[Muestra]                            //SIN reutlizar, no se usan 1024 muestras sino las 13200 - único
                    ); // si queremos escuchar los 13.200 mix de senos + 13.200 ruidos + ataque + envolvente exponencial de amplitud

        } 

        #region 2.5.4.CrearAudioReprodu

            // 6. Crear AudioClip
            AudioClip ClipCaja = AudioClip.Create( "CajaProcedural", Co_NumeroDeMuestras, 1, Co_FrecuenciaDeMuestreo, false);
            // 1   número de canales de audio.   -  1 = mono. - 2 = estéreo.
            //false              
            //→ el clip NO se crea como un clip de streaming.
            // es decir no se genera mientras se va reproduciendo, sino que se genera completo antes de reproducirse.
            //Es decir, la Caja de 0.3 s tiene sus 13.230 muestras disponibles antes de reproducirse.

            // 7. Pasar las muestras al AudioClip
            ClipCaja.SetData(Ar_Senal, 0);

            // 8. Asignar el clip al AudioSource de la caja
            Re_AudioSourceCaja.clip = ClipCaja;

            // 9. Reproducir
            Re_AudioSourceCaja.Play();
        #endregion //endregion 2.5.4.CrearAudioReprodu
    }

    #endregion // endregion de Fu_GenerarCaja()

#endregion // endregion de metodos

}
