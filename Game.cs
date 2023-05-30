using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Arkanoid;

public class Game
{
    private enum State
    {
        Game,
        Menu,
        Settings,
        GameOver,
        GameNext
    }

    private State _curState = State.Menu;

    private State CurState
    {
        get => _curState;
        set
        {
            _menu.IsOpen = false;
            _settingsState.IsOpen = false;
            _restartMessage.IsOpen = false;
            _nextLevelMessage.IsOpen = false;
            _curState = value;
            switch (_curState)
            {
                case State.Menu:
                    _menu.IsOpen = true;
                    break;
                case State.Settings:
                    _settingsState.IsOpen = true;
                    break;
                case State.GameOver:
                    _restartMessage.IsOpen = true;
                    break;
                case State.GameNext:
                    _nextLevelMessage.IsOpen = true;
                    break;
                case State.Game:
                    break;
            }
        }
    }

    private readonly GameField _gameField = new();
    private Players _playersRating = new();
    private Player _curPlayer;
    private readonly Menu _menu = new();
    private readonly SettingsState _settingsState;
    private readonly MessageBox _restartMessage = new();
    private readonly MessageBox _nextLevelMessage = new();
    private readonly StatusBar _statusBar = new();
    private readonly RectangleShape _statusBackground = new();

    private readonly Font? _mainFont;

    private readonly RenderWindow _window = new(new VideoMode(1600, 900), "Arkanoid", Styles.None);

    private readonly Timer _gameTimer;
    private readonly Time _tickTime = Time.FromMilliseconds(1000 / 60);

    public Game()
    {
        try
        {
            _mainFont = new Font("fonts/Sansation.ttf");
        }
        catch (SFML.LoadingFailedException e)
        {
            Console.WriteLine(e);
        }

        _gameField.Font = _mainFont;

        _curPlayer = new Player("anonymous");
        _settingsState = new SettingsState(_curPlayer.Name);

        BuildScene(_window.GetView().Size);

        _window.SetKeyRepeatEnabled(false);
        _window.Closed += (_, _) =>
        {
            _gameTimer?.Dispose();
            _window.Close();
        };

        /////////////////////////////////////////////////////
        _window.KeyPressed += _curPlayer.OnKeyPressed;
        _window.KeyReleased += _curPlayer.OnKeyReleased;
        ////////////////////////////////////////////////////

        _window.KeyPressed += (_, args) =>
        {
            if (args.Code == Keyboard.Key.Escape)
            {
                if (CurState == State.Game)
                {
                    CurState = State.Menu;
                }
            }
        };

        _settingsState.ResizeEvent += (_, args) =>
        {
            _window.Size = args.Resolution;
            _window.SetView(new View(new Vector2f(_window.Size.X / 2f, _window.Size.Y / 2f),
                new Vector2f(_window.Size.X, _window.Size.Y)));
            _window.Position = new Vector2i((int)(1920 - _window.Size.X) / 2,
                (int)(1080 - _window.Size.Y) / 2);
            BuildScene(_window.GetView().Size, true);
            CurState = State.Settings;
        };

        _window.MouseButtonPressed += _menu.OnMouseClicked;
        _window.MouseMoved += _menu.OnMouseMoved;

        _window.MouseButtonPressed += _settingsState.OnMouseClicked;
        _window.MouseMoved += _settingsState.OnMouseMoved;
        _window.TextEntered += _settingsState.OnTextEntered;

        _window.MouseButtonPressed += _statusBar.OnMouseClicked;
        _window.MouseMoved += _statusBar.OnMouseMoved;

        _window.MouseButtonPressed += _restartMessage.OnMouseClicked;
        _window.MouseMoved += _restartMessage.OnMouseMoved;

        _window.MouseButtonPressed += _nextLevelMessage.OnMouseClicked;
        _window.MouseMoved += _nextLevelMessage.OnMouseMoved;

        _settingsState.DifficultyChanged += (_, args) =>
        {
            _statusBar.Labels[1].SetText($"Difficulty:\n{args.Difficulty.ToString()}");
            _gameField.Difficulty = args.Difficulty;
        };

        _settingsState.PlayerNameChangedEvent += (_, args) =>
        {
            _statusBar.Labels[4].SetText($"Name:\n{args.Name}");
            _curPlayer.Name = args.Name;
        };

        _gameField.GameOver += (_, _) =>
        {
            CurState = State.GameOver;
            _gameField.CurLevel = 1;
            _statusBar.Labels[0].SetText($"Level:\n{_gameField.CurLevel}");
            _curPlayer.Statistics.CurrentScore = 0;
        };
        _gameField.NextLevel += (_, _) => CurState = State.GameNext;
        _curPlayer.Statistics.ScoreChanged += (_, _) =>
        {
            _statusBar.Labels[2].SetText($"Score:\n{_curPlayer.Statistics.CurrentScore}");
            _statusBar.Labels[3].SetText($"Best Score:\n{_curPlayer.Statistics.BestScore}");
        };

        TimerCallback tm = new(RunTick1);
        _gameTimer = new Timer(tm, null, 0, _tickTime.AsMilliseconds());
    }

