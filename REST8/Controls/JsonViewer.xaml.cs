using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace REST8.Controls
{
    public sealed partial class JsonViewer : UserControl
    {
        public JsonViewer()
        {
            this.InitializeComponent();
        }

        public IJsonValue Json
        {
            get { return (IJsonValue)GetValue(JsonProperty); }
            set { SetValue(JsonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Json.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JsonProperty =
            DependencyProperty.Register("Json", typeof(IJsonValue), typeof(JsonViewer), new PropertyMetadata(null, JsonValueChanged));


        private static void JsonValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var viewer = sender as JsonViewer;
            var jsonValue = args.NewValue as IJsonValue;

            viewer.richTextBlock.Blocks.Clear();

            if (jsonValue == null)
            {
                return;
            }
            else
            {
                viewer.RenderValue(jsonValue, 0.0);
            }
        }

        #region Rendering
        private double defaultIndent = 30;

        private Brush DelimitersBrush = new SolidColorBrush(Colors.Black);
        private Brush KeyBrush = new SolidColorBrush(Colors.Red);
        private Brush StringBrush = new SolidColorBrush(Colors.Green);
        private Brush NumberBrush = new SolidColorBrush(Colors.Cyan);
        private Brush BooleanBrush = new SolidColorBrush(Colors.Blue);

        private void RenderValue(IJsonValue json, double indent)
        {
            switch (json.ValueType)
            {
                case JsonValueType.Array:
                    RenderArray(json.GetArray(), indent);
                    break;
                case JsonValueType.Object:
                    RenderObject(json.GetObject(), indent);
                    break;
                case JsonValueType.Null:
                    AddInlines(new Run() { Text = "null", FontStyle = Windows.UI.Text.FontStyle.Italic, Foreground = BooleanBrush });
                    break;
                case JsonValueType.Number:
                    AddInlines(new Run() { Text = json.GetNumber().ToString(), Foreground = NumberBrush });
                    break;
                case JsonValueType.String:
                    AddInlines(new Run() { Text = "\"" + json.GetString() + "\"", Foreground = StringBrush });
                    break;
                case JsonValueType.Boolean:
                    AddInlines(new Run() { Text = json.GetBoolean().ToString(), Foreground = BooleanBrush });
                    break;
            }
        }

        private void RenderArray(JsonArray json, double indent)
        {
            AddParagraph(indent, new Run() { Text = "[", Foreground = DelimitersBrush });

            foreach (var value in json)
            {
                AddParagraph(indent + defaultIndent);
                RenderValue(value, indent + defaultIndent);
                AddInlines(new Run() { Text = ",", Foreground = DelimitersBrush });
            }

            AddParagraph(indent, new Run() { Text = "]", Foreground = DelimitersBrush });
        }

        private void RenderObject(JsonObject json, double indent)
        {
            AddParagraph(indent, new Run() { Text = "{", Foreground = DelimitersBrush });

            foreach (var value in json.Keys)
            {
                AddParagraph(indent + defaultIndent, new Run() { Text = "\"" + value + "\":", Foreground = KeyBrush });
                RenderValue(json[value], indent + defaultIndent);
                AddInlines(new Run() { Text = ",", Foreground = DelimitersBrush });
            }

            AddParagraph(indent, new Run() { Text = "}", Foreground = DelimitersBrush });
        }

        private void AddInlines(params Inline[] inlines)
        {
            if (richTextBlock.Blocks.Any())
            {
                foreach (var inline in inlines)
                {
                    (richTextBlock.Blocks.Last() as Paragraph).Inlines.Add(inline);
                }
            }
            else
            {
                AddParagraph(0, inlines);
            }
        }

        private void AddParagraph(double indent, params Inline[] inlines)
        {
            var paragraph = new Paragraph();
            paragraph.LineHeight = 15;
            paragraph.TextIndent = indent;
            if (inlines != null)
            {
                foreach (var inline in inlines)
                {
                    paragraph.Inlines.Add(inline);
                }
            }
            richTextBlock.Blocks.Add(paragraph);
        }
        #endregion
    }
}
