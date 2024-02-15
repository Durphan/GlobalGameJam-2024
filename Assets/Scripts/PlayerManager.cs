using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private bool saltoDesbloqueado;
    [SerializeField] AudioSource sonidoMuerte;
    [SerializeField] public bool activo;
    [SerializeField] private bool disparoDesbloqueado;
    [SerializeField] private float BalasXSegundo;
    [SerializeField] private Transform PuntoDeDisparo;
    [SerializeField] private GameObject bala;
    [SerializeField] private float Velocidad;
    [SerializeField] private float FuerzaDeSalto;

    [SerializeField] private float DistanciaDeDeteccion;
    [SerializeField] private GameObject pistola;
    private float Horizontal;
    private Rigidbody2D RB2D;
    private Collider2D coll;
    private Animator animator;
    private AudioSource audioSource;
    public UnityEvent AlMorir = new UnityEvent();
    [SerializeField] public Transform checkpointPosition;
    private float TiempoDeUltimoDisparo;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //checkpointPosition = transform.position;
        coll = GetComponent<Collider2D>();
        RB2D = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        AlMorir.AddListener(Morir);
    }

    // Update is called once per frame
    void Update()
    {
        if (!activo)
        {
            return;
        }
        SaltarEnElSuelo();
        Caminar();
        DispararSiPuede();
    }
    private void FixedUpdate()
    {
        if (!activo)
        {
            RB2D.velocity = Vector2.zero;
            return;
        }
        CaminarFixed();
    }

    private void CaminarFixed()
    {
        RB2D.velocity = new Vector2(Horizontal * Velocidad, RB2D.velocity.y);
    }



    void Caminar()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        if (Horizontal > 0)
        {
            GirarA_(0);
            animator.SetBool("Caminando", true);
        }
        else if (Horizontal < 0)
        {
            GirarA_(180);
            animator.SetBool("Caminando", true);
        }
        else
        {
            animator.SetBool("Caminando", false);
        }
        void GirarA_(int direccion)
        {
            transform.rotation = Quaternion.Euler(0, direccion, 0);
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = transform.position;
        Vector2 direction = Vector2.down;

        // Lanzar un rayo hacia abajo
        RaycastHit2D hit = Physics2D.Raycast(origin + Vector2.left * 0.5f, direction, DistanciaDeDeteccion, LayerMask.GetMask("Platforms"));
        RaycastHit2D hit2 = Physics2D.Raycast(origin + Vector2.right * 0.5f, direction, DistanciaDeDeteccion, LayerMask.GetMask("Platforms"));

        // Si el rayo golpea algo, estamos en el suelo
        return hit.collider != null || hit2.collider != null;
    }
    private void SaltarEnElSuelo()
    {
        if (saltoDesbloqueado && Input.GetButtonDown("Jump") && IsGrounded())
        {
            StartCoroutine(Saltar());
        }
    }

    IEnumerator Saltar()
    {
        RB2D.velocity = new Vector2(RB2D.velocity.x, FuerzaDeSalto);
        animator.SetTrigger("Saltar");

        yield return new WaitWhile(() => IsGrounded());
        yield return new WaitUntil(() => RB2D.velocity.y < 0.05f);
        animator.SetBool("Cayendo", true);
        yield return new WaitUntil(() => IsGrounded());
        animator.SetBool("Cayendo", false);
    }


    private void DispararSiPuede()
    {
        if (disparoDesbloqueado && Input.GetButtonDown("Fire1") && Time.time > TiempoDeUltimoDisparo + 1 / BalasXSegundo)
        {
            animator.SetTrigger("Disparar");
            Disparar();
            TiempoDeUltimoDisparo = Time.time;
        }
    }
    private void Disparar()
    {
        Instantiate(bala, PuntoDeDisparo.position, transform.rotation);
    }

    void Morir()
    {
        animator.SetTrigger("Morir");
        sonidoMuerte.Play();
        coll.enabled = false;
        activo = false;
    }
    public void Revivir()
    {
        transform.position = checkpointPosition.position;
        coll.enabled = true;
        animator.SetTrigger("Revivir");
        activo = true;
        GameObject.Find("EggKnight").transform.position = GameObject.Find("Spawn Caballero").transform.position;
        GameObject.Find("EggKnight").GetComponent<HeroController>().spawneo = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            checkpointPosition = collision.transform;
            collision.gameObject.GetComponent<Animator>()?.SetTrigger("Activar");
        }

        switch (collision.gameObject.name)
        {
            case "Mago":
                collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                collision.gameObject.GetComponent<MageController>().audioAparicion.Play();
                break;
            case "Aprender Salto":
                saltoDesbloqueado = true;
                break;
            case "Aprender Disparo":
                disparoDesbloqueado = true;
                pistola.SetActive(true);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Mago")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<MageController>().audioDesaparicion.Play();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Muerte") || collision.gameObject.layer == 7)
        {
            AlMorir.Invoke();
        }
    }
    public void ReproducirSonido(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}