    private void BuildScene(Vector2f size, bool saveContext = false)
    {
        BuildMenu(size);
        BuildSettings(size);
        BuildGameField(size, saveContext);
        BuildStatusBar(size);
        BuildRestartMessage(size);
        BuildNextLevelMessage(size);

        CurState = State.Menu;
    }

    private void BuildMenu(Vector2f size)
    {
        _menu.Clear();

        MenuItem resumeItem = new MenuItem(new Vector2f(size.X / 5f, size.Y / 10));
        resumeItem.BtnContent = new Label("Resume", (int)(size.Y / 30), _mainFont);
        resumeItem.RefPoint = new Vector2f(size.X / 2f, size.Y / 10f);
        resumeItem.ForeGroundColor = Color.White;
        resumeItem.BackGroundColor = new Color(100, 100, 100);
        resumeItem.ForeSelectedColor = Color.Black;
        resumeItem.BackSelectedColor = Color.White;
        resumeItem.SetNormalStyle();
        resumeItem.Clicked += (_, _) =>
        {
            CurState = State.Game;
            _gameField.StartGame();
            resumeItem.Deselect();
        };
        _menu.AddItem(resumeItem);

        MenuItem loadButton = new MenuItem(new Vector2f(size.X / 5f, size.Y / 10));
        loadButton.BtnContent = new Label("Load Txt", (int)(size.Y / 30), _mainFont);
        loadButton.RefPoint = new Vector2f(size.X / 2f, size.Y * 0.22f);
        loadButton.ForeGroundColor = Color.White;
        loadButton.BackGroundColor = new Color(100, 100, 100);
        loadButton.ForeSelectedColor = Color.Black;
        loadButton.BackSelectedColor = Color.White;
        loadButton.SetNormalStyle();
        loadButton.Clicked += (_, _) =>
        {
            LoadTXTGame();
            loadButton.Deselect();
        };
        _menu.AddItem(loadButton);

        MenuItem loadJsonButton = new MenuItem(new Vector2f(size.X / 5f, size.Y / 10));
        loadJsonButton.BtnContent = new Label("Load Json", (int)(size.Y / 30), _mainFont);
        loadJsonButton.RefPoint = new Vector2f(size.X / 2f, size.Y * 0.34f);
        loadJsonButton.ForeGroundColor = Color.White;
        loadJsonButton.BackGroundColor = new Color(100, 100, 100);
        loadJsonButton.ForeSelectedColor = Color.Black;
        loadJsonButton.BackSelectedColor = Color.White;
        loadJsonButton.SetNormalStyle();
        loadJsonButton.Clicked += (_, _) =>
        {
            LoadJSONGame();
            loadJsonButton.Deselect();
        };
        _menu.AddItem(loadJsonButton);


        MenuItem saveButton = new MenuItem(new Vector2f(size.X / 5f, size.Y / 10));
        saveButton.BtnContent = new Label("Save", (int)(size.Y / 30), _mainFont);
        saveButton.RefPoint = new Vector2f(size.X / 2f, size.Y * 0.46f);
        saveButton.ForeGroundColor = Color.White;
        saveButton.BackGroundColor = new Color(100, 100, 100);
        saveButton.ForeSelectedColor = Color.Black;
        saveButton.BackSelectedColor = Color.White;
        saveButton.SetNormalStyle();
        saveButton.Clicked += (_, _) =>
        {
            SaveGame();
            saveButton.Deselect();
        };
        _menu.AddItem(saveButton);

        MenuItem settingsButton = new MenuItem(new Vector2f(size.X / 5f, size.Y / 10));
        settingsButton.BtnContent = new Label("Settings", (int)(size.Y / 30), _mainFont);
        settingsButton.RefPoint = new Vector2f(size.X / 2f, size.Y * 0.58f);
        settingsButton.ForeGroundColor = Color.White;
        settingsButton.BackGroundColor = new Color(100, 100, 100);
        settingsButton.ForeSelectedColor = Color.Black;
        settingsButton.BackSelectedColor = Color.White;
        settingsButton.SetNormalStyle();
        settingsButton.Clicked += (_, _) =>
        {
            CurState = State.Settings;
            settingsButton.Deselect();
        };
        _menu.AddItem(settingsButton);

        MenuItem exitButton = new MenuItem(new Vector2f(size.X / 5f, size.Y / 10));
        exitButton.BtnContent = new Label("Exit", (int)(size.Y / 30), _mainFont);
        exitButton.RefPoint = new Vector2f(size.X / 2f, size.Y * 0.70f);
        exitButton.ForeGroundColor = Color.White;
        exitButton.BackGroundColor = new Color(100, 100, 100);
        exitButton.ForeSelectedColor = Color.Black;
        exitButton.BackSelectedColor = Color.White;
        exitButton.SetNormalStyle();
        exitButton.Clicked += (_, _) => { _window.Close(); };
        _menu.AddItem(exitButton);
        
        // MenuItem statisticButton = new MenuItem(new Vector2f(size.X / 5f, size.Y / 10));
        // statisticButton.BtnContent = new Label("Rating", (int)(size.Y / 30), _mainFont);
        // statisticButton.RefPoint = new Vector2f(size.X / 2f, size.Y * 0.82f);
        // statisticButton.ForeGroundColor = Color.White;
        // statisticButton.BackGroundColor = new Color(100, 100, 100);
        // statisticButton.ForeSelectedColor = Color.Black;
        // statisticButton.BackSelectedColor = Color.White;
        // statisticButton.SetNormalStyle();
        // statisticButton.Clicked += (_, _) => { };
        // _menu.AddItem(statisticButton);
    }

