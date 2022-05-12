using System.Threading.Tasks;
using UnityEngine;

namespace Code.Services
{
    public class HUDService
    {
        private const string WinWindowAddress = "Win Window";
        private const string LooseWindowAddress = "Loose Window";

        private readonly IAssetProvider _assetProvider;

        private GameObject _winWindowPrefab;
        private GameObject _looseWindowPrefab;

        private GameObject _winWindow;
        private GameObject _looseWindow;

        public HUDService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async Task Warmup()
        {
            _winWindowPrefab = await _assetProvider.Load<GameObject>(WinWindowAddress);
            _looseWindowPrefab = await _assetProvider.Load<GameObject>(LooseWindowAddress);

            _winWindow = GameObject.Instantiate(_winWindowPrefab);
            _looseWindow = GameObject.Instantiate(_looseWindowPrefab);

            HideWinWindow();
            HideLooseWindow();
        }

        public void ShowWinWindow()
        {
            _winWindow.SetActive(true);
        }

        public void HideWinWindow()
        {
            _winWindow.SetActive(false);
        }

        public void ShowLooseWindow()
        {
            _looseWindow.SetActive(true);
        }

        public void HideLooseWindow()
        {
            _looseWindow.SetActive(false);
        }
    }
}