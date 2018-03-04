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
	int k;
	while(true)
	{
		cin>>k;
		cout<<Fibonacci(k)<<endl;
		cout<<"方法二:"<<f2(k)<<endl;
	}
	return 0;

}

