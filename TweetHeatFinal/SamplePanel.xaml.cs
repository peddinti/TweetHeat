/* Code Title: Tweet Heat Final
 * Version: 1.0
 * Author: Raghava Viswa Mani Kiran Peddinti
 * Initials: VMK
 * Description:
 *      This Plugin to Bing Maps Provides the User a graphical representation of the Twitter users feeling towards any product.
 *      It takes a Search word, Seed Location, Location Range, Language and a Plot Style.
 *      Search word: this is the keyword used to fetch twitter feeds; DEFAULT VALUE: bing
 *      Seed Location: This is teh central location from where feeds are fetched within the Location Range DEFAULT VALUE:bellevue (47.610020,-122.187549)
 *      Location Range: This is the range in KM from where the Feeds are fetched. MINIMUM VALUE: 1 KM
 *      Language: This is the Language with which tweets are twitted. DEFAULT VALUE: ENG (english)
 *      Plot Style: This is the graphical representation of the tweets. Can be a Heat Map or a Push pin for each tweet or both. DEFAULT STYLE: both
*/
using System.Windows;
using System.Windows.Controls.Primitives;
using System;
namespace TweetHeatFinal
{
    using System.Windows.Controls;

    /// <summary>
    /// A sample panel control
    /// Width of your UserControl must not exceed 350px 
    /// to fit in the space available without being cropped
    /// </summary>
    public partial class SamplePanel : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the SamplePanel class
        /// </summary>
        internal SamplePanel()
        {
            InitializeComponent();
        }

        private void clear_click(object sender, RoutedEventArgs e)
        {   
            search.Text = "";
            loc.Text = "";
            lang.SelectedIndex = -1;
            range.Value = 300;
            hm.IsChecked = false;
            tp.IsChecked = false;
            both.IsChecked = true;
            string name = tabcontrol1.Name;
            name = tab1.Name;
            name = tab2.Name;
            
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(search.Text))
                search.Text = "Bing";
            TweetHeatPlugin.pluginObject.Search = search.Text;
            if (string.IsNullOrEmpty(loc.Text))
                loc.Text = "Bellevue, WA";
            TweetHeatPlugin.pluginObject.Location = loc.Text;
            if (lang.SelectedIndex < 0)
                lang.SelectedIndex = 0;
            TweetHeatPlugin.pluginObject.Language = (TweetHeatPlugin.LanguageOptions)Enum.Parse(typeof(TweetHeatPlugin.LanguageOptions), lang.SelectionBoxItem.ToString(), true);            
            TweetHeatPlugin.pluginObject.Range = (float)range.Value;
            if (hm.IsChecked == true)
                TweetHeatPlugin.pluginObject.uiLayer = TweetHeatPlugin.UiLayers.HeatMap;
            else if (tp.IsChecked == true)
                TweetHeatPlugin.pluginObject.uiLayer = TweetHeatPlugin.UiLayers.TweetMap;
            else
                TweetHeatPlugin.pluginObject.uiLayer = TweetHeatPlugin.UiLayers.BothMaps;

            TweetHeatPlugin.pluginObject.GetSearchLocation();
        }

        private void range_valueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {   
            if (range == null)
                return;                       
            range_value.Text = Convert.ToInt32(range.Value) + " km";
        }

        private void hm_Checked(object sender, RoutedEventArgs e)
        {
            TweetHeatPlugin.pluginObject.uiLayer = (TweetHeatPlugin.UiLayers)Enum.Parse(typeof(TweetHeatPlugin.UiLayers), "HeatMap", true);
            TweetHeatPlugin.pluginObject.updateLayers();
            hm.IsChecked = true;
        }

        private void tp_Checked(object sender, RoutedEventArgs e)
        {
            TweetHeatPlugin.pluginObject.uiLayer = (TweetHeatPlugin.UiLayers)Enum.Parse(typeof(TweetHeatPlugin.UiLayers), "TweetMap", true);
            TweetHeatPlugin.pluginObject.updateLayers();
            tp.IsChecked = true;
        }

        private void both_Checked(object sender, RoutedEventArgs e)
        {
            if (TweetHeatPlugin.pluginObject == null)
                return;
            TweetHeatPlugin.pluginObject.uiLayer = (TweetHeatPlugin.UiLayers)Enum.Parse(typeof(TweetHeatPlugin.UiLayers), "BothMaps", true);
            TweetHeatPlugin.pluginObject.updateLayers();
            both.IsChecked = true;
        } 
    }
}
