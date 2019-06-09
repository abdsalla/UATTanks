using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tank_Data : MonoBehaviour
{
    public int playerNumber = 1;
    public float shellFiringDelay = 1.5f;
    public float shellMaxLifeTime = 2f;
    public float playerMoveSpeed = 12f;
    public float shellSpeed = 2.5f;
    public float shellBuffer;
    public float maxDamage = 100f;
    public float turnSpeed = 180f;
    public float pitchRange = .2f;
    public float startingHealth = 100f;
    public AudioSource movementAudio;
    public AudioClip engineIdle; 
    public AudioClip engineDrive;
    public GameObject shellExitPoint;
    public GameObject shell;
    public Image fillImage;
    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;
    public Slider slider;
    public LayerMask tankMask;
 
    private float currentHealth;
    private float originalPitch = 1f;
    private float movementInputValue;
    private float turnInputValue;
    private string movementAxisName;
    private string turnAxisName;
    private bool dead;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.isKinematic = true;
        movementInputValue = 0f;
        turnInputValue = 0f;
        currentHealth = startingHealth;
        dead = false;

        SetHealthUI();
    }

    private void OnDisable()
    {
        rb.isKinematic = false;
    }

    public void Start()
    {
        float nextShootCallTime = Time.deltaTime;

        movementAxisName = "Vertical" + playerNumber;
        turnAxisName = "Horizontal" + playerNumber;

        originalPitch = movementAudio.pitch;

        nextShootCallTime = Time.time + shellFiringDelay;
    }

    private void Update()
    {

        movementInputValue = Input.GetAxis(movementAxisName);
        turnInputValue = Input.GetAxis(turnAxisName);

        EngineAudio();
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
        Shoot();
    }

    private void EngineAudio()
    {
        if (Mathf.Abs(movementInputValue) < 0.1f && Mathf.Abs(turnInputValue) < 0.1f)
        {
            if (movementAudio.clip == engineDrive)
            {
                movementAudio.clip = engineIdle;
                movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudio.Play();
            }
        }
        else
        {
            if (movementAudio.clip == engineIdle)
            {
                movementAudio.clip = engineDrive;
                movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudio.Play();
            }
        }
    }

    private void Move()
    {
        Vector3 movement = transform.right * movementInputValue * playerMoveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void Turn()
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }


    private void Shoot()
    {
        float nextShootCallTime = Time.deltaTime;

        if (Time.time >= nextShootCallTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject tempShellHandler;
                tempShellHandler = Instantiate(shell, shellExitPoint.transform.position, shellExitPoint.transform.rotation) as GameObject;

                tempShellHandler.transform.Rotate(Vector3.down * 90);

                Rigidbody tempRbRef;
                tempRbRef = tempShellHandler.GetComponent<Rigidbody>();

                tempRbRef.AddForce(-transform.right * shellSpeed);
                nextShootCallTime = Time.time + shellFiringDelay;
                Destroy(tempShellHandler, 4.0f);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        SetHealthUI();

        if (currentHealth <= 0f && !dead)
        {
            OnDeath();
        }
    }

    private void SetHealthUI()
    {
        slider.value = currentHealth;

        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / startingHealth);
    }

    private void OnDeath()
    {
        dead = true;
        gameObject.SetActive(false);
    }
}