// 最大公约数与最小公倍数.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include"iostream"
using namespace std;

int getMax(int a,int b)
{
	if(a<b)
	{
		int temp=a;
		a=b;
		b=temp;
	}
	int c=0;
	while(b!=0)
	{
		c=a%b;
		a=b;
		b=c;
	}
	return a;
}
int getMin(int a,int b)
{
	if(a<b)
	{
		int temp=a;
		a=b;
		b=temp;
	}
	int c=getMax(a,b);
	int d=a/c;
	return b*d;
}

int _tmain(int argc, _TCHAR* argv[])
{
	int m,n;
	while(true)
	{
		cin>>m>>n;
		cout<<m<<"和"<<n<<"的最大公约数为:"<<getMax(m,n)<<endl;
		cout<<m<<"和"<<n<<"的最小公倍数为:"<<getMin(m,n)<<endl;
	}
	return 0;
}

