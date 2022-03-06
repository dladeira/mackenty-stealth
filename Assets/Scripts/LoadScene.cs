using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [Scene] [SerializeField] string sceneToLoad; 

    public void LoadSelectedScene() {
        SceneManager.LoadScene(sceneToLoad);
    }
}
