using UnityEngine;

public class Player : MonoBehaviour
{
    private const float GROUND_CHECK_DISTANCE = 0.7f;
    //private const float WALL_CHECK_DISTANCE = 0.2f;
    public float speed = 5;
    public float jumpUp = 1;
    public Vector3 direction;

    Animator pAnimator;
    Rigidbody2D pRig2D;
    SpriteRenderer sp;
    public GameObject Jdust;
    public GameObject wallDust;

    [Header("벽점프")]
    public Transform wallChk;  // 벽 위치
    public float wallchkDistance; // 체크 거리
    public LayerMask wLayer; // 벽 레이어마스크
    private bool isWall; // 현재 벽에 붙어있는지 상태 체크
    public float slidingSpeed; //벽에 붙었을때 미끄럼 속도
    public float wallJumpPower; // 벽점프에 사용할 추진력
    public bool isWallJump; // 벽점프 진행 중인지 여부 
    float isRight = 1; // 플레이어가 바라보는 방향 오 : 1 왼 : -1
    void Start()
    {
        pAnimator = GetComponent<Animator>(); //애니메이터 컴포넌트 가져오기
        pRig2D = GetComponent<Rigidbody2D>(); //리지드바디2D 컴포넌트 가져오기
        sp = GetComponent<SpriteRenderer>(); //스프라이트렌더러 컴포넌트 가져오기
        direction = Vector2.zero; //방향 초기화
    }

    void KeyInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal"); // -1 0 1

        if(direction.x <0)
        {
            //left
            sp.flipX = true;
            pAnimator.SetBool("Run", true);
            isRight = -1;
        }
        else if(direction.x >0)
        {
            //right
            sp.flipX = false;
            pAnimator.SetBool("Run", true);
            isRight = 1;
        }
        else if(direction.x == 0)
        {
            pAnimator.SetBool("Run", false);
        }
    }
    void Update()
    {
        KeyInput();
        Move();
        if(Input.GetKeyDown(KeyCode.W))
        {
            //점프 입력 (착지 상태일 때만)
            if(pAnimator.GetBool("Jump") == false)
            {
                Jump();
                if(!isWall)
                {
                    Instantiate(Jdust, transform.position, Quaternion.identity);
                }
            }
        }
        // 벽검사
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallchkDistance, wLayer);
        pAnimator.SetBool("Grab", isWall);
        if(isWall)
        {
            isWallJump = false;
            pRig2D.linearVelocity = new Vector2(pRig2D.linearVelocityX, pRig2D.linearVelocityY * slidingSpeed);
            if(Input.GetKeyDown(KeyCode.W))
            {
                isWallJump = true;
                GameObject go = Instantiate(wallDust, transform.position + new Vector3(-0.8f * isRight, 0, 0), Quaternion.identity);
                go.GetComponent<SpriteRenderer>().flipX = !sp.flipX;
                Invoke("FreezeX", 0.3f);
                pRig2D.linearVelocity = new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower);
                sp.flipX = sp.flipX == false ? true : false;
                isRight = -isRight; 
            }
        }
    }
    void FreezeX()
    {
        isWallJump = false;
    }
    public  void Jump()
    {
        //점프 전 속도 초기화
        pRig2D.linearVelocity = Vector2.zero;

        //점프 힘 적용
        pRig2D.AddForce(new Vector2(0, jumpUp), ForceMode2D.Impulse);
    }
    void Move()
    {
        //이동
        transform.position += direction * speed * Time.deltaTime;

        //pRig2D.linearVelocity = direction * speed;

    }
    private void FixedUpdate()
    {
        Debug.DrawRay(pRig2D.position, Vector3.down, new Color(0, GROUND_CHECK_DISTANCE, 0));

        //바닥 레이어로 레이캐스트
        RaycastHit2D rayHit = Physics2D.Raycast(pRig2D.position, Vector3.down, GROUND_CHECK_DISTANCE, LayerMask.GetMask("Ground"));
        CheckGroundedState(rayHit);
    }
    void CheckGroundedState(RaycastHit2D rayHit)
    {
        bool isGrounded = rayHit.collider != null && rayHit.distance < GROUND_CHECK_DISTANCE;

        if(isGrounded)
        {
            //착지 상태
            pAnimator.SetBool("Jump", false);
        }
        else
        {
            if(!isWall)
            {
                pAnimator.SetBool("Jump", true);
            }
            else
            {
                pAnimator.SetBool("Grab", true);
            }
        }
    }

    public void RandDust(GameObject dust)
    {
        // GameObject go = Instantiate(dust, transform.position + new Vector3(-0.114f, -0.467f,0), Quaternion.identity);
        // Destroy(go, 1f);
        // 위에처럼 해도되고, 아니면  dust 오브젝트에 Dust.cs 넣어서 그냥 삭제로직 관리해도 될듯/
        Instantiate(dust, transform.position + new Vector3(-0.114f, -0.467f,0), Quaternion.identity);
    }

}