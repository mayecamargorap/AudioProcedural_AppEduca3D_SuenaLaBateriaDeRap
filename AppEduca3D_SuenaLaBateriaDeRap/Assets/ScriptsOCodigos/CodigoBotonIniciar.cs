using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class CodigoBotonIniciar : MonoBehaviour
{
    public void Fu_IrAInstrucciones()
    {
        SceneManager.LoadScene("EscenaInstrucciones");
    }
}