using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinCollider : MonoBehaviour
{

    [SerializeField] string playerName;
    [Scene] [SerializeField] string newScene;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == playerName) {
        SceneManager.LoadScene(newScene);
        }
    }
}
