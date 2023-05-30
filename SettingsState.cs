using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Arkanoid;

public class SettingsState : Transformable, Drawable
{
    public event EventHandler<ResizeEventArgs>? ResizeEvent;
    public event EventHandler<DifficultyChangedEventArgs>? DifficultyChanged;
    public event EventHandler<PlayerNameChangedEventArgs>? PlayerNameChangedEvent;

    public class ResizeEventArgs : EventArgs
    {
        public Vector2u Resolution;

        public ResizeEventArgs(Vector2u size)
        {
            Resolution = size;
        }
    }

    public class DifficultyChangedEventArgs : EventArgs
    {
        public readonly Settings.Difficulty Difficulty;

        public DifficultyChangedEventArgs(Settings.Difficulty difficulty)
        {
            Difficulty = difficulty;
        }
    }

    public class PlayerNameChangedEventArgs : EventArgs
    {
        public readonly string Name;

        public PlayerNameChangedEventArgs(string name)
        {
            Name = name;
        }
    }

    public event EventHandler<MouseButtonEventArgs>? MouseClicked;
    public event EventHandler<MouseMoveEventArgs>? MouseMoved;
    public event EventHandler<TextEventArgs>? TextEntered;

    public readonly Settings CurSettings = new();

    private Label? _header;
    private Label? _resolutionsLabel;
    private Label? _difficultyLabel;
    private Label? _playerNameLabel;

    private readonly List<Button> _resolutions = new();
    private readonly List<Button> _difficulties = new();
    private InputText? _inputText;

    public Button? BackButton;

    private readonly RectangleShape _background = new();


    private int _activeResolution = -1;
    private int _activeDifficulty = -1;

    public bool IsOpen { get; set; }

    private int ActiveResolution
    {
        get => _activeResolution;
        set
        {
            if (_activeResolution != -1)
            {
                _resolutions[_activeResolution].ForeGroundColor = Color.White;
                _resolutions[_activeResolution].BackGroundColor = new Color(100, 100, 100);
                _resolutions[_activeResolution].SetNormalStyle();
            }

            _activeResolution = value;
            _resolutions[_activeResolution].ForeGroundColor = Color.White;
            _resolutions[_activeResolution].BackGroundColor = Color.Red;
            _resolutions[_activeResolution].SetNormalStyle();
            ResizeEvent?.Invoke(this, new ResizeEventArgs(CurSettings.Resolution));
        }
    }

    private int ActiveDifficulty
    {
        get => _activeDifficulty;
        set
        {
            if (_activeDifficulty != -1)
            {
                _difficulties[_activeDifficulty].ForeGroundColor = Color.White;
                _difficulties[_activeDifficulty].BackGroundColor = new Color(100, 100, 100);
                _difficulties[_activeDifficulty].SetNormalStyle();
            }

            _activeDifficulty = value;
            _difficulties[_activeDifficulty].ForeGroundColor = Color.White;
            _difficulties[_activeDifficulty].BackGroundColor = Color.Red;
            _difficulties[_activeDifficulty].SetNormalStyle();
            DifficultyChanged?.Invoke(this, new DifficultyChangedEventArgs(CurSettings.CurrentDifficulty));
        }
    }

    public SettingsState(string playerName)
    {
        CurSettings.PlayerName = playerName;
        PlayerNameChangedEvent += (_, args) => CurSettings.PlayerName
            = args.Name;
    }

