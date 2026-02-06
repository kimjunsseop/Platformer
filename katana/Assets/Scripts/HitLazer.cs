using System.Data;
using UnityEngine;

public class HitLazer : MonoBehaviour
{
    [SerializeField]
    private float Speed = 50f;
    Vector2 MousePos;
    Transform tr;
    Vector3 dir;

    float angle;
    Vector3 dirNo;


    void Start()
    {

        tr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        MousePos = Input.mousePosition;
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector3 Pos = new Vector3(MousePos.x, MousePos.y, 0);
        dir = Pos - tr.position; //마우스 - 플레이어 포지션 빼면 마우스를 바라보는 벡터

        //바라보는 각도구하기
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        //normalized 단위벡터
        dirNo = new Vector3(dir.x, dir.y, 0).normalized;

        Destroy(gameObject, 4f);
    }

    
    void Update()
    {

        //사원수(Quaternion)를 써야 하는 이유는 크게 깜빡임(Gimbal Lock) 방지,
        //부드러운 회전(Interpolation), 안정적인 연산이라는 세 가지 핵심 이유가 있어.
        //🔥 1.깜빡임(Gimbal Lock) 방지
        //** 깜빡임(Gimbal Lock)**은 오일러 각(Euler Angles)로 회전을 표현할 때
        //특정한 각도에서 축 하나가 잠겨버려서 회전이 제대로 안 되는 문제야.
        //🔹 예제: 유니티에서 X축을 90도 회전하면...?
        //오일러 각으로(90, 0, 0)을 적용하면, Y축과 Z축이 정렬되면서 Y축 회전이 Z축과 겹쳐버려.
        //→ 결과적으로 Y축을 돌려도 Z축이 같이 돌아가면서 원하는 회전이 불가능해지는 현상이 생겨.
        //✅ 사원수(Quaternion)를 사용하면?
        //4차원 벡터로 회전을 표현해서 깜빡임 없이 부드러운 회전이 가능해!
        //오일러 각처럼 특정한 축이 없어지거나 겹치는 문제가 없음.


        //회전적용
        transform.rotation = Quaternion.Euler(0f, 0f, angle);


        //이동
        transform.position += dirNo * Speed * Time.deltaTime;
    }
}
