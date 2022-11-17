using UnityEngine;

public static class WaveGenerator
{
    public static Vector2 aim;
    public static Vector2 spawnMinMax;
    public static int[] gemValue = new int[6] { 1, 5, 10, 25, 50, 100 };
    private static float[] gemProbability = new float[6] { 1, 0.6f, 0.1f, 0.6f, 0.1f, 0.75f };
    public static int[] GenerateWave(int wave)
    {
        var value = 5 + wave * wave / 2;
        var items = new int[6];
        GetGem(items, value);
        return items;
    }

    public static void SetAim(Vector2 _spawn, Vector2 _aim)
    {
        spawnMinMax = _spawn;
        aim = _aim;
    }

    public static int GenerateBombs(int wave)
    {
        var boms = 0;
        for(int i = 0; i < wave; i++)
        {
            if(Random.value > 0.5f)
                boms++;
        }
        return boms;
    }

    private static void GetGem(int[] items, int valueRemaining)
    {
        while (valueRemaining > 0)
        {
            var v = Random.value;
            for (int i = 0; i < gemValue.Length; i++)
            {
                if (valueRemaining >= gemValue[i] && v <= gemProbability[i])
                {
                    items[i] += 1;
                    valueRemaining -= gemValue[i];
                }
            }
        }
    }

}
