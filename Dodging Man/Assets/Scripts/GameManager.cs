using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private TextManager textManager;

    private bool isGameOver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        textManager = GetComponent<TextManager>();
        isGameOver = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isGameOver) {
            textManager.CalTime();
        }
    }

    void Update() {
        if(isGameOver && Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("스페이스 누름");
            isGameOver = false;
            textManager.SetDefault();
            Time.timeScale = 1;
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void EndGame() {
        isGameOver = true;
        Time.timeScale = 0;
        textManager.SetBestTime();
        textManager.EndGameText();
    }
}
