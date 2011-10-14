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
namespace TweetHeatFinal
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using Microsoft.Maps.Core;
    using Microsoft.Maps.MapControl;
    using Microsoft.Maps.Plugins;

    /// <summary>
    /// Simple layer class to display map entities
    /// </summary>
    public class CreateLayer : Layer
    {
        /// <summary>
        /// Private reference parent plugin
        /// </summary>
        private TweetHeatPlugin parent;

        /// <summary>
        /// Initializes a new instance of the SampleLayer class
        /// </summary>
        /// <param name="pluginToken">plugin token</param>
        /// <param name="layerTitle">Title to display on layer</param>
        /// <param name="plugin">Title for layer shown in panel on left hand side</param>
        public CreateLayer(PluginToken pluginToken, string layerTitle, TweetHeatPlugin plugin)
            : base(pluginToken)
        {
            // Give the layer a title, is shown above the panel area on the left hand side
            this.Title = layerTitle;
            this.parent = plugin;

            // Handle icon generation requests for the layer (above the panel) and 'map manager'
            this.IconGenerator = new Func<LayerIconContext, UIElement>(this.RenderIcon);

            // Add a new sample panel control
            this.Panel = new SamplePanel();
        }

        /// <summary>
        /// Add an entity to the Entities collection
        /// </summary>
        /// <param name="mapEntity">Entity to be added/activated</param>
        public void AddEntity(Entity mapEntity)
        {
            this.Entities.Add(mapEntity);
        }

        /// <summary>
        /// Handles the rendering of icons used in the layer title and 'map manager'
        /// </summary>
        /// <param name="context">Identifies whether panel icon or layer list icon is requested</param>
        /// <returns>Icon for required LayerIconContext</returns>
        private UIElement RenderIcon(LayerIconContext context)
        {
            switch (context)
            {
                case LayerIconContext.PanelTitle:
                    {
                        // Return an image to be used for the layer title shown above the panel control on the left
                        Image image = new Image();

                        // As the image file is an embedded resource you need to add "/AssemblyName;component/" before the image path
                        image.Source = new BitmapImage(new Uri("/TweetHeatFinal;component/Resources/TweetHeatIcon.png", UriKind.Relative));
                        return image;
                    }

                case LayerIconContext.LayerList:
                    {
                        // Return a custom UIElement to be used in the 'map manager' using the basic pushpin control

                        // Create a pushpin primitive
                        PointPrimitive primitive = this.parent.PushpinFactoryContract.CreateStandardPushpin(new Location(0, 0));

                        // Use the layers color for the new pushpin control
                        if (this.Color.HasValue)
                        {
                            primitive.Color = this.Color;
                        }

                        // Render the pushpin primitive to a UIElement
                        UIElement element = primitive.Render(null);
                        return element;
                    }
            }

            return null;
        }
    }
}
