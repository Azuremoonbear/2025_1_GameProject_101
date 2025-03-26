using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100; //ü���� ���� �Ѵ�. (����)
    public float Timer = 1.0f; //Ÿ�̸Ӹ� ���� �Ѵ�.
    public int Attackpoint = 10; //���ݷ��� ���� �Ѵ�.

    // Start is called before the first frame update
    void Start()
    {
        Health += 100; //ù ������ ������ ���۵ɶ� 100ü���� �߰� ���� �ش�.
    }

   

    void Update()
    {
        CharacterHealthUp();
        CheckDeath(); //�Լ� ȣ��
    }

    void CharacterHealthUp()
    {
        Timer -= Time.deltaTime; //�ð��� �� �����Ӹ��� ���� ��Ų��. (deltaTime �������� �ð� ������ �ǹ��Ѵ�.)

        if (Timer <= 0)//���� Timer�� ��ġ�� 0���Ϸ� ������ ��� 
        {
            Timer = 1.0f; //�ٽ� 1�ʷ� ���� �����ش�.
            Health += 20; //1�ʸ��� ü�� 20�� �÷��ش�. (Health = Health + 20)
        }
    }

    public void CharacterHit(int Damage) //Ŀ���� �������� �޴� �Լ��� ����Ѵ�.
    {
        Health -= Damage; //���� ���ݷ¿� ���� ü���� ���� ��Ų��.
    }

    void CheckDeath()
    {
        if (Health <= 0)
            Destroy(gameObject);
    }
     
}
