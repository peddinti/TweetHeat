#############################################################################################
######################            reduce_features.py                #########################
###################### reduces the dimensionality of feature space  #########################
######################            Author : Peddinti                 #########################
#############################################################################################
# Input format
# reduce_features.py <original feature file> > <new feature file> 2> deleted features
import sys
import re
import math
import operator

feature_file = sys.argv[1]

def rules(line):
    [feature,c1,c2] = line.split('\t')
    c1 = int(c1)
    c2 = int(c2)
#    c3 = int(c3)   
    rii = abs(c2-c1)
    rii = rii/(c1+c2)
    if(rii <= 0.1):
        return False
    # If the counts of the feature is very small.. remove the feature
    if((c1+c2) <= 2):
        return False
    # if no rules are satisfied, we retain the feature
    return True
if __name__ == "__main__":
    fr = open(feature_file)
    for line in fr.readlines():
        line = line.strip()
        if(rules(line)):
            print line
        else:
            sys.stderr.write(line+"\n")
