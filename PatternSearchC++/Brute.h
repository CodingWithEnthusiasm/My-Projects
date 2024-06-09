#pragma once
#ifndef BRUTE2_H_INCLUDED
#define BRUTE2_H_INCLUDED


using namespace std;


void Brute(string T, string P)
{


     int LT = T.length();
    int LP = P.length();

    //int counter = 0;
    int j = 0;

    for (int i = 0; T[i] != 0; ++i)
    {

        if (T[i] == P[j])// Checking if there is a match and if yes, add 1 to j
        {


            j++;

            if (P[j] == 0) //If all characters in pattern are matching we add it's position to the array
            {


                //cout <<"Brute:"<< i-LP+1<<endl;
                //arr[counter] = i-2;
                //counter++;
            }
        }

        else if (T[i] != P[j]) //If they are not matching we set the counter of right characters to zero
        {

                j = 0;


        }

    }

    //if (counter == 0)
    //{
     //   cout << "No match" << endl;    //Checking for at least 1 match
    //}



   // return arr;


}


#endif // BRUTE2_H_INCLUDED
