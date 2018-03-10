// FIbonacci.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include"iostream"
#include"ctime"
using namespace std;

int Fibonacci(int n)
{
	if(n<=0)
		return 0;
	if(n==1)
		return 1;

	return Fibonacci(n-1)+Fibonacci(n-2);
}

void mysort(int a[], int low, int high)
{
	if (low < high)
	{
		int i = low, j = high, tmp = a[low];
		while (i < j)
		{
			while (i<j && a[j] >= tmp)j--;
			if (i < j)
			{
				a[i++] = a[j];
			}
			while (i<j && a[i] < tmp)i++;
			if (i < j)
			{
				a[j--] = a[i];
			}
			a[i] = tmp;
			mysort(a, low, i-1);
			mysort(a, i + 1, high);
		}
	}
}

void qSort(int low, int high, int a[])
{
	if (low<high)
	{
		int i = low, j = high;
		int tmp = a[low];
		while (i<j)
		{
			while (i<j&&a[j] >= tmp)j--;
			if (i<j)
				a[i++] = a[j];

			while (i<j&&a[i]<tmp)i++;
			if (i<j)
				a[j--] = a[i];
		}
		a[i] = tmp;
		qSort(low, i - 1, a);
		qSort(i + 1, high, a);
	}
}
int f2(int n)
{
	int r[2]={0,1};
	if(n<2)
		return r[n];
	int a=1;
	int b=0;
	int c=0;
	for(int i=2;i<=n;i++)
	{
		c=a+b;
		b=a;
		a=c;
	}
	return c;

}
int _tmain(int argc, _TCHAR* argv[])
{
	/*int k;
	while(true)
	{
		cin>>k;
		cout<<Fibonacci(k)<<endl;
		cout<<"方法二:"<<f2(k)<<endl;
	}*/
	int a[] = { 12, -1, 56, 2342, 0, 78 };
	mysort(a, 0, 5);
	for (int i = 0; i < 6; i++)
	{
		cout << a[i] << "  " ;
	}
	cout << endl;
	return 0;

}

