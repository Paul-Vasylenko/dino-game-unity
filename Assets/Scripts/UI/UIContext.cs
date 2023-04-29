using System;
using System.Collections.Generic;
using InputReader;
using Items;
using Items.Data;
using UI.Core;
using UI.Enum;
using UI.InventoryUI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI
{
    public class UIContext : IDisposable
    {
        private const string LoadPath = "UI/";

        private readonly Dictionary<ScreenType, IScreenController> _controllers;
        private readonly Transform _uiContainer;
        private readonly List<IWindowsInputSource> _inputSources;
        private readonly Data _data;

        private IScreenController _currentController;

        public UIContext(List<IWindowsInputSource> inputSources, Data data)
        {
            _controllers = new Dictionary<ScreenType, IScreenController>();
            _inputSources = inputSources;
            foreach (IWindowsInputSource inputSource in _inputSources)
            {
                inputSource.InventoryRequested += OpenInventory;
            }

            GameObject container = new()
            {
                name = nameof(UIContext)
            };
            _uiContainer = container.transform;
            _data = data;
        }

        public void CloseCurrentScreen()
        {
            _currentController.Complete();
            _currentController = null;
        }

        public void Dispose()
        {
            foreach (IWindowsInputSource inputSource in _inputSources)
            {
                inputSource.InventoryRequested -= OpenInventory;
            }

            foreach (IScreenController screenPresenter in _controllers.Values)
            {
                screenPresenter.CloseRequested -= CloseCurrentScreen;
                screenPresenter.OpenScreenRequested -= OpenScreen;
            }
        }

        private void OpenInventory() => OpenScreen(ScreenType.Inventory);

        private void OpenScreen(ScreenType screenType)
        {
            _currentController?.Complete();

            if (!_controllers.TryGetValue(screenType, out IScreenController screenController))
            {
                screenController = GetPresenter(screenType);
                screenController.CloseRequested += CloseCurrentScreen;
                screenController.OpenScreenRequested += OpenScreen;
                _controllers.Add(screenType, screenController);
            }

            _currentController = screenController;
            _currentController.Initialize();
        }

        private IScreenController GetPresenter(ScreenType screenType)
        {
            return screenType switch
            {
                ScreenType.Inventory =>
                    new InventoryScreenAdapter(GetView<InventoryScreenView>(screenType), _data.Inventory, _data.RarityDescriptors),
                _ => throw new NullReferenceException()
            };
        }

        private TView GetView<TView>(ScreenType screenType) where TView : ScreenView
        {
            TView prefab = Resources.Load<TView>($"{LoadPath}{screenType}");
            return Object.Instantiate(prefab, _uiContainer);
        }

        public struct Data
        {
            public Inventory Inventory { get; }
            public List<RarityDescriptor> RarityDescriptors { get; }

            public Data(Inventory inventory, List<RarityDescriptor> rarityDescriptors)
            {
                Inventory = inventory;
                RarityDescriptors = rarityDescriptors;
            }
        }
    }
}