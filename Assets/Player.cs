using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
/// <summary>
/// 状态类型
/// </summary>
public enum eStateName
{
    Nomal = 0,
    Amaze,
    Happy,
    Disconnect,
}
/// <summary>
///状态类 
/// </summary>
public class State
{
    public UnityEngine.Object[] state;
    public eStateName stateName;
}

/// <summary>
/// 玩家类
/// </summary>
public class Player
{
    int stateLength = 5;//状态资源个数
    List<State> state = new List<State>(); //状态资源
    eStateName nowState = eStateName.Nomal;//记录当前状态
    int gifNum = 0;

    float lastTime;//上一个切换时间
    int actTimes = 0;//是否第一次切换
    float t;
    float interval = 0.5f;//切换间隔
    float times;//当前可切换次数
    float maxTimes = 20;//最大切换次数

    Image playerLook;//当前玩家Image

    public Player(Image playerLook, int roleNum, string face)
    {
        //遍历设置每一个状态
        for (int i = 0; i < stateLength; i++)
        {
            State sta = new State();
            sta.state = Resources.LoadAll(roleNum.ToString() + "/" + face + i.ToString());
            sta.stateName = (eStateName)i;
            state.Add(sta);
        }
        this.playerLook = playerLook;
    }

    /// <summary>
    /// 设置状态
    /// </summary>   
    void SetState(eStateName nowState)
    {
        this.nowState = nowState;
    }

    /// <summary>
    /// 动作判断
    /// </summary> 
    public void WhichState()
    {
        if (Act(nowState) == 1)
        {
            SetState(eStateName.Nomal);
        }
    }

    /// <summary>
    /// 调用的不同状态
    /// </summary>  
    public int Act(eStateName nowState)
    {
        //Debug.Log(times);
        //切换状态刷新次数
        if (this.nowState != nowState)
        {
            times = maxTimes;
            SetState(nowState);
        }

        //根据枚举类型设置状态
        if (nowState == eStateName.Nomal)
        { 
            return SetAct(state[0].stateName, state[gifNum].state[1], state[gifNum].state[2]);
        }
        else
        {
            for (int i = 0; i < state.Count; i++)
            {
                if (state[i].stateName == nowState + 1)
                {
                    return SetAct(state[i].stateName - 1, state[i].state[1], state[i].state[2]);
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// 状态的切换
    /// </summary>   
    int SetAct(eStateName type, Object state1, Object state2)
    {
        t += Time.deltaTime;
        if (t - lastTime >= interval)
        {          
            switch (actTimes)
            {
                case 0:
                    playerLook.overrideSprite = state1 as Sprite;
                    actTimes = 1;
                    break;
                case 1:
                    playerLook.overrideSprite = state2 as Sprite;
                    actTimes = 0;
                    //设置正常状态gif个数
                    if (gifNum == 0)
                    {
                        gifNum = 1;
                    }
                    else
                    {
                        gifNum = 0;
                    }
                    break;
            }
            lastTime = t;
            return TimesLower(type);
        }
        return 0;
    }
 
    /// <summary>
    /// 判断是否切换次数耗尽
    /// </summary>    
    int TimesLower(eStateName type)
    {
        if (type == eStateName.Amaze || type == eStateName.Happy)
        {
            times--;
            if (times <= 0)
            {
                times = maxTimes;
                return 1;//反回1代表次数刷新过重回普通状态
            }
        }
        return 0;
    }
}
