using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

        public JsonObject Json
        {
            get { return (JsonObject)GetValue(JsonProperty); }
            set { SetValue(JsonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Json.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JsonProperty =
            DependencyProperty.Register("Json", typeof(JsonObject), typeof(JsonViewer), new PropertyMetadata(null, JsonValueChanged));


        private static void JsonValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var richTextBlock = (sender as JsonViewer).richTextBlock;
            var jsonValue = args.NewValue as JsonObject;

            richTextBlock.Blocks.Clear();

            if (jsonValue == null)
            {
                return;
            }
            else
            {
                switch (jsonValue.ValueType)
                {
                    case JsonValueType.Array:
                    case JsonValueType.Object:
                    case JsonValueType.Null:
                    case JsonValueType.Number:                    
                    case JsonValueType.String:
                    case JsonValueType.Boolean:
                        var paragraph = new Paragraph();
                        paragraph.Inlines.Add(new Run() { Text = jsonValue.Stringify() });
                        richTextBlock.Blocks.Add(paragraph);
                        break;
                }
            }
        }
    }
}
