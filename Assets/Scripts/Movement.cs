using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem leftThruster;
    [SerializeField] ParticleSystem rightThruster;
    [SerializeField] ParticleSystem mainThruster;

    Rigidbody rb;
    AudioSource audioSource;

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
        else
        {
            audioSource.Stop();
            mainThruster.Stop();
        }
    }
    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!rightThruster.isPlaying)
            {
                rightThruster.Play();
            }

            ApplyRotation(rotationThrust); 
        }

        else if (Input.GetKey(KeyCode.D))
        { 
           if (!leftThruster.isPlaying)
            {
                leftThruster.Play();
            }
            ApplyRotation(-rotationThrust);
        }
        
        else
        {
            rightThruster.Stop();
            leftThruster.Stop();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing physics rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing the rotation so the physics rotation takes over
    }
}

