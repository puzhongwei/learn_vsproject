// �׳�ĩβ����.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"

#include<iostream>

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}
int way1(int N)
{
	int r=0;
	int j;
	for(int i=1;i<=N;i++)
	{
		 j=i;
		 while(j%5==0)
		 {
			 r++;
			 j/=5;
		 }

	}
	return r;
}

int way2(int N)
{
	int r=0;
	while(N)
	{
		r+=N/5;
		N/=5;
	}
	return r;
}
int main()
{
	cout<<"����1�� "<<way1(10)<<endl;
	cout<<"����2�� "<<way2(10)<<endl;
	return 0;
}