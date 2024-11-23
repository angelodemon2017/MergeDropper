
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        //        public int money = 1;                       // Можно задать полям значения по умолчанию
        //        public string newPlayerName = "Hello!";
        //        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        public int HiScore;
        public bool IsAudio;
        public bool IsVFX;

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            IsAudio = true;
            IsVFX = true;
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            //            openLevels[1] = true;
        }

        public void Update(PlayerData playerData)
        {
            HiScore = playerData.HiScore;
            IsAudio = playerData.IsAudio;
            IsVFX = playerData.IsVFX;
        }

        public static SavesYG operator +(SavesYG savesYG, PlayerData playerData)
        {
            savesYG.HiScore = playerData.HiScore;
            savesYG.IsAudio = playerData.IsAudio;
            savesYG.IsVFX = playerData.IsVFX;

            return savesYG;
        }
    }
}
