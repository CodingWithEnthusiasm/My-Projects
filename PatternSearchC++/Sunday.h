#pragma once
#ifndef SUNDAY_H_INCLUDED
#define SUNDAY_H_INCLUDED
#include <string>
using namespace std;
//int* alph = new int[LP] {-1};
static  int alph[256] = { 0 };//Library size

int LT, LP; //length of the strings T and P

void Sunday(string T, string P)
{
    int LT = T.length();
    int LP = P.length();


    //int counter=0;


    int test = 0;



    //Begin to build the library for characters
    for (int i = 0; i <= 256; ++i)
    {

        alph[i] = -1; //All characters outside of the pattern are equal -1


    }


    for (int i = 0, j = 1; i != LP - 1; j++, i++)
    {
        alph[P[i]] = LP - j;  //The values are equal to their distance from the rightmost character
    }

    if (alph[P[LP- 1]] == -1)
    {
        alph[P[LP - 1]] = LP; //If the rightmost character wasn't given a value previously then it's equal to the size of the pattern
    }
    //cout << "LIBRARY:" << alph[P[0]] << endl;
    //cout << "LIBRARY:" << alph[P[1]] << endl;
    //cout << "LIBRARY:" << alph[P[2]] << endl;





    int w = 0;     //variable which is used to move go through text
    for (w = 0; w < LT-1 ; w++)//Works until we checked the whole text
    {


        int i = 0;

        //counter increases by 1 after finding letter from pattern
        bool f = false;
        while (i <= LP - 2 && f == false) //Works
        {


            if (T[w + i] != P[i]) //i on the left and right sight represent the correct character from pattern
            {


                f = true; //When we found the first unmatch, we set f to true and begin our procedure


            }
            else if (T[w + i] == P[i]) // If we found we set the pattern counter by 1
            {
                i++;



            }

            if (i > 0)     //We check if the the character mathched and take the value of it's character from library
                test = alph[P[i]];




        }

        if (f == false) // if i equals the lenght of pattern we add pattern position to array and procede further
        {
            //cout <<"Sunday: "<< w << endl;
            //arr[counter] = w;
            //counter++;
            w++;

            test = 0; //We set our test variable to zero after full match


        }

        else
        {

            i = LP; //Set the value to the lenght of the pattern


            if (alph[T[w + i]] == -1) //Letter which doesn't exist in the pattern equals to it's lenght
            {



                if (test > 0)
                    w = w + test; //If the test variable has the value from library add it to the w


                else if (test <= 0)
                    w = w + i; //If not add the size of the pattern to the w





            }

            else //If the rightmost character of the pattern isn't equal to the character of the text, it check if this character exist in pattern library and if yes we substract it and use the result a our indentation
            {


                w = w + alph[T[w + i]];





            }
        }

    }


    //if (counter > 0)
    //{
        //cout << "Success" << endl;


    //}
                                      //Checking for at least 1 match
   // else
    //{

       // cout << "mission failed";

    //}

    //return arr;











}



#endif // SUNDAY2_H_INCLUDED
