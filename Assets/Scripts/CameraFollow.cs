using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;

    private void LateUpdate()   //ī�޶� �������� ���� LateUpdate ���� ó��
    {
        //LateUpdate()�� ����ϴ� ������ ī�޶� �÷��̾��� �̵��� ��� ó���� ���Ŀ� ���󰡰� �ϱ� ����

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); //���� ��ġ ����
        transform.position = smoothPosition; //���� ������Ʈ ��ġ�� ����ش�

        transform.LookAt(transform.position); //ī�޶� �׻� �÷��̾ �ٶ󺸵��� ����
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
