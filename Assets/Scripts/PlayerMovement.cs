using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5f; //이동 속도 변수 설정
    public float jumpForce = 7f; //점프의 힘 값을 준다.
    public float turnSpeed = 10f; //회전 속도

    [Header("점프 개선 설정")]
    public float fallMultiplier = 2.5f; //하강 중력 배율
    public float lowjumpMultipier = 2.0f; //짧은 점프 배율

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f; //지면 관성 시간
    public float coyoteTimeCounter; //관성 타이머
    public bool realGround = true; //실제 지면 상태

    [Header("글라이더 설정")]
    public GameObject gliderObject;         //글라이더 오브젝트
    public float gliderFallSpeed = 1.0f;    //글라이더 낙하 속도
    public float gliderMoveSpeed = 7.0f;    //글라이더 이동 속도
    public float gliderMaxTime = 5.0f;      //최대 사용 시간
    public float gliderTimeLeft;            //남은 사용 시간
    public bool isGliding = false;          //글라이딩 중인지 여부

    public bool isGrounded = true; //땅에 있는지 체크 하는 변수 (true/false)

    public int coinCount = 0; //코인 획득 필요 변수 선언
    public int totalCoins = 5; //총 코인 획득 필요 변수 선언

    public Rigidbody rb; //플레이어 강체를 선언

    // Start is called before the first frame update
    void Start()
    {
        if (gliderObject != null)           //글라이더 오브젝트 초기화
        {
            gliderObject.SetActive(false);  //시작 시 비활성화
        }

        gliderTimeLeft = gliderMaxTime;     //글라이더 시간 초기화

        coyoteTimeCounter = 0;
    }

    //글라이더 활성화 함수

    void EnableGlider()
    {
        isGliding = true;

        //글라이더 오브젝트 표시
        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        //하강 속도 초기화
        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);
    }

    //글라이더 비활성화 함수
    void DisalbeGlider()
    {
        isGliding = false;

        //글라이더 오브젝트 숨기기
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        //즉시 낙하하도록 중력 작용
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    //글라이더 이동 착용
    void ApplyGliderMovement(float horizontal, float vertical)
    {
        //
        Vector3 gliderVelocity = new Vector3(
            horizontal * gliderMoveSpeed,
            -gliderFallSpeed,
            vertical * gliderMoveSpeed
        );

        rb.velocity = gliderVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        //지면 감지 안정화
        UpdateGroundState();

        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //이동 방향 벡터
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical); //이동 방향 감지

        if (movement.magnitude > 0.1f) //입력이 있을때만 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement); //바라보는 방향으로 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0) //G키를 누르면서 땅에 있지 않고 글라이더 남은 시간에 있을 때 (3가지 조건)
        {
            if(!isGliding) //글라이더 활성화 (누르고 있는 동안)
            {
                //글라이더 활성화 함수 (아래 정의)
                EnableGlider();
            }

            //글라이더 사용 시간 감소
            gliderTimeLeft -= Time.deltaTime;

            //글라이더 시간이 다 되면 비활성화
            if(gliderTimeLeft <= 0)
            {
                //글라이더 비활성화 함수 (아래 정의)
                DisalbeGlider();
            }
        }
        else if (isGliding)
        {
            //G키를 떼면 글라이더 비활성화
            DisalbeGlider();
        }

        if(isGliding) //움직임 처리
        {
            ApplyGliderMovement(moveHorizontal, moveVertical); //글라이더 사용 중 이동
        }
        else //(기존 움직임 코드들을 else문 안으로 넣는다.)
        {
            //속도로 직접 이동
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            //착시 점프 높이 구현
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowjumpMultipier - 1) * Time.deltaTime;
            }
        }

        //속도로 직접 이동
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //착시 점프 높이 구현
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime; //하강 시 중력 강화
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump")) //상승 중 점프 버튼을 떼면 낮게 점프
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowjumpMultipier - 1) * Time.deltaTime;
        }

        //점프 입력
        if (Input.GetButtonDown("Jump")&& isGrounded) //&& 두 값을 만족할때 -> (스페이스 버튼을 눌렀을 때 와 IsGrounded 가 True 일때)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //위쪽으로 설정한 힘만큼 강체에 준다.
            isGrounded = false; //점프를 하는 순간 땅에서 떨어졌기 때문에 false라고 해준다.
            realGround = false;
            coyoteTimeCounter = 0; //코요테 타일 즉시 리셋
        }

        //지면에 있으면 글라이더 시간 회복 및 글라이더 비활성화
        if(isGrounded)
        {
            if(isGliding)
            {
                DisalbeGlider();
            }

            //지상에 있을 때 시간 회복
            gliderTimeLeft = gliderMaxTime;
        }
    }


    void OnCollisionEnter(Collision collision) //충돌 처리 함수 (변경)
    {
        if (collision.gameObject.CompareTag("Ground")) //충돌이 일어난 물체의 Tag가 Ground 경우
        {
            isGrounded = true; //땅과 충돌하면 ture로 변경한다.
        }
    }

    private void OnCollisionStay(Collision collision) //지면과의 충돌이 유지되는지 확인 (추가)
    {
        if (collision.gameObject.CompareTag("Ground")) //충돌이 유지되는 물체의 Tag가 Ground 경우
        {
            realGround = true; //충돌이 유지되기 때문에 true
        }
    }

    private void OnCollisionExit(Collision collision) //지면에서 떨어졌는지 확인 (추가)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGround = false; //지면에서 떨어졌기 때문에 false
        }
    }


    private void OnTriggerEnter(Collider other) //트리거 영역 안에 들어왔나를 검사하는 함수
    {
        //코인 수집
        if (other.CompareTag("Coin")) //코인 트리거와 충돌 하면
        {
            coinCount++; //coinCount = coinCount + 1 코인 변수 1을 올려준다.
            Destroy(other.gameObject); //코인 오브젝트를 없앤다.
            Debug.Log($"코인 수집 : {coinCount}/{totalCoins}");
        }

        //목적지 도착 시 종료 로그 출력
        if(other.gameObject.tag == "Door" && coinCount == totalCoins) //모든 코인을 획득 후 문으로 가면 게임 종료
        {
            Debug.Log("게임 클리어");
            //게임 완료 로직 추가 가능
        }
    }

    //지면 상태 업데이트 함수
    void UpdateGroundState()
    {
        if (realGround)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
}
