// Ա����������o(n)��.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include"iostream"
using namespace std;
void sort(int a[],int length)
{
	if(a==NULL||length<=0)
		return;
	const int maxage=99;
	int times[maxage+1];
	for(int i=0;i<maxage;i++)
	{
		times[i]=0;
	}
	for(int i=0;i<length;i++)
	{
		int age=a[i];
		if(age<0||maxage>99)
			throw new std::exception("���䲻�ڸ�����Χ�ڣ�");
		++times[age];
	}
	int index=0;
	for(int i=0;i<=maxage;i++)
	{
		for(int j=0;j<times[i];j++)
				a[index]=i;
		index++;
	}

}

int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}

