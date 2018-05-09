using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    private int levelIndex = 1;
    [SerializeField]
    private Sprite[] levelImages;
    [SerializeField]
    private Sprite currentLevelSprite;


    public void LoadLevelIndex()
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }

    public void LevelIndexModifier(int amount)
    {
        levelIndex += amount;
        if (levelIndex > levelImages.Length)
            levelIndex = 1;
        else if (levelIndex < levelImages.Length)
            levelIndex = levelImages.Length - 1;

        currentLevelSprite = levelImages[levelIndex];
    }
}