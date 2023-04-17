using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
/ A class containing assorted utilities to help with specific scenarios. 
*/
public static class BSGUtility
{
    public const int DEF_SNAP = 1;

    /// <summary>
    /// Converts a bool value into its integer equivalent. True is 1, and 
    /// false is 0. 
    /// </summary>
    /// <param name="val"></param>
    /// <returns>1 if val is true, 0 otherwise. </returns>
    public static int BoolAsInt(bool val) {
        return val ? 1 : 0;
    }

    /// <summary>
    /// Returns positive or negative
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int BoolAsSign(bool val) {
        return val ? 1 : -1;
    }

    //Returns a position that is equal in distance from the two Vectors. 
    public static Vector3 Midpoint(Vector3 a, Vector3 b) {
        return new Vector3(a.x + ((b.x - a.x) / 2), 
            a.y + ((b.y - a.y) / 2), 
            a.z + ((b.z - a.z) / 2));
    }

    public static Vector3 ScalarMultiply(Vector3 a, float scalar) {
        return new Vector3(a.x * scalar, a.y * scalar, a.z * scalar);
    }

    /// <summary>
    /// Converts a Vector3 into a Vector3Int
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static Vector3Int ToVectorInt(Vector3 vec, bool round) {

        if (round) {
            return new Vector3Int(BSGUtility.Round(vec.x), BSGUtility.Round(vec.y), BSGUtility.Round(vec.z));
        }

        return new Vector3Int((int) vec.x, (int) vec.y, (int) vec.z);
    }

    /// <summary>
    /// Converts a Vector3Int into a Vector3
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static Vector3 ToVector(Vector3Int vec) {
        return new Vector3(vec.x, vec.y, vec.z);
    }

    public static int Round(float value) {
        return value < 0 ? (int)(value - 0.5f) : (int)(value + 0.5f);
    }

    /// <summary>
    /// Hypotenuse of a and b. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Hyp(float a, float b) {
        return Mathf.Pow(Mathf.Pow(a, 2) + Mathf.Pow(b, 2), 0.5f);
    }

    public static float RoundToDigit(float number, int digit) {
        if (digit == 0) {
            return number;
        }

        //Move val n places to the left and round that number
        float dec = Mathf.Round(number * Mathf.Pow(10f, (float)digit));

        //Move it back to where it was. 
        dec /= Mathf.Pow(10f, (float)digit);

        return dec;
    }

    //Snap a Vector's position to the nearest multiple of value. 
    public static float Snap(float value, int snap) {
        return snap * BSGUtility.Round((value / snap));
    }

    /// <summary>
    /// Checks if a value is within the min and max values. 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="closed">whether the bounds are closed (value can be equal to min or max) or open 
    /// (value must be greater than min but less than max) </param>
    /// <returns></returns>
    public static bool WithinRange(float value, float min, float max, bool closed) {
        return closed ? !(value < min || value > max) : !(value <= min || value >= max);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="closed"></param>
    /// <returns></returns>
    public static bool Within(float value, float target, float e) {
        return Mathf.Abs(value - target) <= e;
    }

    /**
     * Checks wether the given value is within the given boundary, inclusive. 
     */
    public static bool WithinBoundsOf(Vector2 vec, float left, float right, float top, float bottom) {
        return (vec.x >= left && vec.x <= right) && (vec.y >= bottom && vec.y <= top);
    }

    /**
    * Checks wether the given value is within the given boundary, exclusive. 
    */
    public static bool WithinBoundsOfEx(Vector2 vec, float left, float right, float top, float bottom) {
        return (vec.x > left && vec.x < right) && (vec.y > bottom && vec.y < top);
    }
}
