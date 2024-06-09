#include <iostream>
#include <fstream>
#include "dqueue.h"
#include<deque>
#include "tax.h"

using namespace std;

int main()
{

    const int M =5;


    deque<Tax> queue[M];

    ifstream in("payers.txt");
    ifstream in2("numbers.txt");
    string id, name,surname,debt;
    while(!in.eof())
    {
        in>>name>>surname;

        getline(in,id);
        getline(in2,debt);
        in.ignore();

        Tax tx;
        tx.setID(id);
        tx.setName(name);
        tx.setSurname(surname);
        tx.setDebt(debt);


        for(int i=0; i<M; i++)
        {


            queue[i].push_back(tx);



        }

    }

    in.close();
    in2.close();


    for(int i=0; i<M; i++)
    {
        ofstream out ("results.txt");

        while(!queue[i].empty())
        {
            Tax tx = queue[i].back();
            cout << "NAME:" <<tx.getID() <<endl;
            cout << "ID:" <<tx.getName() <<endl;
            cout << "SURNAME:"<<tx.getSurname()<<endl;
            cout << "DEBT"<<tx.getDebt()<<endl;

            queue[i].pop_back();

        }

        out.close();
    }


}
