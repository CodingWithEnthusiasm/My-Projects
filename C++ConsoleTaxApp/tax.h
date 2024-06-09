#ifndef TAX_H_INCLUDED
#define TAX_H_INCLUDED
using namespace std;
struct Tax
{
    private:
string id;
string name;
string surname;
string debt;
    public:

    void setID(string);


    void setName(string);

    void setSurname(string);

    void setDebt(string );


    string getID() const;


    string getName() const;


    string getDebt() const;


    string getSurname()const;



    bool operator<(const Tax&) const;
};


void Tax::setID(string i)
{
    id=i;
}

void Tax::setName(string n)
{
    name=n;
}

void Tax::setDebt(string d)
{
   debt=d;
}

void Tax::setSurname(string s)
{
    surname=s;
}

string Tax::getID()const
{
    return id;
}

string  Tax::getName()const
{
return name;
}

string Tax::getDebt()const
{
    return debt;
}

string Tax::getSurname()const
{
    return surname;
}

bool Tax::operator<(const Tax &t) const
{
    return name>t.name;
}


#endif // TAX_H_INCLUDED
