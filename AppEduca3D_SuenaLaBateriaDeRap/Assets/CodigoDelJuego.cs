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

using UnityEngine;

public class CodigoDelJuego : MonoBehaviour
{
    
#region 1 Variables
//                         ╔═══════════════════════════════════════════════════════════════╗
//                         ║                            VARIABLES                          ║
//                         ╚═══════════════════════════════════════════════════════════════╝

    // ------------------------------------------ Variables del personaje ------------------------------------------

    public float Va_VelocidadDelPersonaje = 3f;

    // ------------------------------------------ Variables del audio HiHat ------------------------------------------

    public AudioSource Ob_AudioSourceAudioHiHat;

    public int Co_FrecuenciaDeMuestreo = 44100; //en muesttras por segundo
    //La frecuencia de muestreo es cuántas muestras genera o procesa el computador 
    // por segundo, es decir la Va_VelocidadDelPersonaje con que el computador lee o genera la señal.
    //Ejemplo:
    //Si tenemos una señal de 44.000 muestras entonces N=44.000 muestras
    //Si tenemos que procesa 1.000 muestras por segundo entonces FS= 1.000 muestrassegundo
    //Lo que nos lleva a deducir que la duración de la señal será:
    // Co_DuracionDeAudioEnSegundos = N / Fs
    // Co_DuracionDeAudioEnSegundos = 44.000 / 1.000
    // Co_DuracionDeAudioEnSegundos = 44 segundos
    // Por lo tanto, la señal tardará 44 segundos en reproducirse.
    //A mayor frecuencia de muestreo podremos representar la onda auditiva que queremos más 
    // fielmente osea mas precisa, porque vamos a tener más puntos que describen la onda por segundo

    public float Co_DuracionDeAudioEnSegundos = 0.3f; // en segundos
    public float Co_FrecuenciaCorteEnHz = 3000f;
    //Es la frecuencia a partir de la cual se "corta" la señal y solo se dejan pasar las frecuencias superiores a ese valor.

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

        Vector3 Ve_Movimiento = new Vector3(Va_MovimientoX, 0f, Va_MovimientoZ);

        transform.Translate(Ve_Movimiento * Va_VelocidadDelPersonaje * Time.deltaTime);

