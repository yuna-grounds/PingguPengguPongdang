using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStringGenerator : MonoBehaviour
{
    public string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] randomChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            randomChars[i] = chars[Random.Range(0, chars.Length)];
        }

        return new string(randomChars);
    }
}
