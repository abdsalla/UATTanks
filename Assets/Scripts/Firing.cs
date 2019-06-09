using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour
{

    public int playerNumber;
    public float minLaunchForce = 15f;
    public float maxLaunchForce = 30f;
    public float maxChargeTime = .75f;
    public Tank_Data tankRef;
    public Rigidbody shell;
    public Transform fireTransform;
    public AudioSource shootingAudio;
    public AudioClip chargingClip;
    public AudioClip fireClip;

    private string firedButton;
    private float chargeSpeed;
    private float currentLaunchForce;
    private bool fired;

    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
        tankRef.slider.value = minLaunchForce;
    }

    void Start()
    {
        firedButton = "Fire" + tankRef.playerNumber;

        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }


    void Update()
    {

        if (currentLaunchForce >= maxLaunchForce && !fired)
        {
            currentLaunchForce = maxLaunchForce;
            Fire();
        }

        else if (Input.GetButtonDown(firedButton))
        {
            fired = false;
            currentLaunchForce = minLaunchForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }

        else if (Input.GetButton(firedButton) && fired)
        {
            currentLaunchForce += chargeSpeed * Time.deltaTime;

            tankRef.slider.value = currentLaunchForce;
        }

        else if (Input.GetButtonUp(firedButton) && fired)
        {
            Fire();
        }
    }

    private void Fire()
    {
        fired = true;
        Rigidbody shellinstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;

        shellinstance.velocity = currentLaunchForce * fireTransform.forward;

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentLaunchForce = minLaunchForce;
    }
}
