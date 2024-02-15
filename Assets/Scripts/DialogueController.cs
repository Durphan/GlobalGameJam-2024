using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public TMP_Text cuadroDeTexto;
    public GameObject cuadroNegro;
    //public UnityEvent AlCrearDialogo = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        cuadroNegro = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerarDialogo(string[] mensajes)
    {

        StartCoroutine(GenerarMensajes(mensajes));
    }

    public IEnumerator GenerarMensajes(string[] mensajes)
    {
        cuadroNegro.GetComponent<Image>().color = new Color(0, 0, 0, 255);
        foreach (string mensaje in mensajes)
        {
            cuadroDeTexto.text = mensaje;
            yield return new WaitForSeconds(3);
        }
        cuadroDeTexto.text = "";
        cuadroNegro.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
}
