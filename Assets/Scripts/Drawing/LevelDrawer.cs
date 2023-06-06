using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Drawing.Data;
using UnityEngine;

namespace Drawing
{
    public class LevelDrawer : IDisposable
    {
        private readonly LevelsDrawingDataStorage _levelsDrawingDataStorage;
        private readonly Dictionary<float, GraphicGameLayer> _gameLayers;
        private readonly List<ILevelGraphicElement> _graphicElements;

        private LevelDrawingDataStorage _levelDrawingData;
        
        public LevelDrawer(LevelId levelId)
        {
            _levelsDrawingDataStorage = Resources.Load<LevelsDrawingDataStorage>($"LevelsData/{nameof(LevelsDrawingDataStorage)}");
            _gameLayers = new Dictionary<float, GraphicGameLayer>();
            _graphicElements = new List<ILevelGraphicElement>();
            Initialize(levelId);
        }

        private void Initialize(LevelId levelId)
        {
            _levelDrawingData =
                _levelsDrawingDataStorage.LevelsData.Find(element => element.LevelId == levelId);
            _gameLayers.Add(0, new GraphicGameLayer(_levelDrawingData.OrdersPerLayer * _gameLayers.Count));
        }

        public void RedrawStaticEnvironment(Transform staticElementsContainer)
        {
            var elements = staticElementsContainer.GetComponentsInChildren<ILevelGraphicElement>();
            foreach (var element in elements)
                UpdateElement(element);
        }

        public void RegisterElement(ILevelGraphicElement element)
        {
            _graphicElements.Add(element);
            UpdateElement(element);
        }

        public void UnregisterElement(ILevelGraphicElement element)
        {
            _graphicElements.Remove(element);
            var prevLayer = _gameLayers.Values.FirstOrDefault(layer => layer.ContainsElement(element));
            prevLayer?.RemoveElement(element);
        }

        public void Dispose()
        {
            foreach (var layer in _gameLayers.Values)
                layer.Clear();

            _gameLayers.Clear();
        }

        private void UpdateElement(ILevelGraphicElement element)
        {
            var layerValue = _gameLayers.Keys.LastOrDefault(value => value > 0 + _levelDrawingData.MovementLayerStep);

            if (!_gameLayers.TryGetValue(layerValue, out var currentLayer) || currentLayer.ContainsElement(element))
                return;

            var prevLayer = _gameLayers.Values.FirstOrDefault(layer => layer.ContainsElement(element));
            prevLayer?.RemoveElement(element);
            _gameLayers[layerValue].AddElement(element);
        }
    }
}