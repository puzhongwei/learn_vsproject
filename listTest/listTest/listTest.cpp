// listTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include<list>
#include<iostream>
using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	list<int> l;
	list<int> ::iterator it;
	for (int i =0;i<100;i++)
	{
		l.push_back(i);
	}
	for (it=l.begin();it!=l.end();it++)
	{
		cout<<*it<<endl;
	}
	return 0;
}

