// ���Լ������С������.cpp : �������̨Ӧ�ó������ڵ㡣
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
		cout<<m<<"��"<<n<<"�����Լ��Ϊ:"<<getMax(m,n)<<endl;
		cout<<m<<"��"<<n<<"����С������Ϊ:"<<getMin(m,n)<<endl;
	}
	return 0;
}

