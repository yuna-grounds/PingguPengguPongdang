using UnityEngine;
using System.Globalization;

public class KoreanNameGenerator : MonoBehaviour
{
    private static readonly string[] frontWords = { "신성한", "멍청한", "어두운", "밝은", "용감한", "소중한", "아름다운", "거대한", "작은", "화려한", "용감한", "착한" };
    private static readonly string[] backWords = { "탁자", "의자", "소파", "컴퓨터", "가방", "신발", "옷", "시계", "안경", "화장품", "카메라", "책", "노트북", "냉장고", "세탁기", "밥솥", "수저" };

    
    public string GenerateKoreanName()
    {
        int syllableCount = Random.Range(2, 4); // 이름은 2~3음절로 시작
        string name = frontWords[Random.Range(0, frontWords.Length)];
        name += backWords[Random.Range(0, frontWords.Length)];
        return name;
    }
}