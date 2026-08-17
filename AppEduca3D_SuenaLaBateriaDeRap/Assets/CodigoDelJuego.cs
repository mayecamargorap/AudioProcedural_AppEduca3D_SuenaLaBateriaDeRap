using UnityEngine;

public class CodigoDelJuego : MonoBehaviour
{
    public float velocidad = 2f;

    void Update()
    {
        //Jugador se mueve con las teclas de flecha o WASD
        float movimientoX = Input.GetAxis("Horizontal");
        float movimientoZ = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(movimientoX, 0f, movimientoZ);

        transform.Translate(movimiento * velocidad * Time.deltaTime);
    }
}