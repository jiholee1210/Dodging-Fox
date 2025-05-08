using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] float spawnSpeed;

    string[] objectName = {"Bear", "Bettle", "Frog", "Rabbit"};
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GenTimer());
    }

    private IEnumerator GenTimer() {
        while(true) {
            GenObject();
            yield return new WaitForSeconds(spawnSpeed);
        }
    }

    private void GenObject() {
        int randomIndex = Random.Range(0, objectName.Length);
        string prefabName = objectName[randomIndex];
        Debug.Log("오브젝트 랜덤 선택 이름" + prefabName);

        GameObject obstacle = ObjectPoolManager.Instance.GetPooledObject(prefabName);
        Debug.Log("오브젝트 풀링 후 생성" + obstacle);
        if(obstacle != null) {
            float xPos = Random.Range(-8.5f, 8.5f);
            Vector3 randomPos = new Vector3(xPos, 8f, 0);
            
            obstacle.GetComponent<ObjectFall>().Init(randomPos);
        }
    }
}
