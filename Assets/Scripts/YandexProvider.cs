using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using YG;
using TMPro;

public class YandexProvider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _testLocalTest;
    [SerializeField] private GameSystem _gameSystem;
    [SerializeField] private GameObject _loadingIndicator;
    [SerializeField] private GameObject _buttonStart;

    public string _currentLanguage;

    private const int IdBonus1 = 1;
    private const int IdBonus2 = 2;

    private const string LeaderBoardName = "CryptoTop";

    [SerializeField] private Button _buttonMix1;
    [SerializeField] private Button _buttonMix2;

    private bool isOnTabGame = true;

    public bool IsActiveGame => isOnTabGame && YandexGame.isVisibilityWindowGame;

    private void Awake()
    {
        YandexGame.InitializationGame();
        YandexGame.RewardVideoEvent += DoneADS;
        YandexGame.GetDataEvent += LoadedData;
        YandexGame.onVisibilityWindowGame += ToggleVisibleTab;

        _buttonMix1.onClick.AddListener(ShowRewAds1);
        _buttonMix2.onClick.AddListener(ShowRewAds2);
    }

    private void ToggleVisibleTab(bool ison)
    {
        isOnTabGame = ison;
    }

    private void Update()
    {
        if (_testLocalTest.text != _currentLanguage)
        {
            _testLocalTest.text = _currentLanguage;
            ChangeLanguage(_currentLanguage);
        }
    }

    private void LoadedData()
    {
        PlayerDataController.Load();
        _loadingIndicator.SetActive(false);
        _buttonStart.SetActive(true);
        _gameSystem.InitPlayerData();
        var lang = YandexGame.EnvironmentData.language;
        _currentLanguage = lang;
        Debug.LogError($"LoadedData({lang})");
        YandexGame.GameReadyAPI();
    }
    private void ChangeLanguage(string localeIdentifier)
    {
        // Получаем список доступных локалей
        var locales = LocalizationSettings.AvailableLocales.Locales;    // Находим локаль по идентификатору
        Locale selectedLocale = locales.FirstOrDefault(l => l.Identifier.Code.Equals(localeIdentifier));    // Если локаль найдена, применяем её
        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;
            Debug.Log($"Язык успешно изменён на: {selectedLocale.Identifier}");
        }
        else
        {
            Debug.LogWarning($"Локаль с идентификатором {localeIdentifier} не найдена.");
        }
    }

    private void ShowRewAds1()
    {
        YandexGame.RewVideoShow(IdBonus1);
    }

    private void ShowRewAds2()
    {
        YandexGame.RewVideoShow(IdBonus2);
    }

    private void DoneADS(int id)
    {
        switch (id)
        {
            case IdBonus1:
                _gameSystem.Bust1();
                break;
            case IdBonus2:
                _gameSystem.Bonus2();
                break;
            default:
                break;
        }
    }

    public void UpdateLeaderHiScore(int score)
    {
        YandexGame.NewLeaderboardScores(LeaderBoardName, score);
    }

    private void OnDestroy()
    {
        YandexGame.RewardVideoEvent -= DoneADS;

        _buttonMix1.onClick.RemoveAllListeners();
        _buttonMix2.onClick.RemoveAllListeners();
    }
}