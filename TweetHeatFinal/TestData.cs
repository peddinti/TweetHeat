using System.Collections.Generic;
using TweetHeatFinal.Resources;
using Microsoft.Maps.Core;
using Microsoft.Maps.Plugins;
using Microsoft.Maps.MapControl;
using System;
using System.Net;
using System.Windows.Browser;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text.RegularExpressions;

namespace TweetHeatFinal
{
    public class TestData : Definition
    {
        public List<Definition.TwitterTweet> GetData1()
        {
            // Data from Twitter
            List<Definition.TwitterTweet> tweetList = new List<Definition.TwitterTweet>();
            List<string> stringList = new List<string>();
            stringList.Add("Hey, everything is about dot-this and dot-that. Dotta-bing, dotta-boom and you're done, Oh!  #dnrlive;;#;;43.648565;;#;;-79.385329;;#;;5");
            stringList.Add("I'm off to raise hell at the Save Our Parks rally.  Mayor Dave Bing and the City Council want to close 77 parks in Detroit.  #Detroit;;#;;42.350561;;#;;-83.044769;;#;;5");
            stringList.Add("RT @bing RT @rickdejarnette: POST from Bing Webmaster Center: Bing crawler: bingbot on the horizon  http://bit.ly/9Ukurb;;#;;39.952270;;#;;-75.162369;;#;;5");
            stringList.Add("Retweet this & @Bing will donate $10 to @CNN's Gulf Telethon up to $100K. More at http://bit.ly/howtodonate. #BingforGulf;;#;;40.143230;;#;;-74.726707;;#;;5");
            stringList.Add("@bing iPhone app-Tighten up some functional stuff & you've got me going to 1 place for all my social needs. Search & Maps will follow.;;#;;41.410135;;#;;-75.660794;;#;;5");
            stringList.Add("@carolanneeee im only shady when im sitting under a tree in the summer.  bada bing!;;#;;40.143230;;#;;-74.726707;;#;;5");
            stringList.Add("A nice lil synopsis and visual stats of nations in #WorldCup 2010 - http://bit.ly/9E6kpJ;;#;;43.652500;;#;;-79.381667;;#;;5");
            stringList.Add("Microsoft Search Engine to replace MSNbot with Bingbot. Bing Webmaster Blog: http://bit.ly/9Ukurb #SEO;;#;;43.648565;;#;;-79.385329;;#;;5");
            stringList.Add("Microsoft Search Engine to replace MSNbot with Bingbot. Bing Webmaster Blog: http://bit.ly/9Ukurb #SEO;;#;;43.648565;;#;;-79.385329;;#;;5");
            stringList.Add("Work til 9...then ambers... home to clean my room.. then to bing with amber to watch eclipse:).. txt me;;#;;42.536255;;#;;-75.524239;;#;;3");
            stringList.Add("Microsoft To Replace MSNBot With BingBot October 1, 2010: The Bing Search Blog announced that on October 1st, MSNB... http://bit.ly/avp2eq;;#;;39.952270;;#;;-75.162369;;#;;5");
            stringList.Add("Bing finally updates it's mobile site making it much more iPhone friendly. This might be adios for Google search on my iPhone.;;#;;40.024590;;#;;-75.214149;;#;;5");
            stringList.Add("HAHA: Bing's autocomplete thinks it's \"stupid\" and \"rubbish\". http://ow.ly/24kDh;;#;;38.890370;;#;;-77.031959;;#;;3");
            stringList.Add("Every time someone @ mentions me and @TweetDeck goes BING I feel like a kid who gets a birthday letter in the mail. #shibby;;#;;40.190331;;#;;-82.669472;;#;;5");
            stringList.Add("\"I'm off the grid untrending and playing with words and pixels to soar before I become sore. You can't Bing or Google my thoughts or fears.\");;#;;34.232945;;#;;-102.410204;;#;;5");
            stringList.Add("@AccordionGuy when will #MSFT release Bing iPhone App in Canada?;;#;;43.648565;;#;;-79.385329;;#;;3");
            stringList.Add("$500 Best Buy Giftcard - immediate ship, use 10% Bing!: US $559.99 End Date: Monday Jul-26-2010 16:57:26 PDTBuy http://url4.eu/55emd;;#;;40.190331;;#;;-82.669472;;#;;5");
            stringList.Add("Watson explores taking Bing to court over plan to close Detroit parks: Detroit -- Councilwoman JoAnn Watson wants ... http://bit.ly/9IXPgD;;#;;42.331685;;#;;-83.047924;;#;;4");
            stringList.Add("Coming in October: BingBot! - & it will honor msnbot instructions http://ow.ly/24jDQ;;#;;43.648565;;#;;-79.385329;;#;;5");
            stringList.Add("@marcoarment This is supposed to be what Bing is better for, but I haven't tried it myself.;;#;;39.952270;;#;;-75.162369;;#;;3");
            stringList.Add("Bing's first year: good for advertisers? http://bit.ly/9mYQNK;;#;;44.272570;;#;;-121.175874;;#;;5");
            stringList.Add("http://detnews.com Watson explores taking Bing to court over plan to close Detroit parks: Detroit -- Counc... http://bit.ly/9IXPgD #detroit;;#;;42.331685;;#;;-83.047924;;#;;5");
            stringList.Add("Watson explores taking Bing to court over plan to close Detroit parks: Detroit -- Councilwoman JoAnn Watson wants ... http://bit.ly/9IXPgD;;#;;42.331685;;#;;-83.047924;;#;;4");
            stringList.Add("Samsung Fascinate (Galaxy S) coming to Verizon sporting Bing Maps and Search http://bit.ly/aRhm40;;#;;37.370972;;#;;-79.319653;;#;;5");
            stringList.Add("\"This is my timey-wimey detector.  Goes \"bing\" when there's stuff.\"  I love Doctor Who.;;#;;37.540700;;#;;-77.433654;;#;;5");
            stringList.Add("FMQB FYI... Feature on WMG & 360 deals, Lollapalooza under investigation, Microsoft integrating Zune into Bing searches http://bit.ly/b7LDup;;#;;39.927500;;#;;-75.030549;;#;;5");
            stringList.Add("http://detnews.com Bing cancels confab on mayoral control of DPS: Detroit --A meeting today between Mayor ... http://bit.ly/9iZ1xv #detroit;;#;;42.331685;;#;;-83.047924;;#;;4");
            stringList.Add("Bing cancels confab on mayoral control of DPS: Detroit --A meeting today between Mayor Dave Bing and community lea... http://bit.ly/9iZ1xv;;#;;42.331685;;#;;-83.047924;;#;;5");
            stringList.Add("Bing Community: I said I don't bother with the keywords meta tag - I didn't say that I don't bother with Bing ... B... http://awe.sm/57wEY;;#;;42.331685;;#;;-83.047924;;#;;2");
            stringList.Add("Microsoft's Bing turns up the heat in war with Google http://ht.ly/246uc;;#;;41.084195;;#;;-81.514059;;#;;5");
            stringList.Add("Retweet this & @Bing will donate $10 to @CNN's Gulf Telethon up to $100K. More at http://bit.ly/howtodonate. #BingforGulf;;#;;43.648565;;#;;-79.385329;;#;;5");
            stringList.Add("http://bit.ly/cxizNZ although we lost to ghana, its great to see the enthusiasm compared to 06..bring on 2014! (cont) http://tl.gd/26gk7k;;#;;40.438335;;#;;-79.997459;;#;;5");
            stringList.Add("Looking for clients or to be found in local web searches? Try Local University! Presenters from Bing, Google and more. http://bit.ly/c3z2SI;;#;;41.504365;;#;;-81.690459;;#;;5");
            stringList.Add("$500 SEARS KMART LANDS END Gift Card, 8% Bing cash bk: US $492.00 End Date: Tuesday Jul-27-2010 23:09:28 PDTBuy http://url4.eu/55QEa;;#;;40.190331;;#;;-82.669472;;#;;5");
            stringList.Add("See, when a carrier changes the default search of an Android to Yahoo!, or adds Bing Maps, it defeats the purpose of being a \"with...;;#;;43.648565;;#;;-79.385329;;#;;5");
            stringList.Add("About to head to Bing-bing town to meet the big BIG boss! Field trip!;;#;;42.440495;;#;;-76.495454;;#;;5");
            stringList.Add("Dag :-( I don't even drive thoo *sniff sniff* RT @SLiCe_Th3BeauTy: @SbPlease don't TOYAAAA me bing a ling I'm (cont) http://tl.gd/26g57l;;#;;38.885115;;#;;-76.913619;;#;;2");
            stringList.Add("@marcoarment Google has been unable to defeat the affiliate marketers, making product searches near-useless. @bing should innovate /there/.;;#;;43.648565;;#;;-79.385329;;#;;3");
            stringList.Add("I never understood asking questions on twitter when we have these things called Google, Bing, Wikipedia.;;#;;43.679423;;#;;-79.602306;;#;;5");
            stringList.Add("Retweet this & @Bing will donate $10 to @CNN's Gulf Telethon up to $100K. More at http://bit.ly/howtodonate. #BingforGulf;;#;;38.823502;;#;;-75.923813;;#;;5");
            stringList.Add("New Microsoft Bing Interface Shows What \"Decision Engine\" Really Means http://ow.ly/244BU #Yam #Microsoft #Bing #DecisionEngine;;#;;40.143230;;#;;-74.726707;;#;;5");
            stringList.Add("Bing Changes Could Attract More Users, More SEO Focus http://bit.ly/b7e6Pe;;#;;43.451229;;#;;-80.493050;;#;;5");
            stringList.Add("Microsoft bought a car for their search engine Bing. They called it the \"Bing Bus\".;;#;;41.310465;;#;;-75.319275;;#;;5");
            stringList.Add("Bing Changes Could Attract More Users, More SEO Focus http://bit.ly/cgsS4j;;#;;43.451229;;#;;-80.493050;;#;;5");
            stringList.Add("Bing Changes Could Attract More Users, More SEO Focus http://bit.ly/cRl6GQ;;#;;43.451229;;#;;-80.493050;;#;;5");
            stringList.Add("Bing Changes Could Attract More Users, More SEO Focus http://bit.ly/a71zmJ;;#;;43.451229;;#;;-80.493050;;#;;5");
            stringList.Add("RT @WhatOnEarth: Caption contest: Bing bus goes searching for search results: Bus. Ice cream truck. Personal space invader. Vessel... http://bit.ly/d6EmVb;;#;;33.946175;;#;;-117;;#;;5");
            stringList.Add("$500 Target Gift Card Free Shipping! Use BING CASHBACK: US $505.00 End Date: Sunday Jul-04-2010 19:51:55 PDTBuy http://url4.eu/54MqU;;#;;40.190331;;#;;-82.669472;;#;;5");
            stringList.Add("$500 Target Gift Card Free Shipping! Use BING CASHBACK: US $505.00 End Date: Sunday Jul-04-2010 19:51:55 PDTBuy http://url4.eu/54LBR;;#;;40.190331;;#;;-82.669472;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results - Bus. Ice cream truck. Personal space invader. Vessel o... http://ow.ly/17UBi1;;#;;39.952270;;#;;-75.162369;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results:Bus. Ice cream truck. Personal space invader. Vessel... http://bit.ly/akfLEh;;#;;39.952270;;#;;-75.162369;;#;;5");
            stringList.Add("RT: @engadget Caption contest: Bing bus goes searching for search results http://bit.ly/9Dr8me;;#;;43.648565;;#;;-79.385329;;#;;5");
            stringList.Add("Engadget: Caption contest: Bing bus goes searching for search results: Bus. Ice cream truck. Personal space invad... http://bit.ly/d6EmVb;;#;;48.468601;;#;;-108.620796;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results:Bus. Ice cream truck. Personal space invader. Ves.. http://bit.ly/9WtgeC;;#;;42.317822;;#;;-83.033913;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results:Bus. Ice cream truck. Personal space invader. Ves.. http://bit.ly/9WtgeC;;#;;44.989858;;#;;-64.130674;;#;;5");
            stringList.Add("Technology notebook - Everett Herald: Microsoft's Bing search site is getting an entertainment section with click-... http://bit.ly/9UJENS;;#;;38.890370;;#;;-77.031959;;#;;3");
            stringList.Add("Bada Bing! P-Woww's hair is lookin' good and staying in place! http://bit.ly/bJk5Ek;;#;;42.855350;;#;;-76.501671;;#;;5");
            stringList.Add("$500 WalMart Gift Card + Bonus Rebates - Bing is Back!: US $519.99 End Date: Wednesday Jun-30-2010 17:57:03 http://url4.eu/549Pn;;#;;40.190331;;#;;-82.669472;;#;;5");
            stringList.Add("Dis jail show brings back memories. I never wanna go back 2 da bing. nd I thank god nd my big brotha andy everyday! 25 to life is no joke;;#;;41.504365;;#;;-81.690459;;#;;3");
            stringList.Add("bing cashback ebay tracker #bing #cashback #ebay #tracker   http://www.linkati.com/bing-cashback-ebay-tracker.htm;;#;;38.890370;;#;;-77.031959;;#;;5");
            stringList.Add("Bing to add TV, music to site - Tulsa World: BELLEVUE, Wash. (AP) - Microsoft's Bing search site is getting an ent... http://bit.ly/d4niPK;;#;;38.890370;;#;;-77.031959;;#;;5");

            stringList.Add("Caption contest: Bing bus goes searching for search results - Bus. Ice cream truck. Personal space invader. Vessel o... http://ow.ly/17UBi1;;#;;39.952270;;#;;-75.162369;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results - Bus. Ice cream truck. Personal space invader. Vessel o... http://ow.ly/17UBkk;;#;;39.696255;;#;;-74.258434;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results http://tinyurl.com/257js2d;;#;;40.714550;;#;;-74.007124;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results: ");
            stringList.Add("Bus. Ice cream truck. Personal space invader. Vessel... http://bit.ly/9WtgeC;;#;;40.714550;;#;;-74.007124;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results: ");
            stringList.Add("Bus. Ice cream truck. Personal space invader. Vessel... http://bit.ly/akfLEh;;#;;39.952270;;#;;-75.162369;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results http://bit.ly/9WtgeC;;#;;40.714550;;#;;74.007124;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results http://bit.ly/au4NVz;;#;;40.849831;;#;;-73.936028;;#;;5");
            stringList.Add("Caption contest: Bing bus goes searching for search results: ");
            stringList.Add("Bus. Ice cream truck. Personal space invader. Vessel... http://bit.ly/9WtgeC;;#;;40.714550;;#;;-74.007124;;#;;5");
            stringList.Add("All I hear is bing bing bing bing bing bbm moving like her hands at dat cash register aye bitch u forgot to ask if I wanted paper or plastic;;#;;42.720310;;#;;-77.675880;;#;;3");
            stringList.Add("RT @Yoints_CraveAsh @youngmums09 in bing>duh#40.714550#-74.007124#5");

            foreach (string s in stringList)
            {
                Definition.TwitterTweet tweet = new Definition.TwitterTweet();
                string[] array = Regex.Split(s, ";;#;;");
                tweet.content = array[0];
                if (array.Length > 1)
                    tweet.googleLatitude = array[1];
                if (array.Length > 2)
                    tweet.googleLongitude = array[2];
                if (array.Length > 3)
                    tweet.sentiment = Convert.ToInt32(array[3]);
                tweetList.Add(tweet);
            }

            return tweetList;
        }

