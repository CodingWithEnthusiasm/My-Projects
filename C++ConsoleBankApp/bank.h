#ifndef Bank_H
#define Bank_H

#include <string>

using namespace std;

struct Account
{
private:
    string iban;
	string name;
	double salary;

public:


    void setIBAN(string);
    //pre: the argument is non-empty
    //post: the argument value is student's surname

    void setName(string);
    //pre: the argument is non-empty
    //post: the argument value is student's name(s)

    void setSalary(int);
    //pre: the argument is 6-digit positive integer value
    //post: the argument value is student's index

    string setCode(string);


    string getIBAN() const;
    //pre: none
    //return: the current student's surname

    string getName() const;
    //pre: none
    //return: the current student's name

    int getSalary() const;
    //pre: none
    //return: the current student's index

    string getCode()const;


    bool operator<(const Account&) const;
    //pre: none
    //return: true if the student's index is lower than the argument's index, false otherwise


};

void Account::setIBAN(string i)
{
  iban=i;
}
void Account::setName(string n)
{
    name=n;
}
void Account::setSalary(int s)
{
    salary=s;
}

string Account::getIBAN()const
{
    return iban;
}

string Account::getName()const
{
    return name;
}

int Account::getSalary()const
{
  return salary;
}

string Account::getCode()const
{
string code= iban.substr (5,4);
  return code;
}

bool Account::operator<(const Account &a) const
{
 return name<a.name;
}
#endif //Bank_H
