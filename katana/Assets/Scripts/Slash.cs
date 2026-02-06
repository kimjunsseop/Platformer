using UnityEngine;

public class Slash : MonoBehaviour
{
    GameObject player;
    Vector2 MousePos;
    Vector3 dir;
    float angle;
    Vector3 dirNo;
    public Vector3 direction = Vector3.right;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기
        Transform tr = player.GetComponent<Transform>();
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스는 기본 스크린 좌표이므로, 마우스와 플레이어 사이의 벡트를 알아야하므로 월드로 변환해야함
        Vector3 pos = new Vector3(MousePos.x, MousePos.y, 0); // Vector2 -> Vector3 변환. 플레이어 좌표와의 계산을 위한 포멧 변경같은 느낌
        dir = pos - tr.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        transform.position = player.transform.position;
    }

    public void Des()
    {
        Destroy(gameObject);
    }
}
