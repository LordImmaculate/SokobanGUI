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
    
    private Bitmap wallImage;
    private Bitmap playerImage;
    private Bitmap boxImage;
    private Bitmap goalImage;
    private Bitmap voidImage;

    public MainWindow()
    {
        InitializeComponent();
        _game = new GameState();
        RenderGame();

        var uris = new Dictionary<Bitmap, Uri>();
        
        uris.Add(boxImage, new Uri("avares://SokobanGUI/Assets/box.png"));

        foreach (var (bitmap, uri) in uris)
        {
            using var stream = AssetLoader.Open(uri);
            bitmap = new Bitmap(stream);
        }
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
                            Player => Brushes.Blue,
                            Box => new ImageBrush(wallImage),
                            Goal => Brushes.Green,
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