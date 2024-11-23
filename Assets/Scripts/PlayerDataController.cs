using YG;

public static class PlayerDataController
{
    public static PlayerData playerData = new();

    public static void Save()
    {
        YandexGame.savesData.Update(playerData);
        YandexGame.SaveProgress();
    }

    public static void Load()
    {
        playerData = (PlayerData)YandexGame.savesData;
    }
}