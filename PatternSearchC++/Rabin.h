#ifndef RABIN_H_INCLUDED
#define RABIN_H_INCLUDED
#pragma once
using namespace std;

void Rabin(string T, string P)
{

    if (T.length() && P.length()) {          // check for empty strings
        int LT = T.length(); //n text
        int LP = P.length(); //m pat
                                      // iterators for loops
        int HT = 0, HP = 0;                        // s = hash of string, p = hash of pattern
        const int no = 256;                      // no of characters in alphabet
        const int prime = 101;                       // large prime number
        int mult = 1;                               // h = multiplier for MSB
        bool flag = false;
        //int counter = 0;
        int i, j;
        // the value of h would be eqv to pow(pm, m-1)
        for (i = 0; i < LP - 1; i++)
            mult = (mult * no) % prime;

        // calculating initial hash for string and pattern
        for (i = 0; i < LP; i++) {
            HT = (no * HT + T[i]) % prime;
            HP = (no * HP + P[i]) % prime;
        }

        for (i = 0; i <= LT - LP; i++) {
            if (HT == HP) {                       // comparing hash of string and pattern
                for (j = 0; j < LP; j++)
                    if (T[i + j] != P[j])
                        break;
                if (j == LP) {
                    //cout << "Rabin: " << i << endl;
                    flag = true;
                    //arr[counter] = i;
                }
            }
            // calculating hash of next substring
            HT = (no * (HT - mult * T[i]) + T[i + LP]) % prime;
            if (HT < 0)
                HT= HT + prime;
        }
        if (!flag)
            cout << "Pattern not found" << endl;

    }

    //return arr;

}


#endif // RABIN_H_INCLUDED
