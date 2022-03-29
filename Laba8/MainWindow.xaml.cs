using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Syntax;
using Syntax.Types;
using Line = Syntax.Elements.Line;
using Element = Syntax.Elements.Element;

namespace Laba8
{
    /// <summary>
    ///     WPF main class
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        ///     Width delta
        /// </summary>
        private const int WidthDelta = 10;

        /// <summary>
        ///     Height delta
        /// </summary>
        private const int HeightDelta = 10;

        /// <summary>
        ///     Resize delta
        /// </summary>
        private const double ResizeDelta = 0.5;

        /// <summary>
        ///     Start point
        /// </summary>
        private Point _startPoint;

        /// <summary>
        ///     Is drawing mode enabled
        /// </summary>
        private bool _isDrawingModeEnabled = false;

        /// <summary>
        ///     Geometry group
        /// </summary>
        private GeometryGroup _currentGroup;

        /// <summary>
        ///     Drawn elements
        /// </summary>
        private List<Element> _drawnElements = new List<Element>();

        /// <summary>
        ///     Grammar
        /// </summary>
        private Grammar _grammar = null;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     On synthesis button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void SynthesisButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();

            var home = _grammar.GetImage();
            
            foreach (var line in home.Lines)
            {
                _drawnElements.Add(TerminalElementCreator.GetTerminalElement(line));
            }

            home.ScaleTransform(
                (WindowGrid.ActualWidth - WidthDelta) / home.Length * ResizeDelta,
                (WindowGrid.ActualHeight - HeightDelta) / home.Height * ResizeDelta
            );

            _currentGroup = home.GetGeometryGroup();

            UpdateImage();
        }

        /// <summary>
        ///     On recognition button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void RecognitionButton_Click(object sender, RoutedEventArgs e)
        {
            var recognizingResult = _grammar.IsImage(_drawnElements);

            MessageBox.Show(
                recognizingResult.IsImage ? "Данный рисунок полностью соответствует граматике." :
                $"Данный рисунок не соответствует грамматике!{Environment.NewLine}Не найден элемент: {recognizingResult.ErrorElementName}."
            );
        }

        /// <summary>
        ///     On clear button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        /// <summary>
        ///     On generate button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var grammarSyntax = new GrammarSyntax(_drawnElements);
                _grammar = grammarSyntax.Grammar;

                GrammarTextBox.Text = _grammar.ToString();
                SynthesisButton.IsEnabled = true;
                RecognitionButton.IsEnabled = true;
            }
            catch (InvalidElementException)
            {
                MessageBox.Show("Рисунок не распознан!");
            }
        }

        /// <summary>
        ///     On window loaded
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClearImage();
        }

        /// <summary>
        ///     Clear image
        /// </summary>
        private void Clear()
        {
            _currentGroup = new GeometryGroup();
            _drawnElements = new List<Element>();

            ClearImage();
        }

        /// <summary>
        ///     Clear image
        /// </summary>
        private void ClearImage()
        {
            _currentGroup = new GeometryGroup();

            _currentGroup.Children.Add(new LineGeometry(new Point(0, 0), new Point(0, WindowGrid.ActualHeight)));
            _currentGroup.Children.Add(new LineGeometry(new Point(0, WindowGrid.ActualHeight), new Point(WindowGrid.ActualWidth, WindowGrid.ActualHeight)));
            _currentGroup.Children.Add(new LineGeometry(new Point(WindowGrid.ActualWidth, WindowGrid.ActualHeight), new Point(WindowGrid.ActualWidth, 0)));
            _currentGroup.Children.Add(new LineGeometry(new Point(WindowGrid.ActualWidth, 0), new Point(0, 0)));

            UpdateImage();
        }

        /// <summary>
        ///     Redraw image
        /// </summary>
        private void UpdateImage()
        {
            Image.Source = new DrawingImage(
                new GeometryDrawing(
                    new SolidColorBrush(Color.FromRgb(75, 0, 130)),
                    new Pen(new SolidColorBrush(Color.FromRgb(75, 0, 130)), 1),
                    _currentGroup
                )
            );
        }

        /// <summary>
        ///     On mouse up
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawingModeEnabled)
            {
                _isDrawingModeEnabled = false;

                _drawnElements.Add(
                    TerminalElementCreator.GetTerminalElement(
                        new Line(GetCoordinates(_startPoint), GetCoordinates(e.GetPosition(Image)))
                    )
                );

                _currentGroup.Children.Add(new LineGeometry(_startPoint, e.GetPosition(Image)));

                UpdateImage();
            }
            else
            {
                _isDrawingModeEnabled = true;
                _startPoint = e.GetPosition(Image);
            }
        }

        /// <summary>
        ///     Update coordinates if needed
        /// </summary>
        ///
        /// <param name="position">Point position</param>
        ///
        /// <returns>Point</returns>
        private Point GetCoordinates(Point position)
        {
            return new Point(position.X, WindowGrid.ActualHeight - 20 - position.Y);
        }
    }
}