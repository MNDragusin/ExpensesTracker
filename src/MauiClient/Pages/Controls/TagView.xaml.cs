namespace MauiClient.Pages.Controls
{
    public partial class TagView
    {
        public static BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(TagView),
            propertyChanged: (bindable, oldValue, newValue) => 
            {
                var control = (TagView)bindable;
                control.TitleLabel.Text = newValue as string;
            });

        public static BindableProperty HexColorProperty = BindableProperty.Create(nameof(HexColor), typeof(string), typeof(TagView),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (TagView)bindable;
                control.ColorBadge.Background = HexConverter.Convert(newValue, typeof(Color), null, System.Globalization.CultureInfo.CurrentCulture) as Color;
            });

        public TagView()
        {
            InitializeComponent();
            BindingContext = this;

            ColorBadge.Background = HexConverter.Convert(HexColor, typeof(Color), null, System.Globalization.CultureInfo.CurrentCulture) as Color;
            TitleLabel.Text = Name ;
        }

        public string Name { 
            get => (string)GetValue(NameProperty);  
            set => SetValue(NameProperty, value); }

        public string HexColor { 
            get => (string)GetValue(HexColorProperty);
            set => SetValue(HexColorProperty, value); }
    }
}