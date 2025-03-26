using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")  //충돌이 일어난 물체의 Tag가 Ground 경우
        {
            Debug.Log("땅과 충돌"); //디버그 로그를 쓴다.
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거 안에 들어옴");
    }
}
