// 连续字数组最大和.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include"iostream"
using namespace std;
int * getMax(int *p,int length)
{
	int *r;
	r=new int[2];
	if(p==NULL||length<=0)
	{
		return 0;
	}
	int cursum=p[0];
	int result=0;
	int i=0;
	int end=0;
	while(i<length)
	{
		if(cursum<0)
		{
			cursum=p[i];
		}
		else 
			cursum+=p[i];
		if(cursum>result)
			{
				result=cursum;
				end=i;
		}
		i++;
	}
	r[0]=result;
	r[1]=end;
	return r;
}

int _tmain(int argc, _TCHAR* argv[])
{
	int p[]={1,-2,3,10,-4,7,2,-5};
	int *r;
	r=getMax(p,8);
	cout<<"结果:"<<r[0]<<endl;
	cout<<"数组:"<<endl;
	int i=r[1];
	int sum=0;
	while(sum<r[0])
	{
		cout<<p[i]<<"  ";
		sum+=p[i];
		i--;
	}	
	cout<<endl;
	return 0;
}

