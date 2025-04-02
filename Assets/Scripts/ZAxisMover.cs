using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisMover : MonoBehaviour //z������ �̵��ϴ� Ŭ����
{
    public float speed = 5.0f; //�̵� �ӵ�
    public float timer = 5.0f; //Ÿ�̸� ����

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //z�� �������� ������ �̵�
        transform.Translate(0, 0, speed * Time.deltaTime);

        timer = Time.deltaTime; //�ð��� ī��Ʈ �ٿ� �Ѵ�.
        if (timer < 0) //�ð��� ����Ǹ�
        {
            Destroy(gameObject); //�ڱ� �ڽ��� �ı� �Ѵ�.
        }
    }
}
