using Godot;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Representation;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Screens.GameSubScreens;
using RogueCustomsGodotClient.Utils;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public partial class MapPanel : GamePanel
{
    private GlobalState _globalState;
    private Label _floorTitleLabel, _turnNumberLabel;
    private RichTextLabel _tileMap;

    private const int _mapWidthInTiles = 64;
    private const int _mapHeightInTiles = 32;

    private Vector2I? CursorMapLocation;
    public Vector2I? CursorCoords {
        get
        {
            if (CursorMapLocation == null) return null;
            return new(CursorMapLocation.Value.X, CursorMapLocation.Value.Y );
        }
    }

    private Vector2I TopLeftCornerCoords = new();
    private Vector2I TopLeftCornerPosition = new();

    private AimingSquare _aimingSquare;
    private int widthToDisplay, heightToDisplay;

    public Vector2 MapPosition => _tileMap.Position;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _floorTitleLabel = GetNode<Label>("VBoxContainer/FloorTitleLabel");
        _turnNumberLabel = GetNode<Label>("VBoxContainer2/TurnNumberLabel");
        _tileMap = GetNode<RichTextLabel>("TileMap");
        _aimingSquare = GetNode<AimingSquare>("AimingSquare");
        SetUp();
    }

    private void SetUp()
    {
        _tileMap.Text = "";
        _tileMap.BbcodeEnabled = true;
        CursorMapLocation = null;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
    }

    public override void Update()
    {
        var dungeonStatus = _globalState.DungeonInfo;
        _floorTitleLabel.Text = dungeonStatus.FloorName;
        _turnNumberLabel.Text = TranslationServer.Translate("TurnNumberText").ToString().Format(new { TurnNumber = dungeonStatus.TurnCount.ToString() });

        var playerEntity = dungeonStatus.PlayerEntity;
        if (playerEntity != null)
        {
            TopLeftCornerCoords = new(0, 0);
            TopLeftCornerPosition = new(0, 0);
            widthToDisplay = 0;
            heightToDisplay = 0;

            int cursorX = CursorMapLocation != null ? CursorMapLocation.Value.X : playerEntity.X;
            int cursorY = CursorMapLocation != null ? CursorMapLocation.Value.Y : playerEntity.Y;

            if (dungeonStatus.Width == _mapWidthInTiles)
            {
                TopLeftCornerCoords.X = 0;
                TopLeftCornerPosition.X = 0;
                widthToDisplay = _mapWidthInTiles;
            }
            else if (dungeonStatus.Width > _mapWidthInTiles)
            {
                TopLeftCornerCoords.X = Math.Max(0, cursorX - (_mapWidthInTiles / 2));
                if (TopLeftCornerCoords.X + _mapWidthInTiles > dungeonStatus.Width)
                    TopLeftCornerCoords.X = dungeonStatus.Width - _mapWidthInTiles;
                TopLeftCornerPosition.X = 0;
                widthToDisplay = _mapWidthInTiles;
            }
            else if (dungeonStatus.Width < _mapWidthInTiles)
            {
                TopLeftCornerCoords.X = 0;
                TopLeftCornerPosition.X = ((_mapWidthInTiles - dungeonStatus.Width) / 2);
                widthToDisplay = dungeonStatus.Width;
            }

            if (dungeonStatus.Height == _mapHeightInTiles)
            {
                TopLeftCornerCoords.Y = 0;
                TopLeftCornerPosition.Y = 0;
                heightToDisplay = _mapHeightInTiles;
            }
            else if (dungeonStatus.Height > _mapHeightInTiles)
            {
                TopLeftCornerCoords.Y = Math.Max(0, cursorY - (_mapHeightInTiles / 2));
                if (TopLeftCornerCoords.Y + _mapHeightInTiles > dungeonStatus.Height)
                    TopLeftCornerCoords.Y = dungeonStatus.Height - _mapHeightInTiles;
                TopLeftCornerPosition.Y = 0;
                heightToDisplay = _mapHeightInTiles;
            }
            else if (dungeonStatus.Height < _mapHeightInTiles)
            {
                TopLeftCornerCoords.Y = 0;
                TopLeftCornerPosition.Y = ((_mapHeightInTiles - dungeonStatus.Height) / 2);
                heightToDisplay = dungeonStatus.Height;
            }

            _tileMap.Text = "";

            // BBCode does not properly represent spaces
            var emptyCharacter = new ConsoleRepresentation
            {
                BackgroundColor = dungeonStatus.EmptyTile.BackgroundColor,
                ForegroundColor = new GameColor { R = 0, G = 0, B = 0, A = 0 },
                Character = 'A'
            };

            _tileMap.Position = new Vector2(16 * (TopLeftCornerPosition.X + 1), 16 * (TopLeftCornerPosition.Y + 1));

            for (var y = 0; y < heightToDisplay; y++)
            {
                _tileMap.PushParagraph(HorizontalAlignment.Left);
                for (var x = 0; x < widthToDisplay; x++)
                {
                    var actualMapCoords = new Vector2I(x + TopLeftCornerCoords.X, y + TopLeftCornerCoords.Y);
                    var tileInCoordinates = dungeonStatus.GetTileConsoleRepresentationFromCoordinates(actualMapCoords.X, actualMapCoords.Y);
                    if (tileInCoordinates.Character == ' ')
                    {
                        tileInCoordinates = emptyCharacter;
                    }
                    _tileMap.AppendText($"{tileInCoordinates.ToBbCodeRepresentation()}");
                }
                _tileMap.Pop();
            }
            _tileMap.PopAll();
        }
    }

    private Vector2 GetPositionForCoordinates(Vector2 coords)
    {
        if (_globalState.DungeonInfo == null) return default;
        var displayedCoords = new Vector2(coords.X - TopLeftCornerCoords.X + TopLeftCornerPosition.X, coords.Y - TopLeftCornerCoords.Y + TopLeftCornerPosition.Y);
        return new(displayedCoords.X, displayedCoords.Y);
    }

    public void StartTargeting()
    {
        var playerEntity = _globalState.DungeonInfo.PlayerEntity;
        _aimingSquare.Coordinates = GetPositionForCoordinates(new(playerEntity.X, playerEntity.Y));
        _aimingSquare.StartTargeting();
        CursorMapLocation = new(playerEntity.X, playerEntity.Y);
    }

    public void MoveTarget(Vector2 direction)
    {
        if(CursorMapLocation == null) return;
        if (_aimingSquare.Disabled) return;
        if (_globalState.DungeonInfo == null) return;
        var dungeonStatus = _globalState.DungeonInfo;
        var newCoordinates = new Vector2I(CursorMapLocation.Value.X + (int) direction.X, CursorMapLocation.Value.Y + (int) direction.Y);
        if (newCoordinates.X < 0 || newCoordinates.X >= dungeonStatus.Width) return;
        if (newCoordinates.Y < 0 || newCoordinates.Y >= dungeonStatus.Height) return;
        var tile = dungeonStatus.GetTileFromCoordinates((int) newCoordinates.X, (int) newCoordinates.Y);
        if (!tile.Targetable) return;
        CursorMapLocation = newCoordinates;
        Update();
        _aimingSquare.Coordinates = GetPositionForCoordinates(newCoordinates);
    }

    public void StopTargeting()
    {
        if (_aimingSquare.Disabled) return;
        _aimingSquare.StopTargeting();
        CursorMapLocation = null;
        Update();
    }
}