    private void BuildSettings(Vector2f size)
    {
        _settingsState.BuildScene(size / 2f,
            new Vector2f(size.X / 2f, size.Y * 0.8f), _mainFont);

        _settingsState.BackButton!.Clicked += (_, _) =>
        {
            CurState = State.Menu;
            _settingsState.BackButton.Deselect();
        };
    }


    private void BuildStatusBar(Vector2f size)
    {
        _statusBackground.Size = new Vector2f(size.X, size.Y * 0.1f);
        _statusBackground.FillColor = new Color(150, 150, 150);

        _statusBar.Clear();

        Label levelLabel = new Label($"Level:\n{_gameField.CurLevel}", (int)(size.Y / 25f),
            _mainFont);
        levelLabel.ForeGroundColor = Color.White;
        levelLabel.RefPoint = new Vector2f(size.X * 0.05f, size.Y * 0.05f);
        _statusBar.Labels.Add(levelLabel);

        Label difficultLabel = new Label($"Difficulty:\n{_settingsState.CurSettings.CurrentDifficulty.ToString()}",
            (int)(size.Y / 25f), _mainFont);
        difficultLabel.ForeGroundColor = Color.White;
        difficultLabel.RefPoint = new Vector2f(size.X * 0.18f, size.Y * 0.05f);
        _statusBar.Labels.Add(difficultLabel);

        Label statisticLabel =
            new Label($"Score:\n{_curPlayer.Statistics.CurrentScore}", (int)(size.Y / 25f), _mainFont);
        statisticLabel.ForeGroundColor = Color.White;
        statisticLabel.RefPoint = new Vector2f(size.X * 0.31f, size.Y * 0.05f);
        _statusBar.Labels.Add(statisticLabel);

        Label bestScoreLabel =
            new Label($"Best Score:\n{_curPlayer.Statistics.BestScore}", (int)(size.Y / 25f), _mainFont);
        bestScoreLabel.ForeGroundColor = Color.White;
        bestScoreLabel.RefPoint = new Vector2f(size.X * 0.45f, size.Y * 0.05f);
        _statusBar.Labels.Add(bestScoreLabel);

        Label nameLabel = new Label($"Name:\n{_curPlayer.Name}", (int)(size.Y / 25f), _mainFont);
        nameLabel.ForeGroundColor = Color.White;
        nameLabel.CurAlignment = Label.Alignment.Left;
        nameLabel.RefPoint = new Vector2f(size.X * 0.53f, size.Y * 0.05f);
        _statusBar.Labels.Add(nameLabel);

        Button menuBtn = new Button(new Vector2f(size.X / 6, size.Y * 0.1f));
        menuBtn.BtnContent = new Label("Menu", (int)(size.Y / 30), _mainFont);
        menuBtn.RefPoint = new Vector2f(size.X / 12 * 11, size.Y * 0.05f);
        menuBtn.ForeGroundColor = Color.White;
        menuBtn.BackGroundColor = new Color(100, 100, 100);
        menuBtn.ForeSelectedColor = Color.Black;
        menuBtn.BackSelectedColor = Color.White;
        menuBtn.SetNormalStyle();
        menuBtn.Clicked += (_, _) =>
        {
            if (CurState == State.Game)
            {
                CurState = State.Menu;
                menuBtn.Deselect();
            }
        };
        _statusBar.AddButton(menuBtn);
    }

