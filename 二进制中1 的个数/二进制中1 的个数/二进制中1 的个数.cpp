// 二进制中1 的个数.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include"iostream"
using namespace std;

int getNumof1(long long n )
{
	int count =0;
	while(n)
	{
		count+=n&0x1;
		n=n>>1;
	}
	return count;
}

int _tmain(int argc, _TCHAR* argv[])
{
	long long a=0;
	
	while(cin>>a)
	{
		a = (a >=0)?a:(~a+0x01);
		cout<<getNumof1(a)<<endl;
	}
	return 0;
}

