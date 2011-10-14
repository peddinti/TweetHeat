using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.IO;
using System.Windows.Browser;


namespace TweetHeatFinal
{
    public class TwitterApi : Definition
    {
        public XDocument doc = new XDocument();
        public int test;
        public List<Tweet> GetTweets(Dictionary<string, string> queryParameters)
        {
            List<Tweet> tweetList = new List<Tweet>();
            this.test = 5;
            string nameSpace = "http://www.w3.org/2005/Atom";
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
            if (!string.IsNullOrEmpty(queryParameters["since"]))
            {
                url += "&since_id" + HttpUtility.UrlEncode(queryParameters["since"]);
            }
            if ((!string.IsNullOrEmpty(queryParameters["lat"])) && (!string.IsNullOrEmpty(queryParameters["long"])) && (!string.IsNullOrEmpty(queryParameters["range"])))
            {
                url += "&geocode=" + HttpUtility.UrlEncode(queryParameters["lat"] + "," + queryParameters["long"] + "," + queryParameters["range"]);
            }

            
            // Creating Web Request
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri("http://search.twitter.com/search.atom?q=twitter"));
            string s = Definition.content;            
            return tweetList;
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string s = String.Empty;                        
            if (e.Error == null)
            {
                s = e.Result;
                Definition.content = s;                
                XDocument doc = XDocument.Parse(s);                
           }            
        }

        public Tweet GetTweet(XPathNavigator nav)
        {
            Tweet tweet = new Tweet();
            nav.MoveToFirstChild(); tweet.createdAt = nav.Value;
            nav.MoveToNext(); tweet.from = nav.Value;
            nav.MoveToNext(); tweet.id = nav.Value;
            nav.MoveToNext(); tweet.Latitude = nav.Value;
            nav.MoveToNext(); tweet.Longitude = nav.Value;
            nav.MoveToNext(); nav.MoveToNext(); nav.MoveToNext();
            nav.MoveToNext(); tweet.SourceText = nav.Value;
            nav.MoveToNext(); tweet.Text = nav.Value;
            nav.MoveToNext();
            nav.MoveToNext(); tweet.userName = nav.Value;
            return tweet;
        }       
    }
}
