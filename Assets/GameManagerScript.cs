using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //�z��̐錾
    int[] map;

    //�����̏o��
    void PrintArray()
    {
        //������̐錾�Ə�����
        string debugText = "";

        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ",";
        }

        Debug.Log(debugText);
    }

    //�v���C���[�̈ʒu�̎擾
    int GetPlayerIndex()
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        return -1;
    }

    //�v���C���[�̈ړ�����
    bool MoveNumber(int number,int moveFrom,int moveTo) {
        //��ɓ����Ȃ��������ɕ`���A���^�[������
        if (moveTo < 0 || moveTo >= map.Length) { return false; }
        //�ړ���ɔ��������ꍇ
        if (map[moveTo] == 2) {
            //�ǂ̕����ֈړ����邩�����o
            int velocity = moveTo - moveFrom;

            //�v���C���[�̈ړ��悩��A����ɐ�֔����ړ�������
            //���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h���ĂсA�������ċA���Ă���B�ړ��s��bool�ŋL�^
            bool success = MoveNumber(2, moveTo, moveTo + velocity);

            if (!success) { return false; }
        }


        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    
    }

    

    // Start is called before the first frame update
    void Start()
    {
        //�z��̎��Ԃ̍쐬�Ə�����
        map = new int[] { 0, 0, 2, 1, 0, 2, 0, 0, 0 };

        PrintArray();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.RightArrow)) {/*�E�L�[����͂����ꍇ*/ }
        //if (Input.GetKeyDown(KeyCode.UpArrow)) {/*��L�[����͂����ꍇ*/ }
        //if (Input.GetKeyDown(KeyCode.Space)) {/*�X�y�[�X����͂����ꍇ*/ }
        //if (Input.GetKeyDown(KeyCode.Return)) {/*�G���^�[����͂����ꍇ*/ }
        //if (Input.GetKeyDown(KeyCode.KeypadEnter)) {/*�e���L�[�̃G���^�[����͂����ꍇ*/ }
        //if (Input.GetKeyDown(KeyCode.Alpha0)) {/*�����L�[����͂����ꍇ*/ }
        //if (Input.GetKeyDown(KeyCode.Keypad0)) {/*�e���L�[�̐����L�[����͂����ꍇ*/ }

        //if (Input.GetKey(KeyCode.Space)) {/*�X�y�[�X�������Ă����*/ }
        //if (Input.GetKeyDown(KeyCode.Space)) {/*�X�y�[�X���������u��*/ }
        //if (Input.GetKeyUp(KeyCode.Space)) {/*�X�y�[�X�𗣂����u��*/ }

        //������Ȃ��������̂��߂�-1�ŏ�����
        int playerIndex = GetPlayerIndex();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveNumber(1, playerIndex, playerIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveNumber(1,playerIndex, playerIndex - 1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PrintArray();
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            PrintArray();
        }
    }
}
