using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class HeroController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float velocidadMaxima;
    [SerializeField] private float aceleracion;
    [SerializeField] private float fuerzaDeSalto;
    [SerializeField] private LayerMask capasDeObstaculo;
    [SerializeField] private float distanciaDeteccionDePared;
    [SerializeField] private LayerMask capaDeSlime;
    [SerializeField] private float distanciaDeteccionDeSlime;
    [SerializeField] private float distanciaMeleeSlime;
    [SerializeField] private AudioSource audioMovimiento;
    [SerializeField] private AudioSource audioAtaque;
    [SerializeField] private int salud;
    public bool spawneo = false;
    UnityEvent OnDeath = new UnityEvent();
    private int direccion;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        direccion = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float deltaVelocidad = Mathf.Clamp(velocidadMaxima - Mathf.Abs(rb.velocity.x), -aceleracion, aceleracion);
        float fuerza = rb.mass * deltaVelocidad;
        rb.AddForce(Vector2.right * fuerza * direccion, ForceMode2D.Impulse);

        if (ObstaculoDetectado() && EnElPiso())
        {
            if (!spawneo)
            {
                StartCoroutine(Saltar());
            }
            else
            {
                DarseVuelta();
            }
        }

        if (SlimeDetectado())
        {
            DarseVuelta();
        }

        if (SlimeAMelee())
        {
            anim.SetTrigger("Atacar");
            if (!audioAtaque.isPlaying)
                audioAtaque.Play();
        }

    }

    IEnumerator Saltar()
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaDeSalto);
        yield return new WaitUntil(() => !EnElPiso());
        audioMovimiento.Pause();
        yield return new WaitUntil(() => EnElPiso());
        audioMovimiento.UnPause();
    }

    void DarseVuelta()
    {
        transform.Rotate(new Vector3(0, 180, 0));
        direccion *= -1;
    }

    bool ObstaculoDetectado()
    {
        return Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector2.right * direccion, distanciaDeteccionDePared, capasDeObstaculo);
    }

    bool SlimeDetectado()
    {
        return Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector2.left * direccion, distanciaDeteccionDeSlime, capaDeSlime);
    }

    bool SlimeAMelee()
    {
        return Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector2.right * direccion, distanciaMeleeSlime, capaDeSlime);
    }

    bool EnElPiso()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.05f, capasDeObstaculo);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "bala")
        {
            salud--;
            if (salud <= 0)
            {
                SceneManager.LoadScene("Final");
            }
        }
    }
}
