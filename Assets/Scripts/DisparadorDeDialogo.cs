using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisparadorDeDialogo : MonoBehaviour
{
    //[SerializeField] private TMP_Text Texto;
    //[SerializeField] private GameObject CuadroDeDialogo;
    //[SerializeField] private string dialogo;
    //[SerializeField] private Color color;
    [SerializeField] public string[] mensajes;
    private bool abierto;

    private void Start()
    {
        //CuadroDeDialogo.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Texto.text = dialogo;
            //Texto.color = color;
            //CuadroDeDialogo.SetActive(true);
            if (!abierto)
            {
                GameObject.Find("Cuadro De Dialogo").GetComponent<DialogueController>().GenerarDialogo(mensajes);
                abierto = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //CuadroDeDialogo.SetActive(false);
            Debug.Log("cerrar dialogo");
        }
    }

}
