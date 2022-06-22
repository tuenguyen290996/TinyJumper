using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs
{
    public static int bestScore
    {
        set{
            if(PlayerPrefs.GetInt(PrefConst.BEST_SCORE, 0) < value)
            {
                PlayerPrefs.SetInt(PrefConst.BEST_SCORE, value);
            }
        }

        get => PlayerPrefs.GetInt(PrefConst.BEST_SCORE, 0);
    }
}
