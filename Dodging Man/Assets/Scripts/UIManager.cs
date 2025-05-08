using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;

    private bool isOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            ToggleSetting();
        }
    }

    public void ToggleSetting() {
        isOpen = !isOpen;

        if(isOpen) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }
        
        settingPanel.SetActive(isOpen);
    }
    
    public void ExitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else 
            Application.Quit();
        #endif
    }
}