    private void BuildRestartMessage(Vector2f size)
    {
        _restartMessage.Clear();

        _restartMessage.Background.Size = new Vector2f(size.X * 0.4f, size.Y / 4f);
        _restartMessage.Background.FillColor = new Color(150, 150, 150);
        _restartMessage.Background.Origin = new Vector2f(size.X * 0.2f, size.Y / 8f);
        _restartMessage.Background.Position = size / 2;

        Label messageBoxLabel = new Label("Game Over", (int)(size.Y / 10f),
            _mainFont);
        messageBoxLabel.ForeGroundColor = Color.White;
        messageBoxLabel.RefPoint = new Vector2f(size.X * 0.5f, size.Y * 0.45f);

        _restartMessage.MessageContent = messageBoxLabel;

        Button startBtn = new Button(new Vector2f(size.X * 0.1f, size.Y * 0.07f));
        startBtn.BtnContent = new Label("Restart", (int)(size.Y / 30), _mainFont);
        startBtn.RefPoint = new Vector2f(size.X / 2f, size.Y * 0.55f);
        startBtn.ForeGroundColor = Color.White;
        startBtn.BackGroundColor = new Color(100, 100, 100);
        startBtn.ForeSelectedColor = Color.Black;
        startBtn.BackSelectedColor = Color.White;
        startBtn.SetNormalStyle();
        startBtn.Clicked += (_, _) =>
        {
            _gameField.BuildField(new Vector2f(_window.GetView().Size.X, _window.GetView().Size.Y * 0.9f), _curPlayer);
            _gameField.Position = new Vector2f(0f, 0.1f * _window.GetView().Size.Y);
            CurState = State.Game;
            _gameField.StartGame();
            startBtn.Deselect();
        };
        _restartMessage.AddButton(startBtn);
    }

