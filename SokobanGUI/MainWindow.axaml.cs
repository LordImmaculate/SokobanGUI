using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using SokobanGUI.Models;

namespace SokobanGUI;

public partial class MainWindow : Window
{
    private readonly GameState _game;

    public MainWindow()
    {
        InitializeComponent();
        _game = new GameState();
        RenderGame();
    }
    
    private void RenderGame()
    {
        GameCanvas.Children.Clear();
        const int tileSize = 60;

        for (int y = 0; y < _game.Rows; y++)
        {
            for (int x = 0; x < _game.Cols; x++)
            {
                var tile = _game.Grid[y, x];
                var rect = new Avalonia.Controls.Shapes.Rectangle
                {
                    Width = tileSize,
                    Height = tileSize,
                    Fill = tile switch
                    {
                        Wall => Brushes.DarkGray,
                        Player => Brushes.Blue,
                        Box => Brushes.Brown,
                        Goal => Brushes.Green,
                        Void => Brushes.Black,
                        null => Brushes.White
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