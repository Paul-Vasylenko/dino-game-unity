using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using InputReader;
using Items;
using Items.Data;
using Items.Rarity;
using Items.Storage;
using Player;
using UnityEngine;

namespace Core
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private GameUIInputView _gameUIInputView;
        [SerializeField] private ItemRarityDescriptorsStorage _rarityDescriptorsStorage;
        [SerializeField] private LayerMask _whatIsPlayer;
        [SerializeField] private ItemsStorage _itemsStorage;

        private List<IDisposable> _disposables;

        private ExternalDevicesInputReader _externalDevicesInputReader;

        private bool _onPause;
        private PlayerSystem _playerSystem;
        private ProjectUpdater _projectUpdater;
        private DropGenerator _dropGenerator;
        private ItemsSystem _itemsSystem;

        private void Awake()
        {
            _disposables = new List<IDisposable>();
            if (ProjectUpdater.Instance == null)
                _projectUpdater = new GameObject().AddComponent<ProjectUpdater>();
            else
                _projectUpdater = ProjectUpdater.Instance as ProjectUpdater;
            
            _externalDevicesInputReader = new ExternalDevicesInputReader();
            _disposables.Add(_externalDevicesInputReader);
            _playerSystem = new PlayerSystem(_playerEntity, new List<IEntityInputSource>
            {
                _gameUIInputView,
                _externalDevicesInputReader
            });
            _disposables.Add(_playerSystem);

            ItemsFactory itemsFactory = new ItemsFactory(_playerSystem.StatsController);
            List<IItemRarityColor> rarityColors = _rarityDescriptorsStorage.RarityDescriptors.Cast<IItemRarityColor>().ToList();
            _itemsSystem = new ItemsSystem(rarityColors, itemsFactory, _whatIsPlayer);
            List<ItemDescriptor> descriptors = _itemsStorage.ItemScriptables.Select(scriptable => scriptable.ItemDescriptor).ToList();
            _dropGenerator = new DropGenerator(descriptors, _playerEntity, _itemsSystem);
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}