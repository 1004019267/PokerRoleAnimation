using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class ActControl : MonoBehaviour
{
    int btnLength=4;//btn长度
    Button[] btnState;//储存Btn

    string []stateName;//状态名

    List<Image> PlayerImage = new List<Image>();//显示的Image

    List<Player> playerAct = new List<Player>();//玩家类
    int playerLength = 4;  //玩家长度

    System.Random r = new System.Random();
    int[] roleNum;//角色个数

    void Start()
    {
        //初始化数组长度
        roleNum = new int[playerLength];
        btnState = new Button[btnLength];

        //从枚举获取状态名加载Btn
        stateName = Enum.GetNames(typeof(eStateName));

        for (int i = 0; i < stateName.Length; i++)
        {
            btnState[i]=transform.Find("BtnManager/"+stateName[i]).GetComponent<Button>();
        }

        //获取玩家Image
        for (int i = 0; i < playerLength; i++)
        {
            PlayerImage.Add(transform.Find("PlayerManager/Player" + (i + 1).ToString()).GetComponent<Image>());
        }

        InitRole();
        InitPlayer();

         //遍历添加Btn
        foreach (var item in btnState)
        {
            item.onClick.AddListener(() => OnBtn(item.name));
        }       
    }

    /// <summary>
    /// 设置是哪个角色
    /// </summary>  
    void InitRole()
    {
        int num;
        for (int i = 0; i < roleNum.Length; i++)
        {
            num = r.Next(0, 24);
            if (!roleNum.Contains(num))
            {
                roleNum[i] = num;
            }
        }
    }

    /// <summary>
    /// 添加玩家
    /// </summary>  
    void InitPlayer()
    {
        for (int i = 0; i < playerLength; i++)
        {
            if (i <= 1)
            {
                playerAct.Add(new Player(PlayerImage[i], roleNum[i], "l"));
            }
            else
            {
                playerAct.Add(new Player(PlayerImage[i], roleNum[i], "r"));
            }
        }
    }


    void  Update()
    {
        
        for (int i = 0; i < playerAct.Count; i++)
        {
            playerAct[i].WhichState();//启动玩家
        }
    }
    
    /// <summary>
    /// 按键响应
    /// </summary>   
    private void OnBtn(string btnName)
    {        
        for (int i = 0; i < playerAct.Count; i++)
        {           
            playerAct[i].Act((eStateName)Enum.Parse(typeof(eStateName), btnName));
        }
    }
}
