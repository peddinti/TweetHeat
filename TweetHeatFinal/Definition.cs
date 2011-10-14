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
using System.Collections.Generic;

namespace TweetHeatFinal
{
    public class Definition
    {
        public class TwitterTweet
        {
            internal string id;
            internal string published;
            internal string link;
            internal string title;
            internal string content;
            internal string updated;
            internal string imageLink;
            internal string location;
            internal string googleLatitude;
            internal string googleLongitude;
            internal string authorName;
            internal string authorUrl;
            internal int sentiment;
        }


        #region static variables
        internal static Dictionary<string, long> PositiveUnigrams = new Dictionary<string, long>();
        internal static Dictionary<string, long> NegativeUnigrams = new Dictionary<string, long>();
        internal static Dictionary<string, long> PositiveBigrams = new Dictionary<string, long>();
        internal static Dictionary<string, long> NegativeBigrams = new Dictionary<string, long>();
        internal static Dictionary<string, long> PositiveTrigrams = new Dictionary<string, long>();
        internal static Dictionary<string, long> NegativeTrigrams = new Dictionary<string, long>();
        internal static long PUC = new long();
        internal static long NUC = new long();
        internal static long PBC = new long();
        internal static long NBC = new long();
        internal static long PTC = new long();
        internal static long NTC = new long();
        #endregion
    }
}