        public List<Definition.Tweet> GetData2()
        {

            List<Definition.Tweet> tweetList = new List<Definition.Tweet>();
            List<string> stringList = new List<string>(); 
            stringList.Add("im quelltext von <span class=\"x_keywordHighlight\">bing</span>.com erkennt man die qualit„t jeder ms-software wieder...;;#;;49.40729;;#;;8.639857;;#;;5");
            stringList.Add("Study: &quot;Drivers who text are six times more likely to crash.&quot; So if you hate somebody...text them. You might get lucky.;;#;;21.20239907;;#;;72.80629765;;#;;5");
            stringList.Add("Going to St James in a while for a site viewing with the Japanese guests. Celebrating birthday w Heng and<span class=\"x_keywordHighlight\">Bing</span> tonight as well.;;#;;1.353751;;#;;103.939346;;#;;5");
            stringList.Add("@psam I think facebook uses only <span class=\"x_keywordHighlight\">bing</span> maps, particularly Microsoft products!.Though I am not sure.;;#;;17.3529077;;#;;78.499327;;#;;5");
            stringList.Add("@robertsammons <span class=\"x_keywordHighlight\">bing</span>? Microsoft takes the cake with evil. The company was founded on software it never even created...;;#;;35.67726;;#;;139.708085;;#;;5");
            stringList.Add("<span class=\"x_keywordHighlight\">Bing</span>'s Destination Maps <a href=\"http://j.mp/amgooc\" target=\"_blank\">http://j.mp/amgooc</a> could be tremendously useful /=;;#;;47.108545;;#;;4.48617738;;#;;5");
            stringList.Add("we got a black hawk down. we got a black hawk down.;;#;;21.20182651;;#;;72.80651336;;#;;5");
            stringList.Add("@michaelsync ouch!! :P tell me you want to be a technical discriminator or microsoft technology cheer leader? Or u havn't seen<span class=\"x_keywordHighlight\">bing</span> on iOS.;;#;;1.42928457;;#;;103.83545541;;#;;4");
            stringList.Add("@AL3X_M iPhone 3G. it might get slow what u think;;#;;21.20052438;;#;;72.80688846;;#;;5");
            stringList.Add("RT @twitter: Retweet this &amp; @<span class=\"x_keywordHighlight\"><span class=\"x_keywordHighlight\">Bing</span></span> will donate $10 to @CNN's Gulf Telethon up to $100K. More at<a href=\"http://bit.ly/howtodonate.\" target=\"_blank\">http://bit.ly/howtodonate.</a> #<span class=\"x_keywordHighlight\"><span class=\"x_keywordHighlight\">Bing</span></span>forGulf;;#;;25.214569;;#;;55.299763;;#;;5");
            stringList.Add("RT @hilaryfanorg: RT @RyanSeacrest: Retweet this &amp; @<span class=\"x_keywordHighlight\">Bing</span> will donate $10 to @CNN's Gulf Telethon up (cont)<a href=\"http://tl.gd/21re81\" target=\"_blank\">http://tl.gd/21re81</a>;;#;;51.89432;;#;;4.469877;;#;;5");
            stringList.Add("<span class=\"x_keywordHighlight\">Bing</span> updates it's iPhone app - I live in Japan, I have the app but can't update<a href=\"http://tcrn.ch/bxr4AQ\" target=\"_blank\">http://tcrn.ch/bxr4AQ</a>;;#;;35.6454007;;#;;139.6235669;;#;;3");
            stringList.Add("Hahaha RT @msmobiles: Microsoft idiots release new features to <span class=\"x_keywordHighlight\">Bing</span> for iPhone before Windows Phone: <a href=\"http://bit.ly/cgbaWJ\" target=\"_blank\">http://bit.ly/cgbaWJ</a>;;#;;41.092101;;#;;29.019673;;#;;5");
            stringList.Add("do a &quot;lady gaga&quot; search in <span class=\"x_keywordHighlight\"><span class=\"x_keywordHighlight\">bing</span></span> to see all the new<span class=\"x_keywordHighlight\"><span class=\"x_keywordHighlight\">bing</span></span> entertainment features just launched. I like it alot!;;#;;4.47785649;;#;;8.26171875;;#;;5");
            stringList.Add("Lahore High Court orders ban on #google #<span class=\"x_keywordHighlight\">bing</span> #amazon #yahoo and lot more for publishing content against Holy Quran.<a href=\"http://is.gd/d0el0\" target=\"_blank\">http://is.gd/d0el0</a> #mad;;#;;10;;#;;76.5;;#;;5");
            stringList.Add("They're going to build their own version ? \"@ahsan_bagwan: Pakistan to ban Google,<span class=\"x_keywordHighlight\">Bing</span>, Yahoo and other Popular Sites <a href=\"http://bit.ly/cmW3Yh\" target=\"_blank\">http://bit.ly/cmW3Yh</a>\";;#;;13.08266478;;#;;80.2748627;;#;;5");
            stringList.Add("@Durf when google buys them from yahoo, or when yahoo gives up on map ventures and uses google maps. Or<span class=\"x_keywordHighlight\">bing</span>, which are ok for japan.;;#;;36.59648919;;#;;140.02792614;;#;;5");
            stringList.Add("RT @DangerousV: Retweet this &amp; @<span class=\"x_keywordHighlight\"><span class=\"x_keywordHighlight\">Bing</span></span> will donate $10 to @CNN's Gulf Telethon up to $100K. More at<a href=\"http://bit.ly/howtodonate.\" target=\"_blank\">http://bit.ly/howtodonate.</a> #<span class=\"x_keywordHighlight\"><span class=\"x_keywordHighlight\">Bing</span></span>forGulf;;#;;5.27614712;;#;;100.27154904;;#;;5");
            stringList.Add("Wo bin ich? Die Antwort gibt es nicht auf <span class=\"x_keywordHighlight\">Bing</span>.;;#;;53.70204842;;#;;7.70270961;;#;;5");
            stringList.Add("Go Google go! RT @GenkiGenki: This is why I use Google, not <span class=\"x_keywordHighlight\">Bing</span> <a href=\"http://imgur.com/cl8qo\" target=\"_blank\">http://imgur.com/cl8qo</a>;;#;;1.34610003;;#;;103.70372852;;#;;5");
            stringList.Add("??????! \"@attrip: ????? &quot;<span class=\"x_keywordHighlight\">Bing</span>???????????#0000FF????#0044CC? ??????????????????8,000????????????&quot; ?<a href=\"http://am6.jp/bSzFPl\" target=\"_blank\">http://am6.jp/bSzFPl</a> \";;#;;35.62665772;;#;;139.65893203;;#;;5");

            foreach (string s in stringList)
            {
                Definition.Tweet tweet = new Definition.Tweet();
                string[] array = Regex.Split(s, ";;#;;");
                //string[] array = s.Split('#');
                tweet.SourceText = array[0];
                if (array.Length > 1)
                    tweet.Latitude = array[1];
                if (array.Length > 2)
                    tweet.Longitude = array[2];
                if (array.Length > 3)
                    tweet.sentiment = Convert.ToInt32(array[3]);
                tweetList.Add(tweet);
            }

            return tweetList;
        }


    }
}
