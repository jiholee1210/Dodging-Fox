using UnityEngine;
using UnityEngine.Pool;

public class ObjectFall : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    [SerializeField] private float rotationForce = 10f;
    [SerializeField] private float bounceForce = 2f;
    [SerializeField] private LayerMask hitLayer;

    public IObjectPool<GameObject> pool {get; set;}
     // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Die();   
    }

    private void SetTorque() {
        float direction = Random.value < 0.5f ? -1f : 1f;
        Debug.Log("토그 적용됨" + gameObject.name + " " + direction);
        rigidbody2D.AddTorque(rotationForce * direction, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(((1 << other.gameObject.layer) & hitLayer) != 0) {
            if(other.gameObject.CompareTag("Player"))  // 플레이어 태그 확인
            {
                // 플레이어에게 충돌 이벤트를 보내고
                other.gameObject.GetComponent<PlayerMovement>()?.OnHit(transform.position);
            }
            rigidbody2D.linearVelocityY = 0f;
            rigidbody2D.AddForceY(bounceForce, ForceMode2D.Impulse);
            gameObject.layer = LayerMask.NameToLayer("Disabled");
        }
    }

    public void Init(Vector3 spawnPos) {
        transform.position = spawnPos;
        transform.rotation = Quaternion.identity;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.linearVelocity = Vector2.zero; // 속도 초기화
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        SetTorque();
    }

    private void Die() {
        if(transform.position.y < -8f) {
            Debug.Log("오브젝트 풀링 릴리즈");
            pool.Release(gameObject);
        }
    }
}
