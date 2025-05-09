using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using SokobanGUI.Models;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;

namespace SokobanGUI;

public partial class MainWindow : Window
{
    private readonly GameState _game;
    
    private Dictionary<string, Bitmap> _bitmaps = new();

    public MainWindow()
    {
        Dictionary<Uri, string> uris = new();
        
        uris.Add(new Uri("avares://SokobanGUI/Assets/box.png"), "box");
        uris.Add(new Uri("avares://SokobanGUI/Assets/bateman.png"), "player");
        uris.Add(new Uri("avares://SokobanGUI/Assets/movemouse.png"), "goal");

        foreach (var (uri, name) in uris)
        {
            using var stream = AssetLoader.Open(uri);
            Bitmap bitmap = new Bitmap(stream);
            
            _bitmaps.Add(name, bitmap);
        }
        
        InitializeComponent();
        _game = new GameState();
        RenderGame();
    }
    
    private void RenderGame()
    {
        WinTextBlock.Text = _game.HasWon ? "You won!" : "";
        GameCanvas.Children.Clear();
        const int tileSize = 60;

        for (int y = 0; y < _game.Rows; y++)
        {
            for (int x = 0; x < _game.Cols; x++)
            {
                for (int z = 0; z < 2; z++)
                {
                    var tile = _game.Grid[y, x, z];
                    var rect = new Avalonia.Controls.Shapes.Rectangle
                    {
                        Width = tileSize,
                        Height = tileSize,
                        Fill = tile switch
                        {
                            Wall => Brushes.DarkGray,
                            Player => new ImageBrush(_bitmaps["player"]),
                            Box => new ImageBrush(_bitmaps["box"]),
                            Goal => new ImageBrush(_bitmaps["goal"]),
                            Void => Brushes.Black,
                            null => z == 1 ? null : Brushes.White
                        },
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };

                    Canvas.SetLeft(rect, x * tileSize);
                    Canvas.SetTop(rect, y * tileSize);
                    GameCanvas.Children.Add(rect);
                }
            }
        }
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Left:
                _game.Move(-1, 0);
                break;
            case Key.Right:
                _game.Move(1, 0);
                break;
            case Key.Up:
                _game.Move(0, -1);
                break;
            case Key.Down:
                _game.Move(0, 1);
                break;
        }

        RenderGame();
    }
}