        // GENERAR HI-HAT
        if (Input.GetKeyDown(KeyCode.Space))
        {
           Fu_GenerarHiHat();
        }
    }
    #endregion // endregion de Update

    #region 2.3 Fu_GenerarHiHat()
    void Fu_GenerarHiHat()
    {
        Debug.Log("SE GENERÓ HI-HAT");

        int Co_NumeroDeMuestras = Mathf.RoundToInt(Co_FrecuenciaDeMuestreo * Co_DuracionDeAudioEnSegundos);
        //numero de muestras = frecuencia de muestreo * duración en segundos
        //numero de muestras = 44.000 * 0.3
        //numero de muestras = 13.200 
        // N = 13.200 muestras
        // VariableNumeroDeMuestras N representa la longitud de la señal en muestras,
        // es decir, cuántos puntos de datos tiene la señal.

        float[] Ve_Senal = new float[Co_NumeroDeMuestras];
        //creamos un vector de la señal de tipo float con una longitud igual al número de muestras
        //en nuestro caso 
        // float[] Ve_Senal = new float[13200];
        // 13.200 muestras, es decir, 13.200 puntos de datos que representan la señal de audio.

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

        // 3. Calculamos Co_Alpha
        float Co_Alpha = Va_FrecuenciaDeCorteDeHzASegundos / (Va_FrecuenciaDeCorteDeHzASegundos + Va_TiempoEntreUnaMuestraYLaSiguiente);
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
            // VaRuido [13200] = 0.5
            // hasta completar los 13.200 ciclos del for que representan el # de muestras de la señal de audio.
            #endregion // endregion de Ruido


            #region 2.3.2.3 Senos Agudos
            // Varias componentes SINUSOIDALES para darle carácter metálico
            // En nuestro caso: 
            // float SenalSeno1 = Mathf.Sin(2f * Mathf.PI * 5000f * Muestra / ConstanteFrecuenciaDeMuestreoFsenMuestrasPorSegundo);
            
            // SenalSeno1= Mathf.Sin (2*π * 5000 * 1 / 44100) = 0.6536
            // SenalSeno2= Mathf.Sin (2*π * 7000 * 1 / 44100) = 0.8400
            // SenalSeno3= Mathf.Sin (2*π * 9000 * 1 / 44100) = 0.9586

            // SenalSeno1= Mathf.Sin (2*π * 7000 * 2000 / 44100) = -0.9989
            // SenalSeno2= Mathf.Sin (2*π * 9000 * 2000 / 44100) = 0.2467
            // SenalSeno3= Mathf.Sin (2*π * 9000 * 2000 / 44100) = 0.8551

            // SenalSeno1= Mathf.Sin (2*π * 7000 * 10000 / 44100) = -0.9733
            // SenalSeno2= Mathf.Sin (2*π * 9000 * 10000 / 44100) = 0.9479
            // SenalSeno3= Mathf.Sin (2*π * 9000 * 10000 / 44100) = -0.9144

            // SenalSeno1= Mathf.Sin (2*π * 7000 * 13200 / 44100) = 0.9999
            // SenalSeno2= Mathf.Sin (2*π * 9000 * 13200 / 44100) = -0.9999
            // SenalSeno3= Mathf.Sin (2*π * 9000 *  13200 / 44100) = 0.9999 

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
            float Va_FiltradoActual = Co_Alpha * (Va_FiltradoAnterior +  Va_RuidoMasSenosActual -  Va_RuidoMasSenosAnterior);

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
            // porque realmente no son 4 muestras sino 13.200 muestras
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
            //                                         = -0.4906
            
            //Muestra 3:
            //          Va_RuidoMasSenosActual          = 1.2
            //          Va_RuidoMasSenosAnterior  = -0.7   
            //          Va_FiltradoAnterior            = -0.4906
            //          Va_FiltradoActual               =  Va_Alpha *( Va_FiltradoAnterior+Va_SenalRuidoMasSenos - Va_SenalRuidoMasSenosAnterior)
            //                                         = 0.9878

            //Muestra 4:
            //          Va_RuidoMasSenosActual          = 0.5
            //          Va_RuidoMasSenosAnterior  = 1.2   
            //          Va_FiltradoAnterior            = 0.9878
            //          Va_FiltradoActual               =  Va_Alpha *( Va_FiltradoAnterior+Va_SenalRuidoMasSenos - Va_SenalRuidoMasSenosAnterior)
            //                                         = 0.2018
            
            //De manera que teniamos 
            //Va_SenalRuidoMasSenos es [-1, -0.7, 1.2, 0.5 ] y terminamos con 
            //Va_SeñalFiltrada [-0.70, -0.49, 0.98, 0.20]

            #region 2.3.2.6 Envolvente
            float Va_EnvolventeDeLaMuestra = Mathf.Exp(-40f * Muestra / Co_FrecuenciaDeMuestreo);
            // Va_EnvolventeEnLaMuestra
            // La envolvente controla cómo cambia el volumen/amplitud del ruido a lo largo del tiempo.
            // Al inicio → a  envolvente que controla la amplitud o volumen cercana a 1 → sonido fuerte;
            // conforme avanzan las muestras → a  envolvente que controla la la amplitud o volumen disminuye;
            // al final → a  envolvente que controla la amplitud o volumen  se acerca a 0 → silencio.

            // Entonces por medio del volumen convertimos el ruido continuo en un golpe corto , 
            // fuerfe y que disminuye rapidamente hacia cero.
            //Y el 50f controla qué tan rápido ocurre esa caída: mayor valor → caída más rápida; menor valor → caída más lenta   
            
            // La envolvente se calcula usando una función exponencial asi
            // En nuestro caso: 
            //En cada ciclo for generaria uno de estos valores:
            //Va_EnvolventeEnLaMuestraActual = e^(-50*1/44100)     = 0.9988668
            //Va_EnvolventeEnLaMuestraActual = e^(-50*2000/44100)  = 0.1035631
            //Va_EnvolventeEnLaMuestraActual = e^(-50*10000/44100) = 0.0000119
            //Va_EnvolventeEnLaMuestraActual = e^(-50*13200/44100) = 0.0000003

            // Aplicar ENVOLVENTE 
            Ve_Senal[Muestra] =  Va_FiltradoActual * Va_EnvolventeDeLaMuestra;
            #endregion // endregion de Filtro + Envolvente
        }

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

        ClipHiHat.SetData(Ve_Senal, 0);
        //Le pasamos nuestra señal al vector de datos del AudioClip que acabamos de crear, 
        // para que el AudioClip tenga la señal que generamos.

        Ob_AudioSourceAudioHiHat.clip = ClipHiHat;
        //Le decimos al Audio Source: “El audio que vas a reproducir es este ClipHiHat que acabamos de generar”.

        Ob_AudioSourceAudioHiHat.Play();
        //Le decimos al Audio Source “El audio que vas a reproducir es este ClipHiHat que acabamos de generar”.

        #endregion // endregion de llenar señal
    }

    #endregion // endregion de HiHat

#endregion // endregion de metodos

}

