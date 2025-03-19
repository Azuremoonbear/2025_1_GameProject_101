using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100; //체력을 선언 한다. (정수)
    public float Timer = 1.0f; //타이머를 선언 한다.
    public int Attackpoint = 10; //공격력을 선언 한다.

    // Start is called before the first frame update
    void Start()
    {
        Health += 100; //첫 프레임 이전에 실핼될때 100체력을 추가 시켜 준다.
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime; //시간을 매 프레임마다 감소 시킨다. (deltaTime 프레임간의 시간 간격을 의미한다.)

        if(Timer <= 0) //만약 타이머의 시간이 0이하로 내려갈 경우
        {
            Timer = 1.0f; //다시 1초로 변경시켜준다.
            Health += 20; //1초마다 체력 20을 올려준다.
        }

        if (Input.GetKeyDown(KeyCode.Space)) //스페이스 키를 눌렀을 때
        {
            CharacterHit(Attackpoint);
        }

        CheckDeath(); //함수 호출

        if (Input.GetKeyDown(KeyCode.Space)) //스페이스 키를 눌렀을 때
        {
            Health -= Attackpoint; //체력 포인트를 공격 포인트 만큼 감소 시켜준다. (health = Health - AttackPoint)
        }

        if (Health <= 0) //체력이 0이하로 내려가면 파괴 시킨다.
            Destroy(gameObject); //이 오브젝트를 파괴 한다. 
    }

    void Update()
    {
        CharacterHealthUp();
        CheckDeath(); //함수 호출
    }

    void CharacterHealthUp()
    {
        Timer -= Time.deltaTime; //시간을 매 프레임마다 감소 시킨다. (deltaTime 프레임의 시간 간격을 의미한다.)

        if (Timer <= 0)//만약 Timer의 수치가 0이하로 내려갈 경우 
        {
            Timer = 1.0f; //다시 1초로 변경 시켜준다.
            Health += 20; //1초마다 체력 20을 올려준다. (Health = Health + 20)
        }
    }

    public CharacterHit(int Damage) //커스텀 데미지를 받는 함수를 사용한다.
    {
        Health -= Damage; //받은 공격력에 대한 체력을 감소 시킨다.
    }

    void CheckDeath()
    {
        if (Health <= 0)
            Destroy(gameObject);
    }
     
}
