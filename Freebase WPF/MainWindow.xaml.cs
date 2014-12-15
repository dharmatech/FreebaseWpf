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

using RestSharp;
using Newtonsoft.Json;

namespace Freebase_WPF
{
    public class ValueSet
    {
        public string valuetype;
        public List<Dictionary<string, object>> values;
        public double count;
    }

    public class FreebaseTopic
    {
        public string id { get; set; }
        public Dictionary<string, ValueSet> property { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void topicTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                topicPropertiesListBoxA.ItemsSource = 
                    JsonConvert.DeserializeObject<FreebaseTopic>(
                        new RestClient("https://www.googleapis.com")
                            .Execute(new RestRequest(String.Format("freebase/v1/topic{0}", topicTextBox.Text)))
                            .Content)
                        .property;

                propertyValuesListBoxA.ItemsSource = null;
                topicPropertiesListBoxB.ItemsSource = null;
                propertyValuesListBoxB.ItemsSource = null;
            }
        }

        private void topicPropertiesListBoxA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (topicPropertiesListBoxA.SelectedItem != null)
                propertyValuesListBoxA.ItemsSource = ((KeyValuePair<string, ValueSet>)topicPropertiesListBoxA.SelectedItem).Value.values;

            topicPropertiesListBoxB.ItemsSource = null;
            propertyValuesListBoxB.ItemsSource = null;
        }

        private void propertyValuesListBoxA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = propertyValuesListBoxA.SelectedItem;

            if (obj is Dictionary<string, object>)
            {
                var tbl = obj as Dictionary<string, object>;

                if (tbl.ContainsKey("id"))
                {
                    topicPropertiesListBoxB.ItemsSource =
                        JsonConvert.DeserializeObject<FreebaseTopic>(
                            new RestClient("https://www.GoogleAPIs.com")
                                .Execute(new RestRequest(String.Format("freebase/v1/topic{0}", tbl["id"])))
                                .Content)
                        .property;
                }
                else topicPropertiesListBoxB.ItemsSource = new List<int>();
            }

            propertyValuesListBoxB.ItemsSource = null;
        }

        private void topicPropertiesListBoxB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (topicPropertiesListBoxB.SelectedItem != null)
                propertyValuesListBoxB.ItemsSource = ((KeyValuePair<string, ValueSet>)topicPropertiesListBoxB.SelectedItem).Value.values;
        }
    }
}
