//-----------------------------------------------------------------------
// <copyright file="MapEntitiesLayer.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace TweetHeatFinal
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using Microsoft.Maps.Core;
    using Microsoft.Maps.MapControl;
    using Microsoft.Maps.Plugins;

    /// <summary>
    /// Custom Layer type
    /// contains map entities which represent pushpins
    /// </summary>
    public class MapEntitiesLayer : Layer
    {
        /// <summary>
        /// Private reference to contract create by parent
        /// </summary>
        private TweetHeatPlugin parent;

        /// <summary>
        /// Initializes a new instance of the MapEntitiesLayer class
        /// </summary>
        /// <param name="pluginToken">plugin requirement</param>
        /// <param name="layerTitle">Title shown on layer</param>
        /// <param name="parentPushpinFactoryContract">PushpinFactoryContract from plugin</param>
        public MapEntitiesLayer(PluginToken pluginToken, string layerTitle, TweetHeatPlugin plugin)
            : base(pluginToken)
        {
            // Give the layer a title, is shown above the panel area
            this.Title = layerTitle;
            this.parent = plugin;

            // Handle icon generation requests for the layer(above the panel) and Legend(now 'Map Manager')
            this.IconGenerator = new Func<LayerIconContext, UIElement>(this.RenderIcon);

            // Add the panel
            this.MainPanel = new SamplePanel();
            this.Panel = this.MainPanel;
        }

        /// <summary>
        /// Gets or sets panel seen to the left of the map
        /// </summary>
        public SamplePanel MainPanel { get; set; }

        /// <summary>
        /// Add an entity to the Entities collection
        /// </summary>
        /// <param name="mapEntity">Entity to be added/activated</param>
        public void AddEntity(Entity mapEntity)
        {
            this.Entities.Add(mapEntity);
        }

        /// <summary>
        /// Rendered icon for required LayerIconContext
        /// </summary>
        /// <param name="context">PanelTitle or LayerList(Manage Map/Legend)</param>
        /// <returns>Icon for required LayerIconContext</returns>
        private UIElement RenderIcon(LayerIconContext context)
        {
            switch (context)
            {
                case LayerIconContext.PanelTitle:
                    {
                        Image image = new Image();
                        image.Source = new BitmapImage(new Uri("/" + GetType().Namespace + ";component/Resources/TemplateIcon.png", UriKind.Relative));
                        return image;
                    }

                case LayerIconContext.LayerList:
                    {
                        // create a pushpin primitive
                        PointPrimitive primitive = this.parent.PushpinFactoryContract.CreateStandardPushpin(new Location());
                        if (this.Color.HasValue)
                        {
                            primitive.Color = this.Color;
                        }

                        // render the pushpin primitive to a UIElement
                        UIElement element = primitive.Render(null);
                        return element;
                    }
            }

            return null;
        }
    }
}
