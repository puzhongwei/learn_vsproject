// HeapTest.cpp : �������̨Ӧ�ó������ڵ㡣
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

  make_heap(ivec.begin(), ivec.end());//����
  for(vector<int>::iterator iter=ivec.begin();iter!=ivec.end();++iter)
    cout<<*iter<<" ";
  cout<<endl;

  pop_heap(ivec.begin(), ivec.end());//��pop,Ȼ����������ɾ��
  ivec.pop_back();
  for(vector<int>::iterator iter=ivec.begin();iter!=ivec.end();++iter)
    cout<<*iter<<" ";
  cout<<endl;

  ivec.push_back(99);//���������м��룬��push
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

