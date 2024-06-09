#pragma once
#ifndef KMP_H_INCLUDED
#define KMP_H_INCLUDED


using namespace std;



void KMP(string T, string P)
{


    //Building library for all letters in the pattern
    int LP = P.length();
    int LT = T.length();
    int* alph = new int[LP] {-1};

    int pref = 0;
    int suff = 1;


     while (suff < LP)
    {


        if (P[suff] == P[pref]) //Checking if there is a match
        {
            alph[suff] = alph[pref];

        }



        else
        {
            alph[suff] = pref;
            while (pref >= 0 && P[suff] != P[pref])
            {
                pref = alph[pref];

            }


        }
        suff++;
        pref++;


    }
     alph[suff] = pref;

     //Second part

     int number=0;
     int j = 0;
     int k = 0;

     while (j < LT)
     {

         if (P[k] == T[j])
         {
             j++;
             k++;
             if (k == LP)
             {
                //cout <<"KMP:"<< j - k<<endl;
                 //arr[number] = j - k;
                 number++;
                 k = alph[k];
             }
         }

         else
         {
             k = alph[k];
             if (k < 0)
             {
                 j++;
                 k++;
             }

         }

     }



    //return arr;


}



#endif // KMP_H_INCLUDED