    private void BuildNextLevelMessage(Vector2f size)
    {
        _nextLevelMessage.Clear();

        _nextLevelMessage.Background.Size = new Vector2f(size.X * 0.4f, size.Y / 4f);
        _nextLevelMessage.Background.FillColor = new Color(150, 150, 150);
        _nextLevelMessage.Background.Origin = new Vector2f(size.X * 0.2f, size.Y / 8f);
        _nextLevelMessage.Background.Position = size / 2;

        Label messageBoxLabel = new Label("Congratulation", (int)(size.Y / 15f),
            _mainFont);
        messageBoxLabel.ForeGroundColor = Color.White;
        messageBoxLabel.RefPoint = new Vector2f(size.X * 0.5f, size.Y * 0.45f);

        _nextLevelMessage.MessageContent = messageBoxLabel;

        Button nextBtn = new Button(new Vector2f(size.X * 0.1f, size.Y * 0.07f));
        nextBtn.BtnContent = new Label("Next Level", (int)(size.Y / 35), _mainFont);
        nextBtn.RefPoint = new Vector2f(size.X / 2f, size.Y * 0.55f);
        nextBtn.ForeGroundColor = Color.White;
        nextBtn.BackGroundColor = new Color(100, 100, 100);
        nextBtn.ForeSelectedColor = Color.Black;
        nextBtn.BackSelectedColor = Color.White;
        nextBtn.SetNormalStyle();
        nextBtn.Clicked += (_, _) =>
        {
            _gameField.CurLevel++;
            _statusBar.Labels[0].SetText($"Level:\n{_gameField.CurLevel}");
            CurState = State.Game;
            _gameField.StartGame();
            nextBtn.Deselect();
        };
        _nextLevelMessage.AddButton(nextBtn);
    }

    private void BuildGameField(Vector2f size, bool saveContext)
    {
        if (saveContext)
        {
            _gameField.RebuildField(new Vector2f(size.X, size.Y * 0.9f), _curPlayer);
            _gameField.Position = new Vector2f(0f, 0.1f * size.Y);
        }
        else
        {
            _gameField.BuildField(new Vector2f(size.X, size.Y * 0.9f), _curPlayer);
            _gameField.Position = new Vector2f(0f, 0.1f * size.Y);
        }
    }

    private void SaveGame()
    {
        throw new NotImplementedException();
    }

    private void LoadJSONGame()
    {
        throw new NotImplementedException();
    }

    private void LoadTXTGame()
    {
        throw new NotImplementedException();
    }


    private void RunTick(object obj)
    {
        _window.SetActive(true);

        _window.DispatchEvents();
        Update(_tickTime);
        Render();

        _window.SetActive(false);
    }

    private void Render()
    {
        _window.Clear();
        _window.Draw(_gameField);
        _window.Draw(_statusBackground);
        _window.Draw(_statusBar);
        if (CurState == State.Menu)
        {
            _window.Draw(_menu);
        }

        if (CurState == State.Settings)
        {
            _window.Draw(_settingsState);
        }

        if (CurState == State.GameNext)
        {
            _window.Draw(_nextLevelMessage);
        }

        if (CurState == State.GameOver)
        {
            _window.Draw(_restartMessage);
        }


        _window.Display();
    }

    private void Update(Time elapsedTime)
    {
        if (CurState == State.Game)
        {
            _gameField.Update(elapsedTime);
        }
    }

    public void Run()
    {
        Clock clock = new();
        Time timeSinceLastUpdate = Time.Zero;
        Time timePerFrame = Time.FromSeconds(1 / 144f);

        while (_window.IsOpen)
        {
            Time dt = clock.Restart();
            timeSinceLastUpdate += dt;
            while (timeSinceLastUpdate >= timePerFrame)
            {
                timeSinceLastUpdate -= timePerFrame;

                _window.DispatchEvents();
                Update(timePerFrame);
            }

            Render();
        }
    }

    private void RunTick1(object obj)
    {
    }
}