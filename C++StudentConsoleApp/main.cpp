#include <iostream>
#include <fstream>
#include "Sorted.h"
#include<forward_list>
#include "Student.h"

using namespace std;

int main()
{
    forward_list<Student> l;

    ifstream in("gr1.txt");
    string surname, name;
    int index;
    float mark;

    while(in >> surname >> name >> index >> mark)
    {
    Student s;
    s.setSurname(surname);
    s.setName(name);
    s.setIndex(index);
    s.setMark(mark);

    l.push(s);
    }

     ifstream in2("gr2.txt");


    while(in2 >> surname >> name >> index >> mark)
    {

    Student s;
    s.setSurname(surname);
    s.setName(name);
    s.setIndex(index);
    s.setMark(mark);

    l.push(s);
    }

    in.close();


in2.close();


ofstream out("results.txt");

    while(!l.empty())
    {
        Student s =l.top();
        out << s.getIndex() << ";" << s.getMark() <<";"<<s.getSurname()<<" "<<s.getName()[0]<<"." << endl;
        l.pop();
    }
    // second file







}
