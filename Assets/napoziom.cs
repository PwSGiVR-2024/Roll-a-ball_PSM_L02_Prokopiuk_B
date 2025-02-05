using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneTransitionTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene"; 
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            SceneManager.LoadScene(sceneName);
        }
    }
}