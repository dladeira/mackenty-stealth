using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menuGameObject;

    public bool pauseMenuOpen { private set; get; }

    void Start()
    {
        TogglePauseMenu(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu(!pauseMenuOpen);
        }
    }

    void TogglePauseMenu(bool open)
    {
        pauseMenuOpen = open;
        menuGameObject.SetActive(open);
    }
}
