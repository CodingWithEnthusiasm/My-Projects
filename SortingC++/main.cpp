#include <algorithm>
#include <chrono>
//#include <cassert>
#include <iostream>
#include <random>

//using namespace std;

constexpr int step = 100;
constexpr int maxlen = 10000;
constexpr int times = 100;

using std::chrono::nanoseconds;

void mergeSort(int *arr, int len)
{
    if(len <= 1)
        return;

    int* temp = new int[len];
    int middle = len / 2;

    mergeSort(arr, middle);
    mergeSort(arr, len - middle);

    for(int i = 0; i < len; i++)
    {
        temp[i] = arr[i];
    }

    int left_first = 0;
    int right_first = middle;

    int i = 0;

    while(left_first < middle && right_first < len)
    {
        if(temp[left_first] < temp[right_first])
        {
            arr[i] = temp[left_first];
            left_first++;
        }
        else
        {
            arr[i] = temp[right_first];
            right_first++;
        }
        i++;
    }

    if(left_first == middle)
    {
        for(; i < len; i++)
        {
            arr[i] = temp[right_first];
            right_first++;
        }
    }
    else if(right_first == len)
    {
        for(; i < len; i++)
        {
            arr[i] = temp[left_first];
            left_first++;
        }

    }
    //delete[] temp;
}

void swap(int *a, int *b)
{
    int temp;
    temp = *a;
    *a = *b;
    *b = temp;
}

void heapSort(int *arr, int len)
{
    int child;
    int parent;

    while(len > 0)
    {
        parent = len - 1;

        while(parent >= 0)
        {
            child = parent * 2 + 1;

            if(child < len)
            {
                if(arr[child] < arr[child + 1] && child + 1 < len)
                    std::swap(arr[child], arr[child + 1]);

                if(arr[child] > arr[parent])
                    std::swap(arr[child], arr[parent]);
            }
            parent--;
        }
        std::swap(arr[0], arr[len - 1]);
        len--;
    }
}

void partition(int *arr, int left, int right)
{
    int i;
    int j;
    int pivot;

    if(left >= right)
        return;

    i = left;
    j = right;
    pivot = left;

    while(i < j)
    {
        while(arr[i] < arr[pivot])
        {
            i++;
        }
        while(arr[pivot] < arr[j])
        {
            j--;
        }
        std::swap(arr[i], arr[j]);
    }

    if(left < i - 1)
    {
        partition(arr, left, i - 1);
    }

    if(j + 1 < right)
    {
        partition(arr, j + 1, right);
    }
}

void quickSort(int *arr, int len)
{
    partition(arr, 0, len - 1);
}

nanoseconds
timeit(int *original, int *arr, int len, void what(int*, int))
{
    std::copy(original, original + len, arr);

    auto begin = std::chrono::steady_clock::now();
    what(arr, len);
    auto end = std::chrono::steady_clock::now();

    // assert(std::is_sorted(arr, arr+len));

    nanoseconds total(end - begin);
    // std::sort(original, original+len);
    return total;
}


int main()
{
    std::random_device rd;
    std::mt19937 gen(rd());

    for (int len = step ; len < maxlen ; len += step)
    {

        nanoseconds merge_sort(0);
        nanoseconds heap_sort(0);
        nanoseconds quick_sort(0);
        nanoseconds merge_sort_best(0);
        nanoseconds heap_sort_best(0);
        nanoseconds quick_sort_best(0);

        //int *original = new int[len];
        int* original = new int[len];
        for (int i = 0 ; i < len ; i++)
        {
            original[i] = i;
        }

//        int *arr = new int[len];
//       int *sorted = new int[len];

        int* arr = new int[len];
        int* sorted = new int[len];

        for (int i = 0 ; i < times ; i++)
        {
            std::shuffle(original, original + len, gen);

            merge_sort += timeit(original, arr, len, &mergeSort);
            heap_sort += timeit(original, arr, len, &heapSort);
            quick_sort += timeit(original, arr, len, &quickSort);

            merge_sort_best += timeit(arr, sorted, len, &mergeSort);
            heap_sort_best += timeit(arr, sorted, len, &heapSort);
            quick_sort_best += timeit(arr, sorted, len, &quickSort);

        }

        std::cout
                << len << " "
                << merge_sort.count() / times << " "
                << heap_sort.count() / times << " "
                << quick_sort.count() / times << " "
                << merge_sort_best.count() / times << " "
                << heap_sort_best.count() / times << " "
                << quick_sort_best.count() / times << std::endl;

        delete [] arr;
        delete [] sorted;
        delete [] original;

    }

    return 0;
}

