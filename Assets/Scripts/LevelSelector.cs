using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private int levelIndex = 1;
    [SerializeField]
    private Sprite[] levelImages;
    [SerializeField]
    private Image curLVLImg;


    public void LoadLevelIndex()
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }

    public void LevelIndexModifier(int amount)
    {
        levelIndex += amount;
        if (levelIndex > levelImages.Length)
            levelIndex = 1;
        else if (levelIndex <= 0)
            levelIndex = levelImages.Length;

        curLVLImg.sprite = levelImages[levelIndex-1];
    }
}