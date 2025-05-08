using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    private Camera mainCamera;
    private EdgeCollider2D edgeCollider2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        edgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
        UpdateBounds();
    }
    
    private void UpdateBounds()
    {
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;
        
        // 카메라 경계에 맞춰 EdgeCollider2D 포인트 설정
        Vector2[] points = new Vector2[5];
        points[0] = new Vector2(-halfWidth, -halfHeight); // 좌하단
        points[1] = new Vector2(-halfWidth, halfHeight);  // 좌상단
        points[2] = new Vector2(halfWidth, halfHeight);   // 우상단
        points[3] = new Vector2(halfWidth, -halfHeight);  // 우하단
        points[4] = points[0]; // 다시 처음으로 (사각형 완성)
        
        edgeCollider2D.points = points;
    }
}
