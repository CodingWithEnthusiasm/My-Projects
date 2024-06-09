#include <iostream>
#include <fstream>
#include "dqueue.h"
#include "bank.h"
using namespace std;

int main()
{
    const int M =3;
    string banks[M]= {"1020","1140","2490"};
    DQueue<Account> queue[M];

    ifstream in("accounts.txt");
    string iban, name;
    double salary;
    while(!in.eof())
    {
        getline(in,iban);
        in>>name;
        in>>salary;
        in.ignore();

        Account acc;
        acc.setIBAN(iban);
        acc.setName(name);
        acc.setSalary(salary);

        for(int i=0; i<M; i++)
        {
            if(acc.getCode() == banks[i])
            {

                queue[i].push_back(acc);

                break;
            }
        }

    }
    in.close();

     for(int i=0; i<M; i++)
     {
        ofstream out (banks[i]+".txt");

        while(!queue[i].empty())
        {
            Account acc = queue[i].front();
            cout << "IBAN:" <<acc.getIBAN() <<endl;
            cout << "NAME:" <<acc.getName() <<endl;
            cout << "SALARY"<<acc.getSalary()<<endl;
            //Name and Salary
            queue[i].pop_front();

        }

        out.close();
     }

    return 0;
}
