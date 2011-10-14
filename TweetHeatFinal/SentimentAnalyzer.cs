using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Browser;
using System.Text.RegularExpressions;

namespace TweetHeatFinal
{
    public class SentimentAnalyzer : Definition
    {
        public int SA(string tweet)
        {
            int score = 1;
            string sanTweet = Sanitize(tweet);
            List<string[]> features = GetFeatures(sanTweet);
            double[] prob = NaiveBayes(features);
            if ((prob[0] - prob[1]) > 0.2)
                score = 5;
            else if (Math.Abs(prob[0] - prob[1]) < 0.2)
                score = 3;
            score = Emote(tweet, score);
            return score;
        }

        private static int Emote(string tweet, int sentiment)
        {
            // Checking for postive emotions.            
            string regExpressionEmoticon = "((?<= )(:|;)[^0-9A-Za-z ]+(?= ))|((?<= )(:|;)[a-zA-Z0-9][^a-zA-Z0-9](?= ))|((?<= )(:|;)[^a-zA-Z0-9][a-zA-Z0-9](?= ))|((?<= )(:|;)[a-zA-Z0-9](?= ))|((?<= )(\\(){0,1}[^a-zA-Z0-9].[^a-zA-Z0-9](\\)){0,1}(?= ))|((?<= )[^a-zA-Z0-9](\\)|\\/)(?= ))";
            string regExpressionPositive = ":-)|:)|:o)|:]|:3|:c)|:>|=]|8)|=)|:D|C:|( )|:-D|:D|8D|XD|=D|=3|<=3|<=8|\\o o/";
            regExpressionPositive = Regex.Escape(regExpressionPositive);
            Regex rgxEmoticon = new Regex(regExpressionEmoticon);
            Regex rgxPositive = new Regex(regExpressionPositive);
            if (rgxEmoticon.Matches(tweet).Count > 0)
                if (rgxPositive.Matches(tweet).Count > 0)
                    sentiment = Math.Min(5, sentiment + 1);
                else
                    sentiment = Math.Max(1, sentiment - 1);

            return sentiment;
        }

        private static string[] UniGrams(string sentence)
        {
            string[] words = sentence.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }

        private static string[] BiGrams(string sentence)
        {
            string[] words = sentence.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] biwords = new string[words.Length - 1];
            for (int i = 0; i < words.Length - 1; i++)
            {
                biwords[i] = words[i] + " " + words[i + 1];
            }
            return biwords;
        }

