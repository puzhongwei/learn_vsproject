// ��������1�ĸ���.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"


int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}
#include<iostream>
using namespace std;

int Count(int v)
{
	int num=0;
	while(v)
	{	if(v%2 == 1)
			{
				num++;
	         }
	
		 
			v=v/2;
		
	}
	return num;
}

int Count2(int v)
{
	int num=0;
	while(v)
	{
		num+=v&0x01;
		v>>=1;

	}
	return num;

}
int Count3(int v)
{
	int num=0;
	while(v)
	{
		v&=(v-1);
		num++;
	}
	return num;

}
int main()
{
	cout<<Count(16463546354165)<<endl;

	cout<<"����2���:"<<Count2(8)<<endl;
	cout<<"����3���:"<<Count3(8)<<endl;
	return 0;
}