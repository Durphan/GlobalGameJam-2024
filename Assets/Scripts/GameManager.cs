using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camaraVirtual;
    [SerializeField] private GameObject slime;
    [SerializeField] float tiempoDeIntro;
    [SerializeField] private SpriteRenderer telon;
    [SerializeField] private GameObject botonRevivir;

    // Start is called before the first frame update
    void Start()
    {
        slime.GetComponent<PlayerManager>().AlMorir.AddListener(BajarTelon);
        slime.GetComponent<PlayerManager>().AlMorir.AddListener(TextoGameOver);
        StartCoroutine(Intro());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        telon.transform.position = new Vector3(camaraVirtual.transform.position.x, camaraVirtual.transform.position.y, 0);
    }

    IEnumerator Intro()
    {
        string[] mensajes = { "Había una vez un valiente caballero con una misión.", "Un caballero muy peculiar...", "De ti dependerá su destino.", "Nah, mentira. Vos sos un slime en el camino.", "Tu objetivo es detener al valiente caballero.", "Sin embargo, esto se trata de un juego...", "...por lo tanto, deberás comprarlo primero." , "Está bien, esta primera vida es gratis.", "¿Qué esperás? ¡Enfrentate al héroe!"};
        GameObject.Find("Cuadro De Dialogo").GetComponent<DialogueController>().GenerarDialogo(mensajes);
        yield return new WaitForSeconds(3);
        camaraVirtual.m_Lens.OrthographicSize = 4.5f;
        yield return new WaitForSeconds(6);
        camaraVirtual.Follow = GameObject.Find("Slime").transform;
        yield return new WaitForSeconds(15);
        slime.GetComponent<PlayerManager>().activo = true;
    }

    public void SubirTelon()
    {
        telon.color = new Color(0, 0, 0, 0);
    }

    public void BajarTelon()
    {
        telon.color = new Color(0, 0, 0, 255);
    }

    void TextoGameOver()
    {
        StartCoroutine(ActivarTextoGameOver());
    }

    IEnumerator ActivarTextoGameOver()
    {
        yield return new WaitForSeconds(1);
        string[] mensaje = { "Pero qué lástima...", "¿Continuar? Se debitarán ARS100 de manera automática" };
        GameObject.Find("Cuadro De Dialogo").GetComponent<DialogueController>().GenerarDialogo(mensaje);
        yield return new WaitForSeconds(3.5f);
        botonRevivir.SetActive(true);
    }
}
