﻿Пояснительная записка:

- игра рассчитана на мобильную платформу (Android) с портретным режимом
- выбор планет осуществляется прямоугольным выделением
- атака планеты осуществляется тапом (кликом) по планете
- есть возможность перераспределить корабли между своими планетами
- все мною написанные скрипты находятся в папке CustomAssets
- в игре реализована "мультисцена"
- первой должна запускаться ZeroScene, где находится компонент MainInitializator
- чтобы корректно запустить игру в эдиторе предусмотрена опция Tools/Autoloader/Enabled (загружает игру всегда с нулевой сцены)
- основной геймплей и управление происходят в сценах GameWorld и GameUI
- кораблики реализованы через Pure ECS. логика находится в скрипте BoidComponentSystem.cs
- в игру можно легко добавить бота-оппонента, скопировав объект PlayerController и написав для него простой AI
- в билде на IL2CPP пропадают модели кораблей. скорее всего это связано с недоработкой пакета "Hybrid Renderer"
- в игре задействована система всплывающих окон (папка CustomAssets/Scripts/Features/Interface/Popups)
