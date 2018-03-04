// Vectortest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include<stdio.h>
#include<algorithm>
#include<vector>
#include<iostream>
using namespace std;


int _tmain(int argc, _TCHAR* argv[])
{
	vector<int> vec;
	for(int i =0;i<100;i++)
	{
		vec.push_back(i);
	}
	vector<int>::iterator it;
	//for(it=vec.begin();it!=vec.end();it++)
    //cout<<*it<<endl;
	for(int i=0;i<vec.size();i++)
		cout<<vec[i]<<endl;
	reverse(vec.begin(),vec.end());

	for (int j =0;j<vec.size();j++)
		cout<<vec[j]<<endl;

	return 0;
}

