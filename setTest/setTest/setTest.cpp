// setTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include<set>
#include<iostream>
using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	set<int> s;
	set<int >::iterator it;
	for(int  i=0;i<100;i++)
	{
	s.insert(i);
	}
	for(  it=s.begin();it!=s.end();it++)
	{
		cout<<*it<<endl;
	}
	it=s.upper_bound(51);
	while(it!=s.end()){

		cout<<*it<<endl;
		it++;
	}
	
	return 0;
}

