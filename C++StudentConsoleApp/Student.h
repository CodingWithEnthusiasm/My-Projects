#ifndef STUDENT_H_INCLUDED
#define STUDENT_H_INCLUDED

#include <string>

using namespace std;

struct Student
{

private:

    string surname = "None";
    string name = "None";
    int index = 0;
    float mark = 2.0;

    //invariants:
    //surname is not empty
    //name is not empty
    //index is 0 or is 6-digit positive integer value
    //mark is real value between 2 and 5

public:

    void setSurname(string);
    //pre: the argument is non-empty
    //post: the argument value is student's surname

    void setName(string);
    //pre: the argument is non-empty
    //post: the argument value is student's name(s)

    void setIndex(int);
    //pre: the argument is 6-digit positive integer value
    //post: the argument value is student's index

    void setMark(float);
    //pre: the argument is a real value between 2 and 5
    //post: the argument value is student's mark

    string getSurname() const;
    //pre: none
    //return: the current student's surname

    string getName() const;
    //pre: none
    //return: the current student's name

    int getIndex() const;
    //pre: none
    //return: the current student's index

    float getMark() const;
    //pre: none
    //return: the current student's mark

    bool operator<(const Student&) const;
    //pre: none
    //return: true if the student's index is lower than the argument's index, false otherwise



/*void setIndex(int n){ind=i;}
void setMark(float m){mark=m;}
string getSurname()const{return sur;}
string getName() const {return name;}
int getIndex()const{return ind;}
float setMark()const{return mark;}
bool operator<(const Student &s) const
{
    return index < s.index;
}
*/
};
void Student::setSurname(string s)
{surname=s;}
void Student::setName(string n){name=n;}
void Student::setIndex(int i){index=i;}
void Student::setMark(float m){mark=m;}
bool Student::operator<(const Student &s) const
{
    return index > s.index;
}
 string Student::getSurname() const
 {
     return surname;
 }

    string Student::getName() const
    {
        return name;
    }

    int Student::getIndex() const
    {
        return index;
    }


    float Student::getMark() const
    {
        return mark;
    }

#endif // STUDENT_H_INCLUDED



