using YG;

[System.Serializable]
public class PlayerData
{
    public int HiScore;
    public bool IsAudio;
    public bool IsVFX;
    public bool IsNumeric;

    public static explicit operator PlayerData(SavesYG v)
    {
        return new PlayerData()
        {
            HiScore = v.HiScore,
            IsAudio = v.IsAudio,
            IsVFX = v.IsVFX,
        };
    }
}