using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        // foward(z��)�̕��������邱�Ƃŕ��������]����̂��C��
        transform.localScale = new Vector3(-1, 1, 1);
    }

    void Update()
    {
        // ���g�̌������J�����Ɍ�����
        transform.LookAt(Camera.main.transform);
    }
}
