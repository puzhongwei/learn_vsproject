// HeapTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include<iostream>
#include<vector>
#include<algorithm>
using namespace std;
int _tmain(int argc, _TCHAR* argv[])
{
	 int a[] = {15, 1, 12, 30, 20};
  vector<int> ivec(a, a+5);
  for(vector<int>::iterator iter=ivec.begin();iter!=ivec.end();++iter)
    cout<<*iter<<" ";
  cout<<endl;

  make_heap(ivec.begin(), ivec.end());//建堆
  for(vector<int>::iterator iter=ivec.begin();iter!=ivec.end();++iter)
    cout<<*iter<<" ";
  cout<<endl;

  pop_heap(ivec.begin(), ivec.end());//先pop,然后在容器中删除
  ivec.pop_back();
  for(vector<int>::iterator iter=ivec.begin();iter!=ivec.end();++iter)
    cout<<*iter<<" ";
  cout<<endl;

  ivec.push_back(99);//先在容器中加入，再push
  push_heap(ivec.begin(), ivec.end());
  for(vector<int>::iterator iter=ivec.begin();iter!=ivec.end();++iter)
    cout<<*iter<<" ";
  cout<<endl;

  sort_heap(ivec.begin(), ivec.end());
  for(vector<int>::iterator iter=ivec.begin();iter!=ivec.end();++iter)
    cout<<*iter<<" ";
  cout<<endl;

	return 0;
}

