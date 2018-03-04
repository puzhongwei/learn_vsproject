// mapTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include<map>
#include<iostream>
#include<string>
using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	map<int,string> m1;
	map<int,string>::iterator it;
	m1.insert(pair<int,string>(1,"dfgdfg"));
	m1.insert(pair<int,string>(2,"bbbb"));
	m1.insert(pair<int,string>(3,"cccc"));
	m1.insert(pair<int,string>(4,"ddd"));
	m1.insert(pair<int,string>(5,"eee"));
	for(it=m1.begin();it!=m1.end();it++)
	{
		cout<<it->first<<"-->"<<it->second<<endl;
	}

	return 0;
}

