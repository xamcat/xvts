using System;
using System.Globalization;
using Xamarin.Forms;
namespace MCWeather.Forms.Converters
{
    public class TemperatureFormatterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                var formattedString = new FormattedString();

                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '°')
                    {
                        formattedString.Spans.Add(new Span
                        {
                            Text = text.Substring(0, i - 1),
                            FontSize = 80
                        });
                        formattedString.Spans.Add(new Span
                        {
                            Text = text.Substring(i, text.Length - i),
                            FontSize = 40
                        });

                        break;
                    }
                }
                return formattedString;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
