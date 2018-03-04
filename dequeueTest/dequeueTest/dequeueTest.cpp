// dequeueTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include<deque>
#include<string>
#include <iostream>  
#include<algorithm>
using namespace std;


int _tmain(int argc, _TCHAR* argv[])
{
	deque<int> demo;
	deque<int>::iterator it;
	for(int i=0;i<100;i++)
	{
		demo.push_front(i);
	}
	for(int i=0;i<100;i++)
	{
		demo.push_back(i);
	}
	for(int i=0;i<demo.size();i++)
		cout<<demo[i]<<endl;
	for(it=demo.begin();it<demo.end();it++)
		cout<<*it<<endl;
	bool r=demo.empty();
	cout<<r;
	return 0;
}

