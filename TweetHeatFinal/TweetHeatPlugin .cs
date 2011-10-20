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
    using Microsoft.Maps.Core;
    using Microsoft.Maps.Plugins;
    using Microsoft.Maps.MapControl;
    using BingGeoCodeService;

    using System;    
    using System.Net;
    using System.Windows;
    using System.Windows.Browser;    
    using System.Xml.Linq;
    using System.Xml.XPath;    
    using System.ServiceModel;
    using System.Collections.Generic;
    

    /// <summary>
    /// Entry point for map application plugin
    /// Extends Microsoft.Maps.Plugins.Plugin
    /// </summary>
    public class TweetHeatPlugin : Plugin
    {
        #region Variables
        /// <summary>
        /// Mapping layer
        /// </summary>
        // private CreateLayer mainLayer;
        private MapEntitiesLayer mainLayer;
        /// <summary>
        /// Layer to hold Tweet pushpins
        /// </summary>
        private List<Entity> TweetEntities = new List<Entity>();                                
        private HeatMap heatMap;
        private Queue<Definition.TwitterTweet> tweetsQueue = new Queue<Definition.TwitterTweet>();
        private int TweetCount;

        // UI parameters
        internal string Search;
        internal string Location;
        internal float Range;
        internal LanguageOptions Language;
        internal UiLayers uiLayer;

        internal static TweetHeatPlugin pluginObject;
        private static MapOverlay HeatMapOverlay;
        internal enum LanguageOptions
        {
            English,
            French,
            German,
            Japanese,
            Spanish,
            Portuguese
        }
        private Dictionary<string, string> twitter_lang = new Dictionary<string, string>(){
            {"English","en"},
            {"French","fr"},
            {"German","de"},
            {"Japanese","ja"},
            {"Spanish","pt"},
            {"Portuguese","es"}
        };

        private Dictionary<int, Definition.TwitterTweet> tweet_dict = new Dictionary<int, Definition.TwitterTweet>();

        internal enum UiLayers
        {
            HeatMap,
            TweetMap,
            BothMaps
        }
        
        #endregion

        // Contracts import specific functionality to use in the plugin
        #region Contracts

        /// <summary>
        /// Gets or sets MapContract, defining the main map control
        /// </summary>
        [ImportSingle("Microsoft/MapContract", ImportLoadPolicy.Synchronous)]
        public MapContract DefaultMap { get; set; }

        /// <summary>
        /// Gets or sets LayerManagerContract, for handling layers of map data
        /// </summary>
        [ImportSingle("Microsoft/LayerManagerContract", ImportLoadPolicy.Synchronous)]
        public LayerManagerContract LayerManagerContract { get; set; }

        /// <summary>
        /// Gets or sets CredentialsGrantingContract, for Bing Geo coding API.
        /// </summary>
        [ImportSingle("Microsoft/CredentialsGrantingContract", ImportLoadPolicy.Synchronous)]
        public CredentialsGrantingContract CredentialsGrantingService { get; set; }

        /// <summary>
        /// Gets or sets PushpinFactoryContract, for creation and customization of pushpins
        /// </summary>
        [ImportSingle("Microsoft/PushpinFactoryContract", ImportLoadPolicy.Synchronous)]
        public PushpinFactoryContract PushpinFactoryContract { get; set; }

        /// <summary>
        /// Gets or sets PopupContract, for cretion and customization of popups
        /// </summary>
        [ImportSingle("Microsoft/PopupContract", ImportLoadPolicy.Synchronous)]
        public PopupContract PopupContract { get; set; }

        /// <summary>
        /// Gets LayerSerializationContract, handling custom plugin state information
        /// </summary>
        [ExportSingle("Microsoft/LayerSerializationContract")]
        public SerializationContract<Layer> LayerSerializationContract { get; private set; }

        #endregion

        /// <summary>
        /// Initialize plugin 
        /// provides initialization information for the plug-in after the import and export contracts have been hooked up
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            // Create a new Layer to hold map entities (i.e. heatmap)            
            // this.mainLayer = new CreateLayer(this.Token, "Tweet Heat", this);
            this.mainLayer = new MapEntitiesLayer(this.Token, "Tweet Heat", this);
            ActivateLayer(this.mainLayer);
            heatMap = new HeatMap(DefaultMap);

            // Assign a serialization contract to handle serialization and deserialization 
            // request from the Bing Maps site, saving and restoring custom plugin state
            this.LayerSerializationContract = new ConcreteLayerSerializationContract(this.mainLayer);

            // Importing Training Data and training Sentiment Analyzer
            SentimentAnalyzer sentiment = new SentimentAnalyzer();
            sentiment.Train();           
            
            // Setting the static object to the created TweetHeatPlugin object
            pluginObject = this;
        }

        /// <summary>
        /// Called when the user presses the Submit Button
        /// Gets the Seed Location Geo Coordinates.
        /// Default Geo Coordinates are 47.610020,-122.187549
        /// </summary>
        internal void GetSearchLocation()
        {
            // waiting until training is over
            //while (SentimentAnalyzer.PositiveUnigrams.Count == 0 || SentimentAnalyzer.NegativeUnigrams.Count == 0 || SentimentAnalyzer.PositiveBigrams.Count == 0 || SentimentAnalyzer.NegativeBigrams.Count == 0);                        
            # region Resetting details
            // Setting No. of tweets = 0 for a fresh start.
            tweetsQueue.Clear();
            TweetCount = 0;
            this.mainLayer.Entities.Clear();
            this.TweetEntities.Clear();
            this.heatMap.Points.Clear();            
            #endregion
            EndpointAddress endPointOfService = new EndpointAddress(new Uri(this.CredentialsGrantingService.GeocodeEndpoint));            
            BasicHttpBinding httpBinding = new BasicHttpBinding()
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
            };

            GeocodeServiceClient geocodeClient = new GeocodeServiceClient(httpBinding, endPointOfService);
            geocodeClient.GeocodeCompleted += seedLocationGeocodeClient_GeocodeCompleted;

            GeocodeRequest request = new GeocodeRequest();            
            request.Culture = "en-US";
            request.Query = Location;           
            request.ExecutionOptions = new ExecutionOptions() { SuppressFaults = true };
            this.CredentialsGrantingService.CreateCredentials(
                this.Token,
                (CredentialsResult credentialResult) =>
                {
                    request.Credentials = new Credentials() { ApplicationId = credentialResult.AuthenticationKey };
                    GeocodeRequest1 request1 = new GeocodeRequest1(request);
                    geocodeClient.GeocodeAsync(request1);
                });            
        }

        /// <summary>
        /// The Twitter Module for getting Twitter feeds.
        /// </summary>
        private void GoTwitter()
        {
            // Initialize Parameters            
            string query = CreateTwitterQuery();
            // Getting Tweets
            if(TweetCount == 0)
            GetTweets(query);             
        }

        /// <summary>
        /// Constructs are Twitter API query Url based on the static parameters.
        /// </summary>
        /// <returns>Constructed Url</returns>
        private string CreateTwitterQuery()
        {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters["query"] = Search;
            queryParameters["language"] = twitter_lang[Convert.ToString(Language)];
            queryParameters["rpp"] = "50";
            queryParameters["page"] = "1";            
            queryParameters["lat"] = Location.Split(',')[0];
            queryParameters["long"] = Location.Split(',')[1];
            queryParameters["range"] = Convert.ToString(Range)+"km";

            string url = "http://search.twitter.com/search.atom?";
            url += "q=" + queryParameters["query"];
            if (!string.IsNullOrEmpty(queryParameters["language"]))
            {
                url += "&lang=" + HttpUtility.UrlEncode(queryParameters["language"]);
            }
            if (!string.IsNullOrEmpty(queryParameters["rpp"]))
            {
                url += "&rpp=" + HttpUtility.UrlEncode(queryParameters["rpp"]);
            }
            if (!string.IsNullOrEmpty(queryParameters["page"]))
            {
                url += "&page=" + HttpUtility.UrlEncode(queryParameters["page"]);
            }            
            if ((!string.IsNullOrEmpty(queryParameters["lat"])) && (!string.IsNullOrEmpty(queryParameters["long"])) && (!string.IsNullOrEmpty(queryParameters["range"])))
            {
                url += "&geocode=" + HttpUtility.UrlEncode(queryParameters["lat"] + "," + queryParameters["long"] + "," + queryParameters["range"]);
            }

            return url;
        }

        /// <summary>
        /// Makes the request to obtain the Tweets for a given query url
        /// </summary>
        /// <param name="queryUrl">Url to query</param>
        private void GetTweets(string queryUrl)
        {
            WebClient client = new WebClient();            
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(TweetsDownloadCompleted);
            client.DownloadStringAsync(new Uri(queryUrl));

            // Used for debuggin purpose. Gets Sample XML Data.
            #region Uncomment for Debugging and Comment above all
            //string tweetsXml = SampleData();
            //ParseTwitterXml(tweetsXml);
            #endregion
        }       

        /// <summary>
        /// Parses the resulting xml obtained from Twitter and creates a queue for Geocoding
        /// </summary>
        /// <param name="response">Twitter Xml response</param>
        private void ParseTwitterXml(string response)
        {
            string nameSpace = "http://www.w3.org/2005/Atom";            
            XDocument doc = XDocument.Parse(response);            
            XPathNavigator nav = doc.CreateNavigator();            
            nav.MoveToRoot(); nav.MoveToFirstChild();
            XPathNodeIterator iterator = nav.SelectChildren("entry", nameSpace);           
            foreach (XPathNavigator n in iterator)
            {
                // Adding all tweets to queue;
                tweetsQueue.Enqueue(GetTwitterTweet(n,nameSpace));                
            }
            TweetCount = tweetsQueue.Count;
        }

        /// <summary>
        /// Parses inidivdual record and creates a tweet object from the extracted data
        /// </summary>
        /// <param name="nav">navigation object of the result xml</param>
        /// <returns>Twitter Tweet Object</returns>
        private static Definition.TwitterTweet GetTwitterTweet(XPathNavigator nav,string nameSpace)
        {
            //System.Xml.XmlNamespaceManager manager = new System.Xml.XmlNamespaceManager(nav.NameTable);
            Definition.TwitterTweet tweet = new Definition.TwitterTweet();
            XPathNodeIterator iterator = nav.SelectChildren(XPathNodeType.Element);            
            foreach (XPathNavigator n in iterator)
            {
                switch (n.LocalName)
                {
                    case "id":
                        tweet.id = n.Value;
                        break;
                    case "published":
                        tweet.published = n.Value;
                        break;
                    case "link":
                        if(n.GetAttribute("type","") == "text/html")
                        {
                            tweet.link = n.Value;
                        }
                        else
                        {
                            tweet.imageLink = n.Value;
                        }
                        break;
                    case "title":
                        tweet.title = n.Value;
                        break;
                    case "content":
                        tweet.content = n.Value;
                        break;
                    case "updated":
                        tweet.updated = n.Value;
                        break;
                    case "geo":
                        if (n.Value != "")
                        {
                            XPathNodeIterator ngeo = n.SelectDescendants(XPathNodeType.Element, false);
                            ngeo.MoveNext();
                            tweet.googleLatitude = ngeo.Current.Value.Split(' ')[0];
                            tweet.googleLongitude = ngeo.Current.Value.Split(' ')[1];                            
                        }
                        break;
                    case "location":
                        if (n.Value != "")
                        {
                            tweet.location = n.Value;
                        }
                        break;
                    case "place":
                        if (n.Value != "")
                        {
                            XPathNodeIterator nplc = n.SelectDescendants(XPathNodeType.Element, false);
                            nplc.MoveNext(); nplc.MoveNext();
                            tweet.location = nplc.Current.Value;
                        }
                        break;
                    case "author":
                        XPathNodeIterator naut = n.SelectDescendants(XPathNodeType.Element,false);
                        naut.MoveNext();
                        tweet.authorName = naut.Current.Value;
                        naut.MoveNext();
                        tweet.authorUrl = naut.Current.Value;                        
                        break;
                }
                
            }            
            return tweet;
        }        

        /// <summary>
        /// Creates the Geocoding request for the Location values obtained from the twitter feed.
        /// If the feed already contains the latitude and longitude information. No query is made.
        /// </summary>
        /// <param name="queryLocation">The location for which Geo codes are computed</param>
        private void MakeGeoCodingRequest(Definition.TwitterTweet Geotweet)
        {
            // if latitude and longitude values are not available then use geocoding
            if (Geotweet.googleLatitude == "" || Geotweet.googleLongitude == "" || Geotweet.googleLatitude == null || Geotweet.googleLongitude == null)
            {
                EndpointAddress endPointOfService = new EndpointAddress(new Uri(this.CredentialsGrantingService.GeocodeEndpoint));
                BasicHttpBinding httpBinding = new BasicHttpBinding()
                {
                    MaxReceivedMessageSize = int.MaxValue,
                    MaxBufferSize = int.MaxValue,
                };

                GeocodeServiceClient geocodeClient = new GeocodeServiceClient(httpBinding, endPointOfService);
                geocodeClient.GeocodeCompleted += geocodeClient_GeocodeCompleted;

                GeocodeRequest request = new GeocodeRequest();
                request.Culture = "en-US";
                request.Query = Geotweet.location;
                #region Create Confidence filters - Commented out
                //ConfidenceFilter filter =  new ConfidenceFilter;
                //filter.MinimumConfidence = Confidence.High;
                //GeocodeOptions options = GeocodeOptions();
                //options.Filters = filter;
                //request.Options = options;
                #endregion
                request.ExecutionOptions = new ExecutionOptions() { SuppressFaults = true };
                this.CredentialsGrantingService.CreateCredentials(
                    this.Token,
                    (CredentialsResult credentialResult) =>
                    {
                        request.Credentials = new Credentials() { ApplicationId = credentialResult.AuthenticationKey };
                        GeocodeRequest1 request1 = new GeocodeRequest1(request);
                        geocodeClient.GeocodeAsync(request1);
                    });
            }
            else
            {
                // contains inherent Coordinates - No need to call API
                Definition.TwitterTweet tweet = tweetsQueue.Dequeue();
                // the values are already present in the latitude and longitude coordinates
                DrawPoint(tweet);
                if (tweetsQueue.Count > 0)
                {
                    // Get Geo for next tweet
                    tweet = tweetsQueue.Peek();
                    MakeGeoCodingRequest(tweet);
                }
                else
                {
                    // Do nothing
                }
            }
        }                

        /// <summary>
        /// Performs the task of plotting the Twitter feeds on map. It palces tweets as pushpins and also creates the Heat Map
        /// Inherently calls the Geocoding to get the Geo Code locations for plotting on Map.
        /// </summary>
        private void MapArtist()
        {
            // Adding Heat Map to the overlays
            HeatMapOverlay = new MapOverlay(heatMap);
            this.mainLayer.MapOverlays.Add(HeatMapOverlay);
            //updateLayers();
            if (tweetsQueue.Count > 0)
            {
                Definition.TwitterTweet tweet = tweetsQueue.Peek();                
                MakeGeoCodingRequest(tweet);
            }            
        }

        /// <summary>
        /// Does the Action of Creating the colour for the tweets.
        /// performs sentiment analysis, assigns the color and adds the point.
        /// </summary>
        /// <param name="tweet">Tweet Object fow which a point is assigned</param>
        private void DrawPoint(Definition.TwitterTweet tweet)
        {
            // Calculating the Sentiment.
            SentimentAnalyzer sentiment = new SentimentAnalyzer();
            int score = sentiment.SA(tweet.content);
            tweet.sentiment = score;
            // Creating an entity for each tweet and plotting it.
            if (!string.IsNullOrEmpty(tweet.googleLatitude))
            {
                Location tweetLoc = new Location(Convert.ToDouble(tweet.googleLatitude), Convert.ToDouble(tweet.googleLongitude));
                byte red = Convert.ToByte(63 * (5 - tweet.sentiment));
                byte green = Convert.ToByte(63 * (tweet.sentiment - 1));
                AddPin(tweetLoc, red, green, tweet);
                //DefaultMap.SetView(tweetLoc, 2);
                
                // Adding point to Heat Map
                HeatMapPoint point = new HeatMapPoint(new Location(Convert.ToDouble(tweet.googleLatitude), Convert.ToDouble(tweet.googleLongitude)), (tweet.sentiment-3)/2, 0.25);                
                heatMap.Points.Add(point);
            }
        }        

        /// <summary>
        /// Updates the Layers when user chooses to see either the Heat Map or Twitter Pushpins or both of them.
        /// </summary>
        internal void updateLayers()
        {
            if (LayerManagerContract.ContainsLayer(mainLayer))
            switch (uiLayer)
            {
                case UiLayers.HeatMap:                    
                    this.heatMap.Visibility = Visibility.Visible;
                    this.mainLayer.Entities.Clear();                    
                    break;
                case UiLayers.TweetMap:
                    this.heatMap.Visibility = Visibility.Collapsed;                    
                    if(this.mainLayer.Entities.Count == 0)
                        // AddEntities(this.mainLayer, this.TweetEntities);
                        foreach (Entity entity in TweetEntities)
                        {
                            this.mainLayer.AddEntity(entity);
                        }                        
                    break;
                case UiLayers.BothMaps:
                    if (this.mainLayer.Entities.Count == 0)
                        // AddEntities(this.mainLayer, this.TweetEntities);
                        foreach (Entity entity in TweetEntities)
                        {
                            this.mainLayer.AddEntity(entity);
                        }
                    if (this.heatMap.Visibility == Visibility.Collapsed)
                        this.heatMap.Visibility = Visibility.Visible;                    
                    break;
            }
        }

        #region Event Handlers

        /// <summary>
        /// Called when the query to Twitter Api returns an Xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TweetsDownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string tweetsXml;
            if (e.Error == null)
            {
                tweetsXml = e.Result;
                ParseTwitterXml(tweetsXml);
                MapArtist();
            }
        }

        /// <summary>
        /// Caleed when the Bing GeoLocation API returns the geolocation coordinates for the tweet location.
        /// Checks for the queue. If queue is empty it returns the control. Else a new API request is made for the next tweet in the queue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void geocodeClient_GeocodeCompleted(object sender, GeocodeCompletedEventArgs args)
        {
            GeocodeResponse1 response1 = args.Result;
            GeocodeResponse response = response1.GeocodeResult;            
            
            if (tweetsQueue.Count == 0)
                return;
            Definition.TwitterTweet tweet = tweetsQueue.Dequeue();
            if (response.Results != null && response.Results.Count > 0)
            {
                tweet.googleLatitude = response.Results[0].Locations[0].Latitude + "";
                tweet.googleLongitude = response.Results[0].Locations[0].Longitude + "";
            }
            else
            {
                tweet.googleLatitude = "";
                tweet.googleLongitude = "";
            }

            DrawPoint(tweet);
            if (tweetsQueue.Count > 0)
            {
                // Start Api request again
                tweet = tweetsQueue.Peek();
                MakeGeoCodingRequest(tweet);
            }
            else
            {
                // Update Layers
                updateLayers();
            }
        }

        /// <summary>
        /// Called when the Bing GeoLocation API returns the geolocation coordinates for the Seed location provided by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void seedLocationGeocodeClient_GeocodeCompleted(object sender, GeocodeCompletedEventArgs args)
        {
            GeocodeResponse1 response1 = args.Result;
            GeocodeResponse response = response1.GeocodeResult;
            Location = response.Results[0].Locations[0].Latitude + "," + response.Results[0].Locations[0].Longitude;
            // Setting Initial view
            Location loc = new Location(response.Results[0].Locations[0].Latitude, response.Results[0].Locations[0].Longitude);            
            DefaultMap.SetView(loc, Math.Min(20, (1500 / this.Range)));
            // Calling the Twitter Module after getting the Seed Location
            GoTwitter();
        }        

        /// <summary>
        /// Called when popup is triggered for a pushpin
        /// </summary>
        /// <param name="context"></param>
        private void OnShowPopup(PopupStateChangeContext context)
        {
            int entityid = context.Entity.GetHashCode();
            Definition.TwitterTweet tweet;
            if (tweet_dict.ContainsKey(entityid))
            {
                tweet = tweet_dict[entityid];
            }
            else
            {
                return;
            }            
            
            if (context.State == PopupState.Preview)
            {
                context.Title = "Posted by " + tweet.authorName;
            }
            if (context.State == PopupState.Normal)
            {
                context.Title = tweet.authorName;
                string content = "";
                // Wrapping Text
                int i = 0;
                int len = tweet.title.Length;
                while ((len-i) > 50)
                {
                    content += tweet.title.Substring(i, 50) + "\n";
                    i += 50;                    
                }
                content += tweet.title.Substring(i) + "\n\n";
                content += "Sentiment Score: " + tweet.sentiment + "\n";                                
                DateTime time = DateTime.Parse(tweet.published);
                content += "Posted at: " + time.ToLocalTime();
                
                context.Content = content;
            }
        }



        #endregion

        #region Standard Map Functions

        /// <summary>
        /// Creates a single pushpin at the center of the map
        /// </summary>
        private void AddPin(Location loc, byte red, byte green, Definition.TwitterTweet tweet)
        {
            // Create the pushpin primative at the current map's center point
            // optional text property should be three or less characters to be readable
            PointPrimitive pushpinPrimitive = PushpinFactoryContract.CreateStandardPushpin(loc, "t");
            pushpinPrimitive.Color = System.Windows.Media.Color.FromArgb(255, red, green, 0);                        
            Entity entity = new Entity(pushpinPrimitive);            
            
            // Register Popup for the pushpin
            if (PopupContract != null)
            {
                PopupContract.Register(entity, OnShowPopup);
                tweet_dict[entity.GetHashCode()] = tweet;
            }

            // Add this entity to the layer
            this.TweetEntities.Add(entity);
            if(uiLayer == UiLayers.TweetMap||uiLayer == UiLayers.BothMaps)
                this.mainLayer.AddEntity(entity);            
        }        

        ///// <summary>
        ///// Activate layer by either creating it new or making it active
        ///// </summary>
        ///// <param name="layer">Layer to add/activate</param>
        //private void ActivateLayer(CreateLayer layer)
        //{
        //    if (LayerManagerContract.ContainsLayer(layer))
        //    {
        //        // If layer already exists bring to front
        //        LayerManagerContract.BringToFront(layer);
        //    }
        //    else
        //    {
        //        // Otherwise create the layer for this first time
        //        LayerManagerContract.AddLayer(layer);
        //    }
        //}

        private static void AddEntities(CreateLayer layer, IList<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                layer.AddEntity(entity);
            }
        }
        #endregion

        /// <summary>
        /// Activate layer by either creating it new or making it active
        /// </summary>
        /// <param name="layer">Layer to add/activate</param>
        private void ActivateLayer(Layer layer)
        {
            if (LayerManagerContract.ContainsLayer(layer))
            {
                // If layer already exists bring to front
                LayerManagerContract.BringToFront(layer);
            }
            else
            {
                // Otherwise create the layer for this first time
                LayerManagerContract.AddLayer(layer);
            }
        }        
    }
}
