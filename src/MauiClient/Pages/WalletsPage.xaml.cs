namespace MauiClient.Pages;

public partial class WalletsPage : ContentPage
{
    public WalletsPage(WalletViewModel walletViewModel)
    {
        walletViewModel.InvalidateRequest += () => Canvas.Invalidate();
        InitializeComponent();
        BindingContext = walletViewModel;
        int size = 220;
        Canvas.HeightRequest = size;
        Canvas.WidthRequest = size;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Canvas.Invalidate();
    }
    
    public class MyCanvas : IDrawable
    {
        private float _value1, _value2, _endPoint1, _endPoint2;
        private readonly int _width, _height;
        
        private readonly int _xPadding = 20;
        private readonly int _yPadding = 20;
        
        public MyCanvas(float value1, float value2, int width, int height)
        {
            _width = width - _xPadding;
            _height = height - _yPadding;
            
            UpdateValues(value1, value2);
        }

        public void UpdateValues(float value1, float value2)
        {
            _value1 = Math.Abs(value1);
            _value2 = Math.Abs(value2);
            ComputeValues();
        }
        
        private void ComputeValues()
        {
            _endPoint1 = 0;
            _endPoint2 = 360;
            
            if (_value1 != 0)
            {
                _endPoint1 = Math.Clamp(_value1 * 360 / (_value1 + _value2), 0, 360);
            }
            
            if (_value2 == 0)
            {
                _endPoint2 = _endPoint1;
            }
        }
        
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.StrokeSize = 20;
            canvas.DrawEllipse(10,10,_width, _height);
            
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 18;

            var splitPoint = _endPoint1 - 180;
            var startPoint1 = 0f;
            
            if (splitPoint > 0)
            {
                canvas.DrawArc(10,10,_width,_height,startPoint1,splitPoint, true, false);
                startPoint1 = splitPoint;
            }
            canvas.DrawArc(10,10,_width,_height,startPoint1,_endPoint1, true, false);
            
            canvas.StrokeColor = Colors.Green;
            
            splitPoint =  _endPoint2 - _endPoint1 - 180;
            if (splitPoint > 0)
            {
                canvas.DrawArc(10,10,_width,_height,_endPoint1,splitPoint, true, false);
                _endPoint1 = splitPoint;
            }
            canvas.DrawArc(10,10,_width,_height,_endPoint1,_endPoint2, true, false);
        }
    }
}