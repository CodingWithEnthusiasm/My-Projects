#ifndef DQueue_H
#define DQueue_H

#include <stdexcept>
#include <iostream>

using namespace std;

template<typename T>
struct DQueue
{

private:

    struct Node
    {
        T value;
        Node* prev;
        Node* next;
        Node(T v, Node* p = nullptr, Node* n = nullptr)
        {
            value = v;
            prev = p;
            next = n;
        }
    };

    Node* head = nullptr;
    Node* tail = nullptr;
    int counter = 0;

    //invariants:
    //if nonempty, head points to the first node (it contains the first value)
    //if nonempty, tail points to the last node (it contains the last value)
    //tail is null if and only if head is null (empty queue)
    //the next for the first node points to the second node, etc.
    //the next for the last node is null
    //the prev for the last node points to the before last node, etc.
    //the prev for the first node is null
    //counter is at least 0 (empty queue) and it is the number of nodes (size of the queue)

public:

    //constructors

    DQueue() = default;
    //pre: none
    //post: the queue is empty

    DQueue(const DQueue&);
    //pre: none
    //post: the queue is the copy of the argument

    //destructor

    ~DQueue();
    //pre: none
    //post: none

    //modifiers

    void push_back(T);
    //pre: none
    //post: the argument value is placed as the last one, size is increased by 1

    void pop_back();
    //pre: the queue is not empty
    //post: the last value is removed, size is decreased by 1

    void push_front(T);
    //pre: none
    //post: the argument value is placed as the first one, size is increased by 1

    void pop_front();
    //pre: the queue is not empty
    //post: the first value is removed, size is decreased by 1

	T front() const;
    //pre: the queue is not empty
	//return: the first value

	T back() const;
    //pre: the queue is not empty
	//return: the last value

    void clear();
    //pre: none
    //post: the queue is empty

    //selectors

    int size() const;
    //pre: none
    //return: the current size of the queue

    //queries

    bool empty() const;
    //pre: none
    //return: true if the queue is empty, false otherwise

    //operators

    DQueue& operator=(const DQueue&);
    //pre: none
    //post: the queue is the copy of the argument
    //return: the reference to the queue

    bool operator==(const DQueue&) const;
    //pre: none
    //return: true if the contents of the queue is the same as the contents of the argument, false otherwise

};

template<typename T>
DQueue<T>::DQueue(const DQueue<T> &q)
{
    Node *walker=q.head;
    while(walker!=nullptr)
    {
        push_back(walker->value);
        walker=walker->next;
    }
}
template<typename T>
DQueue<T>& DQueue<T>::operator=(const DQueue<T> &q)
{
clear();
Node* walker=q.head;
while(walker !=nullptr)
{


push_back(walker->value);
walker=walker->next;
}
return *this;


}

template<typename T>
void DQueue<T>::push_back(T value )
{
    Node* creator= new Node(value, tail,nullptr);
    if(tail!=nullptr)
        tail->next=creator;
    else
        head=creator;
    tail=creator;
    counter++;
}

template<typename T>
DQueue<T>::~DQueue()
{
 clear();
}

template<typename T>
void DQueue<T>::clear()
{
  while (head!=nullptr)
  {
      Node *killer=head;
      head=killer->next;
      delete killer;
  }
  tail =nullptr;
  counter=0;
}


template<typename T>
void DQueue<T>::push_front(T value)
{
    Node* creator= new Node(value, nullptr,head);
    if(head!=nullptr)
        head->prev=creator;
    else
        tail=creator;
    head=creator;
    counter++;
}


template<typename T>
void DQueue<T>::pop_back()
{
    if (counter==0)
        throw logic_error("Empty queue");
    Node* killer=tail;
    if(tail->prev!=nullptr)

        tail->prev->next=nullptr;
    else
        head=nullptr;
    tail=tail->prev;
         delete killer;
    counter--;


}


template<typename T>
void DQueue<T>::pop_front()
{
    if (counter==0)
        throw logic_error("Empty queue");
    Node *killer = head;
    if(head->next!=nullptr)
    head->next->prev=nullptr;
    else
        tail=nullptr;
    head=head->next;
    delete killer;
    counter--;
}


template<typename T>
T DQueue<T>::front() const
{
    if (counter==0)
        throw logic_error("Empty queue");
    return head->value;
}


template<typename T>
T DQueue<T>::back() const
{
    if(counter==0)
        throw logic_error("Empty queue");

        return tail->value;
}


template<typename T>
int DQueue<T>::size() const
{
    return counter;
}


template<typename T>
bool DQueue<T>::empty() const
{
if (counter==0)
    return true;
else
    return false;
}
#endif
