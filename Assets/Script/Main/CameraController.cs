using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Vector3 playerPos;
    private float speed = 300f;
    private float mouseInputX;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = player.transform.position;

        //---�}�E�X�J�[�\��-----------------
        Cursor.visible = false;                     //�J�[�\����\��
        Cursor.lockState = CursorLockMode.Locked;   //�J�[�\���𒆉��ɌŒ�
        //--------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += player.transform.position - playerPos;
        playerPos = player.transform.position;

        mouseInputX = Input.GetAxis("Mouse X");
        transform.RotateAround(playerPos, Vector3.up, mouseInputX * Time.deltaTime * speed);
    }
}
