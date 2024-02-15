using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaScript : MonoBehaviour
{
    [SerializeField]
    private float Velocidad;
    [SerializeField]
    private float SegundosParadesaparecer = 4;
    private void Start()
    {
        StartCoroutine(DesaparecerEn(SegundosParadesaparecer));
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.rotation.eulerAngles.y == 180)
        {
            transform.localPosition += Vector3.left * Velocidad * Time.deltaTime;
            return;
        }
        transform.localPosition += Vector3.right * Velocidad * Time.deltaTime;
    }
    
    IEnumerator DesaparecerEn(float SegundosParadesaparecer)
    {
        yield return new WaitForSeconds(SegundosParadesaparecer);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
