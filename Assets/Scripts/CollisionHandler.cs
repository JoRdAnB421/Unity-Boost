using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{   
    
    [SerializeField] float levelLoadDelay = 0.5f;
    [SerializeField] AudioClip crashExplosion;
    [SerializeField] AudioClip success;
    
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false;

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning){ return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            
            case "Finish":
                StartNextLevelSequence();

                break;

            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        crashParticles.Play();
        audioSource.PlayOneShot(crashExplosion);

        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }
    void StartNextLevelSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        successParticles.Play();
        audioSource.PlayOneShot(success);

        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // finds current scene index
        int nextSceneIndex = currentSceneIndex + 1; // finds next scene index
        
        // if no next scene then restart game from level 1
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        
        // if there is a next scene, move to the next level
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
