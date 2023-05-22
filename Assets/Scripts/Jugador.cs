using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Jugador : MonoBehaviour {
    private Rigidbody rb;
    private float velocidad = 6.0f;
    public GameObject pantallaFin;

    public TextMeshProUGUI tiempoTxt;
    public TextMeshProUGUI estadoTxt;
    private float tiempoInicio;
    private int tiempoEmpleado;

    private int premios;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        tiempoInicio = Time.time;
        premios = 5;
    }

    // Update is called once per frame
    void Update() {
        //Capturo el movimiento en horizontal y vertical de nuestro teclado
        float movimientoH = Input.GetAxis("Horizontal");
        float movimientoV = Input.GetAxis("Vertical");

        transform.position += new Vector3(movimientoH, 0, movimientoV) * velocidad * Time.deltaTime;

        // Actualizamos tiempo
        tiempoEmpleado = (int)Time.time - (int)tiempoInicio;
        SetTiempo(tiempoEmpleado);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Esbirro")) {
            estadoTxt.text = "Derrota!";
            FinPartida();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Premio")) {
            Destroy(other.gameObject);
            premios--;
            Debug.Log("Premios--");
        }

        if (premios <= 0) {
            estadoTxt.text = "Victoria!";
            FinPartida();
        }
    }

    public void SetTiempo(int tiempo) {
        int segundos = tiempo % 60;
        int minutos = tiempo / 60;
        tiempoTxt.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

    public void FinPartida() {
        Time.timeScale = 0;
        pantallaFin.SetActive(true);
        Debug.Log("Fin");
    }
}