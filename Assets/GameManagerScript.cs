using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject ClearPrefab;

    public GameObject clearText;
    //配列の宣言
    int[,] map;//レベルデザイン用の配列
    GameObject[,] field;//ゲームデザイン用の配列

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

        //配列の実態の作成と初期化
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


    //文字の出力
    void PrintArray()
    {
        //文字列の宣言と初期化
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
        //    debugText += "\n";//改行
        //}
        Debug.Log(debugText);
    }

    ////プレイヤーの位置の取得
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

        ////プレイヤーの移動処理
        bool MovePlayer(string tagName, Vector2Int moveFrom, Vector2Int moveTo) {
            //先に動けない条件を先に描き、リターンする
            if (moveTo.y < 0 || moveTo.y >= map.GetLength(0)) { return false; }
            if (moveTo.x < 0 || moveTo.x >= map.GetLength(1)) { return false; }
            //移動先に箱がいた場合
            if (field[moveTo.y, moveTo.x] != null && field[moveTo.y,moveTo.x].tag=="Box")
            {
                //どの方向へ移動するかを検出
                Vector2Int velocity = moveTo - moveFrom;

                //プレイヤーの移動先から、さらに先へ箱を移動させる
                //箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを呼び、処理が再帰している。移動可不可をboolで記録
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
        //    //if (Input.GetKeyDown(KeyCode.RightArrow)) {/*右キーを入力した場合*/ }
        //    //if (Input.GetKeyDown(KeyCode.UpArrow)) {/*上キーを入力した場合*/ }
        //    //if (Input.GetKeyDown(KeyCode.Space)) {/*スペースを入力した場合*/ }
        //    //if (Input.GetKeyDown(KeyCode.Return)) {/*エンターを入力した場合*/ }
        //    //if (Input.GetKeyDown(KeyCode.KeypadEnter)) {/*テンキーのエンターを入力した場合*/ }
        //    //if (Input.GetKeyDown(KeyCode.Alpha0)) {/*数字キーを入力した場合*/ }
        //    //if (Input.GetKeyDown(KeyCode.Keypad0)) {/*テンキーの数字キーを入力した場合*/ }

        //    //if (Input.GetKey(KeyCode.Space)) {/*スペースを押している間*/ }
        //    //if (Input.GetKeyDown(KeyCode.Space)) {/*スペースを押した瞬間*/ }
        //    //if (Input.GetKeyUp(KeyCode.Space)) {/*スペースを離した瞬間*/ }

        //見つからなかった時のために-1で初期化
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


