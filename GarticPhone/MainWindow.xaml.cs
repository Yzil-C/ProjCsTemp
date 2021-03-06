using GarticPhone.Models;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GarticPhone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public enum State
    {
        pen,
        rubber,
        square,
        circle,
        squareFilled,
        circleFilled,
        line,
        fill,
        undo,
        redo
    }
    public partial class MainWindow : Window
    {
        private Stack<Object> _history = new Stack<Object>();
        private Stack<Object> _historyToRedo = new Stack<Object>();
        private Dictionary<Object,SKPaint> _paintHistory = new Dictionary<Object,SKPaint>();
        private Dictionary<Object, SKPaint> _paintToRedo = new Dictionary<Object, SKPaint>();

		private SKPath _skPath;
		private SKColor _skColor = new SKColor(0,0,0);
		private SKPoint? _skPoint;
        private SKLine _skLine;
        private SKRect _skRect;
        private SKCircle _skCircle;
        private SKPaintStyle _isFilled = SKPaintStyle.Stroke;
        private int _strokeWidth = 5;

		private bool _isMouseDown = false;
        private bool _canRedo = false;
		
        private State _state = State.pen;
        private State _previousState = State.pen;

		public Window Window => Application.Current.MainWindow;
		public DpiScale DpiScale => VisualTreeHelper.GetDpi(Window);


		public MainWindow()
		{
			InitializeComponent();
		}

		private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			var surface = e.Surface;
			var canvas = surface.Canvas;

            //Verifications
            if (_state == State.squareFilled || _state == State.circleFilled || _state == State.fill)
            {
                _isFilled = SKPaintStyle.Fill;
            }
            else
            {
                _isFilled = SKPaintStyle.Stroke;
            }
            if(_state == State.rubber)
            {
                _skColor = new SKColor(255, 255, 255);
            }
            else
            {
                byte r = ((Color)CurrentColor.Fill.GetValue(SolidColorBrush.ColorProperty)).R;
                byte g = ((Color)CurrentColor.Fill.GetValue(SolidColorBrush.ColorProperty)).G;
                byte b = ((Color)CurrentColor.Fill.GetValue(SolidColorBrush.ColorProperty)).B;
                _skColor = new SKColor(r, g, b);
            }

			var paint = new SKPaint()
			{
				Style = _isFilled,
				Color = _skColor,
				StrokeWidth = _strokeWidth
			};
            var paintForPointAndPath = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = _skColor,
                StrokeWidth = _strokeWidth
            };

            switch (_state)
            {
                default:
                    if (_skPath != null)
                    {
                        //_paintHistory.Add(_skPath,paint);
                        _history.Push(_skPath);
                        canvas.DrawPath(_skPath, paint);
                    }
                    if (_skPoint != null)
                    {
                        //_paintHistory.Add(_skPoint,paint);
                        _history.Push(_skPoint);
                        canvas.DrawPoint(_skPoint.Value, paint);
                    }
                    break;
                case State.square:
                    _paintHistory.Add(_skRect,paint);
                    _history.Push(_skRect);
                    canvas.DrawRect(_skRect, paint);
                    break;

                case State.circle:
                    _paintHistory.Add(_skCircle,paint);
                    _history.Push(_skCircle);
                    canvas.DrawCircle(_skCircle.Point, _skCircle.Radius, paint);
                    break;

                case State.squareFilled:
                    _paintHistory.Add(_skRect,paint);
                    _history.Push(_skRect);
                    canvas.DrawRect(_skRect, paint);
                    break;

                case State.circleFilled:
                    _paintHistory.Add(_skCircle,paint);
                    _history.Push(_skCircle);
                    canvas.DrawCircle(_skCircle.Point, _skCircle.Radius, paint);
                    break;

                case State.line:
                    _paintHistory.Add(_skLine,paint);
                    _history.Push(_skLine);
                    canvas.DrawLine(_skLine.FirstPoint, _skLine.SecondPoint, paint);
                    break;

                case State.fill:
                    canvas.DrawPath(_skPath, paint);
                    break;

                case State.undo:
                    if(_history.Count > 0)
                    {
                        _canRedo = true;
                        canvas.Clear();
                        _paintToRedo.Add(_history.Peek(), _paintHistory[_history.Peek()]);
                        _paintHistory.Remove(_history.Peek());
                        _historyToRedo.Push(_history.Pop());
                        foreach (var element in _history)
                        {
                            switch (element.GetType().Name)
                            {
                                case "SKPath":
                                    canvas.DrawPath((SKPath)element, paintForPointAndPath);
                                    break;

                                case "SKPoint":
                                    canvas.DrawPoint((SKPoint)element, paintForPointAndPath);
                                    break;

                                case "SKCircle":
                                    SKCircle tempCircle = (SKCircle)element;
                                    canvas.DrawCircle(tempCircle.Point, tempCircle.Radius, _paintHistory[element]);
                                    break;

                                case "SKRect":
                                    canvas.DrawRect((SKRect)element, _paintHistory[element]);
                                    break;

                                case "SKLine":
                                    SKLine tempLine = (SKLine)element;
                                    canvas.DrawLine(tempLine.FirstPoint, tempLine.SecondPoint, _paintHistory[element]);
                                    break;

                            }
                        }
                        _state = _previousState;
                    }
                    break;

                case State.redo:
                    if (_canRedo == true && _historyToRedo.Count > 0)
                    {
                        var element = _historyToRedo.Pop();
                        switch (element.GetType().Name)
                        {
                            case "SKPath":
                                canvas.DrawPath((SKPath)element, paintForPointAndPath);
                                break;

                            case "SKPoint":
                                canvas.DrawPoint((SKPoint)element, paintForPointAndPath);
                                break;

                            case "SKCircle":
                                SKCircle tempCircle = (SKCircle)element;
                                canvas.DrawCircle(tempCircle.Point, tempCircle.Radius, _paintToRedo[element]);
                                break;

                            case "SKRect":
                                canvas.DrawRect((SKRect)element, _paintToRedo[element]);
                                break;

                            case "SKLine":
                                SKLine tempLine = (SKLine)element;
                                canvas.DrawLine(tempLine.FirstPoint, tempLine.SecondPoint, _paintToRedo[element]);
                                break;
                        }
                        _history.Push(element);
                        _paintHistory.Add(element, _paintToRedo[element]);
                        _paintToRedo.Remove(element);

                    }  
                    break;
            }
            

		}

        private void skElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _canRedo = false;
            var pixelPosition = e.GetPosition(sender as Canvas);
            var x = pixelPosition.X * DpiScale.DpiScaleX;
            var y = pixelPosition.Y * DpiScale.DpiScaleY;
            switch (_state)
            {
                default:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    skElement.InvalidateVisual();
                    break;
                case State.square:
                    break;
                case State.squareFilled:
                    break;
                case State.circle:
                    break;
                case State.circleFilled:
                    break;
                case State.line:
                    break;
                case State.fill:
                    _skPath = new SKPath
                    {
                        FillType = SKPathFillType.InverseWinding
                    };
                    _skPath.MoveTo(new SKPoint((float)x - 250, (float)y - 150));
                    //_skPath.LineTo(new SKPoint((float)x - 250, (float)y - 150));
                    skElement.InvalidateVisual();
                    break;
            }
        }

        private void skElement_MouseMove(object sender, MouseEventArgs e)
        {
			if (_isMouseDown)
			{
                _canRedo = false;
                var pixelPosition = e.GetPosition(sender as Canvas);
                var x = pixelPosition.X * DpiScale.DpiScaleX;
                var y = pixelPosition.Y * DpiScale.DpiScaleY;
                switch (_state)
                {
                    default:
                        var skPoint = new SKPoint((float)x - 250, (float)y - 150);
                        _skPath.LineTo(skPoint);
                        skElement.InvalidateVisual();
                        break;
                    case State.square: 
                        break;
                    case State.circle:
                        break;
                    case State.squareFilled:
                        break;
                    case State.circleFilled:
                        break;
                    case State.line:
                        break;
                    case State.fill:
                        break;
                }
				
			}
		}

        private void skElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _canRedo = false;
			_isMouseDown = true;
			var pixelPosition = e.GetPosition(sender as Canvas);
			var x = pixelPosition.X * DpiScale.DpiScaleX;
			var y = pixelPosition.Y * DpiScale.DpiScaleY;
            switch (_state)
            {
                default:
                    var skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    _skPath = new SKPath();
                    _skPath.MoveTo(skPoint);
                    break;
                case State.square:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    break;

                case State.circle:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    break;

                case State.squareFilled:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    break;

                case State.circleFilled:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    break;

                case State.line:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    break;

                case State.fill:
                    break;
            }
        }

        private void skElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _canRedo = false;
			_isMouseDown = false;
            var pixelPosition = e.GetPosition(sender as Canvas);
            var x = pixelPosition.X * DpiScale.DpiScaleX;
            var y = pixelPosition.Y * DpiScale.DpiScaleY;
            switch (_state)
            {
                default:
                    _skPath = null;
                    break;
                case State.square:
                    _skRect = new SKRect((float)_skPoint.Value.X, (float)_skPoint.Value.Y, (float)(x - 250), (float)(y - 150));
                    skElement.InvalidateVisual();
                    break;

                case State.circle:
                    float radius = (float)Math.Sqrt(Math.Pow((x - 250) - _skPoint.Value.X, 2) + Math.Pow((y - 150) - _skPoint.Value.Y, 2));
                    _skCircle = new SKCircle(_skPoint.Value, radius);
                    skElement.InvalidateVisual();
                    break;

                case State.squareFilled:
                    _skRect = new SKRect((float)_skPoint.Value.X, (float)_skPoint.Value.Y, (float)(x - 250), (float)(y - 150));
                    skElement.InvalidateVisual();
                    break;

                case State.circleFilled:
                    float radiusFilled = (float)Math.Sqrt(Math.Pow((x - 250) - _skPoint.Value.X, 2) + Math.Pow((y - 150) - _skPoint.Value.Y, 2));
                    _skCircle = new SKCircle(_skPoint.Value,radiusFilled);
                    skElement.InvalidateVisual();
                    break;

                case State.line:
                    _skLine = new SKLine(_skPoint.Value, new SKPoint((float)x - 250, (float)y - 150));
                    skElement.InvalidateVisual();
                    break;

                case State.fill:
                    break;
            }

        }

        private void StrokeWidth1_Button_Click(object sender, RoutedEventArgs e)
        {
			_strokeWidth = 3;
        }

        private void StrokeWidth2_Button_Click(object sender, RoutedEventArgs e)
        {
			_strokeWidth = 5;
        }

        private void StrokeWidth3_Button_Click(object sender, RoutedEventArgs e)
        {
			_strokeWidth = 7;
        }

        private void StrokeWidth4_Button_Click(object sender, RoutedEventArgs e)
        {
			_strokeWidth = 10;
        }

        private void StrokeWidth5_Button_Click(object sender, RoutedEventArgs e)
        {
			_strokeWidth = 14;
        }

        private void BlackColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void SlateGrayColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(112, 128, 144));
        }

        private void WhiteColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void MediumOrchidColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(186, 85, 211));
        }

        private void DarkOrchid(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(153, 50, 204));
        }

        private void DarkBlueColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 139));
        }

        private void MediumSlateBlueColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(123, 104, 238));
        }

        private void LightSkyBlueColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(135, 206, 250));
        }

        private void YellowGreenColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(154, 205, 50));
        }

        private void GreenColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(0, 128, 0));
        }

        private void YellowColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 0));
        }

        private void OrangeColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
        }

        private void PinkColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 192, 203));
        }

        private void HotPinkColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 105, 180));
        }

        private void RedColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        }

        private void FireBrickColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(178, 34, 34));
        }

        private void AnthiqueWhiteColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(250, 235, 215));
        }

        private void SaddleBrownColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(139, 69, 19));
        }

        private void OnPen(object sender, RoutedEventArgs e)
        {
            _state = State.pen;
        }

        private void OnRubber(object sender, RoutedEventArgs e)
        {
            _state = State.rubber;
        }

        private void OnSquare(object sender, RoutedEventArgs e)
        {
            _state = State.square;
        }

        private void OnCircle(object sender, RoutedEventArgs e)
        {
            _state = State.circle;
        }

        private void OnSquareFilled(object sender, RoutedEventArgs e)
        {
            _state = State.squareFilled;
        }

        private void OnCircleFilled(object sender, RoutedEventArgs e)
        {
            _state = State.circleFilled;
            
        }

        private void OnLine(object sender, RoutedEventArgs e)
        {
            _state = State.line;
        }

        private void OnFill(object sender, RoutedEventArgs e)
        {
            _state = State.fill;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            _previousState = _state;
            _state = State.undo;
            skElement.InvalidateVisual();
        }

        private void OnRestore(object sender, RoutedEventArgs e)
        {
            _previousState = _state;
            _state = State.redo;
            skElement.InvalidateVisual();
        }
    }
}
