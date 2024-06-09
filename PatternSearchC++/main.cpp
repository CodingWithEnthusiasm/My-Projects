
#include <iostream>
#include <fstream>
#include <chrono>
#include "Brute.h"
#include "Sunday.h"
#include "KMP.h"
#include "FSM.h"
#include "Rabin.h"

using namespace std::chrono;


int main()
{
    const int max_amount=100;



    string T;


    string P="they";


    nanoseconds _Brute(0);
    nanoseconds _Sunday(0);
    nanoseconds _KMP(0);       //variables for storing average time of each algorithm
    nanoseconds _FSM(0);
    nanoseconds _Rabin(0);










    ifstream file_("Book500.txt");
    if (file_.is_open())
    {

        while (getline(file_, T))
        {

            cout << T << endl;         //Reading the text from file and printing it on the screen

        }
        cout << endl;
        file_.close();
    }
    else
        cout << "File is not open" << endl;

    //cout << "Insert the word you want to find: ";
    //cin.ignore();
    //getline(cin, P);






    for (int i = 0; i < max_amount; i++)
   {




          auto begin_B = chrono :: steady_clock ::now();
        Brute(T, P);
        auto end_B = chrono :: steady_clock :: now();
        nanoseconds time_B(end_B - begin_B);
         _Brute += (end_B - begin_B);



    }


     for (int i = 0; i < max_amount; i++)
   {
    auto begin_S = chrono :: steady_clock ::now();
        Sunday(T, P);
        auto end_S = chrono :: steady_clock :: now();
        nanoseconds time_S(end_S - begin_S);
         _Sunday += (end_S - begin_S);

   }

 for (int i = 0; i < max_amount; i++)
   {
       auto begin_K = chrono :: steady_clock ::now();
        KMP(T, P);
        auto end_K = chrono :: steady_clock :: now();
        nanoseconds time_K(end_K - begin_K);
         _KMP += (end_K - begin_K);

   }

         for (int i = 0; i < max_amount; i++)
   {
        auto begin_F = chrono :: steady_clock ::now();
        FSM(T, P);
        auto end_F = chrono :: steady_clock :: now();
        nanoseconds time_F(end_F - begin_F);
         _FSM += (end_F - begin_F);

   }
 for (int i = 0; i < max_amount; i++)
   {
        auto begin_R = chrono :: steady_clock ::now();
        Rabin(T, P);
        auto end_R = chrono :: steady_clock :: now();
        nanoseconds time_R(end_R - begin_R);
         _Rabin += (end_R- begin_R);
   }





    cout << "Average time" << endl

       <<"Brute force time: "<<_Brute.count() / max_amount <<"ns"<<endl
       <<"Sunday time: "<< _Sunday.count() / max_amount <<"ns"<<endl
        <<"KMP time: "<< _KMP.count() / max_amount <<"ns"<<endl
       <<"FSM time: "<< _FSM.count() / max_amount <<"ns"<<endl        //Printing average time in nanoseconds

       <<"Rabin Karp time: "<< _Rabin.count() / max_amount <<"ns"<<endl
        << endl;




}
