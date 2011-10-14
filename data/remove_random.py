#!/usr/bin/env python
####################################################################################
######################            remove_random.py         #########################
###################### Code to Perform all legal aligments #########################
######################          Author : Peddinti          #########################
####################################################################################
# Input format
# remove_random.py <filename> <count of sentences> <count of sentences to be removed>
import sys
import random
logs = sys.stderr

filename = sys.argv[1]
total = int(sys.argv[2])
remove = int(sys.argv[3])

if __name__ == "__main__":
    elements = list(set([random.choice(range(total)) for x in range(2*remove)]))
    elements = elements[:remove]
    fr = open(filename)
    count = 0
    for line in fr.readlines():        
        if count not in elements:
            print line.strip()
        else:
            sys.stderr.write(line)
        count += 1
    fr.close()
    
