using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip sideThrusters;

    [SerializeField] ParticleSystem leftThruster;
    [SerializeField] ParticleSystem rightThruster;
    [SerializeField] ParticleSystem mainThruster;

    Rigidbody rb;
    AudioSource audioSource;

    bool mainThrustIsOn = false;
    bool sideThrustIsOn = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        // need to work out how to switch off particle effect when transitioning
        
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }

        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }

        else
        {
            StopRotating();
        }
    }

    void StartThrusting()
    {
        mainThrustIsOn = true;

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!mainThruster.isPlaying)
        {
            mainThruster.Play();
        }
    }

    void StopThrusting()
    {
        if (mainThrustIsOn)
        {
            audioSource.Stop();
        }

        mainThruster.Stop();
        mainThrustIsOn = false;
    }

    void StopRotating()
    {
        if (sideThrustIsOn)
        {
            audioSource.Stop();
        }
        rightThruster.Stop();
        leftThruster.Stop();
        sideThrustIsOn = false;
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing physics rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing the rotation so the physics rotation takes over
    }

    void RotateRight()
    {
        sideThrustIsOn = true;

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(sideThrusters);
        }

        if (!leftThruster.isPlaying)
        {
            leftThruster.Play();
        }
        ApplyRotation(-rotationThrust);
    }

    void RotateLeft()
    {
        sideThrustIsOn = true;

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(sideThrusters);
        }

        if (!rightThruster.isPlaying)
        {
            rightThruster.Play();
        }

        ApplyRotation(rotationThrust);
    }

}

