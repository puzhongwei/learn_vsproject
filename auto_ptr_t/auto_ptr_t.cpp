// auto_ptr_t.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
using namespace std;

template <class T>
class my_auto_ptr
{
private:
	T *m_ptr;
	T *get_ptr()
	{
		T *tmp = m_ptr;
		m_ptr = 0;
		return tmp;
	}
public:
	explicit my_auto_ptr(T *p = 0): m_ptr(p)
	{}
	
	~my_auto_ptr()
	{
		delete m_ptr;
	}
	
	T& operator*() { return *m_ptr;}
	T* operator->() { return m_ptr;}
	
	my_auto_ptr(my_auto_ptr& mp){   //复制构造函数
		m_ptr = mp.GetPtr(); //mp复制过来后它自己原来的指针相当于失效了.
	}
	my_auto_ptr& operator=(my_auto_ptr& ap){ 
		if(ap != *this)
		{
			delete m_ptr;
			m_ptr = ap.GetPtr();
		}
		return *this;
	}
	
	void reset(T* p){  //指针重置,相当于把指针指向另外一个地方去
		if(p != m_ptr)
			delete m_ptr;
		m_ptr = p;
	}
	
};

struct Arwen{
int age;
Arwen(int gg) :age(gg) { };
};

void mysort(int a[] , int low, int high)
{
	int i = low, j =high, tmp = a[low];
	if(i<j)
	{
		while(i < j)
		{	
			while(i<j && a[j]>= tmp)j--;
			if(i<j)
			{
				a[i++] = a[j];
			}
			while(i<j && a[i]< tmp)i++;
			if(i<j)
			{
				a[j--] = a[i];
			}
		}
	a[i] = tmp;
	mysort(a,low,i-1);
	mysort(a,i+1,high);
	}

}

int fun(int n)
{	
	int count = 0;
	while(n)
	{
		count ++;
		n&=(n-1);
	}
	return count;
}

char *my_strcpy(char *dest, char *src)
{
	if(NULL == src)
		return NULL;
	while(*src!='\0')
	{
		*dest++ = *src++;
	}
	return dest;
}

int main(int argc, char* argv[])
{
	return 0;
}