        private static string[] TriGrams(string sentence)
        {
            string[] words = sentence.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] triwords = new string[words.Length - 2];
            for (int i = 0; i < words.Length - 2; i++)
            {
                triwords[i] = words[i] + " " + words[i + 1] + " " + words[i + 2];
            }
            return triwords;
        }

        private static List<string[]> GetFeatures(string tweet)
        {
            List<string[]> temp = new List<string[]>();
            tweet = tweet.ToLower();
            temp.Add(UniGrams(tweet));
            temp.Add(BiGrams(tweet));
            temp.Add(TriGrams(tweet));
            return temp;
        }

        private Queue<string> urls = new Queue<string>();       

        private static double[] NaiveBayes(List<string[]> features)
        {
            //float[] featureProb = new float[3];
            double goodCount = 0;
            double badCount = 0;
            double[] classifierProb = new double[2];
            // Dealing with Unigrams
            foreach (string word in features.ElementAt(0))
            {
                if (PositiveUnigrams.ContainsKey(word) && (PositiveUnigrams[word] != 0))
                    goodCount += Math.Log10(PUC) - Math.Log10(PositiveUnigrams[word]);

                if (NegativeUnigrams.ContainsKey(word) && (NegativeUnigrams[word] != 0))
                    badCount += Math.Log10(NUC) - Math.Log10(NegativeUnigrams[word]);              
            }
            if (goodCount > 0)
            {
                classifierProb[0] = 0.2 * Math.Pow(10, -goodCount);
            }
            if (badCount > 0)
            {
                classifierProb[1] = 0.2 * Math.Pow(10, -badCount);
            }
            
            goodCount = 0;
            badCount = 0;

            // Dealing with Bigrams
            foreach (string word in features.ElementAt(1))
            {
                if (PositiveBigrams.ContainsKey(word) && (PositiveBigrams[word] != 0))
                    goodCount += Math.Log10(PBC) - Math.Log10(PositiveBigrams[word]);

                if (NegativeBigrams.ContainsKey(word) && (NegativeBigrams[word] != 0))
                    badCount += Math.Log10(NBC) - Math.Log10(NegativeBigrams[word]);
            }
            if (goodCount > 0)
            {
                classifierProb[0] += 0.3 * Math.Pow(10, -goodCount);
            }
            if (badCount > 0)
            {
                classifierProb[1] += 0.3 * Math.Pow(10, -badCount);
            }
            //featureProb[1] = (float)(goodCount) / (goodCount + badCount);

            goodCount = 0;
            badCount = 0;

            // Dealing with Trigrams
            foreach (string word in features.ElementAt(2))
            {
                if (PositiveTrigrams.ContainsKey(word) && (PositiveTrigrams[word] != 0))
                    goodCount += Math.Log10(PTC) - Math.Log10(PositiveTrigrams[word]);

                if (NegativeTrigrams.ContainsKey(word) && (NegativeTrigrams[word] != 0))
                    badCount += Math.Log10(NTC) - Math.Log10(NegativeTrigrams[word]);
            }
            if (goodCount > 0)
            {
                classifierProb[0] += 0.5 * Math.Pow(10, -goodCount);
            }
            if (badCount > 0)
            {
                classifierProb[1] += 0.5 * Math.Pow(10, -badCount);
            }
            
            // Normalizing the probabilities
            if (classifierProb[0] == classifierProb[1])
            {
                classifierProb[0] = classifierProb[1] = (double)0.5;
            }
            else
            {
                double total = classifierProb[0] + classifierProb[1];
                classifierProb[0] = classifierProb[0] / total;
                classifierProb[1] = classifierProb[1] / total;
            }
            return classifierProb;
        }        

        public void Train()
        {
            urls.Enqueue("http://viswatwitter.freehostingcloud.com/TweetHeatFiles/unigrams_small1.txt");
            urls.Enqueue("http://viswatwitter.freehostingcloud.com/TweetHeatFiles/bigrams_small1.txt");
            urls.Enqueue("http://viswatwitter.freehostingcloud.com/TweetHeatFiles/trigrams_small1.txt");
            GetDictionaries();            
        }

        private void GetDictionaries()
        {
            if (urls.Count > 0)
            {
                WebClient client = new WebClient();
                //client.Headers["Accept-Encoding"] = "gzip";
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DictionaryDownloadCompleted);                
                client.DownloadStringAsync(new Uri(urls.Dequeue()));
            }

        }

        private string AddSpace(Match m)
        {
            string s = " " + m.ToString() + " ";
            return s;
        }

        private string RemExtra(Match m)
        {
            string s = m.ToString();
            return s.Substring(0, 2);
        }

        private string Sanitize(string tweet)
        {
            // replacing emoticons
            string regExpression = "((?<= )(:|;)[^0-9A-Za-z ]+(?= ))|((?<= )(:|;)[a-zA-Z0-9][^a-zA-Z0-9](?= ))|((?<= )(:|;)[^a-zA-Z0-9][a-zA-Z0-9](?= ))|((?<= )(:|;)[a-zA-Z0-9](?= ))|((?<= )(\\(){0,1}[^a-zA-Z0-9].[^a-zA-Z0-9](\\)){0,1}(?= ))|((?<= )[^a-zA-Z0-9](\\)|\\/)(?= ))";
            Regex rgx = new Regex(regExpression);
            tweet = rgx.Replace(" " + tweet + " ", "EMOTICON");
            tweet = tweet.Trim();

            //HTML Decode 
            tweet = HttpUtility.HtmlDecode(tweet);

            // Replace @ with USERNAME
            regExpression = "(?<= )\\@[^ ]*(?= )";
            rgx = new Regex(regExpression);
            tweet = rgx.Replace(" " + tweet + " ", "USERNAME");
            tweet = tweet.Trim();

            // replace URL's with URL.
            regExpression = "http://[^ ]+(?= )";
            rgx = new Regex(regExpression);
            tweet = rgx.Replace(" " + tweet + " ", "URL");
            tweet = tweet.Trim();

            // Remove anyword starting with # (hash tag) and also any word with two letters and : ex. OH: RT: or special character '
            regExpression = "((?<= )#[^ ]+(?= ))|((?<= )[a-zA-Z]{2}:(?= ))|'";
            rgx = new Regex(regExpression);
            tweet = rgx.Replace(" " + tweet + " ", String.Empty);
            tweet = tweet.Trim();

            // Get rid of some special characters with space. except  ! ?. because they indicate emotion
            regExpression = "[^?!a-zA-Z0-9 ]";
            rgx = new Regex(regExpression);
            tweet = rgx.Replace(" " + tweet + " ", " ");
            tweet = tweet.Trim();

            // Replace ? or ! with additional space with nothing
            regExpression = "[\\?\\!]";
            rgx = new Regex(regExpression);
            tweet = rgx.Replace(" " + tweet + " ", new MatchEvaluator(AddSpace));
            tweet = tweet.Trim();

            // Remove Repeated Letters like looovvveeeee to loovvee.
            regExpression = "([a-z])\\1+";
            rgx = new Regex(regExpression);
            tweet = rgx.Replace(" " + tweet + " ", new MatchEvaluator(RemExtra));
            tweet = tweet.Trim();

            return tweet;
        }

        void DictionaryDownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string s = e.Result;
                char[] seperator = new char[] {'\n', '\r'};
                string[] entries = s.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, long> localposDict = new Dictionary<string, long>();
                Dictionary<string, long> localnegDict = new Dictionary<string, long>();
                long pc = 0;
                long nc = 0;
                foreach (string entry in entries)
                {
                    string[] values = entry.Split('\t');
                    string key = values[0];
                    string pvalue = values[1];
                    string nvalue = values[2];
                    localposDict[key] = Convert.ToInt32(pvalue);
                    pc = pc + Convert.ToInt32(pvalue);
                    localnegDict[key] = Convert.ToInt32(nvalue);
                    nc = nc + Convert.ToInt32(nvalue);
                }
                switch (urls.Count)
                {
                    case 0:
                        PositiveTrigrams = localposDict;
                        NegativeTrigrams = localnegDict;
                        PTC = pc;
                        NTC = nc;                        
                        break;
                    case 1:
                        PositiveBigrams = localposDict;
                        NegativeBigrams = localnegDict;
                        PBC = pc;
                        NBC = nc;                        
                        break;
                    case 2:
                        PositiveUnigrams = localposDict;
                        NegativeUnigrams = localnegDict;
                        PUC = pc;
                        NUC = nc;                        
                        break;                    
                }
            }
            GetDictionaries();
        }
    }
    
}