    public void BuildScene(Vector2f position, Vector2f size, Font? font)
    {
        foreach (var btn in _resolutions)
        {
            MouseClicked -= btn.OnMouseClicked;
            MouseMoved -= btn.OnMouseMoved;
        }

        _resolutions.Clear();
        foreach (var btn in _difficulties)
        {
            MouseClicked -= btn.OnMouseClicked;
            MouseMoved -= btn.OnMouseMoved;
        }

        _difficulties.Clear();

        if (BackButton is not null)
        {
            MouseClicked -= BackButton.OnMouseClicked;
            MouseMoved -= BackButton.OnMouseMoved;
        }

        if (_inputText is not null)
        {
            TextEntered -= _inputText.OnTextEntered;
        }

        _background.Position = position;
        _background.Origin = size / 2f;
        _background.Size = size;
        _background.FillColor = new(50, 50, 50);
        _background.OutlineColor = Color.White;
        _background.OutlineThickness = 1;

        _header = new Label("Settings", (int)(size.Y / 10f), font);
        _header.ForeGroundColor = Color.White;
        _header.RefPoint = new Vector2f(position.X, position.Y - 0.44f * size.Y);

        _resolutionsLabel = new Label("Resolutions", (int)(size.Y / 20f), font);
        _resolutionsLabel.ForeGroundColor = Color.White;
        _resolutionsLabel.RefPoint = new Vector2f(position.X, position.Y - 0.35f * size.Y);

        _difficultyLabel = new Label("Difficulties", (int)(size.Y / 20f), font);
        _difficultyLabel.ForeGroundColor = Color.White;
        _difficultyLabel.RefPoint = new Vector2f(position.X, position.Y - 0.15f * size.Y);

        _playerNameLabel = new Label("Name:", (int)(size.Y / 20f), font);
        _playerNameLabel.ForeGroundColor = Color.White;
        _playerNameLabel.RefPoint = new Vector2f(position.X, position.Y + 0.05f * size.Y);

        {
            Button btn1 = new Button(new Vector2f(size.X / 5f, size.Y / 8));
            btn1.BtnContent = new Label("800/600", (int)(size.Y / 30), font);
            btn1.RefPoint = new Vector2f(position.X - 0.4f * size.X, position.Y - 0.25f * size.Y);
            btn1.ForeGroundColor = Color.White;
            btn1.BackGroundColor = new Color(100, 100, 100);
            btn1.ForeSelectedColor = Color.Black;
            btn1.BackSelectedColor = Color.White;
            btn1.SetNormalStyle();
            btn1.Clicked += (_, _) =>
            {
                CurSettings.Resolution = new Vector2u(800, 600);
                ActiveResolution = 0;
            };
            _resolutions.Add(btn1);
            MouseClicked += btn1.OnMouseClicked;
            MouseMoved += btn1.OnMouseMoved;

            Button btn2 = new Button(new Vector2f(size.X / 5f, size.Y / 8));
            btn2.BtnContent = new Label("1280/720", (int)(size.Y / 30), font);
            btn2.RefPoint = new Vector2f(position.X - 0.2f * size.X, position.Y - 0.25f * size.Y);
            btn2.ForeGroundColor = Color.White;
            btn2.BackGroundColor = new Color(100, 100, 100);
            btn2.ForeSelectedColor = Color.Black;
            btn2.BackSelectedColor = Color.White;
            btn2.SetNormalStyle();
            btn2.Clicked += (_, _) =>
            {
                CurSettings.Resolution = new Vector2u(1280, 720);
                ActiveResolution = 1;
            };
            _resolutions.Add(btn2);
            MouseClicked += btn2.OnMouseClicked;
            MouseMoved += btn2.OnMouseMoved;


            Button btn3 = new Button(new Vector2f(size.X / 5f, size.Y / 8));
            btn3.BtnContent = new Label("1366/768", (int)(size.Y / 30), font);
            btn3.RefPoint = new Vector2f(position.X, position.Y - 0.25f * size.Y);
            btn3.ForeGroundColor = Color.White;
            btn3.BackGroundColor = new Color(100, 100, 100);
            btn3.ForeSelectedColor = Color.Black;
            btn3.BackSelectedColor = Color.White;
            btn3.SetNormalStyle();
            btn3.Clicked += (_, _) =>
            {
                CurSettings.Resolution = new Vector2u(1366, 768);
                ActiveResolution = 2;
            };
            _resolutions.Add(btn3);
            MouseClicked += btn3.OnMouseClicked;
            MouseMoved += btn3.OnMouseMoved;

            Button btn4 = new Button(new Vector2f(size.X / 5f, size.Y / 8));
            btn4.BtnContent = new Label("1600/900", (int)(size.Y / 30), font);
            btn4.RefPoint = new Vector2f(position.X + 0.2f * size.X, position.Y - 0.25f * size.Y);
            btn4.ForeGroundColor = Color.White;
            btn4.BackGroundColor = new Color(100, 100, 100);
            btn4.ForeSelectedColor = Color.Black;
            btn4.BackSelectedColor = Color.White;
            btn4.SetNormalStyle();
            btn4.Clicked += (_, _) =>
            {
                CurSettings.Resolution = new Vector2u(1600, 900);
                ActiveResolution = 3;
            };
            _resolutions.Add(btn4);
            MouseClicked += btn4.OnMouseClicked;
            MouseMoved += btn4.OnMouseMoved;

            Button btn5 = new Button(new Vector2f(size.X / 5f, size.Y / 8));
            btn5.BtnContent = new Label("1920/1080", (int)(size.Y / 30), font);
            btn5.RefPoint = new Vector2f(position.X + 0.4f * size.X, position.Y - 0.25f * size.Y);
            btn5.ForeGroundColor = Color.White;
            btn5.BackGroundColor = new Color(100, 100, 100);
            btn5.ForeSelectedColor = Color.Black;
            btn5.BackSelectedColor = Color.White;
            btn5.SetNormalStyle();
            btn5.Clicked += (_, _) =>
            {
                CurSettings.Resolution = new Vector2u(1920, 1080);
                ActiveResolution = 4;
            };
            _resolutions.Add(btn5);
            MouseClicked += btn5.OnMouseClicked;
            MouseMoved += btn5.OnMouseMoved;

            if (ActiveResolution == -1)
            {
                ActiveResolution = 3;
            }
            else
            {
                _resolutions[ActiveResolution].ForeGroundColor = Color.White;
                _resolutions[ActiveResolution].BackGroundColor = Color.Red;
                _resolutions[ActiveResolution].SetNormalStyle();
            }
        }

        {
            Button btn1 = new Button(new Vector2f(size.X / 5f, size.Y / 8));
            btn1.BtnContent = new Label("Very Easy", (int)(size.Y / 30), font);
            btn1.RefPoint = new Vector2f(position.X - 0.4f * size.X, position.Y - 0.05f * size.Y);
            btn1.ForeGroundColor = Color.White;
            btn1.BackGroundColor = new Color(100, 100, 100);
            btn1.ForeSelectedColor = Color.Black;
            btn1.BackSelectedColor = Color.White;
            btn1.SetNormalStyle();
            btn1.Clicked += (_, _) =>
            {
                CurSettings.CurrentDifficulty = Settings.Difficulty.VeryEasy;
                ActiveDifficulty = 0;
            };
            _difficulties.Add(btn1);
            MouseClicked += btn1.OnMouseClicked;
            MouseMoved += btn1.OnMouseMoved;

            Button btn2 = new Button(new Vector2f(size.X / 5f, size.Y / 8));
            btn2.BtnContent = new Label("Easy", (int)(size.Y / 30), font);
            btn2.RefPoint = new Vector2f(position.X - 0.2f * size.X, position.Y - 0.05f * size.Y);
            btn2.ForeGroundColor = Color.White;
            btn2.BackGroundColor = new Color(100, 100, 100);
            btn2.ForeSelectedColor = Color.Black;
            btn2.BackSelectedColor = Color.White;
            btn2.SetNormalStyle();
            btn2.Clicked += (_, _) =>
            {
                CurSettings.CurrentDifficulty = Settings.Difficulty.Easy;
                ActiveDifficulty = 1;
            };
            _difficulties.Add(btn2);
            MouseClicked += btn2.OnMouseClicked;
            MouseMoved += btn2.OnMouseMoved;


            Button btn3 = new Button(new Vector2f(size.X / 5f, size.Y / 8));
            btn3.BtnContent = new Label("Medium", (int)(size.Y / 30), font);
            btn3.RefPoint = new Vector2f(position.X, position.Y - 0.05f * size.Y);
            btn3.ForeGroundColor = Color.White;
            btn3.BackGroundColor = new Color(100, 100, 100);
            btn3.ForeSelectedColor = Color.Black;
            btn3.BackSelectedColor = Color.White;
            btn3.SetNormalStyle();
            btn3.Clicked += (_, _) =>
            {
                CurSettings.CurrentDifficulty = Settings.Difficulty.Medium;
                ActiveDifficulty = 2;
            };
            _difficulties.Add(btn3);
            MouseClicked += btn3.OnMouseClicked;
            MouseMoved += btn3.OnMouseMoved;

            Button btn4 = new Button(new Vector2f(size.X / 5f, size.Y / 8));
            btn4.BtnContent = new Label("Hard", (int)(size.Y / 30), font);
            btn4.RefPoint = new Vector2f(position.X + 0.2f * size.X, position.Y - 0.05f * size.Y);
            btn4.ForeGroundColor = Color.White;
            btn4.BackGroundColor = new Color(100, 100, 100);
            btn4.ForeSelectedColor = Color.Black;
            btn4.BackSelectedColor = Color.White;
            btn4.SetNormalStyle();
            btn4.Clicked += (_, _) =>
            {
                CurSettings.CurrentDifficulty = Settings.Difficulty.Hard;
                ActiveDifficulty = 3;
            };
            _difficulties.Add(btn4);
            MouseClicked += btn4.OnMouseClicked;
            MouseMoved += btn4.OnMouseMoved;

            Button btn5 = new Button(new Vector2f(size.X / 5f, size.Y / 8));
            btn5.BtnContent = new Label("Impossible", (int)(size.Y / 30), font);
            btn5.RefPoint = new Vector2f(position.X + 0.4f * size.X, position.Y - 0.05f * size.Y);
            btn5.ForeGroundColor = Color.White;
            btn5.BackGroundColor = new Color(100, 100, 100);
            btn5.ForeSelectedColor = Color.Black;
            btn5.BackSelectedColor = Color.White;
            btn5.SetNormalStyle();
            btn5.Clicked += (_, _) =>
            {
                CurSettings.CurrentDifficulty = Settings.Difficulty.Impossible;
                ActiveDifficulty = 4;
            };
            _difficulties.Add(btn5);
            MouseClicked += btn5.OnMouseClicked;
            MouseMoved += btn5.OnMouseMoved;

            if (ActiveDifficulty == -1)
            {
                ActiveDifficulty = 0;
            }
            else
            {
                _difficulties[ActiveDifficulty].ForeGroundColor = Color.White;
                _difficulties[ActiveDifficulty].BackGroundColor = Color.Red;
                _difficulties[ActiveDifficulty].SetNormalStyle();
            }
        }

        BackButton = new Button(new Vector2f(0.25f * size.X, 0.1f * size.Y));
        BackButton.BtnContent = new Label("Back", (int)(size.Y / 30), font);
        BackButton.RefPoint = new Vector2f(position.X, position.Y + 0.42f * size.Y);
        BackButton.ForeGroundColor = Color.White;
        BackButton.BackGroundColor = new Color(100, 100, 100);
        BackButton.ForeSelectedColor = Color.Black;
        BackButton.BackSelectedColor = Color.White;
        BackButton.SetNormalStyle();

        MouseClicked += BackButton.OnMouseClicked;
        MouseMoved += BackButton.OnMouseMoved;

        _inputText = new InputText(new Vector2f(size.X * 0.9f, size.Y * 0.1f), (uint)(size.Y / 20f), font);
        _inputText.BackGroundColor = Color.White;
        _inputText.ForeGroundColor = Color.Black;
        _inputText.Position = new Vector2f(position.X, position.Y + size.Y * 0.16f);
        _inputText.TextEntered += (sender, args) =>
        {
            if (args.Unicode == "\b")
            {
                if (_inputText.Content.Length > 0)
                {
                    _inputText.Content = _inputText.Content.Remove(_inputText.Content.Length - 1);
                    PlayerNameChangedEvent?.Invoke(sender, new PlayerNameChangedEventArgs(_inputText.Content));
                }
            }
            else if (args.Unicode.All(Char.IsAsciiLetterOrDigit) &&
                     (_inputText.Content.Length + args.Unicode.Length) < 18)
            {
                _inputText.Content += args.Unicode;
                PlayerNameChangedEvent?.Invoke(sender, new PlayerNameChangedEventArgs(_inputText.Content));
            }
        };
        _inputText.Content = CurSettings.PlayerName;

        TextEntered += _inputText.OnTextEntered;
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        target.Draw(_background, states);
        target.Draw(_header, states);
        target.Draw(BackButton, states);
        target.Draw(_resolutionsLabel, states);
        target.Draw(_difficultyLabel, states);
        target.Draw(_playerNameLabel, states);
        target.Draw(_inputText, states);
        foreach (var btn in _resolutions)
        {
            target.Draw(btn, states);
        }

        foreach (var btn in _difficulties)
        {
            target.Draw(btn, states);
        }
    }

    public void OnMouseClicked(object? sender, MouseButtonEventArgs args)
    {
        if (IsOpen)
        {
            MouseClicked?.Invoke(sender, args);
        }
    }

    public void OnMouseMoved(object? sender, MouseMoveEventArgs args)
    {
        if (IsOpen)
        {
            MouseMoved?.Invoke(sender, args);
        }
    }

    public void OnTextEntered(object? sender, TextEventArgs args)
    {
        if (IsOpen)
        {
            TextEntered?.Invoke(sender, args);
        }
    }
}