using UnityEngine;

public class HpManager : MonoBehaviour
{
    public static HpManager Instance { get; private set;}

    [SerializeField] private GameObject hpObject;
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void UpdateHp(int hp) {
        hpObject.transform.GetChild(hp).gameObject.SetActive(false);
    }

    public void ResetHp() {
        for(int i = 0; i < 3; i++) {
            hpObject.transform.GetChild(i).gameObject.SetActive(true);
        } 
    }
}
