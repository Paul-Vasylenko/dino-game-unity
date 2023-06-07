using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Core.Services.Updater;
using Drawing;
using InputReader;
using Items;
using Items.Data;
using Items.Rarity;
using Items.Storage;
using NPC.Spawn;
using Player;
using UI;
using UnityEngine;
using NPC.Enum;

namespace Core
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private GameUIInputView _gameUIInputView;
        [SerializeField] private ItemRarityDescriptorsStorage _rarityDescriptorsStorage;
        [SerializeField] private LayerMask _whatIsPlayer;
        [SerializeField] private ItemsStorage _itemsStorage;
        [SerializeField] private Transform _spawnPoint;

        private ExternalDevicesInputReader _externalDevicesInputReader;

        private PlayerSystem _playerSystem;
        private ProjectUpdater _projectUpdater;
        private DropGenerator _dropGenerator;
        private ItemsSystem _itemsSystem;
        private UIContext _uiContext;
        private LevelDrawer _levelDrawer;
        private EntitySpawner _entitySpawner;

        private void Awake()
        {
            if (ProjectUpdater.Instance == null)
                _projectUpdater = new GameObject().AddComponent<ProjectUpdater>();
            else
                _projectUpdater = ProjectUpdater.Instance as ProjectUpdater;

            _externalDevicesInputReader = new ExternalDevicesInputReader();
            _playerSystem = new PlayerSystem(_playerEntity, new List<IEntityInputSource>
            {
                _gameUIInputView,
                _externalDevicesInputReader
            });

            ItemsFactory itemsFactory = new ItemsFactory(_playerSystem.StatsController);
            List<IItemRarityColor> rarityColors = _rarityDescriptorsStorage.RarityDescriptors.Cast<IItemRarityColor>().ToList();
            _itemsSystem = new ItemsSystem(rarityColors, itemsFactory, _whatIsPlayer, _playerSystem.Inventory);
            List<ItemDescriptor> descriptors = _itemsStorage.ItemScriptables.Select(scriptable => scriptable.ItemDescriptor).ToList();
            _dropGenerator = new DropGenerator(descriptors, _playerEntity, _itemsSystem);

            UIContext.Data data = new(_playerSystem.Inventory, _rarityDescriptorsStorage.RarityDescriptors, _playerSystem.StatsController);
            _uiContext = new UIContext(new List<IWindowsInputSource>
            {
                _gameUIInputView,
                _externalDevicesInputReader
            }, data);

            _levelDrawer = new LevelDrawer(LevelId.Level1);
            _levelDrawer.RegisterElement(_playerSystem.PlayerEntity);

            _entitySpawner = new EntitySpawner(_levelDrawer, _dropGenerator);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _uiContext.CloseCurrentScreen();

            if (Input.GetKeyDown(KeyCode.Q))
                _entitySpawner.SpawnEntity(EntityId.Enemy, _spawnPoint.position);
        }
    }
}