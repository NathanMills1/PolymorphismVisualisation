using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class RandomGen
{
    private static System.Random randomGen;

    private static System.Random getRandomGen()
    {
        if(randomGen == null)
        {
            randomGen = new System.Random();
        }
        return randomGen;
    }
    public static int next(int max)
    {
        System.Random randomGen = getRandomGen();
        return randomGen.Next(max);
    }

    public static int next(int min, int max)
    {
        System.Random randomGen = getRandomGen();
        return randomGen.Next(min, max);
    }
}
