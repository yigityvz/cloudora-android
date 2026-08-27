using System.Collections.Generic;
using UnityEngine;

namespace Cloudora.Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        [Header("Puzzle References")]

        [SerializeField]
        private Transform boardRoot;

        [SerializeField]
        private CloudContainerView cloudPrefab;

        private const int PrototypeCapacity = 4;

        // Level içindeki bütün cloud'ları burada tutuyoruz.
        private readonly List<CloudContainerView> _containers = new();

        // Oyuncunun kaynak olarak seçtiği cloud.
        private CloudContainerView _selectedCloud;

        // Puzzle bittikten sonra tekrar hareket yapılmasını engeller.
        private bool _isCompleted;

        private void Start()
        {
            CreatePrototypeLevel();
        }

        private void CreatePrototypeLevel()
        {
            WeatherType[][] levelData =
            {
                new[]
                {
                    WeatherType.Sun,
                    WeatherType.Rain,
                    WeatherType.Snow,
                    WeatherType.Sun
                },

                new[]
                {
                    WeatherType.Rain,
                    WeatherType.Snow,
                    WeatherType.Sun,
                    WeatherType.Rain
                },

                new[]
                {
                    WeatherType.Snow,
                    WeatherType.Sun,
                    WeatherType.Rain,
                    WeatherType.Snow
                },

                new WeatherType[] { },

                new WeatherType[] { }
            };

            _containers.Clear();
            _isCompleted = false;

            for (int i = 0; i < levelData.Length; i++)
            {
                CloudContainerView newCloud =
                    Instantiate(cloudPrefab, boardRoot);

                newCloud.name = $"Cloud_{i + 1}";

                newCloud.Initialize(
                    PrototypeCapacity,
                    levelData[i],
                    HandleCloudClicked);

                // Üretilen cloud'u listeye kaydet.
                _containers.Add(newCloud);
            }
        }

        private void HandleCloudClicked(
            CloudContainerView clickedCloud)
        {
            // Puzzle bittiyse yeni hamle yapma.
            if (_isCompleted)
            {
                return;
            }

            // İlk cloud seçimi.
            if (_selectedCloud == null)
            {
                if (clickedCloud.IsEmpty)
                {
                    return;
                }

                SelectCloud(clickedCloud);
                return;
            }

            // Aynı cloud'a tekrar basılırsa seçimi kaldır.
            if (_selectedCloud == clickedCloud)
            {
                ClearSelection();
                return;
            }

            bool moveSucceeded =
                _selectedCloud.TryMoveTopGroupTo(clickedCloud);

            if (moveSucceeded)
            {
                ClearSelection();

                // Her başarılı hamleden sonra
                // puzzle çözülmüş mü kontrol et.
                CheckWin();

                return;
            }

            // Geçersiz hedef doluysa onu yeni kaynak seç.
            if (!clickedCloud.IsEmpty)
            {
                SelectCloud(clickedCloud);
            }
        }

        private void SelectCloud(
            CloudContainerView cloud)
        {
            if (_selectedCloud != null)
            {
                _selectedCloud.SetSelected(false);
            }

            _selectedCloud = cloud;
            _selectedCloud.SetSelected(true);
        }

        private void ClearSelection()
        {
            if (_selectedCloud != null)
            {
                _selectedCloud.SetSelected(false);
            }

            _selectedCloud = null;
        }

        private void CheckWin()
        {
            // Tek bir cloud bile çözülmemişse
            // level henüz bitmemiştir.
            foreach (CloudContainerView container in _containers)
            {
                if (!container.IsSolved())
                {
                    return;
                }
            }

            _isCompleted = true;

            Debug.Log("Prototype level completed!");
        }
    }
}