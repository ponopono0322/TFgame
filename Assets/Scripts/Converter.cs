using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : MonoBehaviour
{
    public static float StringToTime(string stringtime)
    {
        float lefttime;
        string[] spstring;
        spstring = stringtime.Split(':');
        lefttime = int.Parse(spstring[0]) * 60 + int.Parse(spstring[1]);
        Debug.Log("return data is:" + lefttime.ToString());
        return lefttime;
    }

    public static void LinkedTech(int i)
    {
        string[] l_list = DatabaseManager.MyItemList[i].INCOME.Split(',');
        for (int j = 0; j < l_list.Length; j++)
        {
            string[] l_data = l_list[j].Split(':');
            if(l_data[0] == "TRUE")
            {
                int l_num = DatabaseManager.MyItemList.FindIndex(x => x.CODE == l_data[1]);
                DatabaseManager.MyItemList[l_num].STATE = "TRUE";
                DatabaseManager.MyItemList[l_num].USEABLE = "TRUE";
            }
        }
        
    }
}
