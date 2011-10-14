#!/usr/bin/env python
####################################################################################
######################              features.py            #########################
#################### Code to compute features for given data #######################
######################          Author : Peddinti          #########################
####################################################################################
# Input format
# features.py <data filename for class1> <data filename for class2> <unigram feature file name> <bigram feature filename> <trigram feature filename>
import sys
import re
import operator
from collections import defaultdict


data_1_file = sys.argv[1]
data_2_file = sys.argv[2]
#data_3_file = sys.argv[3]

unigram_feature_file = sys.argv[3]
bigram_feature_file = sys.argv[4]
trigram_feature_file = sys.argv[5]

unigrams_1 = defaultdict(int)
bigrams_1 = defaultdict(int)
trigrams_1 = defaultdict(int)

unigrams_2 = defaultdict(int)
bigrams_2 = defaultdict(int)
trigrams_2 = defaultdict(int)

unigrams_3 = defaultdict(int)
bigrams_3 = defaultdict(int)
trigrams_3 = defaultdict(int)

# Function to be used with regex match which returns only 2 letter if more thatn 2 letters are present
def remextra(m):
    return m.group(0)[0:2]

# function to be used with regex match which adds extra space
def addextra(m):
    return " "+m.group(0)+" "

def sanitize_tweet(tweet):    
    tweet = tweet.lower()
    # Replacing Emotions
    regex = "((?<= )(:|;)[^0-9A-Za-z ]+(?= ))|((?<= )(:|;)[a-zA-Z0-9][^a-zA-Z0-9](?= ))|((?<= )(:|;)[^a-zA-Z0-9][a-zA-Z0-9](?= ))|((?<= )(:|;)[a-zA-Z0-9](?= ))|((?<= )(\\(){0,1}[^a-zA-Z0-9].[^a-zA-Z0-9](\\)){0,1}(?= ))|((?<= )[^a-zA-Z0-9](\\)|\\/)(?= ))"
    tweet = re.sub(regex," EMOTICON "," "+tweet+" ").strip()

    # HTML Decode 
    tweet = tweet.replace('&amp;','&').replace('&lt;','<').replace('&gt;','>').replace('&quot;','"').replace('&#39;',"'")

    # Replace @ redirect/reply with username
    regex = "(?<= )\\@[^ ]*(?= )"
    tweet = re.sub(regex," USERNAME "," "+tweet+" ").strip()

    # Replacing Url's with URL
    regex = "http://[^ ]+(?= )"
    tweet = re.sub(regex," URL "," "+tweet+" ").strip()

    # Removing Hash Tags ( Should examine if this information helps in classification)
    regex = "((?<= )#[^ ]+(?= ))|((?<= )[a-zA-Z]{2}:(?= ))|'"
    tweet = re.sub(regex," HASHTAGS "," "+tweet+" ").strip()

    # Appending Extra space to ? and !
    regex = "[\\?\\!]"
    tweet = re.sub(regex,addextra," "+tweet+" ").strip()

    # modify repeated letters like loooooooooooove to loove 2 letters max.
    regex = "([A-Za-z])\\1+"
    tweet = re.sub(regex,remextra," "+tweet+" ").strip()    

    # Removing all special characters except ! and ? as they are important in conveying information
    regex = "[^\?\!\#a-zA-Z0-9 ]"
    tweet = re.sub(regex," "," "+tweet+" ").strip()

    # Removing all "." not necessary
    regex = "\.(?![A-Za-z0-9]\.)"
    tweet = re.sub(regex," "," "+tweet+" ").strip()

    # Convert Numbers to NUM tagword
    regex = "( [0-9]+(?:\,[0-9]*)+ )|( [0-9]+(?:\.[0-9]*)? )"
    tweet = re.sub(regex," NUM "," "+tweet+" ").strip()    
    
    return tweet

def compute_features(filename,unigrams,bigrams,trigrams):
    fr = open(filename)    
    # Computing Feature counts
    for line in fr.readlines():
        #words = line.strip().split()
	line = line.strip()
        # removing the initial index values
        #line = " ".join(words[3:])
	line = sanitize_tweet(line)
        words = line.split()
        for i,word in enumerate(words[:-2]):
            unigrams[word] += 1
            bigrams[word+" "+words[i+1]] += 1
            trigrams[word+" "+words[i+1]+" "+words[i+2]] += 1
        # adding the last unigrams and bigrams
        if(len(words) > 1):
            unigrams[words[-2]] += 1
            bigrams[words[-2]+" "+words[-1]] += 1
        unigrams[words[-1]] += 1        
    fr.close()

def print_features(filename,class1,class2,feature_list):
    fw = open(filename,'w')
    for feature in feature_list:
        fw.write("%s\t%d\t%d\n"%(feature,class1[feature],class2[feature]))
        #fw.write("%s\t%d\n"%(feature,class1[feature]))
    fw.close()

if __name__ == "__main__":
    compute_features(data_1_file,unigrams_1,bigrams_1,trigrams_1)
    compute_features(data_2_file,unigrams_2,bigrams_2,trigrams_2)
    #compute_features(data_3_file,unigrams_3,bigrams_3,trigrams_3)
    # Combining all unigram featuers
    unigrams_list = list(set(unigrams_1.keys() + unigrams_2.keys()))
    #unigrams_list = list(set(unigrams_1.keys()))
    # combining all bigram features
    bigrams_list = list(set(bigrams_1.keys() + bigrams_2.keys()))
    #bigrams_list = list(set(bigrams_1.keys()))
    # combining all trigram features
    trigrams_list = list(set(trigrams_1.keys() + trigrams_2.keys()))
    #trigrams_list = list(set(trigrams_1.keys()))

    # Prining the features
    print_features(unigram_feature_file,unigrams_1,unigrams_2,unigrams_list)
    print_features(bigram_feature_file,bigrams_1,bigrams_2,bigrams_list)
    print_features(trigram_feature_file,trigrams_1,trigrams_2,trigrams_list)    
