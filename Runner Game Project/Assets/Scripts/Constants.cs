using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PlayerPrefsKeys {
    public const string prevSceneScore = "PreviousScore";
    public const string name = "PlayerName";
    public const string prevCanvasKey = "PreviousCanvas";
}

// Lane-ul este pe Axa X
public static class Lane
{
    public const float Left = Middle - Distance;
    public const float Middle = 0;
    public const float Right = Middle + Distance;

    public const float Distance = 2;

    public static readonly float[] array = { Left, Middle, Right };

    private static float lastGen1 = 0;
    private static float lastGen2 = 0;

    /** Policy:
     * do not/repeat lane
     * dis/allow stairs pattern
     */

    public static float NextRandomLane()
    {
        float gen = array[UnityEngine.Random.Range(0, array.Length)];
        // caz scarita
        if ((gen == Left && lastGen1 == Middle && lastGen2 == Right) || (gen == Right && lastGen1 == Middle && lastGen2 == Left))
            UnityEngine.Debug.LogWarning("generat in scarita");

        lastGen2 = lastGen1;
        lastGen1 = gen;

        return gen;
    }
}

