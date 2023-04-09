using System.Reflection;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Arkanoid;

public class Game
{
    private GameField _gameField;
    private Players _players = new Players();
    private Menu _menu = new Menu();
    
    private static readonly Time TimePerFrame = Time.FromSeconds(1f / 144f);
    
    private Font? _mainFont;
    private Text _statisticsText = new();
    private Time _statisticsUpdateTime = Time.Zero;
    private int _statisticsNumFrames;
    
    private readonly RenderWindow _window = new(new VideoMode(1600, 900), "Arkanoid");
    
    public Game()
    {
        try
        {
            _mainFont = new Font("fonts/Sansation.ttf");
            _statisticsText.Font = _mainFont;
            _statisticsText.CharacterSize = 15u;
            _statisticsText.Position = new Vector2f(5f, 5f);
        }
        catch (SFML.LoadingFailedException e)
        {
            Console.WriteLine(e);
        }

        Player player = new Player("anonymous");
        _players.AddPlayer(player);
        
        _gameField = new GameField(_window.GetView().Size, player);
        
        _window.SetKeyRepeatEnabled(false);
        _window.Closed += (sender, args) => _window.Close();
        _window.MouseButtonPressed += (sender, args) => _gameField.StartGame();
        _window.KeyPressed += _players.OnKeyPressed;
        _window.KeyReleased += _players.OnKeyReleased;
    }

    public void Run()
    {
        Clock clock = new ();
        Time timeSinceLastUpdate = Time.Zero;

        while (_window.IsOpen)
        {
            Time dt = clock.Restart();
            timeSinceLastUpdate += dt;
            while (timeSinceLastUpdate >= TimePerFrame)
            {
                timeSinceLastUpdate -= TimePerFrame;

                _window.DispatchEvents();
                Update(TimePerFrame);
            }

            UpdateStatisticText(dt);
            Render();
        }
    }

    private void Render()
    {
        _window.Clear();
        _window.Draw(_gameField);
        // _window.Draw(_menu);
        _window.Draw(_statisticsText);
        _window.Display();
    }

    private void UpdateStatisticText(Time dt)
    {
        _statisticsNumFrames++;
        _statisticsUpdateTime += dt;

        if (_statisticsUpdateTime >= Time.FromSeconds(1f))
        {
            _statisticsText.DisplayedString = "FPS: " + _statisticsNumFrames + "\n" +
                                      "Update: " + _statisticsUpdateTime.AsMicroseconds() / _statisticsNumFrames + "ms";
            _statisticsNumFrames = 0;
            _statisticsUpdateTime -= Time.FromSeconds(1f);
        }
    }
    

    private void Update(Time elapsedTime)
    {
        _gameField.Update(elapsedTime);
    }

    public void SaveGame()
    {
        throw new NotImplementedException();
    }

    public void LoadGame()
    {
        throw new NotImplementedException();
    }

    public void PauseGame()
    {
        throw new NotImplementedException();
    }
}