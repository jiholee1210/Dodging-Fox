using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set;}
    
    [SerializeField] GameObject[] obstacles;

    private string objectName;
    private int size = 3;
    private Dictionary<string, IObjectPool<GameObject>> poolQueue = new Dictionary<string, IObjectPool<GameObject>>();

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        Init();
    }
    
    private void Init() {
        for(int i = 0; i < obstacles.Length; i++) {
            string prefabName = obstacles[i].name;
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                () => CreatePooledItem(prefabName), OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, size, size);
            poolQueue.Add(prefabName, pool);
            Debug.Log("풀 생성" + prefabName + pool);

            for (int j = 0; j < size; j++) {
                Debug.Log("게임 오브젝트 미리 생성 및 비활성화" + j + prefabName);
                GameObject pooledObject = CreatePooledItem(prefabName); // 오브젝트를 가져와서 활성화
                pool.Release(pooledObject); // 즉시 풀에 반환하여 비활성화 상태로 유지
            }
        }
    }

    private GameObject CreatePooledItem(string name) {
        GameObject prefab = System.Array.Find(obstacles, obj => obj.name == name);

        if(prefab != null) {
            GameObject poolGo = Instantiate(prefab);
            poolGo.GetComponent<ObjectFall>().pool = poolQueue[name];
            return poolGo;
        }
        return null;
    }

    private void OnTakeFromPool(GameObject poolGo) {
        poolGo.SetActive(true);
    }

    public void OnReturnedToPool(GameObject poolGo) {
        poolGo.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject poolGo) {
        Destroy(poolGo);
    }

    public GameObject GetPooledObject(string name) {
        Debug.Log("오브젝트 이름" + name);
        if(poolQueue.ContainsKey(name)) {
            Debug.Log("오브젝트 이름" + name);
            IObjectPool<GameObject> pool = poolQueue[name];
            Debug.Log("오브젝트 풀" + pool);
            return pool.Get();
        }
        return null;
    }
}
