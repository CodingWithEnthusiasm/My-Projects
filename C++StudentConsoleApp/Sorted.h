

#ifndef SortedList_H
#define SortedList_H

#include <stdexcept>
#include <iostream>

using namespace std;

template<typename T>
struct SortedList
{

private:

    struct Node
    {
        T value;
        Node* next;
        Node(T v, Node* n = nullptr)
        {
            value = v;
            next = n;
        }
    };

    Node* head = nullptr;
    int counter = 0;

    //invariants:
    //if nonempty, head points to the first node (the lowest one)
    //the next for the first node points to the second node, etc.
    //the next for the last node is null
    //counter is at least 0 (empty list) and it is the number of nodes (size of the list)

public:

    //constructors

    SortedList() = default;
    //pre: none
    //post: the list is empty

    SortedList(const SortedList&);
    //pre: none
    //post: the list is the copy of the argument

    //destructor

    ~SortedList();
    //pre: none
    //post: none

    //modifiers

    void push(T);
    //pre: none
    //post: the argument value is inserted in the ascending order, size is increased by 1

    void pop();
    //pre: the list is not empty
    //post: the greatest value is removed, size is decreased by 1

    void clear();
    //pre: none
    //post: the list is empty

    //selectors

    T top() const;
    //pre: the list is not empty
    //return: the lowest value

    int count(T) const;
    //pre: none
    //return: the number of occurrences of the argument value

    int size() const;
    //pre: none
    //return: the current size of the list

    void print() const;
    //pre: none
    //return: none, the contents of the list is printed, from the lowest to the greatest value

    //queries

    bool empty() const;
    //pre: none
    //return: true if the list is empty, false otherwise

    //operators

    SortedList& operator=(const SortedList&);
    //pre: none
    //post: the list is the copy of the argument
    //return: the reference to the list

    bool operator==(const SortedList&) const;
    //pre: none
    //return: true if the contents of the list is the same as the contents of the argument, false otherwise

};
template<typename T>
SortedList<T>::~SortedList()
{
    clear();
}


template<typename T>
void SortedList<T>::pop()
{
    if (counter==0)
        throw logic_error("Empty list");
    Node* killer=head;
    head = killer->next;
    delete killer;
    counter--;

}

template<typename T>
void SortedList<T>::clear()
{
while(head !=nullptr)
{
Node *killer =head;
head=killer->next;
delete killer;
}
counter=0;
}


template<typename T>
int SortedList<T>::size() const
{
    return counter;
}

template<typename T>
T SortedList<T>::top() const
{
    if (counter==0)
        throw logic_error("Empty list");
    return head->value;
}


template<typename T>
bool SortedList<T>::empty() const
{
    if (counter==0)
        return true;
    else
        return false;

}
template<typename T>
void SortedList<T>::print() const
{
if(counter==0)
    cout<<"empty";
Node *walker=head;
while (walker!=nullptr)
{
    cout <<walker->value<< " ";
    walker=walker->next;

}
cout <<endl;
}

template<typename T>
void SortedList<T>::push(T value)
{
Node *pred =nullptr;
Node *succ=head;
while(succ !=nullptr && value<succ->value )
{
    pred = succ;
    succ = succ->next;
}
Node *creator = new Node(value, succ);
if(pred !=nullptr)
    pred->next=creator;
else
    head=creator;
counter++;

}

#endif






