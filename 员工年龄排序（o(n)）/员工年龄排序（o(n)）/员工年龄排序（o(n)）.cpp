// 员工年龄排序（o(n)）.cpp : 定义控制台应用程序的入口点。
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
			throw new std::exception("年龄不在给定范围内！");
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

