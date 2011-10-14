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
    using Microsoft.Maps.Core;
    using Microsoft.Maps.MapControl;
    using Microsoft.Maps.Plugins;

    /// <summary>
    /// Serialize and Deserialize layer state
    /// Used in permalinks
    /// </summary>
    public class ConcreteLayerSerializationContract : SerializationContract<Layer>
    {
        /// <summary>
        /// private variable to contain the layer. 
        /// </summary>
        private MapEntitiesLayer layer;

        /// <summary>
        /// Initializes a new instance of the ConcreteLayerSerializationContract class
        /// </summary>
        /// <param name="mapEntitiesLayer">Layer to serialize</param>
        public ConcreteLayerSerializationContract(MapEntitiesLayer mapEntitiesLayer)
        {
            this.layer = mapEntitiesLayer;
        }

        /// <summary>
        /// Return a dictionary of keys that represent the state of the layer.
        /// </summary>
        /// <param name="locallayer">Layer to serialize</param>
        /// <returns>dictionary of keys that represent the state of the layer.</returns>
        public override System.Collections.Generic.Dictionary<string, string> Serialize(Layer locallayer)
        {
            // Return a dictionary of keys that represents the state of your layer.
            // Here is an example of a layer that has no distinct state:
            //return new System.Collections.Generic.Dictionary<string, string>();

            // Here is an example of a layer that serializes the value "Value" against the key "Key"
            System.Collections.Generic.Dictionary<string, string> pluginState = new System.Collections.Generic.Dictionary<string, string>();
            pluginState.Add("Key", "Value");
            return pluginState;
        }

        /// <summary>
        /// Customize the creation of a layer using deserialized properties
        /// </summary>
        /// <param name="serializedResult">dictionary of keys that represented the state of the layer</param>
        /// <param name="callbackAction">callback method</param>
        public override void Deserialize(System.Collections.Generic.Dictionary<string, string> serializedResult, Action<Layer> callbackAction)
        {
            // Here is an example that deserializes the value stored against the key "Key"
            string value;
            serializedResult.TryGetValue("Key", out value);

            // Create your layer and call the callback with the layer.  
            // The callback action will automatically add the layer and bring 
            // it to the front
            callbackAction(this.layer);
        }
    }
}
