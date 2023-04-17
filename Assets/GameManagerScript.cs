using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //配列の宣言
    int[] map;

    //文字の出力
    void PrintArray()
    {
        //文字列の宣言と初期化
        string debugText = "";

        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ",";
        }

        Debug.Log(debugText);
    }

    //プレイヤーの位置の取得
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

    //プレイヤーの移動処理
    bool MoveNumber(int number,int moveFrom,int moveTo) {
        //先に動けない条件を先に描き、リターンする
        if (moveTo < 0 || moveTo >= map.Length) { return false; }
        //移動先に箱がいた場合
        if (map[moveTo] == 2) {
            //どの方向へ移動するかを検出
            int velocity = moveTo - moveFrom;

            //プレイヤーの移動先から、さらに先へ箱を移動させる
            //箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを呼び、処理が再帰している。移動可不可をboolで記録
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
        //配列の実態の作成と初期化
        map = new int[] { 0, 0, 2, 1, 0, 2, 0, 0, 0 };

        PrintArray();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.RightArrow)) {/*右キーを入力した場合*/ }
        //if (Input.GetKeyDown(KeyCode.UpArrow)) {/*上キーを入力した場合*/ }
        //if (Input.GetKeyDown(KeyCode.Space)) {/*スペースを入力した場合*/ }
        //if (Input.GetKeyDown(KeyCode.Return)) {/*エンターを入力した場合*/ }
        //if (Input.GetKeyDown(KeyCode.KeypadEnter)) {/*テンキーのエンターを入力した場合*/ }
        //if (Input.GetKeyDown(KeyCode.Alpha0)) {/*数字キーを入力した場合*/ }
        //if (Input.GetKeyDown(KeyCode.Keypad0)) {/*テンキーの数字キーを入力した場合*/ }

        //if (Input.GetKey(KeyCode.Space)) {/*スペースを押している間*/ }
        //if (Input.GetKeyDown(KeyCode.Space)) {/*スペースを押した瞬間*/ }
        //if (Input.GetKeyUp(KeyCode.Space)) {/*スペースを離した瞬間*/ }

        //見つからなかった時のために-1で初期化
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
