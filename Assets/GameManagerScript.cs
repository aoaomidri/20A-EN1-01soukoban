using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject ClearPrefab;

    public GameObject clearText;
    //�z��̐錾
    int[,] map;//���x���f�U�C���p�̔z��
    GameObject[,] field;//�Q�[���f�U�C���p�̔z��

    bool IsCleard()
    {
        List<Vector2Int>goals = new List<Vector2Int>();
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag!="Box") {

                return false;
            }
        }

        return true;
    }


    // Start is called before the first frame update
    void Start()
    {
        //GameObject instance = Instantiate(
        //    boxPrefab,
        //    new Vector3(0, 0, 0),
        //    Quaternion.identity
        //    );

        //�z��̎��Ԃ̍쐬�Ə�����
        map = new int[,]{
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 3, 0, 3, 0, 0, 0},
            { 0, 0, 0, 2, 1, 2, 0, 0, 0},
            { 0, 0, 0, 0, 2, 0, 0, 0, 0},
            { 0, 0, 0, 0, 3, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };

        field = new GameObject
        [
            map.GetLength(0),
            map.GetLength(1)
        ];






        //for (int y = 0; y < map.GetLength(0); y++)
        //{
        //    for (int x = 0; x < map.GetLength(1); x++)
        //    {
        //        if (map[y, x] == 1)
        //        {
        //            GameObject instance = Instantiate(
        //                playerPrefab,
        //                new Vector3(x,  map.GetLength(0) - y, 0),
        //                Quaternion.identity
        //                ) ;
        //        }
        //    }
        //}

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }
                if (map[y, x] == 3)
                {
                    GameObject instance = Instantiate(
                        ClearPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0.01f),
                        Quaternion.identity
                        );
                }
            }


        }
    }


    //�����̏o��
    void PrintArray()
    {
        //������̐錾�Ə�����
        string debugText = "";
        if (IsCleard()){
            debugText += "Clear!";
        }

        //for (int y = 0; y < map.GetLength(0); y++)
        //{
        //    for (int x = 0; x < map.GetLength(1); x++)
        //    {
        //        debugText += map[y, x].ToString() + ",";
        //    }
        //    debugText += "\n";//���s
        //}
        Debug.Log(debugText);
    }

    ////�v���C���[�̈ʒu�̎擾
    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] != null && field[y, x].tag == "Player") { return new Vector2Int(x, y); }
            }
        }
        return new Vector2Int(-1, -1);
    }

    //Vector2Int GetBoxIndex()
    //{
    //    for (int y = 0; y < field.GetLength(0); y++)
    //    {
    //        for (int x = 0; x < field.GetLength(2); x++)
    //        {
    //            if (field[y, x] != null && field[y, x].tag == "Box") { return new Vector2Int(x, y); }
    //        }
    //    }
    //    return new Vector2Int(-1, -1);
    //}

        ////�v���C���[�̈ړ�����
        bool MovePlayer(string tagName, Vector2Int moveFrom, Vector2Int moveTo) {
            //��ɓ����Ȃ��������ɕ`���A���^�[������
            if (moveTo.y < 0 || moveTo.y >= map.GetLength(0)) { return false; }
            if (moveTo.x < 0 || moveTo.x >= map.GetLength(1)) { return false; }
            //�ړ���ɔ��������ꍇ
            if (field[moveTo.y, moveTo.x] != null && field[moveTo.y,moveTo.x].tag=="Box")
            {
                //�ǂ̕����ֈړ����邩�����o
                Vector2Int velocity = moveTo - moveFrom;

                //�v���C���[�̈ړ��悩��A����ɐ�֔����ړ�������
                //���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h���ĂсA�������ċA���Ă���B�ړ��s��bool�ŋL�^
                bool success = MovePlayer(tag, moveTo, moveTo + velocity);

                if (!success) { return false; }
            }

            field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
            field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
            field[moveFrom.y, moveFrom.x] = null;
            return true;

         }

    //// Update is called once per frame
    void Update()
    {
        //    //if (Input.GetKeyDown(KeyCode.RightArrow)) {/*�E�L�[����͂����ꍇ*/ }
        //    //if (Input.GetKeyDown(KeyCode.UpArrow)) {/*��L�[����͂����ꍇ*/ }
        //    //if (Input.GetKeyDown(KeyCode.Space)) {/*�X�y�[�X����͂����ꍇ*/ }
        //    //if (Input.GetKeyDown(KeyCode.Return)) {/*�G���^�[����͂����ꍇ*/ }
        //    //if (Input.GetKeyDown(KeyCode.KeypadEnter)) {/*�e���L�[�̃G���^�[����͂����ꍇ*/ }
        //    //if (Input.GetKeyDown(KeyCode.Alpha0)) {/*�����L�[����͂����ꍇ*/ }
        //    //if (Input.GetKeyDown(KeyCode.Keypad0)) {/*�e���L�[�̐����L�[����͂����ꍇ*/ }

        //    //if (Input.GetKey(KeyCode.Space)) {/*�X�y�[�X�������Ă����*/ }
        //    //if (Input.GetKeyDown(KeyCode.Space)) {/*�X�y�[�X���������u��*/ }
        //    //if (Input.GetKeyUp(KeyCode.Space)) {/*�X�y�[�X�𗣂����u��*/ }

        //������Ȃ��������̂��߂�-1�ŏ�����
        Vector2Int playerIndex = GetPlayerIndex();
        //Vector2Int boxIndex = GetBoxIndex();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePlayer(field[playerIndex.y, playerIndex.x].tag, new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x + 1, playerIndex.y));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePlayer(field[playerIndex.y, playerIndex.x].tag, new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x - 1, playerIndex.y));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovePlayer(field[playerIndex.y, playerIndex.x].tag, new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x, playerIndex.y - 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePlayer(field[playerIndex.y, playerIndex.x].tag, new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x, playerIndex.y + 1));
        }

        //    if (Input.GetKeyDown(KeyCode.RightArrow))
        //    {
        //        PrintArray();
        //    }
        //    else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
        //        PrintArray();
        //    }
        if (IsCleard())
        {
            clearText.SetActive(true);
        }
        else
        {
            clearText.SetActive(false);
        }
        
    }
}


